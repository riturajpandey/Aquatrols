using Aquatrols.iOS.Controllers;
using Aquatrols.iOS.Helper;
using Aquatrols.iOS.TableVewSource;
using Aquatrols.Models;
using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToastIOS;
using UIKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS
{
    /// <summary>
    /// This class file is used for Course search screen
    /// </summary>
    public partial class TmCourseSearchController : UIViewController
    {
        private int viewWidth;
        private string token,tmCourseID,tmSuperIntendentID;
        private LoadingOverlay loadPop;
        private Utility utility = Utility.GetInstance;
        private List<TmCourseResponseEntity> tmCourseResponseEntity;
        private List<TmSuperIntendantResponseEntity> tmSuperIntendantResponseEntities;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TmCourseSearchController"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public TmCourseSearchController(IntPtr handle) : base(handle)
        {
        }
        /// <summary>
        /// Views the did load.
        /// </summary>
        public async override void ViewDidLoad()
        {
            try
            {
                // get the token
                if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token)))
                {
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                }
                //Check exception data if net is not copnnected and stored the exception in nsuserdefault if user redirect the page first hit the exception in previous saved in shared preferences.
                string exception = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.Exception);
                if (!String.IsNullOrEmpty(exception))
                {
                    utility.SaveExceptionHandling(null, AppResources.ViewDidLoad, AppResources.TmCourseSearch, exception);
                }
                SetFonts();
                SetLogoSize();
                ImgBackClick();
                AddDoneBUtton();
                await GetAllTmCourseList(token); // get the all tm course list
                await GetAllTmSuperIntendentList(token);// get the all tm superintendent list
                LblLogoutDClick();
                LblSupportDeacClick();
                LblFAQDeacClick();
                LblPrivacyPolicyDeacClick();
                LblTermsAndConditionsDeactivateClick();
                ImgMenuDeactiveClick();
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ViewDidLoad, AppResources.TmCourseSearch, null);
            }
        }
        /// <summary>
        /// Views the will appear.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewWillAppear(bool animated)
        {
            tblTmCourse.Hidden = true;
            tblSuperIntendent.Hidden = true;
            txtCourseName.Text = string.Empty;
            txtSuperIntendant.Text = string.Empty;
        }
        /// <summary>
        /// Set Logo size sepcified in iPhone X.
        /// set UI in iPhone X
        /// </summary>
        public void SetLogoSize()
        {
            try
            {
                viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
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
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.setLogoSize, AppResources.TmCourseSearch, null);
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
        /// Gesture implementation for menu click on deactivate dashboard.
        /// hide or show pop up
        /// </summary>
        public void ImgMenuDeactiveClick()
        {
            UITapGestureRecognizer imgMenuDeactiveClicked = new UITapGestureRecognizer(() =>
            {
                if (popUpMenu.Hidden == true)
                {
                    popUpMenu.Hidden = false;
                }
                else
                {
                    popUpMenu.Hidden = true;
                }
            });
            ImgMenu.UserInteractionEnabled = true;
            ImgMenu.AddGestureRecognizer(imgMenuDeactiveClicked);
        }
        /// <summary>
        /// Gesture implementation for terms and conditions clcik on deactivate dashboard.
        /// </summary>
        public void LblTermsAndConditionsDeactivateClick()
        {
            UITapGestureRecognizer lblTermsAndConditionsDeactivateClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenu.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[1], AppResources.PDFValue);
            });
            lblTermsAndConditions.UserInteractionEnabled = true;
            lblTermsAndConditions.AddGestureRecognizer(lblTermsAndConditionsDeactivateClicked);
        }
        /// <summary>
        /// Gesture implementation for privacy policy click on deactivate dashboard.
        /// </summary>
        public void LblPrivacyPolicyDeacClick()
        {
            UITapGestureRecognizer lblPrivacyClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenu.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[4], AppResources.PDFValue);
            });
            lblPrivacy.UserInteractionEnabled = true;
            lblPrivacy.AddGestureRecognizer(lblPrivacyClicked);
        }
        /// <summary>
        /// Gesture implementation for FAQ click on deactivate dashboard.
        /// </summary>
        public void LblFAQDeacClick()
        {
            UITapGestureRecognizer lblFAQDeacClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenu.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[5], AppResources.PDFValue);
            });
            lblFaq.UserInteractionEnabled = true;
            lblFaq.AddGestureRecognizer(lblFAQDeacClicked);
        }
        /// <summary>
        /// Gesture implementation for Support click on deactivate dashboard.
        /// </summary>
        public void LblSupportDeacClick()
        {
            UITapGestureRecognizer lblSupportDeacClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenu.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[6], AppResources.PDFValue);
            });
            lblSupport.UserInteractionEnabled = true;
            lblSupport.AddGestureRecognizer(lblSupportDeacClicked);
        }
        /// <summary>
        /// Gesture implementation for logout the deactivate dashboard.
        /// Logout user
        /// </summary>
        public void LblLogoutDClick()
        {
            UITapGestureRecognizer lblLogoutDClicked = new UITapGestureRecognizer(() =>
            {
                Utility.ClearCachedData();
                UIViewController[] uIViewControllers = NavigationController.ViewControllers;
                foreach (UIViewController item in uIViewControllers)
                {
                    if (item is LoginController)
                    {
                        NavigationController.PopToViewController(item, true);
                        break;
                    }
                }
            });
            lblLogout.UserInteractionEnabled = true;
            lblLogout.AddGestureRecognizer(lblLogoutDClicked);
        }
        /// <summary>
        /// Buttons the course search touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnCourseSearch_TouchUpInside(UIButton sender)
        {
            try
            {
                if (string.IsNullOrEmpty(tmCourseID))
                {
                    Toast.MakeText(AppResources.RequireCourseName).Show();
                }
                else
                {
                    txtCourseName.Text = string.Empty;
                    txtSuperIntendant.Text = string.Empty;
                    txtCourseName.ResignFirstResponder();
                    this.PerformSegue(AppResources.SeguefromTmCourseSearchToTmCourseResult, this);
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex,AppResources.BtnCourseSearch_TouchUpInside, AppResources.TmCourseSearch, null);
            }
        }
        /// <summary>
        /// Buttons the super intendant search touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnSuperIntendantSearch_TouchUpInside(UIButton sender)
        {
            try
            {
                if (string.IsNullOrEmpty(tmSuperIntendentID))
                {
                    Toast.MakeText(AppResources.RequireSuperintendant).Show();
                }
                else
                {
                    txtCourseName.Text = string.Empty;
                    txtSuperIntendant.Text = string.Empty;
                    txtSuperIntendant.ResignFirstResponder();
                    this.PerformSegue(AppResources.SeguefromTmCourseSearchToTmCourseResult, this);
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.BtnSuperIntendantSearch_TouchUpInside, AppResources.TmCourseSearch, null);
            }
        }
        /// <summary>
        /// Prepares for segue.
        /// </summary>
        /// <param name="segue">Segue.</param>
        /// <param name="sender">Sender.</param>
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            try
            {
                if (segue.Identifier == AppResources.SeguefromTmCourseSearchToTmCourseResult)
                {
                    var navctlr = segue.DestinationViewController as TmCourseResultController;
                    if (navctlr != null)
                    {
                        if(!string.IsNullOrEmpty(tmSuperIntendentID))
                        {
                            navctlr.tmSuperIntendentID = this.tmSuperIntendentID;
                            this.tmSuperIntendentID = string.Empty;

                        }
                        if(!string.IsNullOrEmpty(tmCourseID))
                        {
                            navctlr.tmCourseID = this.tmCourseID;
                            this.tmCourseID = string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.PrepareForSegue, AppResources.TmCourseSearch, null);
            }
        }
        /// <summary>
        /// Method implementation to call tm SuperIntendent list API.
        /// </summary>
        /// <param name="token">Token.</param>
        public async Task GetAllTmSuperIntendentList(string token)
        {
            loadPop = new LoadingOverlay(View.Frame);
            this.View.Add(loadPop);
            try
            {
                tmSuperIntendantResponseEntities = await utility.GetSuperIntendentList(token);
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.GetAllTmSuperIntendentList, AppResources.TmCourseSearch, null);
            }
            finally
            {
                loadPop.Hide();
            }
        }
        /// <summary>
        /// Method implementation to call tm course list API.
        /// </summary>
        /// <param name="token">Token.</param>
        public async Task GetAllTmCourseList(string token)
        {
            loadPop = new LoadingOverlay(View.Frame);
            this.View.Add(loadPop);
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    tmCourseResponseEntity = await utility.GetTmCourseList(token);
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.GetAllTmCourseList, AppResources.TmCourseSearch, null);
            }
            finally
            {
                loadPop.Hide();
            }
        }
        /// <summary>
        /// Tm SuperIntendent name text changed event
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void TxtTmSuperIntendent_TextChanged(UITextField sender)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtSuperIntendant.Text))
                {
                    List<TmSuperIntendantResponseEntity> tempTmSuperIntendentResponseEntities = tmSuperIntendantResponseEntities.Where(x =>(x.userName.ToLower().StartsWith(txtSuperIntendant.Text.ToLower().Trim()) || x.userName.ToLower().Contains(txtSuperIntendant.Text.ToLower().Trim())) || ((x.firstName + " " + x.lastName).ToLower().StartsWith(txtSuperIntendant.Text.ToLower().Trim()) || (x.firstName + " " + x.lastName).ToLower().Contains(txtSuperIntendant.Text.ToLower().Trim()))).ToList();
                 
                    if (tempTmSuperIntendentResponseEntities != null && tempTmSuperIntendentResponseEntities.Count != 0)
                    {
                        tblSuperIntendent.Hidden = false;
                        TableViewTmSuperIntendentList tmSuperIntendentList = new TableViewTmSuperIntendentList(this, tempTmSuperIntendentResponseEntities);
                        tblSuperIntendent.Source = tmSuperIntendentList;
                        tblSuperIntendent.ReloadData();
                        tmSuperIntendentList.OnRowSelect += HandleOnRowSelect;
                    }
                    else
                    {
                        tblSuperIntendent.Hidden = true;
                    }
                }
                else
                {
                    tblSuperIntendent.Hidden = true;
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.TxtTmSuperIntendent_TextChanged, AppResources.TmCourseSearch, null);
            }
        }
        /// <summary>
        /// Add done button on keyboard
        /// </summary>
        public void AddDoneBUtton()
        {
            UITextField[] textFields = new UITextField[] { txtCourseName, txtSuperIntendant };
            Utility.AddDoneButtonToKeyboard(textFields);
        }
        /// <summary>
        /// Tm course name text changed event
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void TxtTmCourse_textChanged(UITextField sender)
        {
            try
            {
                if(!String.IsNullOrEmpty(txtCourseName.Text))
                {
                    List<TmCourseResponseEntity> tempTmCourseResponseEntities = tmCourseResponseEntity.Where(x => x.courseName.ToLower().StartsWith(txtCourseName.Text.ToLower()) || x.courseName.ToLower().Contains(txtCourseName.Text.ToLower())).ToList();
                    if (tempTmCourseResponseEntities != null && tempTmCourseResponseEntities.Count != 0)
                    {
                        List<string> courseName = new List<string>();
                        foreach (var item in tempTmCourseResponseEntities)
                        {
                            courseName.Add(item.courseName + " (ZipCode :" + item.zip + ")");
                        }
                        tblTmCourse.Hidden = false;
                        TableViewTmCourseList tableViewTmCourseList = new TableViewTmCourseList(this, courseName);
                        tblTmCourse.Source = tableViewTmCourseList;
                        tblTmCourse.ReloadData();
                        tableViewTmCourseList.OnRowSelect += HandleOnCourseRowSelect;
                    }
                    else
                    {
                        tblTmCourse.Hidden = true;
                    }
                }
                else
                {
                    tblTmCourse.Hidden = true;
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex,AppResources.TxtTmCourse_textChanged, AppResources.TmCourseSearch, null);
            }
        }
        /// <summary>
        /// Handles the on row select on click of Tm course list.
        /// </summary>
        /// <param name="tmCourseName">Coursename.</param>
        public void HandleOnCourseRowSelect(string tmCourseName)
        {
            try
            {
                string[] CourseName = tmCourseName.Split('(');
                string[] Zipcode = CourseName[1].Split(':');
                string zipValue = Zipcode[1].ToString().Replace(')', ' ').Trim();
                txtCourseName.Text = tmCourseName;

                //tmCourseID = tmCourseResponse.cId;
                tmCourseID = tmCourseResponseEntity.Where(a => a.courseName.ToLower().Trim() == CourseName[0].ToLower().Trim() && a.zip == zipValue).Select(a => a.cId).FirstOrDefault();
               // txtCourseName.Text = tmCourseResponse.courseName;
                tblTmCourse.Hidden = true;
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.HandleOnCourseRowSelect, AppResources.TmCourseSearch, null);
            }
        }
        /// <summary>
        /// Handles the on row select for superintendent row click.
        /// </summary>
        /// <param name="tmSuperIntendant">Tm course response.</param>
        public void HandleOnRowSelect(TmSuperIntendantResponseEntity tmSuperIntendant)
        {
            try
            {
                tmSuperIntendentID = tmSuperIntendant.id;
                txtSuperIntendant.Text = tmSuperIntendant.firstName +" "+ tmSuperIntendant.lastName ;
                tblSuperIntendent.Hidden = true; 
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex,AppResources.HandleOnRowSelect, AppResources.TmCourseSearch, null);
            }
        }
        /// <summary>
        /// Set the fonts of UILabel, UITextField and UIButton.
        /// </summary>
        public void SetFonts()
        {
            try
            {
                viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                Utility.SetFonts(null, new UILabel[] { lblReviewInformation }, new UITextField[] { txtCourseName,txtSuperIntendant }, Constant.lstDigit[9], viewWidth);
                Utility.SetFonts(null, new UILabel[] { lblLogout, lblPrivacy, lblSupport, lblTermsAndConditions, lblFaq }, null, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblSuperintendant, lblCourse ,lblCourseSearch}, new UIButton[] { btnCourseSearch, btnSuperIntendantSearch }, Constant.lstDigit[11], viewWidth);
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetFonts, AppResources.TmCourseSearch, null);
            }
        }
    }
}