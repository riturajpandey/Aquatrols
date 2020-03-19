using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using CoreGraphics;
using Foundation;
using MessageUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ToastIOS;
using UIKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS
{
    /// <summary>
    /// This Class file is used for SnapShot Resilt Screen
    /// </summary>
    public partial class TmSnapResultController : UIViewController
    {

        public string token , countryName;
        private int viewWidth;
        private LoadingOverlay loadPop;
        private Utility utility = Utility.GetInstance;    
        private List<TmTotalCommitments> tmTotalCommitments;
        private List<TmCommitmentsPerDistributor> tmCommitmentsPerDistributor;
        MFMailComposeViewController mailController;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TmSnapResultController"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public TmSnapResultController (IntPtr handle) : base (handle)
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
                    utility.SaveExceptionHandling(null, AppResources.ViewDidLoad, AppResources.TmSnapresult, exception);
                }
                SetFonts();
                await GetTmSnapShotResultCountData(token);// get the territory manager result count data 
                await GetProductCommtPerDistributor(token);// get the product commitment based on distributor
                await GetTotalGallonPerLiterCommit(token); // get the total gallon Per liter 
                ImgBackClick();
                SetLogoSize();
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ViewDidLoad, AppResources.TmSnapresult, null);
            }
        }
        /// <summary>
        /// Gets the tm snap shot result count data.
        /// </summary>
        /// <returns>The tm snap shot result count data.</returns>
        /// <param name="token">Token.</param>
        public async Task GetTmSnapShotResultCountData(string token)
        {
            loadPop = new LoadingOverlay(this.View.Frame);
            try
            {
                this.View.Add(loadPop);
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    List<TmSnapshotSignUpCountVm> tmSnapshotSignUpCounts = await utility.GetTmSnapShotResultCountData(token);
                    if (tmSnapshotSignUpCounts != null)
                    {
                        TableTmSnapShotsignupCount tableTmSnapShotsignupCount = new TableTmSnapShotsignupCount(this, tmSnapshotSignUpCounts);
                        if(tableTmSnapShotsignupCount.Equals(0))
                        {
                            lblErrorMessagef.Hidden = false;
                            tblViewSnapShot.Hidden = true;
                        }
                        else
                        {
                            lblErrorMessagef.Hidden = true;
                            tblViewSnapShot.Source = tableTmSnapShotsignupCount;
                            tblViewSnapShot.ReloadData();
                        }
                    }
                    else
                    {
                        Toast.MakeText(AppResources.ErrorRecord, Constant.durationOfToastMessage).Show();
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.GetTmSnapShotResultCountData, AppResources.TmSnapresult, null);
            }
            finally
            {
                loadPop.Hide();
            }
        }
        /// <summary>
        /// GET PRODUCT COMMITMENTS PER DISTRIBUTOR.
        /// </summary>
        /// <returns>The tm snap shot result count data.</returns>
        /// <param name="token">Token.</param>
        public async Task GetProductCommtPerDistributor(string token)
        {
            loadPop = new LoadingOverlay(this.View.Frame);
            try
            {
                this.View.Add(loadPop);
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    tmCommitmentsPerDistributor = await utility.ProductCommtPerDistributor(token);
                    if (tmCommitmentsPerDistributor != null)
                    {
                        TableViewProductCommPerDist tableTmSnapShotsignupCount = new TableViewProductCommPerDist(this, tmCommitmentsPerDistributor);
                        if(tableTmSnapShotsignupCount.Equals(0))
                        {
                            lblErrorMessages.Hidden = false;
                            tblProductCommitmentPerDist.Hidden = true;
                        }
                        else
                        {
                            lblErrorMessages.Hidden = true;
                            tblProductCommitmentPerDist.Source = tableTmSnapShotsignupCount;
                            tblProductCommitmentPerDist.ReloadData();
                        }
                    }
                    else
                    {
                        Toast.MakeText(AppResources.ErrorRecord, Constant.durationOfToastMessage).Show();
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.GetProductCommtPerDistributor, AppResources.TmSnapresult, null);
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
                utility.SaveExceptionHandling(ex, AppResources.setLogoSize, AppResources.TmSnapresult, null);
            }
        }
        /// <summary>
        /// GET TOTAL GALLONS/LITERS COMMITED.
        /// </summary>
        /// <returns>The tm snap shot result count data.</returns>
        /// <param name="token">Token.</param>
        public async Task GetTotalGallonPerLiterCommit(string token)
        {
            loadPop = new LoadingOverlay(this.View.Frame);

            try
            {
                this.View.Add(loadPop);
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    tmTotalCommitments = await utility.GetTotalGallonperLtr(token);
                    if (tmTotalCommitments != null)
                    {
                        TableViewTotalGallonPerLiter tableTmSnapShotsignupCount = new TableViewTotalGallonPerLiter(this, tmTotalCommitments);
                        if(tableTmSnapShotsignupCount.Equals(0))
                        {
                            lblErrorMessaget.Hidden = false;
                            tblGallonperLiter.Hidden = true;
                        }
                        else
                        {
                            lblErrorMessaget.Hidden = true;
                            tblGallonperLiter.Source = tableTmSnapShotsignupCount;
                            tblGallonperLiter.ReloadData();
                        }                       
                    }
                    else
                    {
                        Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex,AppResources.GetTotalGallonPerLiterCommit, AppResources.TmSnapresult, null);
            }
            finally
            {
                loadPop.Hide();
            }
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

                    string filePath = await utility.GetTerritoryDataFile(token);
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
                    loadPop.Hide();
                }
            }
            catch (Exception ex)
            {
                if (loadPop != null)
                {
                    loadPop.Hide();
                }
                utility.SaveExceptionHandling(ex, AppResources.GetFilefromServer, AppResources.TmSnapresult, null);  
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
                    string filePath = await utility.GetTerritoryDataFile(token);
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
                utility.SaveExceptionHandling(ex,AppResources.AttachFileFromThePath, AppResources.TmSnapresult, null);   
            }
        }
        /// <summary>
        /// Open the email if app installed in your device otherwise show message app is not found.
        /// </summary>
        public void OpenEmail(string path,string filename)
        {
            try
            {
                if (MFMailComposeViewController.CanSendMail)
                {
                    var to = new string[] {};
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
                utility.SaveExceptionHandling(ex,AppResources.OpenEmail, AppResources.TmSnapresult, null);
            }
        }
        /// <summary>
        /// Buttons the export excel touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnExportExcel_TouchUpInside(UIButton sender)
        {
            GetFilefromServer(token);
        }
        /// <summary>
        /// Buttons the attach email touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnAttachEmail_TouchUpInside(UIButton sender)
        {
            AttachFileFromThePath(token);
        }
        /// <summary>
        /// Set the fonts of UILabel, UITextField and UIButton.
        /// </summary>
        public void SetFonts()
        {
            try
            {
                viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                Utility.SetFontsforHeader(new UILabel[] { lblTerritoryNo, lblTerritory, lblCount,lblCommit, lblDistributorName, lblCourseCount, lblProductName, lblQuantity,lblCountry }, null, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblGetSignUp, lblProductCommitment, lblTotal , lblErrorMessagef , lblErrorMessaget , lblErrorMessages }, new UIButton[] { btnAttachEmail,btnExportExcel }, Constant.lstDigit[9], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblSnapshot }, null, Constant.lstDigit[11], viewWidth);
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetFonts, AppResources.TmSnapresult, null);
            }
        }
    }
}