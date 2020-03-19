using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using CoreGraphics;
using Foundation;
using MessageUI;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ToastIOS;
using UIKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS
{
    /// <summary>
    /// This class file is used for distributor result screen
    /// </summary>
    public partial class TmDistributorResultController : UIViewController
    {
        private int viewWidth;
        private string token,distributorName,stateName;
        private LoadingOverlay loadPop;
        private Utility utility = Utility.GetInstance;
        private TmDistributorResultResponseEntity tmDistributorResultResponseEntity;
        MFMailComposeViewController mailController;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TmDistributorResultController"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public TmDistributorResultController (IntPtr handle) : base (handle)
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
                    utility.SaveExceptionHandling(null, AppResources.ViewDidLoad, AppResources.TmDistributorResult, exception);
                }
                //get the token
                if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token)))
                {
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                }
                // get the territory manager distributor name
                if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.TmDistributorName)))
                {
                    distributorName = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.TmDistributorName);
                }
                // get the territory manager state name
                if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.TmStateName)))
                {
                    stateName = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.TmStateName);
                }
                await GetDistributorResultData(distributorName, stateName, token);// get the distributor result data 
                ImgBackClick();
                SetLogoSize();
                SetFonts();
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetFonts, AppResources.TmDistributorResult, null);
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
                viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                int viewHeight = (int)UIScreen.MainScreen.Bounds.Size.Height;
                if (viewWidth == (int)DeviceScreenSize.ISix)
                {
                    if (viewHeight == Constant.lstDigit[31])
                    {
                        vwHeader.Frame = new CGRect(vwHeader.Frame.X, vwHeader.Frame.Y + Constant.lstDigit[10], vwHeader.Frame.Width, vwHeader.Frame.Height);
                        vwChildContent.Frame = new CGRect(vwChildContent.Frame.X, vwChildContent.Frame.Y + Constant.lstDigit[10], vwChildContent.Frame.Width, vwChildContent.Frame.Height);
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.setLogoSize, AppResources.TmDistributorResult, null);
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
        /// Gets the territory Manager Distributor Result list.
        /// </summary>
        /// <returns>The tm state list.</returns>
        /// <param name="token">Token.</param>
        public async Task GetDistributorResultData(string distributorName, string state,string token)
        {
            loadPop = new LoadingOverlay(this.View.Frame);
            this.View.Add(loadPop);
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    TmDistributorResultRequestEntity TmDistributorResultRequestEntity = new TmDistributorResultRequestEntity();
                    TmDistributorResultRequestEntity.distributorName = distributorName;
                    TmDistributorResultRequestEntity.state = state;
                    tmDistributorResultResponseEntity = await utility.GetTmDistributorSearchResultData(TmDistributorResultRequestEntity, token);
                    if(tmDistributorResultResponseEntity!=null)
                    {
                        if(tmDistributorResultResponseEntity.tmCourseProductVm.Count > 0)
                        {
                          
                          TableViewDistributorResult tmCourseResultSearch = new TableViewDistributorResult(this, tmDistributorResultResponseEntity.tmCourseProductVm);
                          tblDistributorResult.Source = tmCourseResultSearch;
                          tblDistributorResult.ReloadData();
                          lblErrormsg.Hidden = true;
                          btnDownLoadExcelDistributor.Hidden = false;
                          btnShareExcel.Hidden = false;
                        }
                        else
                        {
                            lblErrormsg.Hidden = false;
                            tblDistributorResult.Hidden = true;
                            btnDownLoadExcelDistributor.Hidden = true;
                            btnShareExcel.Hidden = true;
                        }
                        if(tmDistributorResultResponseEntity.tmDistributors!=null)
                        {
                            NSUserDefaults.StandardUserDefaults.SetString(string.Empty, AppResources.TmDistributorID);
                        }
                    }  
                    else
                    {
                        lblErrormsg.Hidden = false;
                        tblDistributorResult.Hidden = true;
                        btnDownLoadExcelDistributor.Hidden = true;
                        btnShareExcel.Hidden = true;
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.GetDistributorResultData, AppResources.TmDistributorResult, null);
            }
            finally
            {
                loadPop.Hide();
            }
        }
        /// <summary>
        /// Buttons the share excel touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void btnShareExcel_TouchUpInside(UIButton sender)
        {
            AttachFileFromThePath(token);
        }
        /// <summary>
        /// Buttons down load excel distributor touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void btnDownLoadExcelDistributor_TouchUpInside(UIButton sender)
        {
            GetFilefromServer(token);
        }
        /// <summary>
        /// GET territory Excel data file from server.
        /// </summary>
        /// <param name="token">Token.</param>
        public async void GetFilefromServer(string token)
        {
            string pathToDownloadFile = string.Empty;
            loadPop = new LoadingOverlay(this.View.Frame);

            try
            {
                this.View.Add(loadPop);
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    TmDistributorResultRequestEntity tmDistributorResultRequestExcelDownloadEntity = new TmDistributorResultRequestEntity();
                    tmDistributorResultRequestExcelDownloadEntity.distributorName = distributorName;
                    tmDistributorResultRequestExcelDownloadEntity.state = stateName;
                    string filePath = await utility.GetTerritoryDataFileDistributor(tmDistributorResultRequestExcelDownloadEntity,token);
                    if (filePath!=null)
                    {
                        var directoryName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Approach");

                        if (!Directory.Exists(directoryName))
                        {
                            Directory.CreateDirectory(directoryName);
                        }
                        using (WebClient webClient = new WebClient())
                        {
                            pathToDownloadFile = Path.Combine(directoryName, Path.GetFileName(filePath));
                            webClient.DownloadFileCompleted += (s, e) =>
                            {

                                NSUserDefaults.StandardUserDefaults.SetString(Path.GetFileName(filePath), "TmfileDistributor");
                                Toast.MakeText(AppResources.DownLoadCompleted, Constant.durationOfToastMessage).Show();
                                loadPop.Hide();
                            };
                            webClient.DownloadFileAsync(new Uri(filePath), pathToDownloadFile);
                        }
                    }
                    else
                    {
                        Toast.MakeText(AppResources.InternetError).Show();
                        loadPop.Hide();
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                if (loadPop != null)
                {
                    loadPop.Hide();
                }
                utility.SaveExceptionHandling(ex, AppResources.GetDistributorResultData, AppResources.TmDistributorResult, null);   
            }
        }
        /// <summary>
        /// GET territory Excel data file from server.
        /// </summary>
        /// <param name="token">Token.</param>
        public async void AttachFileFromThePath(string token)
        {
            string pathToDownloadFile = string.Empty;
            loadPop = new LoadingOverlay(this.View.Frame);
            this.View.Add(loadPop);
           
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    var directoryName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Approach");
                    TmDistributorResultRequestEntity tmDistributorResultRequestExcelDownloadEntity = new TmDistributorResultRequestEntity();
                    tmDistributorResultRequestExcelDownloadEntity.distributorName = distributorName;
                    tmDistributorResultRequestExcelDownloadEntity.state = stateName;
                    string filePath = await utility.GetTerritoryDataFileDistributor(tmDistributorResultRequestExcelDownloadEntity, token);
                    if (filePath != null)
                    {
                        if (!Directory.Exists(directoryName))
                        {
                            Directory.CreateDirectory(directoryName);
                        }
                        using (WebClient webClient = new WebClient())
                        {
                            pathToDownloadFile = Path.Combine(directoryName, Path.GetFileName(filePath));
                            webClient.DownloadFileCompleted += (s, e) =>
                            {
                                OpenEmail(pathToDownloadFile, Path.GetFileName(filePath));
                                loadPop.Hide();
                            };
                            webClient.DownloadFileAsync(new Uri(filePath), pathToDownloadFile);
                        }
                    }
                    else
                    {
                        Toast.MakeText(AppResources.InternetError).Show();
                        loadPop.Hide();
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                    loadPop.Hide();
                }
            }
            catch (Exception ex)
            {
                if (loadPop != null)
                {
                    loadPop.Hide();
                }
                utility.SaveExceptionHandling(ex, AppResources.AttachFileFromThePath, AppResources.TmDistributorResult, null);
            }
        }
        /// <summary>
        /// Open the email if app installed in your device otherwise show message app is not found.
        /// </summary>
        public void OpenEmail(string path, string filename)
        {
            try
            {
                if (MFMailComposeViewController.CanSendMail)
                {
                    var to = new string[] { };
                    if (MFMailComposeViewController.CanSendMail)
                    {
                        mailController = new MFMailComposeViewController();
                        mailController.SetToRecipients(to);
                        mailController.SetSubject("");
                        mailController.SetMessageBody("", false);
                        NSData data = NSData.FromFile(path);
                        if (data != null)
                        {
                            mailController.AddAttachmentData(data, "application/excel", filename);
                        }
                         mailController.Finished += (object s, MFComposeResultEventArgs args) => {
                            Console.WriteLine(AppResources.result + args.Result.ToString()); // sent or cancelled
                            BeginInvokeOnMainThread(() => {
                                args.Controller.DismissViewController(true, null);
                            });
                        };
                    }
                    this.PresentViewController(mailController, true, null);
                }
                else
                {
                    Toast.MakeText(AppResources.NoAppFound, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex,AppResources.OpenEmail, AppResources.TmDistributorResult, null);
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
                Utility.SetFontsforHeader(new UILabel[] { lblDistributor,lblProgramCommitments , lblErrormsg,lblDistributorSearchResult }, new UIButton[] { btnShareExcel,btnDownLoadExcelDistributor }, Constant.lstDigit[11], viewWidth);
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex,AppResources.SetFonts, AppResources.TmDistributorResult, null);
            }
        }
    }
}