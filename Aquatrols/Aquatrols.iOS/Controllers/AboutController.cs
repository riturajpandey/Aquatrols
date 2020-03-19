using Aquatrols.iOS.Controllers;
using Aquatrols.iOS.Helper;
using Aquatrols.iOS.Picker;
using Aquatrols.iOS.TablevVewSource;
using Aquatrols.Models;
using CoreGraphics;
using Foundation;
using MessageUI;
using Plugin.GoogleAnalytics;
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
    /// This class file is used for About us page
    /// </summary>
    public partial class AboutController : UIViewController
    {
        private LoadingOverlay loadPop;
        private UIPickerView marketPicker = new UIPickerView();
        private Utility utility = Utility.GetInstance;
        private List<ProductListEntity> productListEntities = null;
        private UserInfoEntity userInfoEntity = null;
        private UIButton btnDone= new UIButton();
        private int productDetailScrollViewHeight, viewHeight, viewWidth;
        private string token, productWebPage, label, sds, productId, visitWebPageLabel, phone, mailId,count;
        public bool isFromTermsConditions = false;
        private string userId = string.Empty;
        MFMailComposeViewController mailController;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.AboutController"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public AboutController (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Views the did load.
        /// </summary>
		public override void ViewDidLoad()
		{
            base.ViewDidLoad();
            //Check exception data if net is not copnnected and stored the exception in nsuserdefault if user redirect the page first hit the exception in previous saved in shared preferences.
            string exception = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.Exception);
            if (!String.IsNullOrEmpty(exception))
            {
                utility.SaveExceptionHandling(null, AppResources.ViewDidLoad, AppResources.About, exception);
            }
            AboutUsScrollView.ContentSize = new CGSize(AboutUsChildView.Bounds.Width, AboutUsChildView.Bounds.Height);
            ScrollViewTerritoryManager.ContentSize = new CGSize(SubViewTerritoryManager.Bounds.Width, SubViewTerritoryManager.Bounds.Height);
            //get user id 
            if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId)))
            {
                userId = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId);
            }
            lblPointsPerUnit.Hidden = true;
            LblFAQClick();
            LblSupportClick();
            HitUserInfAPI(userId);
            FacebookClick();
            RssClick();
            TwitterClick();
            YoutubeClick();
            InstagramClick();
            MenuClick();
            LblLogoutClick();
            ImgMyCartClick();
            UserProfileClick();
            HomeClick();
            BookClick();
            RedeemClick();
            AboutMainViewClick();
            TxtMarketClick();
            TermsAndConditionsClick();
            LblPrivacyPolicyClick();
            ShowCaseImgClick();
            ImgAboutClick();
            ImgProductsClick();
            ImgTerritoryManagerClick();
            LblWTMEmailValueClick();
            LblNCTMEmailValueClick();
            LblSWTMEmailValueClick();
            LblCETMEmailValueClick();
            LblCSTMEmailvalueClick();
            LblNETMEmailValueClick();
            LblSETMEmailValueClick();
            LblCTMEmailValueClick();
            LblWTMMobileValueClick();
            LblNCTMPhoneValueClick();
            LblSWTMPhoneValueClick();
            LblCETMMobileValueClick();
            LblCSTMMobileValueClick();
            LblNETMMobileValueClick();
            LblSETMMobileValueClick();
            LblCTMPhoneValueClick();
            LblCTMMobileValueClick();
            SetUIViewDevice();
            SetFonts();
            AboutClick();
            UserProfileAboutClick();
            UserProfileTMClick();
            ShowCaseView.Hidden = true;
            TerritoryManagerView.Hidden = true;
            lblFPAboutUS.Text = AppResources.FirstPAboutUs;
            lblSPAboutUs.Text = AppResources.SecondPAboutUs;
            txtMarket.Text = AppResources.AllProducts;
            lblDetailProTurf.Text = AppResources.lblDetailsProTurf;
            lblDetailAgriculture.Text = AppResources.lblDetailsAgriculture;
            lblDetailHorticulture.Text=AppResources.lblDetailsHorticulture;
            badgeView.BackgroundColor = UIColor.Red; // set back ground color in badge view 
            badgeView.Layer.CornerRadius = badgeView.Frame.Size.Width/Constant.lstDigit[2]; // set corner radius in badge view 
            badgeView.Layer.BorderWidth = 2f;
            badgeView.Layer.BorderColor = UIColor.White.CGColor;// set border color in badge view 
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    // implement the google analytics.
                    GoogleAnalytics.Current.Config.TrackingId = AppResources.TrackingId;
                    GoogleAnalytics.Current.Config.AppId = AppResources.AppId;
                    GoogleAnalytics.Current.Config.AppName = AppResources.AppName;
                    GoogleAnalytics.Current.Config.AppVersion = (NSString)NSBundle.MainBundle.InfoDictionary[AppResources.AppVersion];
                    GoogleAnalytics.Current.InitTracker();
                    GoogleAnalytics.Current.Tracker.SendView(AppResources.About);
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch(Exception ex)
            {
              utility.SaveExceptionHandling(ex, AppResources.ViewDidLoad, AppResources.About, null); // Exception handling 
            }
        }
        /// <summary>
        /// This override method will call when view will appear.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillAppear(bool animated)
		{
            try
            {
                // implement the deep linking
                bool isDeepLink = NSUserDefaults.StandardUserDefaults.BoolForKey(AppResources.deepLink);
                if (isDeepLink)
                {
                    popupMenuAbout.Hidden = true;
                    tblAboutProductList.Hidden = false;
                    ProductDetailScrollView.Hidden = true;
                    AboutUsView.Hidden = true;
                    firstAboutView.Hidden = true;
                    TerritoryManagerView.Hidden = true;
                    ProductDropdownView.Hidden = false;
                }
                else
                {
                    // get the terms and conditions value
                    bool isTerms = NSUserDefaults.StandardUserDefaults.BoolForKey(AppResources.isTermsConditions);
                    if (isTerms)
                    {
                        popupMenuAbout.Hidden = true;
                        tblAboutProductList.Hidden = true;
                        ProductDropdownView.Hidden = true;
                        ProductDetailScrollView.Hidden = false;
                        AboutUsView.Hidden = true;
                        firstAboutView.Hidden = true;
                        TerritoryManagerView.Hidden = true;
                        NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.isTermsConditions);
                    }
                    else
                    {
                        popupMenuAbout.Hidden = true;
                        tblAboutProductList.Hidden = true;
                        ProductDropdownView.Hidden = true;
                        ProductDetailScrollView.Hidden = true;
                        AboutUsView.Hidden = true;
                        firstAboutView.Hidden = false;
                        TerritoryManagerView.Hidden = true;
                    }
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ViewWillAppear, AppResources.About, null); // exception handling
            }
        }
		/// <summary>
		/// Set the fonts of UILabel,UITextField and UIButton.
		/// </summary>
		public void SetFonts()
        {
            try
            {
                viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                Utility.SetFonts(null, new UILabel[] { lblAbout, lblMarket, }, new UITextField[] { txtMarket }, Constant.lstDigit[9], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblAboutHeader, lblValue, lblMarkets }, new UIButton[] { btnSendEmail, btnContactUs }, Constant.lstDigit[11], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblVisio, lblMissio, lblpurpos, lblProTurf, lblAgriculture, lblHorticulture }, null, Constant.lstDigit[9], viewWidth);
                Utility.SetFonts(null, new UILabel[] { lblHome, lblBook, lblRedeem, lblTerms, lblLogout, lblPrivacy, lblFollow, lblPages, lblDetailProTurf, lblDetailAgriculture, lblDetailHorticulture, lblTM, lblWesternTMAdress, lblSETMAddre, lblCETMAddress, lblCSTMAddress, lblNCTMAddress, lblNETMAddress, lblSETMAddress, lblSWTMAddress, lblCETMAddress2, lblCSTMAddress2, lblNCTMAddress2, lblNETMAddress2, lblSWTMAddress2, lblWesternTMAddress2, lblWTMMobileValue, lblCETMMobileValue, lblCSTMMobileValue, lblNETMMobileValue, lblSETMMobileValue, lblNCTMPhoneValue, lblSWTMPhoneValue, lblWTMEmailValue, lblCETMEmailValue, lblCSTMEmailvalue, lblNCTMEmailValue, lblNETMEmailValue, lblSWTMEmailValue, lblSETMEmailValue, lblCTMAddress, lblCTMAddr, lblCTMPhoneValue, lblCTMMobileValue, lblCTMEmailValue, lblSPAboutUs,lblFaq,lblSupport }, null, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblFullName, lblFullnameAbout,lblFullnameTM }, null, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsItalic(new UILabel[] { lblCourseName, lblPointsPerUnit,lblCourseNameTM,lblCourseNameAbout,lblPointsPerUnitTM,lblPointsPerUnitAbout }, Constant.lstDigit[8], viewWidth);
                Utility.SetFonts(null, new UILabel[] { lblVision, lblMission, lblPurpose, }, null, Constant.lstDigit[7], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblBadge }, null, Constant.lstDigit[6], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblCSTMSale, lblNETMSale, lblSETMSale, lblSWTMSale, lblCETMSales, lblNCTMSales, lblWesternTMSales, lblCETMName, lblCSTMName, lblNCTMName, lblNETMName, lblSETMName, lblSWTMName, lblWesternTMName,  lblNCTMPhone, lblSWTMPhone, lblCETMMobile, lblCSTMMobile, lblNETMMobile, lblSETMMobile, lblWesternTMMobile, lblWTMEmail, lblCETMEmail, lblCSTMEmail, lblNCTMEmail, lblSETMEmail, lblSWTMEmail, lblNETMEMail, lblCanadianName, lblCTMSale, lblCTMPhone, lblCTMEmail, lblCTMMobile }, null, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsSenSerif(new UILabel[] { lblFPAboutUS }, Constant.lstDigit[8], viewWidth); 
            }
            catch(Exception ex)
            {
               utility.SaveExceptionHandling(ex, AppResources.SetFonts, AppResources.About, null); // Exception handling 
            }
        }
        /// <summary>
        /// Product detail view scrolling.
        /// design UI product detail
        /// show data in product detail screen
        /// Set the UI According to device
        /// </summary>
        public void ProductDetailViewScrolling()
        {
            ProductDetailScrollView.PagingEnabled = true;
            ProductDetailScrollView.ScrollsToTop = true;
            ProductDetailScrollView.ScrollEnabled = true;
            int x = Constant.lstDigit[0];
            try
            {
                string device = UIDevice.CurrentDevice.Model;
                if (device == AppResources.iPad)
                {   //loop as per the given number of products in product List.
                    if (productListEntities != null && productListEntities.Count > 0)
                    {
                        for (int i = 0; i < productListEntities.Count; i++)
                        {
                            ProductDetailView = new UIView();
                            ProductDetailView.BackgroundColor = UIColor.White;
                            viewWidth = (int)UIScreen.MainScreen.Bounds.Width;
                            if (viewWidth == (int)DeviceScreenSize.IPadPro)
                            {
                                 ProductDetailScrollView.ContentSize = new CGSize(x + viewWidth, productDetailScrollViewHeight + Constant.lstDigit[12]);
                            }
                            else
                            {
                                ProductDetailScrollView.ContentSize = new CGSize(x + viewWidth, productDetailScrollViewHeight + Constant.lstDigit[15]);
                            }
                            //set the size and position of product detail view as per the width of product detail scroll view.
                            ProductDetailView.Frame = new CGRect(x, Constant.lstDigit[0], viewWidth, ProductDetailScrollView.ContentSize.Height);
                            x = (int)((viewWidth + x));
                            //adding product detail view control to product detail scroll view.
                            ProductDetailScrollView.AddSubview(ProductDetailView);

                            UIImageView imgProduct = new UIImageView();
                            //set the size and position of product UIImageView as per the width of product detail view.
                            imgProduct.Frame = new CGRect(Constant.lstDigit[0], ProductDetailView.Frame.Y + Constant.lstDigit[0], ProductDetailView.Frame.Width, 170);
                            string img = productListEntities[i].productImage;
                            if (!String.IsNullOrEmpty(img))
                            {
                                img = img.Replace(Constant.lstDigitString[7], Constant.lstDigitString[8]);
                                img = img.Replace(" ", AppResources.SpaceManage);
                                var url = new NSUrl(ConfigEntity.baseURL + Constant.lstDigitString[9] + img);
                                var data = NSData.FromUrl(url);
                                imgProduct.Image = UIImage.LoadFromData(data);
                            }
                            //adding product UIImageView control to product detail view.
                            ProductDetailView.AddSubview(imgProduct);

                            UILabel lbldescription = new UILabel();
                            lbldescription.Lines = Constant.lstDigit[0];
                            lbldescription.Font = UIFont.FromName(AppResources.RobotoRegular, Constant.lstDigit[9]);
                            lbldescription.TextAlignment = UITextAlignment.Left;
                            //set the size and position of Description Label as per the width of product detail view.
                            lbldescription.Frame = new CGRect(Constant.lstDigit[6], imgProduct.Frame.Y + imgProduct.Frame.Height, ProductDetailView.Frame.Width - Constant.lstDigit[12], Constant.lstDigit[26]);
                            lbldescription.Text = productListEntities[i].productDescription;
                            lbldescription.LineBreakMode = UILineBreakMode.WordWrap;
                            //adding Description UILabel control to product detail view.
                            ProductDetailView.AddSubview(lbldescription);

                            UIView uv = new UIView();
                            uv.BackgroundColor = UIColor.Black;
                            //set the size and position of UIView as per the width of product detail view.
                            uv.Frame = new CGRect(Constant.lstDigit[6], lbldescription.Frame.Y + lbldescription.Frame.Height, ProductDetailView.Frame.Width - Constant.lstDigit[12], Constant.lstDigit[1]);
                            //adding UIView control to product detail view.
                            ProductDetailView.AddSubview(uv);

                            UILabel lbldirections = new UILabel();
                            lbldirections.Lines = Constant.lstDigit[0];
                            lbldirections.TextAlignment = UITextAlignment.Left;
                            lbldirections.Font = UIFont.FromName(AppResources.RobotoBold, Constant.lstDigit[9]);
                            //set the size and position of Directions Label as per the width of product detail view.
                            lbldirections.Frame = new CGRect(Constant.lstDigit[6], uv.Frame.Y + uv.Frame.Height + Constant.lstDigit[6], ProductDetailView.Frame.Width - Constant.lstDigit[12], Constant.lstDigit[12]);
                            lbldirections.TextColor = UIColor.FromRGB(Constant.lstDigit[0], Constant.lstDigit[18], Constant.lstDigit[21]);
                            lbldirections.Text = AppResources.lblDirections;
                            lbldirections.LineBreakMode = UILineBreakMode.WordWrap;
                            //adding Directions UILabel control to product detail view.
                            ProductDetailView.AddSubview(lbldirections);

                            UILabel lblRates = new UILabel();
                            lblRates.Lines = Constant.lstDigit[0];
                            lblRates.TextAlignment = UITextAlignment.Left;
                            lblRates.Font = UIFont.FromName(AppResources.RobotoRegular, Constant.lstDigit[9]);
                            //set the size and position of Rates Label as per the width of product detail view.
                            lblRates.Frame = new CGRect(Constant.lstDigit[6], lbldirections.Frame.Y + lbldirections.Frame.Height, ProductDetailView.Frame.Width - Constant.lstDigit[12], Constant.lstDigit[17]);
                            lblRates.Text = productListEntities[i].rates;
                            lblRates.LineBreakMode = UILineBreakMode.WordWrap;
                            //adding Rates UILabel control to product detail view.
                            ProductDetailView.AddSubview(lblRates);

                            UIButton btnSds = new UIButton();
                            btnSds.BackgroundColor = UIColor.LightGray;
                            btnSds.SetTitle(AppResources.SDS, UIControlState.Normal);
                            btnSds.SetTitleColor(UIColor.Black, UIControlState.Normal);
                            btnSds.Font = UIFont.FromName(AppResources.RobotoBold, Constant.lstDigit[11]);
                            //set the size and position of SDS Button as per the width of product detail view.
                            btnSds.Frame = new CGRect(Constant.lstDigit[6], lblRates.Frame.Y + lblRates.Frame.Height + Constant.lstDigit[12], (ProductDetailView.Frame.Width - Constant.lstDigit[14]) / Constant.lstDigit[2], Constant.lstDigit[15]);
                            btnSds.Tag = i;
                            btnSds.TouchUpInside += (object sender, System.EventArgs e) =>
                            {
                                int tag = Convert.ToInt32(btnSds.Tag);
                                sds = productListEntities[tag].sds;
                                if (!String.IsNullOrEmpty(sds))
                                {
                                    TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                                    termsConditionWebViewController.pdfPath = sds;
                                    this.NavigationController.PushViewController(termsConditionWebViewController, true);
                                    NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[0], AppResources.PDFValue);
                                }
                                else
                                {
                                    Toast.MakeText(AppResources.PdfPath, Constant.durationOfToastMessage).Show();
                                }
                            };

                            UIButton btnLabel = new UIButton();
                            btnLabel.SetTitle(AppResources.Label, UIControlState.Normal);
                            btnLabel.SetTitleColor(UIColor.White, UIControlState.Normal);
                            btnLabel.BackgroundColor = UIColor.FromRGB(Constant.lstDigit[0], Constant.lstDigit[18], Constant.lstDigit[21]);
                            btnLabel.Font = UIFont.FromName(AppResources.RobotoBold, Constant.lstDigit[11]);
                            //set the size and position of Label button as per the width of product detail view.
                            btnLabel.Frame = new CGRect(btnSds.Frame.Width + Constant.lstDigit[13], lblRates.Frame.Y + lblRates.Frame.Height + Constant.lstDigit[12], (ProductDetailView.Frame.Width - Constant.lstDigit[14]) / Constant.lstDigit[2], Constant.lstDigit[15]);
                            btnLabel.Tag = i;
                            btnLabel.TouchUpInside += (object sender, System.EventArgs e) =>
                            {
                                int tag = Convert.ToInt32(btnLabel.Tag);
                                label = productListEntities[tag].labelPermaLink;
                                if (!String.IsNullOrEmpty(label))
                                {
                                    TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                                    termsConditionWebViewController.pdfPath = label;
                                    this.NavigationController.PushViewController(termsConditionWebViewController, true);
                                    NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[2], AppResources.PDFValue);
                                }
                                else
                                {
                                    Toast.MakeText(AppResources.PdfPath, Constant.durationOfToastMessage).Show();
                                }
                            };

                            UIView uv1 = new UIView();
                            uv1.BackgroundColor = UIColor.Black;
                            //set the size and position of UIView as per the width of product detail view.
                            uv1.Frame = new CGRect(Constant.lstDigit[6], btnLabel.Frame.Y + btnLabel.Frame.Height + Constant.lstDigit[12], ProductDetailView.Frame.Width - Constant.lstDigit[12], Constant.lstDigit[1]);
                            //adding UIView control to product detail view.
                            ProductDetailView.AddSubview(uv1);

                            UIImageView imgProductLogos = new UIImageView();
                            //set the size and position of Product Logo ImageView  as per the width of product detail view.
                            imgProductLogos.Frame = new CGRect(Constant.lstDigit[6], uv1.Frame.Y + uv1.Frame.Height + Constant.lstDigit[12], (ProductDetailView.Frame.Width - Constant.lstDigit[14]) / Constant.lstDigit[2], 70);
                            string imgLogo = productListEntities[i].productLogo;
                            if (!String.IsNullOrEmpty(imgLogo))
                            {
                                imgLogo = imgLogo.Replace(Constant.lstDigitString[7], Constant.lstDigitString[8]);
                                imgLogo = imgLogo.Replace(" ", AppResources.SpaceManage);
                                var urlProductLogo = new NSUrl(ConfigEntity.baseURL + Constant.lstDigitString[9] + imgLogo);
                                var dataProductLogo = NSData.FromUrl(urlProductLogo);
                                imgProductLogos.Image = UIImage.LoadFromData(dataProductLogo);
                            }
                            //adding product Logo UIImageView control to product detail view.
                            ProductDetailView.AddSubview(imgProductLogos);

                            UIButton btnVisit = new UIButton();
                            btnVisit.SetTitle(AppResources.VisitWebpage, UIControlState.Normal);
                            btnVisit.SetTitleColor(UIColor.White, UIControlState.Normal);
                            btnVisit.BackgroundColor = UIColor.FromRGB(Constant.lstDigit[0], Constant.lstDigit[18], Constant.lstDigit[21]);
                            btnVisit.Font = UIFont.FromName(AppResources.RobotoBold, Constant.lstDigit[11]);
                            //set the size and position of Description Visit Web Page Button as per the width of product detail view.
                            btnVisit.Frame = new CGRect(imgProductLogos.Frame.Width + Constant.lstDigit[13], uv1.Frame.Y + uv1.Frame.Height + Constant.lstDigit[13], (ProductDetailView.Frame.Width - Constant.lstDigit[14]) / Constant.lstDigit[2], Constant.lstDigit[15]);
                            btnVisit.Tag = i;
                            btnVisit.TouchUpInside += (object sender, System.EventArgs e) =>
                            {
                                int tag = Convert.ToInt32(btnVisit.Tag);
                                productWebPage = productListEntities[tag].productWebpage;
                                visitWebPageLabel = productListEntities[tag].productName;
                                if (!String.IsNullOrEmpty(productWebPage))
                                {
                                    TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                                    termsConditionWebViewController.pdfPath = productWebPage;
                                    termsConditionWebViewController.visitWebPageLabel = visitWebPageLabel;
                                    this.NavigationController.PushViewController(termsConditionWebViewController, true);
                                    NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[3], AppResources.PDFValue);
                                }
                                else
                                {
                                    Toast.MakeText(AppResources.PdfPath, Constant.durationOfToastMessage).Show();
                                }
                            };
                            //adding SDS, Label,VisitWebPage UIButton control to product detail view.
                            ProductDetailView.AddSubviews(btnVisit, btnLabel, btnSds);
                        }
                        var index = productListEntities.FindIndex(z => z.productId == productId);
                        ProductDetailScrollView.ContentOffset = new CGPoint(ProductDetailView.Frame.Width * index, Constant.lstDigit[0]);

                    }
                }
                else
                {
                    //loop as per the given number of products in product List.
                    if (productListEntities != null && productListEntities.Count > 0)
                    {
                        for (int i = 0; i < productListEntities.Count; i++)
                        {
                            ProductDetailView = new UIView();
                            ProductDetailView.BackgroundColor = UIColor.White;
                            viewWidth = (int)UIScreen.MainScreen.Bounds.Width;
                            if (viewWidth == (int)DeviceScreenSize.IFive)
                            {
                                ProductDetailScrollView.ContentSize = new CGSize(x + viewWidth, productDetailScrollViewHeight + Constant.lstDigit[30]);
                            }
                            else if (viewWidth == (int)DeviceScreenSize.ISix)
                            {
                                if (viewHeight == Constant.lstDigit[31])
                                {
                                    ProductDetailScrollView.ContentSize = new CGSize(x + viewWidth, productDetailScrollViewHeight + Constant.lstDigit[17]);
                                }
                                else
                                {
                                    ProductDetailScrollView.ContentSize = new CGSize(x + viewWidth, productDetailScrollViewHeight + Constant.lstDigit[29]);
                                }
                            }
                            else if (viewWidth == (int)DeviceScreenSize.ISixPlus)
                            {
                                ProductDetailScrollView.ContentSize = new CGSize(x + viewWidth, productDetailScrollViewHeight + Constant.lstDigit[25]);
                            }
                            //set the size and position of product detail view as per the width of product detail scroll view.
                            ProductDetailView.Frame = new CGRect(x, Constant.lstDigit[0], viewWidth, ProductDetailScrollView.ContentSize.Height);
                            x = (int)((viewWidth + x));
                            //adding product detail view control to product detail scroll view.
                            ProductDetailScrollView.AddSubview(ProductDetailView);

                            UIImageView imgProduct = new UIImageView();
                            //set the size and position of product UIImageView as per the width of product detail view.
                            imgProduct.Frame = new CGRect(Constant.lstDigit[0], ProductDetailView.Frame.Y + Constant.lstDigit[0], ProductDetailView.Frame.Width, Constant.lstDigit[35]);
                            string img = productListEntities[i].productImage;
                            if (!String.IsNullOrEmpty(img))
                            {
                                img = img.Replace(Constant.lstDigitString[7], Constant.lstDigitString[8]);
                                img = img.Replace(" ", AppResources.SpaceManage);
                                var url = new NSUrl(ConfigEntity.baseURL + Constant.lstDigitString[9] + img);
                                var data = NSData.FromUrl(url);
                                imgProduct.Image = UIImage.LoadFromData(data);
                            }
                            //adding product UIImageView control to product detail view.
                            ProductDetailView.AddSubview(imgProduct);

                            UILabel lbldescription = new UILabel();
                            lbldescription.Lines = Constant.lstDigit[0];
                            lbldescription.Font = UIFont.FromName(AppResources.RobotoRegular, Constant.lstDigit[8]);
                            lbldescription.TextAlignment = UITextAlignment.Left;
                            //set the size and position of Description Label as per the width of product detail view.
                            lbldescription.Frame = new CGRect(Constant.lstDigit[6], imgProduct.Frame.Y + imgProduct.Frame.Height, ProductDetailView.Frame.Width - Constant.lstDigit[12], Constant.lstDigit[26]);
                            lbldescription.Text = productListEntities[i].productDescription;
                            lbldescription.LineBreakMode = UILineBreakMode.WordWrap;
                            //adding Description UILabel control to product detail view.
                            ProductDetailView.AddSubview(lbldescription);

                            UIView uv = new UIView();
                            uv.BackgroundColor = UIColor.Black;
                            //set the size and position of UIView as per the width of product detail view.
                            uv.Frame = new CGRect(Constant.lstDigit[6], lbldescription.Frame.Y + lbldescription.Frame.Height, ProductDetailView.Frame.Width - Constant.lstDigit[12], Constant.lstDigit[1]);
                            //adding UIView control to product detail view.
                            ProductDetailView.AddSubview(uv);

                            UILabel lbldirections = new UILabel();
                            lbldirections.Lines = Constant.lstDigit[0];
                            lbldirections.TextAlignment = UITextAlignment.Left;
                            lbldirections.Font = UIFont.FromName(AppResources.RobotoBold, Constant.lstDigit[8]);
                            //set the size and position of Directions Label as per the width of product detail view.
                            lbldirections.Frame = new CGRect(Constant.lstDigit[6], uv.Frame.Y + uv.Frame.Height + Constant.lstDigit[6], ProductDetailView.Frame.Width - Constant.lstDigit[12], Constant.lstDigit[12]);
                            lbldirections.TextColor = UIColor.FromRGB(Constant.lstDigit[0], Constant.lstDigit[18], Constant.lstDigit[21]);
                            lbldirections.Text = AppResources.lblDirections;
                            lbldirections.LineBreakMode = UILineBreakMode.WordWrap;
                            //adding Directions UILabel control to product detail view.
                            ProductDetailView.AddSubview(lbldirections);

                            UILabel lblRates = new UILabel();
                            lblRates.Lines = Constant.lstDigit[0];
                            lblRates.TextAlignment = UITextAlignment.Left;
                            lblRates.Font = UIFont.FromName(AppResources.RobotoRegular, Constant.lstDigit[8]);
                            //set the size and position of Rates Label as per the width of product detail view.
                            lblRates.Frame = new CGRect(Constant.lstDigit[6], lbldirections.Frame.Y + lbldirections.Frame.Height, ProductDetailView.Frame.Width - Constant.lstDigit[12], Constant.lstDigit[17]);
                            lblRates.Text = productListEntities[i].rates;
                            lblRates.LineBreakMode = UILineBreakMode.WordWrap;
                            //adding Rates UILabel control to product detail view.
                            ProductDetailView.AddSubview(lblRates);

                            UIButton btnSds = new UIButton();
                            btnSds.BackgroundColor = UIColor.LightGray;
                            btnSds.SetTitle(AppResources.SDS, UIControlState.Normal);
                            btnSds.SetTitleColor(UIColor.Black, UIControlState.Normal);
                            btnSds.Font = UIFont.FromName(AppResources.RobotoBold, Constant.lstDigit[11]);
                            //set the size and position of SDS Button as per the width of product detail view.
                            btnSds.Frame = new CGRect(Constant.lstDigit[6], lblRates.Frame.Y + lblRates.Frame.Height + Constant.lstDigit[12], (ProductDetailView.Frame.Width - Constant.lstDigit[14]) / Constant.lstDigit[2], Constant.lstDigit[13]);
                            btnSds.Tag = i;
                            btnSds.TouchUpInside += (object sender, System.EventArgs e) =>
                            {
                                int tag = Convert.ToInt32(btnSds.Tag);
                                sds = productListEntities[tag].sds;
                                if (!String.IsNullOrEmpty(sds))
                                {
                                    TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                                    termsConditionWebViewController.pdfPath = sds;
                                    this.NavigationController.PushViewController(termsConditionWebViewController, true);
                                    NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[0], AppResources.PDFValue);
                                }
                                else
                                {
                                    Toast.MakeText(AppResources.PdfPath, Constant.durationOfToastMessage).Show();
                                }
                            };

                            UIButton btnLabel = new UIButton();
                            btnLabel.SetTitle(AppResources.Label, UIControlState.Normal);
                            btnLabel.SetTitleColor(UIColor.White, UIControlState.Normal);
                            btnLabel.BackgroundColor = UIColor.FromRGB(Constant.lstDigit[0], Constant.lstDigit[18], Constant.lstDigit[21]);
                            btnLabel.Font = UIFont.FromName(AppResources.RobotoBold, Constant.lstDigit[11]);
                            //set the size and position of Label button as per the width of product detail view.
                            btnLabel.Frame = new CGRect(btnSds.Frame.Width + Constant.lstDigit[13], lblRates.Frame.Y + lblRates.Frame.Height + Constant.lstDigit[12], (ProductDetailView.Frame.Width - Constant.lstDigit[14]) / Constant.lstDigit[2], Constant.lstDigit[13]);
                            btnLabel.Tag = i;
                            btnLabel.TouchUpInside += (object sender, System.EventArgs e) =>
                            {
                                int tag = Convert.ToInt32(btnLabel.Tag);
                                label = productListEntities[tag].labelPermaLink;
                                if (!String.IsNullOrEmpty(label))
                                {
                                    TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                                    termsConditionWebViewController.pdfPath = label;
                                    this.NavigationController.PushViewController(termsConditionWebViewController, true);
                                    NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[2], AppResources.PDFValue);
                                }
                                else
                                {
                                    Toast.MakeText(AppResources.PdfPath, Constant.durationOfToastMessage).Show();
                                }
                            };

                            UIView uv1 = new UIView();
                            uv1.BackgroundColor = UIColor.Black;
                            //set the size and position of UIView as per the width of product detail view.
                            uv1.Frame = new CGRect(Constant.lstDigit[6], btnLabel.Frame.Y + btnLabel.Frame.Height + Constant.lstDigit[12], ProductDetailView.Frame.Width - Constant.lstDigit[12], Constant.lstDigit[1]);
                            //adding UIView control to product detail view.
                            ProductDetailView.AddSubview(uv1);

                            UIImageView imgProductLogos = new UIImageView();
                            //set the size and position of Product Logo ImageView  as per the width of product detail view.
                            imgProductLogos.Frame = new CGRect(Constant.lstDigit[6], uv1.Frame.Y + uv1.Frame.Height + Constant.lstDigit[12], (ProductDetailView.Frame.Width - Constant.lstDigit[14]) / Constant.lstDigit[2], Constant.lstDigit[36]);
                            string imgLogo = productListEntities[i].productLogo;
                            if (!String.IsNullOrEmpty(imgLogo))
                            {
                                imgLogo = imgLogo.Replace(Constant.lstDigitString[7], Constant.lstDigitString[8]);
                                imgLogo = imgLogo.Replace(" ", AppResources.SpaceManage);
                                var urlProductLogo = new NSUrl(ConfigEntity.baseURL + Constant.lstDigitString[9] + imgLogo);
                                var dataProductLogo = NSData.FromUrl(urlProductLogo);
                                imgProductLogos.Image = UIImage.LoadFromData(dataProductLogo);
                            }
                            //adding product Logo UIImageView control to product detail view.
                            ProductDetailView.AddSubview(imgProductLogos);

                            UIButton btnVisit = new UIButton();
                            btnVisit.SetTitle(AppResources.VisitWebpage, UIControlState.Normal);
                            btnVisit.SetTitleColor(UIColor.White, UIControlState.Normal);
                            btnVisit.BackgroundColor = UIColor.FromRGB(Constant.lstDigit[0], Constant.lstDigit[18], Constant.lstDigit[21]);
                            btnVisit.Font = UIFont.FromName(AppResources.RobotoBold, Constant.lstDigit[11]);
                            //set the size and position of Description Visit Web Page Button as per the width of product detail view.
                            btnVisit.Frame = new CGRect(imgProductLogos.Frame.Width + Constant.lstDigit[13], uv1.Frame.Y + uv1.Frame.Height + Constant.lstDigit[13], (ProductDetailView.Frame.Width - Constant.lstDigit[14]) / Constant.lstDigit[2], Constant.lstDigit[13]);
                            btnVisit.Tag = i;
                            btnVisit.TouchUpInside += (object sender, System.EventArgs e) =>
                              {
                                  int tag = Convert.ToInt32(btnVisit.Tag);
                                  productWebPage = productListEntities[tag].productWebpage;
                                  visitWebPageLabel = productListEntities[tag].productName;
                                  if (!String.IsNullOrEmpty(productWebPage))
                                  {
                                      TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                                      termsConditionWebViewController.pdfPath = productWebPage;
                                      termsConditionWebViewController.visitWebPageLabel = visitWebPageLabel;
                                      this.NavigationController.PushViewController(termsConditionWebViewController, true);
                                      NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[3], AppResources.PDFValue);
                                  }
                                  else
                                  {
                                      Toast.MakeText(AppResources.PdfPath, Constant.durationOfToastMessage).Show();
                                  }
                              };
                            //adding SDS, Label,VisitWebPage UIButton control to product detail view.
                            ProductDetailView.AddSubviews(btnVisit, btnLabel, btnSds);
                        }
                        var index = productListEntities.FindIndex(z => z.productId == productId);
                        ProductDetailScrollView.ContentOffset = new CGPoint(ProductDetailView.Frame.Width * index, Constant.lstDigit[0]);

                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ProductDetailViewScrolling, AppResources.About, null); // exception handling 
            }
        }
        /// <summary>
        /// Set the UI View According to device.
        /// </summary>
        public void SetUIViewDevice()
        {
            try
            {
                viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                viewHeight = (int)UIScreen.MainScreen.Bounds.Size.Height;
                productDetailScrollViewHeight = (int)(viewHeight - UserProfieVIew.Frame.Height - BottomView.Frame.Height);
                SetLogoSize();
            }  
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.OpenEmail, AppResources.About, null);
            }
}
        /// <summary>
        /// Set Logo size sepcified in device .
        /// Set UI in according to the device.
        /// </summary>
        public void SetLogoSize()
        {
            try
            {
                string device = UIDevice.CurrentDevice.Model;
                if (device == AppResources.iPad)
                {
                    viewWidth = (int)UIScreen.MainScreen.Bounds.Width;
                    int deviceHeight = (int)UIScreen.MainScreen.Bounds.Height;
                    if (viewWidth == (int)DeviceScreenSize.IPadPro)
                    {
                        badgeView.Frame = new CGRect(badgeView.Frame.X, badgeView.Frame.Y, Constant.lstDigit[14], Constant.lstDigit[14]);
                        badgeView.Layer.CornerRadius = badgeView.Frame.Size.Width / Constant.lstDigit[2];
                        badgeView.Layer.BorderWidth = 2f;
                        logoAbout.Frame = new CGRect(logoAbout.Frame.X, logoAbout.Frame.Y, logoAbout.Frame.Width - Constant.digitNinteenPointsFive, logoAbout.Frame.Height + Constant.digitEighteenPointsFive);

                    }
                    else
                    {
                        badgeView.Frame = new CGRect(badgeView.Frame.X, badgeView.Frame.Y, Constant.lstDigit[13], Constant.lstDigit[13]);
                        badgeView.Layer.CornerRadius = badgeView.Frame.Size.Width / Constant.lstDigit[2];
                        badgeView.Layer.BorderWidth = 2f;
                        logoAbout.Frame = new CGRect(logoAbout.Frame.X, logoAbout.Frame.Y, logoAbout.Frame.Width - Constant.digitTwelvePointsFive, logoAbout.Frame.Height + Constant.digitElevenPointsFive);
                    }
                }
                else
                {
                    if (viewWidth == (int)DeviceScreenSize.ISix)
                    {
                        if (viewHeight == Constant.lstDigit[31])
                        {
                            logoAbout.Frame = new CGRect(Constant.lstDigit[6], Constant.lstDigit[41], Constant.lstDigit[42], Constant.lstDigit[43]);
                            badgeView.Frame = new CGRect(Constant.lstDigit[45], Constant.lstDigit[9], Constant.lstDigit[44], Constant.lstDigit[44]);
                            badgeView.Layer.CornerRadius = badgeView.Frame.Size.Width / Constant.lstDigit[2];
                            badgeView.Layer.BorderWidth = 2f;
                            UserProfieVIew.Frame = new CGRect(UserProfieVIew.Frame.X, UserProfieVIew.Frame.Y + Constant.lstDigit[10], UserProfieVIew.Frame.Width, UserProfieVIew.Frame.Height);
                            firstAboutView.Frame = new CGRect(firstAboutView.Frame.X, UserProfieVIew.Frame.Y + UserProfieVIew.Frame.Height, firstAboutView.Frame.Width, firstAboutView.Frame.Height - Constant.lstDigit[10]);
                            popupMenuAbout.Frame = new CGRect(popupMenuAbout.Frame.X, popupMenuAbout.Frame.Y + Constant.lstDigit[10], popupMenuAbout.Frame.Width, popupMenuAbout.Frame.Height);
                            ProductDropdownView.Frame = new CGRect(ProductDropdownView.Frame.X, UserProfieVIew.Frame.Y + UserProfieVIew.Frame.Height, ProductDropdownView.Frame.Width, ProductDropdownView.Frame.Height);
                            tblAboutProductList.Frame = new CGRect(tblAboutProductList.Frame.X, ProductDropdownView.Frame.Y + ProductDropdownView.Frame.Height, tblAboutProductList.Frame.Width, tblAboutProductList.Frame.Height - Constant.lstDigit[10]);
                            ProductDetailScrollView.Frame = new CGRect(ProductDetailScrollView.Frame.X, UserProfieVIew.Frame.Y + UserProfieVIew.Frame.Height, ProductDetailScrollView.Frame.Width, ProductDetailScrollView.Frame.Height - Constant.lstDigit[10]);
                            AboutUsView.Frame = new CGRect(AboutUsView.Frame.X, headerView.Frame.Y + headerView.Frame.Height + Constant.digitFourtyFive, AboutUsView.Frame.Width, AboutUsView.Frame.Height - Constant.lstDigit[10]);
                            AboutUsScrollView.ContentSize = new CGSize(AboutUsChildView.Bounds.Width, AboutUsChildView.Bounds.Height);
                            TerritoryManagerView.Frame = new CGRect(TerritoryManagerView.Frame.X, headerView.Frame.Y + headerView.Frame.Height + Constant.digitFourtyFive, TerritoryManagerView.Frame.Width, TerritoryManagerView.Frame.Height - Constant.lstDigit[10]);
                            ScrollViewTerritoryManager.ContentSize = new CGSize(SubViewTerritoryManager.Bounds.Width, SubViewTerritoryManager.Bounds.Height);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex,AppResources.setLogoSize, AppResources.About, null); // exception handling
            }
        }
        /// <summary>
        /// Hit the wishlist item count API.
        /// </summary>
        public async void HitWishListItemCountAPI()
        {
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    // get the token
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                    count = await utility.GetWishListItemCount(token); // call the wishlist item API
                    if (count != null)
                    {
                        HitProductListAPI();
                        if (count == Constant.lstDigitString[5])
                        {
                            badgeView.Hidden = true;
                        }
                        else
                        {
                            badgeView.Hidden = false;
                            lblBadge.Text = count;
                        }
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error).Show(); // show the toast message
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.HitWishListItemCountAPI, AppResources.About, null); // exception handling 
            }
        }
        /// <summary>
        /// Hit the user information API.
        /// set the value of fullname,course name and balance points
        /// </summary>
        /// <param name="userId">User identifier.</param>
        protected async void HitUserInfAPI(string userId)
        {
            loadPop = new LoadingOverlay(this.View.Frame);
            this.View.Add(loadPop); // show the loader 
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                    userInfoEntity = await utility.GetUserInfo(userId, token);// get the user information through hit the user info API.
                    if (!string.IsNullOrEmpty(userInfoEntity.userId))
                    {
                        HitWishListItemCountAPI();
                        lblFullName.Text = userInfoEntity.firstName + " " + userInfoEntity.lastName;
                        lblCourseName.Text = userInfoEntity.courseName;
                        int balance = userInfoEntity.balancedPoints;
                        string formatted = balance.ToString(AppResources.Comma);
                        lblPointsPerUnit.Text = "";//formatted + AppResources.PointsAvailable;
                        lblFullnameAbout.Text = userInfoEntity.firstName + " " + userInfoEntity.lastName;
                        lblCourseNameAbout.Text = userInfoEntity.courseName;
                        lblPointsPerUnitAbout.Text = "";//formatted + AppResources.PointsAvailable;
                        lblFullnameTM.Text = userInfoEntity.firstName + " " + userInfoEntity.lastName;
                        lblCourseNameTM.Text = userInfoEntity.courseName;
                        lblPointsPerUnitTM.Text = "";//formatted + AppResources.PointsAvailable;
                        NSUserDefaults.StandardUserDefaults.SetString(userInfoEntity.isEmailPreference, AppResources.isEmailPreference);
                        NSUserDefaults.StandardUserDefaults.SetString(userInfoEntity.isNotification, AppResources.isNotification);
                        NSUserDefaults.StandardUserDefaults.SetString(userInfoEntity.role, AppResources.role);
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show(); // show the toast message 
                }
            }
            catch (Exception ex)
            {
                if (loadPop!=null) 
                {
                    loadPop.Hide(); // hide the loader
                }
                utility.SaveExceptionHandling(ex, AppResources.HitUserInfAPI, AppResources.About, null); // exception handling
            }
        }
        /// <summary>
        /// Hit product list API.
        /// show product list based on the country of the user
        /// </summary>
        protected async void HitProductListAPI()
        {
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                    productListEntities = await utility.GetProductList(token); // get the product detail hit the product list api.
                    if(productListEntities!=null)
                    {
                        bool isDeepLink = NSUserDefaults.StandardUserDefaults.BoolForKey(AppResources.deepLink);
                        if (isDeepLink)
                        {
                            List<ProductListEntity> lstproductList = productListEntities.OrderBy(x => x.productName).ToList();
                            List<NSData> lstNSData = new List<NSData>();
                            NSData nsDataProductLogo = new NSData();
                            for (int i = 0; i < lstproductList.Count; i++)
                            {
                                string imgLogo = lstproductList[i].productLogo;
                                if (!String.IsNullOrEmpty(imgLogo))
                                {
                                    imgLogo = imgLogo.Replace(Constant.lstDigitString[7], Constant.lstDigitString[8]);
                                    imgLogo = imgLogo.Replace(" ", AppResources.SpaceManage);
                                    var urlProductLogo = new NSUrl(ConfigEntity.baseURL + Constant.lstDigitString[9] + imgLogo);
                                    nsDataProductLogo = NSData.FromUrl(urlProductLogo);
                                }
                                lstNSData.Add(nsDataProductLogo);
                            }
                            TableViewAboutProductList tableViewAboutProductList = new TableViewAboutProductList(this, productListEntities.OrderBy(x => x.productName).ToList(), lstNSData);
                            tblAboutProductList.Source = tableViewAboutProductList;
                            tblAboutProductList.ReloadData();
                            tblAboutProductList.Hidden = false;
                            ProductDropdownView.Hidden = false;
                            firstAboutView.Hidden = true;
                            marketPicker.Hidden = true;
                            btnDone.Hidden = true; 
                            NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.deepLink); 
                        }
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();// show the toast message 
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.HitProductListAPI, AppResources.About, null); // exception handling 
            }
            finally
            {
                loadPop.Hide(); // hide the loader 
            }
        }
        /// <summary>
        /// Set the all products list in Table View Source on about product list on click of AllProductList button.
        /// </summary>
        public async void AllProductsList()
        {
            loadPop = new LoadingOverlay(View.Frame);
            this.View.Add(loadPop);
            await Task.Delay(Constant.lstDigit[17]);
            try
            {
                if (productListEntities != null && productListEntities.Count>0 )
                {
                    if ((txtMarket.Text).Equals(Constant.lstMarkets[1]))
                    {
                        List<ProductListEntity> tempProductLists = productListEntities.OrderBy(x=>x.productName).ToList();
                        tempProductLists = tempProductLists.Where(x => x.categoryName.ToLower().Trim() == Constant.lstMarkets[1].ToLower()).ToList();
                        if (tempProductLists.Count > Constant.lstDigit[0])
                        {
                            List<NSData> lstNSData = new List<NSData>();
                            NSData nsDataProductLogo = new NSData();
                            for (int i = 0; i < tempProductLists.Count; i++)
                            {
                                string imgLogo = tempProductLists[i].productLogo;
                                if (!String.IsNullOrEmpty(imgLogo))
                                {
                                    imgLogo = imgLogo.Replace(Constant.lstDigitString[7], Constant.lstDigitString[8]);
                                    imgLogo = imgLogo.Replace(" ", AppResources.SpaceManage);
                                    var urlProductLogo = new NSUrl(ConfigEntity.baseURL + Constant.lstDigitString[9] + imgLogo);
                                    nsDataProductLogo = NSData.FromUrl(urlProductLogo);
                                }
                                lstNSData.Add(nsDataProductLogo);
                            }
                            TableViewAboutProductList tableViewAboutProductList = new TableViewAboutProductList(this, tempProductLists,lstNSData);
                            tblAboutProductList.Source = tableViewAboutProductList;
                            tblAboutProductList.ReloadData();
                            tblAboutProductList.Hidden = false;
                            marketPicker.Hidden = true;
                            btnDone.Hidden = true;
                        }
                        else
                        {
                            tblAboutProductList.Hidden = true;
                            Toast.MakeText(AppResources.NoData, Constant.durationOfToastMessage).Show();
                            marketPicker.Hidden = true;
                            btnDone.Hidden = true;
                        }
                    }
                    else if ((txtMarket.Text).Equals(Constant.lstMarkets[2]))
                    {
                        List<ProductListEntity> tempProductLists = productListEntities.OrderBy(x => x.productName).ToList();
                        tempProductLists = tempProductLists.Where(x => x.categoryName.ToLower().Trim() == Constant.lstMarkets[2].ToLower()).ToList();
                        if (tempProductLists.Count > Constant.lstDigit[0])
                        {
                            List<NSData> lstNSData = new List<NSData>();
                            NSData nsDataProductLogo = new NSData();
                            for (int i = 0; i < tempProductLists.Count; i++)
                            {
                                string imgLogo = tempProductLists[i].productLogo;
                                if (!String.IsNullOrEmpty(imgLogo))
                                {
                                    imgLogo = imgLogo.Replace(Constant.lstDigitString[7], Constant.lstDigitString[8]);
                                    imgLogo = imgLogo.Replace(" ", AppResources.SpaceManage);
                                    var urlProductLogo = new NSUrl(ConfigEntity.baseURL + Constant.lstDigitString[9] + imgLogo);
                                    nsDataProductLogo = NSData.FromUrl(urlProductLogo);
                                }
                                lstNSData.Add(nsDataProductLogo);
                            }
                            TableViewAboutProductList tableViewAboutProductList = new TableViewAboutProductList(this, tempProductLists,lstNSData);
                            tblAboutProductList.Source = tableViewAboutProductList;
                            tblAboutProductList.ReloadData();
                            tblAboutProductList.Hidden = false;
                            marketPicker.Hidden = true;
                            btnDone.Hidden = true;
                        }
                        else
                        {
                            tblAboutProductList.Hidden = true;
                            Toast.MakeText(AppResources.NoData, Constant.durationOfToastMessage).Show();
                            marketPicker.Hidden = true;
                            btnDone.Hidden = true;
                        }
                    }
                    else if ((txtMarket.Text).Equals(Constant.lstMarkets[3]))
                    {
                        List<ProductListEntity> tempProductLists = productListEntities.OrderBy(x => x.productName).ToList();
                        tempProductLists = tempProductLists.Where(x => x.categoryName.ToLower().Trim() == Constant.lstMarkets[3].ToLower()).ToList();
                        if (tempProductLists.Count > Constant.lstDigit[0])
                        {
                            List<NSData> lstNSData = new List<NSData>();
                            NSData nsDataProductLogo = new NSData();
                            for (int i = 0; i < tempProductLists.Count; i++)
                            {
                                string imgLogo = tempProductLists[i].productLogo;
                                if (!String.IsNullOrEmpty(imgLogo))
                                {
                                    imgLogo = imgLogo.Replace(Constant.lstDigitString[7], Constant.lstDigitString[8]);
                                    imgLogo = imgLogo.Replace(" ", AppResources.SpaceManage);
                                    var urlProductLogo = new NSUrl(ConfigEntity.baseURL + Constant.lstDigitString[9] + imgLogo);
                                    nsDataProductLogo = NSData.FromUrl(urlProductLogo);
                                }
                                lstNSData.Add(nsDataProductLogo);
                            }
                            TableViewAboutProductList tableViewAboutProductList= new TableViewAboutProductList(this, tempProductLists,lstNSData);
                            tblAboutProductList.Source = tableViewAboutProductList;
                            tblAboutProductList.ReloadData();
                            tblAboutProductList.Hidden = false;
                            marketPicker.Hidden = true;
                            btnDone.Hidden = true;
                        }
                        else
                        {
                            tblAboutProductList.Hidden = true;
                            Toast.MakeText(AppResources.NoData, Constant.durationOfToastMessage).Show();
                            marketPicker.Hidden = true;
                            btnDone.Hidden = true;
                        }
                    }
                    else
                    {
                        List<ProductListEntity> lstproductList= productListEntities.OrderBy(x => x.productName).ToList();
                        List<NSData> lstNSData = new List<NSData>();
                        NSData nsDataProductLogo = new NSData();
                        for (int i = 0; i < lstproductList.Count; i++)
                        {
                            string imgLogo = lstproductList[i].productLogo;
                            if (!String.IsNullOrEmpty(imgLogo))
                            {
                                imgLogo = imgLogo.Replace(Constant.lstDigitString[7], Constant.lstDigitString[8]);
                                imgLogo = imgLogo.Replace(" ", AppResources.SpaceManage);
                                var urlProductLogo = new NSUrl(ConfigEntity.baseURL + Constant.lstDigitString[9] + imgLogo);
                                nsDataProductLogo = NSData.FromUrl(urlProductLogo);
                            }
                            lstNSData.Add(nsDataProductLogo);
                        }
                        TableViewAboutProductList tableViewAboutProductList = new TableViewAboutProductList(this, productListEntities.OrderBy(x => x.productName).ToList(),lstNSData);
                        tblAboutProductList.Source = tableViewAboutProductList;
                        tblAboutProductList.ReloadData();
                        tblAboutProductList.Hidden = false;
                        ProductDropdownView.Hidden = false;
                        firstAboutView.Hidden = true;
                        marketPicker.Hidden = true;
                        btnDone.Hidden = true;
                    }
                }
                else
                {
                    tblAboutProductList.Hidden = true;
                    ProductDropdownView.Hidden = false;
                    firstAboutView.Hidden = true;
                    marketPicker.Hidden = true;
                    btnDone.Hidden = true;
                    Toast.MakeText(AppResources.RequireProductList, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.AllProductsList, AppResources.About, null); // exception handling 
            }
            finally
            {
                loadPop.Hide(); // hide the loader 
            }
        }
        /// <summary>
        /// click the product logo and to find the product details.
        /// If user login first time then show ShowCase to display instruction to move the UI.
        /// Hide or show UI View depends on user requirement.
        /// </summary>
        public async void ProductDetail(string productid)
        {
            loadPop = new LoadingOverlay(View.Frame);
            this.View.Add(loadPop);
            await Task.Delay(Constant.lstDigit[17]);
            try
            {
                var showcase = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.ShowCaseAbout);
                if (String.IsNullOrEmpty(showcase))
                {
                    ShowCaseView.Hidden = false;
                    NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[0], AppResources.ShowCaseAbout);
                }
                else
                {
                    ShowCaseView.Hidden = true;
                } 
                productId = productid;
                ProductDetailScrollView.Hidden = false;
                tblAboutProductList.Hidden = true;
                ProductDropdownView.Hidden = true;
                firstAboutView.Hidden = true;
                AboutUsView.Hidden = true;
                ProductDetailViewScrolling();  
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ProductDetail, AppResources.About, null);
            }
            finally
            {
                loadPop.Hide();
            }
        }
        /// <summary>
        /// Button the send email touch up inside.
        /// To open email app installed in your device
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void btnSendEmail_TouchUpInside(UIButton sender)
        {
            string mailId = AppResources.ToMail;
            OpenEmail(mailId);
        }
        /// <summary>
        /// Button the contact us touch up inside.
        /// To open email app installed in your device
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void btnContactUs_TouchUpInside(UIButton sender)
        { 
            string mailId = AppResources.ToMail;
            OpenEmail(mailId);
        }
        /// <summary>
        /// Click on UITextField then show UIPicker to display the list of market.
        /// </summary>
        public void TxtMarketClick()
        {
            UITapGestureRecognizer txtMarketClicked = new UITapGestureRecognizer(() =>
            {
                MarketPicker();
            });
            txtMarket.UserInteractionEnabled = true;
            txtMarket.AddGestureRecognizer(txtMarketClicked);
        }
        /// <summary>
        /// show UIPicker.
        /// bind and show the list of market in UIPicker
        /// </summary>
        public void MarketPicker()
        {
            try
            {
                marketPicker = new UIPickerView(new CGRect(Constant.lstDigit[0], UIScreen.MainScreen.Bounds.Height - Constant.lstDigit[23], UIScreen.MainScreen.Bounds.Width, Constant.lstDigit[23]));
                marketPicker.BackgroundColor = UIColor.White;
                marketPicker.ShowSelectionIndicator = true;
                btnDone = new UIButton(new CGRect(marketPicker.Frame.X, marketPicker.Frame.Y - Constant.lstDigit[15], UIScreen.MainScreen.Bounds.Width, Constant.lstDigit[15]));
                btnDone.BackgroundColor = UIColor.Gray;
                btnDone.SetTitle(AppResources.Done, UIControlState.Normal);
                var picker = new MarketModel(Constant.lstMarkets);
                marketPicker.Model = picker;
                View.AddSubviews(marketPicker, btnDone);
                picker.ValueChanged += (s, e) =>
                {
                    txtMarket.Text = picker.SelectedValue;
                    AllProductsList();
                };
                btnDone.TouchUpInside += (s, e) =>
                {
                    marketPicker.Hidden = true;
                    btnDone.Hidden = true;
                };
                View.Add(marketPicker); 
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.MarketPicker, AppResources.About, null);
            }
        }
        /// <summary>
        /// Open the email if app installed in your device otherwise show message app is not found.
        /// </summary>
        /// <param name="mailId">Mail identifier.</param>
        public void OpenEmail(string mailId)
        {
            try
            {
                if (MFMailComposeViewController.CanSendMail)
                {
                    var to = new string[] { mailId };
                    if (MFMailComposeViewController.CanSendMail)
                    {
                        mailController = new MFMailComposeViewController();
                        mailController.SetToRecipients(to);
                        mailController.SetSubject("");
                        mailController.SetMessageBody("", false);
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
                utility.SaveExceptionHandling(ex,AppResources.OpenEmail, AppResources.About, null);
            }
        }
        /// <summary>
        /// Make call to click the phone number.
        /// </summary>
        /// <param name="phone">Phone.</param>
        public void OpenCall(string phone)
        {
            try
            {
                string phone1 = phone.Replace("(", String.Empty);
                string phone2 = phone1.Replace(")", "-");
                string phone3 = phone2.Replace(" ", String.Empty);
                phone = phone3;
                // Create a NSUrl 
                var url = new NSUrl(AppResources.Tel + phone);
                // Use URL handler with tel: prefix to invoke Apple's Phone app, 
                // otherwise show an alert dialog 
                if (!UIApplication.SharedApplication.OpenUrl(url))
                {
                    Utility.DebugAlert(AppResources.NotSupported, AppResources.MessageTelephone);
                }; 
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.OpenCall, AppResources.About, null);
            }
        }
        /// <summary>
        /// click the about Aquatrols image to see the detail description of about Aquatrols.
        /// </summary>
        public void ImgAboutClick()
        {
            UITapGestureRecognizer imgAboutClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuAbout.Hidden = true;
                firstAboutView.Hidden = true;
                AboutUsView.Hidden = false;
                lblVision.Text = AppResources.RequireVision;
                lblMission.Text = AppResources.RequireMission;
                lblPurpose.Text = AppResources.RequirePurpose;
            });
            imgAbout.UserInteractionEnabled = true;
            imgAbout.AddGestureRecognizer(imgAboutClicked);
        }
        /// <summary>
        /// Click the Products image to find the products details
        /// </summary>
        public void ImgProductsClick()
        {
            UITapGestureRecognizer imgProductsClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuAbout.Hidden = true;
                AllProductsList();
            });
            imgProducts.UserInteractionEnabled = true;
            imgProducts.AddGestureRecognizer(imgProductsClicked);
        }
        /// <summary>
        /// click the territory managers to find the details of all territory managers.
        /// </summary>
        public void ImgTerritoryManagerClick()
        {
            UITapGestureRecognizer imgTerritoryManagerClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuAbout.Hidden = true;
                firstAboutView.Hidden = true;
                TerritoryManagerView.Hidden = false;
            });
            imgTerritoryManager.UserInteractionEnabled = true;
            imgTerritoryManager.AddGestureRecognizer(imgTerritoryManagerClicked);
        }
        /// <summary>
        /// User profile click to show detail of user in my account screen.
        /// hide the popup menu of about screen.
        /// </summary>
        public void UserProfileClick()
        {
            UITapGestureRecognizer userProfileClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuAbout.Hidden = true;
                this.PerformSegue(AppResources.SegueFromAboutToMyAccount, this);
            });
            UserInfoView.UserInteractionEnabled = true;
            UserInfoView.AddGestureRecognizer(userProfileClicked);
        }
        /// <summary>
        /// User profile click to show detail of user in my account screen.
        /// hide the popup menu of about screen.
        /// </summary>
        public void UserProfileAboutClick()
        {
            UITapGestureRecognizer userProfileClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuAbout.Hidden = true;
                this.PerformSegue(AppResources.SegueFromAboutToMyAccount, this);
            });
            UserInfoViewAbout.UserInteractionEnabled = true;
            UserInfoViewAbout.AddGestureRecognizer(userProfileClicked);
        }
        /// <summary>
        /// User profile click to show detail of user in my account screen.
        /// hide the popup menu of about screen.
        /// </summary>
        public void UserProfileTMClick()
        {
            UITapGestureRecognizer userProfileClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuAbout.Hidden = true;
                this.PerformSegue(AppResources.SegueFromAboutToMyAccount, this);
            });
            UserInfoViewTM.UserInteractionEnabled = true;
            UserInfoViewTM.AddGestureRecognizer(userProfileClicked);
        }
        /// <summary>
        /// Show case image click.
        /// To show message of user in this show case view
        /// </summary>
        public void ShowCaseImgClick()
        {
            UITapGestureRecognizer showCaseImgClicked = new UITapGestureRecognizer(() =>
            {
                ShowCaseView.Hidden = true;
            });
            ImgShowCaseAbout.UserInteractionEnabled = true;
            ImgShowCaseAbout.AddGestureRecognizer(showCaseImgClicked);
        }
        /// <summary>
        /// Gesture implementation for menu of About screen
        /// to hide or show popup of MenuAbout
        /// </summary>
        public void MenuClick()
        {
            UITapGestureRecognizer menuClicked = new UITapGestureRecognizer(() =>
            {
                if (popupMenuAbout.Hidden == true)
                {
                    popupMenuAbout.Hidden = false;
                }
                else
                {
                    popupMenuAbout.Hidden = true;
                }
            });
            MenuAbout.UserInteractionEnabled = true;
            MenuAbout.AddGestureRecognizer(menuClicked);
        }
        /// <summary>
        /// Gesture implementation for my cart icon click
        /// click my cart icon if role is user to show pop up message other wise show my cart screen
        /// </summary>
        public void ImgMyCartClick()
        {
            UITapGestureRecognizer imgMyCartClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuAbout.Hidden = true;
                try
                {
                    if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role)))
                    {
                        string role = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role);
                        if (role.ToLower().Trim() == AppResources.user)
                        {
                            Utility.DebugAlert(AppResources.Message, AppResources.AccountRole);
                        }
                        else if (count == Constant.lstDigitString[5])
                        {
                            Utility.DebugAlert(AppResources.Message, AppResources.MessageEmptyQueue); 
                        }
                        else
                        {
                            this.PerformSegue(AppResources.SegueFromAboutToMyCart, this);
                        }
                    } 
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
            ImgMyCartAbout.UserInteractionEnabled = true;
            ImgMyCartAbout.AddGestureRecognizer(imgMyCartClicked);
        }
        /// <summary>
        /// Redeem button on Bottom navigation
        /// </summary>
        public void RedeemClick()
        {

            UITapGestureRecognizer redeemViewClicked = new UITapGestureRecognizer(() =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role)))
                    {
                        string role = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role);
                        if (role.ToLower().Trim() == "manager")
                        {
                            popupMenuAbout.Hidden = true;
                            int count1 = 0;
                            UIViewController[] uIViewControllers = NavigationController.ViewControllers;
                            foreach (UIViewController item in uIViewControllers)
                            {
                                if (item is LpRedeemWebViewController)
                                {
                                    NavigationController.PopToViewController(item, true);
                                    count1++;
                                    break;
                                }
                            }
                            if (count1 == 0)
                            {
                                LpRedeemWebViewController termsConditionWebViewController = (LpRedeemWebViewController)Storyboard.InstantiateViewController("LpRedeemWebViewController");
                                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                            }
                        }
                        else
                        {
                            Utility.DebugAlert("Only managers can redeem for points", "");
                        }

                    }

                }
                catch (Exception ex)
                {
                    Utility.DebugAlert(ex.Message, "");
                    Console.WriteLine(ex.Message);
                }

            });

            /*
                        UITapGestureRecognizer redeemViewClicked = new UITapGestureRecognizer(() =>
                        {

                                            popupMenuAbout.Hidden = true;
                                            int count1 = 0;
                                            UIViewController[] uIViewControllers = NavigationController.ViewControllers;
                                            foreach (UIViewController item in uIViewControllers)
                                            {
                                                if (item is RedeemPointsController)
                                                {
                                                    NavigationController.PopToViewController(item, true);
                                                    count1++;
                                                    break;
                                                }
                                            }
                                            if (count1 == 0)
                                            {
                                                RedeemPointsController redeemPointsController = (RedeemPointsController)Storyboard.InstantiateViewController(AppResources.RedeemPointsController);
                                                this.NavigationController.PushViewController(redeemPointsController, true);
                                            }

                        });
            */                       
            redeemView.UserInteractionEnabled = true;
            redeemView.AddGestureRecognizer(redeemViewClicked);
        }
        /// <summary>
        /// Home button on Bottom navigation
        /// </summary>
        public void HomeClick()
        {
            UITapGestureRecognizer redeemViewClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuAbout.Hidden = true;
                int count1 = 0;
                UIViewController[] uIViewControllers = NavigationController.ViewControllers;
                foreach (UIViewController item in uIViewControllers)
                {
                    if (item is DashboardController)
                    {
                        NavigationController.PopToViewController(item, true);
                        count1++;
                        break;
                    }
                }
                if (count1 == 0)
                {
                    DashboardController dashboardController = (DashboardController)Storyboard.InstantiateViewController(AppResources.DashboardController);
                    this.NavigationController.PushViewController(dashboardController, true);
                }
            });
            homeView.UserInteractionEnabled = true;
            homeView.AddGestureRecognizer(redeemViewClicked);
        }
        /// <summary>
        /// Book button on Bottom navigation
        /// </summary>
        public void BookClick()
        {
            UITapGestureRecognizer redeemViewClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuAbout.Hidden = true;
                int count1 = 0;
                UIViewController[] uIViewControllers = NavigationController.ViewControllers;
                foreach (UIViewController item in uIViewControllers)
                {
                    if (item is BookProductsController)
                    {
                        NavigationController.PopToViewController(item, true);
                        count1++;
                        break;
                    }
                }
                if (count1 == 0)
                {
                    BookProductsController bookProductsController = (BookProductsController)Storyboard.InstantiateViewController(AppResources.BookProductsController);
                    this.NavigationController.PushViewController(bookProductsController, true);
                }
            });
            bookView.UserInteractionEnabled = true;
            bookView.AddGestureRecognizer(redeemViewClicked);
        }
        /// <summary>
        /// About button on Bottom navigation
        /// </summary>
        public void AboutClick()
        {
            UITapGestureRecognizer aboutViewClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuAbout.Hidden = true;
                tblAboutProductList.Hidden = true;
                ProductDropdownView.Hidden = true;
                ProductDetailScrollView.Hidden = true;
                AboutUsView.Hidden = true;
                firstAboutView.Hidden = false;
                TerritoryManagerView.Hidden = true;

            });
            aboutView.UserInteractionEnabled = true;
            aboutView.AddGestureRecognizer(aboutViewClicked);
        }
        /// <summary>
        /// Terms and conditions click.
        /// To open web view and show Terms and conditions 
        /// </summary>
        public void TermsAndConditionsClick()
        {
            UITapGestureRecognizer termsAndConditionsClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuAbout.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[1], AppResources.PDFValue);
            });
            lblTerms.UserInteractionEnabled = true;
            lblTerms.AddGestureRecognizer(termsAndConditionsClicked);
        }
        /// <summary>
        /// privacy policy click.
        /// To open web view and show privacy policy 
        /// </summary>
        public void LblPrivacyPolicyClick()
        {
            UITapGestureRecognizer lblPrivacyClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuAbout.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[4], AppResources.PDFValue);
            });
            lblPrivacy.UserInteractionEnabled = true;
            lblPrivacy.AddGestureRecognizer(lblPrivacyClicked);
        }
        /// <summary>
        /// facebook icon click.
        /// If app is already installed in device then open app otherwise open browser
        /// </summary>
        public void FacebookClick()
        {
            UITapGestureRecognizer imgViewFacebookClicked = new UITapGestureRecognizer(() =>
            {
                if (UIApplication.SharedApplication.CanOpenUrl(new NSUrl(new NSString(AppResources.fb))))
                {
                    UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(AppResources.fbUrl1));
                }
                else
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(AppResources.fbUrl));
                }
            });
            imgViewFacebook.UserInteractionEnabled = true;
            imgViewFacebook.AddGestureRecognizer(imgViewFacebookClicked);
        }
        /// <summary>
        /// rss icon click.
        /// If app is already installed in device then open app otherwise open browser
        /// </summary>
        public void RssClick()
        {
            UITapGestureRecognizer imgViewRssClicked = new UITapGestureRecognizer(() =>
            {
                if (UIApplication.SharedApplication.CanOpenUrl(new NSUrl(new NSString(AppResources.rss))))
                {
                    UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(AppResources.rss1));
                }
                else
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(AppResources.rssUrl));
                }
            });
            imgViewRss.UserInteractionEnabled = true;
            imgViewRss.AddGestureRecognizer(imgViewRssClicked);
        }
        /// <summary>
        /// twitter icon click.
        /// If app is already installed in device then open app otherwise open browser
        /// </summary>
        public void TwitterClick()
        {
            UITapGestureRecognizer imgViewTwitterClicked = new UITapGestureRecognizer(() =>
            {
                if (UIApplication.SharedApplication.CanOpenUrl(new NSUrl(new NSString(AppResources.twitter))))
                {
                    UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(AppResources.twitter1));
                }
                else
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(AppResources.twitterUrl));
                }
            });
            imgViewTwitter.UserInteractionEnabled = true;
            imgViewTwitter.AddGestureRecognizer(imgViewTwitterClicked);
        }
        /// <summary>
        /// youtube icon click. 
        /// If app is already installed in device then open app otherwise open browser
        /// </summary>
        public void YoutubeClick()
        {
            UITapGestureRecognizer imgViewYoutubeClicked = new UITapGestureRecognizer(() =>
            {
                if (UIApplication.SharedApplication.CanOpenUrl(new NSUrl(new NSString(AppResources.youtube))))
                {
                    UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(AppResources.youtube1));
                }
                else
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(AppResources.youtubeUrl));
                }
            });
            imgViewYoutube.UserInteractionEnabled = true;
            imgViewYoutube.AddGestureRecognizer(imgViewYoutubeClicked);
        }
        /// <summary>
        /// instagram icon click.
        /// If app is already installed in device then open app otherwise open browser
        /// </summary>
        public void InstagramClick()
        {
            UITapGestureRecognizer imgViewInstagramClicked = new UITapGestureRecognizer(() =>
            {
                if (UIApplication.SharedApplication.CanOpenUrl(new NSUrl(new NSString(AppResources.instagram))))
                {
                    UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(AppResources.instagram1));
                }
                else
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(AppResources.instagramUrl));
                }
            });
            imgViewInstagram.UserInteractionEnabled = true;
            imgViewInstagram.AddGestureRecognizer(imgViewInstagramClicked);
        }
        /// <summary>
        /// KenMAuser Email Click
        /// Open the Email application
        /// </summary>
        public void LblWTMEmailValueClick()
        {
            UITapGestureRecognizer lblWTMEmailValueClicked = new UITapGestureRecognizer((sender) =>
            {
                mailId = ((UILabel)sender.View).Text;
                OpenEmail(mailId);
            });
            lblWTMEmailValue.UserInteractionEnabled = true;
            lblWTMEmailValue.AddGestureRecognizer(lblWTMEmailValueClicked);
        }
        /// <summary>
        /// John Email Click
        /// Open the Email application
        /// </summary>
        public void LblNCTMEmailValueClick()
        {
            UITapGestureRecognizer lblNCTMEmailValueClicked = new UITapGestureRecognizer((sender) =>
            {
                mailId = ((UILabel)sender.View).Text;
                OpenEmail(mailId);
            });
            lblNCTMEmailValue.UserInteractionEnabled = true;
            lblNCTMEmailValue.AddGestureRecognizer(lblNCTMEmailValueClicked);
        }
        /// <summary>
        /// Robert Wilson Email Click
        /// Open the Email application
        /// </summary>
        public void LblSWTMEmailValueClick()
        {
            UITapGestureRecognizer lblSWTMEmailValueClicked = new UITapGestureRecognizer((sender) =>
            {
                mailId = ((UILabel)sender.View).Text;
                OpenEmail(mailId);
            });
            lblSWTMEmailValue.UserInteractionEnabled = true;
            lblSWTMEmailValue.AddGestureRecognizer(lblSWTMEmailValueClicked);
        }
        /// <summary>
        /// Greg Lovell Email Click
        /// Open the Email application
        /// </summary>
        public void LblCETMEmailValueClick()
        {
            UITapGestureRecognizer lblCETMEmailValueClicked = new UITapGestureRecognizer((sender) =>
            {
                mailId = ((UILabel)sender.View).Text;
                OpenEmail(mailId);
             
            });
            lblCETMEmailValue.UserInteractionEnabled = true;
            lblCETMEmailValue.AddGestureRecognizer(lblCETMEmailValueClicked);
        }
        /// <summary>
        /// Scott Poynot Email Click
        /// Open the Email application
        /// </summary>
        public void LblCSTMEmailvalueClick()
        {
            UITapGestureRecognizer lblCSTMEmailvalueClicked = new UITapGestureRecognizer((sender) =>
            {
                mailId = ((UILabel)sender.View).Text;
                OpenEmail(mailId);
            });
            lblCSTMEmailvalue.UserInteractionEnabled = true;
            lblCSTMEmailvalue.AddGestureRecognizer(lblCSTMEmailvalueClicked);
        }
        /// <summary>
        /// Tom Valentine Email click.
        /// Open the Email application
        /// </summary>
        public void LblNETMEmailValueClick()
        {
            UITapGestureRecognizer lblNETMEmailValueClicked = new UITapGestureRecognizer((sender) =>
            {
                mailId = ((UILabel)sender.View).Text;
                OpenEmail(mailId);
            });
            lblNETMEmailValue.UserInteractionEnabled = true;
            lblNETMEmailValue.AddGestureRecognizer(lblNETMEmailValueClicked);
        }
        /// <summary>
        /// Wes Hamm Email click.
        /// Open the Email application
        /// </summary>
        public void LblSETMEmailValueClick()
        {
            UITapGestureRecognizer lblSETMEmailValueClicked = new UITapGestureRecognizer((sender) =>
            {
                mailId = ((UILabel)sender.View).Text;
                OpenEmail(mailId);
            });
            lblSETMEmailValue.UserInteractionEnabled = true;
            lblSETMEmailValue.AddGestureRecognizer(lblSETMEmailValueClicked);
        }
        /// <summary>
        /// Walter Dea Email click.
        /// Open the Email application
        /// </summary>
        public void LblCTMEmailValueClick()
        {
            UITapGestureRecognizer lblCTMEmailValueClicked = new UITapGestureRecognizer((sender) =>
            {
                mailId = ((UILabel)sender.View).Text;
                OpenEmail(mailId);
            });
            lblCTMEmailValue.UserInteractionEnabled = true;
            lblCTMEmailValue.AddGestureRecognizer(lblCTMEmailValueClicked);
        }
        /// <summary>
        /// phone numbers active links for calls on Western Territory Manager
        /// </summary>
        public void LblWTMMobileValueClick()
        {
            UITapGestureRecognizer lblWTMMobileValueClicked = new UITapGestureRecognizer((sender) =>
            {
                phone = ((UILabel)sender.View).Text;
                OpenCall(phone);
            });
            lblWTMMobileValue.UserInteractionEnabled = true;
            lblWTMMobileValue.AddGestureRecognizer(lblWTMMobileValueClicked);
        }
        /// <summary>
        /// phone numbers active links for calls on North Central Territory Manager
        /// </summary>
        public void LblNCTMPhoneValueClick()
        {
            UITapGestureRecognizer lblNCTMPhoneValueClicked = new UITapGestureRecognizer((sender) =>
            {
                phone = ((UILabel)sender.View).Text;
                OpenCall(phone);
            });
            lblNCTMPhoneValue.UserInteractionEnabled = true;
            lblNCTMPhoneValue.AddGestureRecognizer(lblNCTMPhoneValueClicked);
        }
        /// <summary>
        /// phone numbers active links for calls on SouthWest Territory Manager
        /// </summary>
        public void LblSWTMPhoneValueClick()
        {
            UITapGestureRecognizer lblSWTMPhoneValueClicked = new UITapGestureRecognizer((sender) =>
            {
                phone = ((UILabel)sender.View).Text;
                OpenCall(phone);
            });
            lblSWTMPhoneValue.UserInteractionEnabled = true;
            lblSWTMPhoneValue.AddGestureRecognizer(lblSWTMPhoneValueClicked);
        }
        /// <summary>
        /// phone numbers active links for calls on Central East Territory Manager
        /// </summary>
        public void LblCETMMobileValueClick()
        {
            UITapGestureRecognizer lblCETMMobileValueClicked = new UITapGestureRecognizer((sender) =>
            {
                phone = ((UILabel)sender.View).Text;
                OpenCall(phone);
            });
            lblCETMMobileValue.UserInteractionEnabled = true;
            lblCETMMobileValue.AddGestureRecognizer(lblCETMMobileValueClicked);
        }
        /// <summary>
        /// phone numbers active links for calls on Central South Territory Manager
        /// </summary>
        public void LblCSTMMobileValueClick()
        {
            UITapGestureRecognizer lblCSTMMobileValueClicked = new UITapGestureRecognizer((sender) =>
            {
                phone = ((UILabel)sender.View).Text;
                OpenCall(phone);
            });
            lblCSTMMobileValue.UserInteractionEnabled = true;
            lblCSTMMobileValue.AddGestureRecognizer(lblCSTMMobileValueClicked);
        }
        /// <summary>
        /// phone numbers active links for calls on Northeast Territory Manager
        /// </summary>
        public void LblNETMMobileValueClick()
        {
            UITapGestureRecognizer lblNETMMobileValueClicked = new UITapGestureRecognizer((sender) =>
            {
                phone = ((UILabel)sender.View).Text;
                OpenCall(phone);
            });
            lblNETMMobileValue.UserInteractionEnabled = true;
            lblNETMMobileValue.AddGestureRecognizer(lblNETMMobileValueClicked);
        }
        /// <summary>
        /// phone numbers active links for calls on Southeast Territory Manager
        /// </summary>
        public void LblSETMMobileValueClick()
        {
            UITapGestureRecognizer lblSETMMobileValueClicked = new UITapGestureRecognizer((sender) =>
            {
                phone = ((UILabel)sender.View).Text;
                OpenCall(phone);
            });
            lblSETMMobileValue.UserInteractionEnabled = true;
            lblSETMMobileValue.AddGestureRecognizer(lblSETMMobileValueClicked);
        }
        /// <summary>
        /// phone numbers active links for calls on Canadian Territory Manager
        /// </summary>
        public void LblCTMPhoneValueClick()
        {
            UITapGestureRecognizer lblCTMPhoneValueClicked = new UITapGestureRecognizer((sender) =>
            {
                phone = ((UILabel)sender.View).Text;
                OpenCall(phone);
            });
            lblCTMPhoneValue.UserInteractionEnabled = true;
            lblCTMPhoneValue.AddGestureRecognizer(lblCTMPhoneValueClicked);
        }
        /// <summary>
        /// mobile numbers active links for calls on Canadian Territory Manager
        /// </summary>
        public void LblCTMMobileValueClick()
        {
            UITapGestureRecognizer lblCTMMobileValueClicked = new UITapGestureRecognizer((sender) =>
            {
                phone = ((UILabel)sender.View).Text;
                OpenCall(phone);
            });
            lblCTMMobileValue.UserInteractionEnabled = true;
            lblCTMMobileValue.AddGestureRecognizer(lblCTMMobileValueClicked);
        }
        /// <summary>
        /// Gesture implementation for About main view click to hide pop menu.
        /// </summary>
        public void AboutMainViewClick()
        {
            UITapGestureRecognizer aboutMainClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuAbout.Hidden = true;
            });
            AboutMainView.UserInteractionEnabled = true;
            AboutMainView.AddGestureRecognizer(aboutMainClicked);
        }
        /// <summary>
        /// Gesture implementation for FAQ click.
        /// </summary>
        public void LblFAQClick()
        {
            UITapGestureRecognizer lblFAQClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuAbout.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[5], AppResources.PDFValue);
            });
            lblFaq.UserInteractionEnabled = true;
            lblFaq.AddGestureRecognizer(lblFAQClicked);
        }
        /// <summary>
        /// Gesture implementation for Support click.
        /// </summary>
        public void LblSupportClick()
        {
            try { 
            UITapGestureRecognizer lblSupportClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuAbout.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[6], AppResources.PDFValue);
            });
            lblSupport.UserInteractionEnabled = true;
            lblSupport.AddGestureRecognizer(lblSupportClicked);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace.ToString());
            }
        }
        /// <summary>
        /// Logout the user
        /// </summary>
        public void LblLogoutClick()
        {
            try
            {
                UITapGestureRecognizer lblLogoutClicked = new UITapGestureRecognizer(() =>
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
                lblLogout.AddGestureRecognizer(lblLogoutClicked);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace.ToString());
            }
        }
        /// <summary>
        /// Dids the receive memory warning.
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}