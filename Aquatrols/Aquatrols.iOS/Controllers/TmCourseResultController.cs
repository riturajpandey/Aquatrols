using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using CoreGraphics;
using Foundation;
using System;
using System.Threading.Tasks;
using ToastIOS;
using UIKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS
{
    /// <summary>
    /// This class file is used for course result screen
    /// </summary>
    public partial class TmCourseResultController : UIViewController
    {
        private int viewWidth;
        public string tmCourseID, tmSuperIntendentID,token;
        private LoadingOverlay loadPop;
        private Utility utility = Utility.GetInstance;
        private TmCourseResultResponseEntity tmCourseResultResponseEntity;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TmCourseResultController"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public TmCourseResultController (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Views the did load.
        /// </summary>
        public async override void ViewDidLoad()
        {
            try
            {
                //Check exception data if net is not copnnected and stored the exception in nsuserdefault if user redirect the page first hit the exception in previous saved in shared preferences.
                string exception = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.Exception);
                if (!String.IsNullOrEmpty(exception))
                {
                    utility.SaveExceptionHandling(null, AppResources.ViewDidLoad, AppResources.TermsConditionWebView, exception);
                }
                // get the token 
                if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token)))
                {
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                }
                // call the Course result API and get the course id passing the api.
                if (!string.IsNullOrEmpty(tmCourseID))
                {
                    await CallTmCourseResultResultAPI(tmCourseID, token);
                }
                // get the superintendentId and call the tmsuperintendentresult Api.
                if (!string.IsNullOrEmpty(tmSuperIntendentID))
                {
                    await CallTmSuperintendentResultResultAPI(tmSuperIntendentID, token);
                }
                ImgBackClick();
                SetLogoSize();
                SetFonts();
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ViewDidLoad, AppResources.TmCourseResult, null);
            }
        }
        /// <summary>
        /// Set Logo size sepcified in iPhone X.
        /// set UI in iPhone X
        /// </summary>
        public void SetLogoSize()
        {
            try
            {
                string device = UIDevice.CurrentDevice.Model;
                viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                if (device == AppResources.iPad)
                {
                    if (viewWidth == (int)DeviceScreenSize.IPadPro)
                    {
                        lblIsCourseEnrolled.Frame = new CGRect(lblIsCourseEnrolled.Frame.X, lblIsCourseEnrolled.Frame.Y, lblIsCourseEnrolled.Frame.Width - Constant.lstDigit[18], lblIsCourseEnrolled.Frame.Height);
                        lblYesOrNo.Frame = new CGRect(lblIsCourseEnrolled.Frame.X+ lblIsCourseEnrolled.Frame.Width, lblYesOrNo.Frame.Y, lblYesOrNo.Frame.Width, lblYesOrNo.Frame.Height);
                    }
                    else
                    {
                        lblIsCourseEnrolled.Frame = new CGRect(lblIsCourseEnrolled.Frame.X+Constant.lstDigit[19], lblIsCourseEnrolled.Frame.Y, lblIsCourseEnrolled.Frame.Width - Constant.digitEighty, lblIsCourseEnrolled.Frame.Height);
                        lblYesOrNo.Frame = new CGRect(lblIsCourseEnrolled.Frame.X+lblIsCourseEnrolled.Frame.Width-Constant.lstDigit[6], lblYesOrNo.Frame.Y, lblYesOrNo.Frame.Width, lblYesOrNo.Frame.Height);
                    }
                }
                else
                {
                    int viewHeight = (int)UIScreen.MainScreen.Bounds.Size.Height;
                    if (viewWidth == (int)DeviceScreenSize.ISix)
                    {
                        if (viewHeight == Constant.lstDigit[31])
                        {
                            vwHeader.Frame = new CGRect(vwHeader.Frame.X, vwHeader.Frame.Y + Constant.lstDigit[10], vwHeader.Frame.Width, vwHeader.Frame.Height);
                            vwChild.Frame = new CGRect(vwChild.Frame.X, vwChild.Frame.Y + Constant.lstDigit[10], vwChild.Frame.Width, vwChild.Frame.Height);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.setLogoSize, AppResources.TmCourseResult, null); // exception handling 
            }
        }
        /// <summary>
        /// Method implementation to call tm course result list API.
        /// </summary>
        /// <param name="token">Token.</param>
        public async Task CallTmCourseResultResultAPI(string courseId,string token)
        {
            loadPop = new LoadingOverlay(View.Frame);
            this.View.Add(loadPop);
            try
            {
                tmCourseResultResponseEntity = await utility.GetTmCourseResultData(courseId,token);
                if (tmCourseResultResponseEntity != null)
                {
                    //  lblErrorMessage.Hidden = true;
                    lblIsCourseEnrolled.Hidden = false;
                    lblYesOrNo.Hidden = false;
                    string UserTiedToCourseValue= tmCourseResultResponseEntity.isUserTiedToCourse;
                    if(UserTiedToCourseValue=="false")
                    {
                        lblYesOrNo.Text = AppResources.No;
                    }
                    else
                    {
                        lblYesOrNo.Text = AppResources.Yes;
                    }

                    lblTmCourseName.Text = tmCourseResultResponseEntity.tmCoursesVm.courseName;
                    lblTmAddressValue.Text = tmCourseResultResponseEntity.tmCoursesVm.address;
                    lblTmCityValue.Text=tmCourseResultResponseEntity.tmCoursesVm.city;
                    lblTmStateValue.Text=tmCourseResultResponseEntity.tmCoursesVm.state;
                    lblTmZipValue.Text=tmCourseResultResponseEntity.tmCoursesVm.zip;
                    lblCountryValue.Text=tmCourseResultResponseEntity.tmCoursesVm.country;
                   
                    TableTmCourseSearchResult tmCourseResultSearch = new TableTmCourseSearchResult(this, tmCourseResultResponseEntity.tmDistributorProductVm);
                    if(tmCourseResultResponseEntity.tmDistributorProductVm.Count==0)
                    {
                        lblErrorMessage.Hidden = true; // hidden because of bussiness requirment 
                        tblTmCourseSearchResult.Hidden = true;                       
                    }
                    else
                    {
                        lblErrorMessage.Hidden = true;
                        tblTmCourseSearchResult.Source = tmCourseResultSearch;
                        tblTmCourseSearchResult.ReloadData();
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.NoData).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.CallTmCourseResultResultAPI, AppResources.TmCourseResult, null);
            }
            finally
            {
                loadPop.Hide();
            }
        }
        /// <summary>
        /// Method implementation to call tm Superintendent result list API.
        /// </summary>
        /// <param name="token">Token.</param>
        public async Task CallTmSuperintendentResultResultAPI(string SuperIntendentID, string token)
        {
            loadPop = new LoadingOverlay(View.Frame);
            this.View.Add(loadPop);
            try
            {
                tmCourseResultResponseEntity = await utility.GetTmSuperIntendentResultData(SuperIntendentID, token);
                if (tmCourseResultResponseEntity != null)
                {
                    lblIsCourseEnrolled.Hidden = true;
                    lblYesOrNo.Hidden = true;
                    lblTmCourseName.Text = tmCourseResultResponseEntity.tmCoursesVm.courseName;
                    lblTmAddressValue.Text = tmCourseResultResponseEntity.tmCoursesVm.address;
                    lblTmCityValue.Text = tmCourseResultResponseEntity.tmCoursesVm.city;
                    lblTmStateValue.Text = tmCourseResultResponseEntity.tmCoursesVm.state;
                    lblTmZipValue.Text = tmCourseResultResponseEntity.tmCoursesVm.zip;
                    lblCountryValue.Text = tmCourseResultResponseEntity.tmCoursesVm.country;

                    TableTmCourseSearchResult tmCourseResultSearch = new TableTmCourseSearchResult(this, tmCourseResultResponseEntity.tmDistributorProductVm);
                    if(tmCourseResultResponseEntity.tmDistributorProductVm.Count==0)
                    {
                        lblErrorMessage.Hidden = true;  // hidden because of bussiness requirment 
                        tblTmCourseSearchResult.Hidden = true;
                    }
                    else
                    {
                        lblErrorMessage.Hidden = true;
                        tblTmCourseSearchResult.Hidden = true;// hidden because of bussiness requirment 
                        tblTmCourseSearchResult.Source = tmCourseResultSearch;
                        tblTmCourseSearchResult.ReloadData();
                    }                   
                }
                else
                {
                    Toast.MakeText(AppResources.NoData).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.CallTmSuperintendentResultResultAPI, AppResources.TmCourseResult, null);
            }
            finally
            {
                loadPop.Hide();
            }
        }
        /// <summary>
        /// Back button click event handler
        /// </summary>
        public void ImgBackClick()
        {
            UITapGestureRecognizer imgBackClicked = new UITapGestureRecognizer(() =>
            {
                this.NavigationController.PopViewController(true);
            });
            imgBack.UserInteractionEnabled = true;
            imgBack.AddGestureRecognizer(imgBackClicked);
        }
        /// <summary>
        /// Set the fonts of UILabel, UITextField and UIButton.
        /// </summary>
        public void SetFonts()
        {
            try
            {
                viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                Utility.SetFonts(null, new UILabel[] { lblAddress, lblTmAddressValue, lblCity, lblTmCityValue, lblState,lblTmStateValue,lblZipCode,lblTmZipValue ,lblCountry,lblCountryValue}, null, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblProgramCommitments, lblCourse,lblCourseSearchResult }, null, Constant.lstDigit[11], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblTmCourseName ,lblIsCourseEnrolled,lblYesOrNo}, null, Constant.lstDigit[9], viewWidth);
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetFonts, AppResources.TmCourseResult, null);
            }
        }
    }
}