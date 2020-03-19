using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Fragments;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aquatrols.Droid.Activities
{
    /// <summary>
    /// This class is used for course screen
    /// </summary>
    [Activity]
    public class TMCourseHomeActivity : Activity, View.IOnClickListener
    {
        private AutoCompleteTextView autoCompleteTmCourseName, autoCompleteTmSuperIntendantName;
        private List<TmCourseResponseEntity> lstTmCourse;
        private List<TmSuperIntendantResponseEntity> lstTmSuperIntendant;
        private string token, tmCourseId, tmSuperIntendantId;
        private ImageView imgMenu, imgBack;
        private TextInputLayout txtInputLayoutCoursename, txtInputLayoutSuperIntendantname;
        private Button btnCourseSearch, btnSuperIntendantSearch;

        /// <summary>
        /// This method is used to initialize page load value and call Course List and SuperIntendant List data
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.TMCourseHomeLayout);
                RequestedOrientation = ScreenOrientation.Portrait;      //disabling Landscape mode. 
                // Handle exception handling 
                if (Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null) != null)
                {
                    using (Utility utility = new Utility(this))
                    {
                        utility.SaveExceptionHandling(null, null, null, Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null));
                    }
                }

                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null)))
                {
                    token = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null);
                }
                FindControlsById();
                await GetTerritoryCourseList();
                await GetTerritorySuperIntendantList();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreate), Resources.GetString(Resource.String.TMCourseHomeActivity), null);
                }
            }
        }
        
        /// <summary>
        /// Method to reset controls when back from another page
        /// </summary>
        protected override void OnResume()
        {
            try
            {
                base.OnResume();
                autoCompleteTmCourseName.Text = null;
                autoCompleteTmSuperIntendantName.Text = null;
                txtInputLayoutCoursename.Focusable = true;
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnResume), Resources.GetString(Resource.String.TMCourseHomeActivity), null);
                }
            }
        }

        /// <summary>
        ///  Method to get reference of controls
        /// </summary>
        public void FindControlsById()
        {
            try
            {
                btnSuperIntendantSearch = FindViewById<Button>(Resource.Id.btnSuperIntendantSearch);
                btnCourseSearch = FindViewById<Button>(Resource.Id.btnCourseSearch);
                imgMenu = FindViewById<ImageView>(Resource.Id.imgMenu);
                imgBack = FindViewById<ImageView>(Resource.Id.imgBack);
                autoCompleteTmCourseName = FindViewById<AutoCompleteTextView>(Resource.Id.autocompleteTmCoursename);
                autoCompleteTmSuperIntendantName = FindViewById<AutoCompleteTextView>(Resource.Id.autocompleteSuperIntendantname);
                txtInputLayoutCoursename = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutCoursename);
                txtInputLayoutSuperIntendantname = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutSuperIntendantname);
                #region //Event handlers            
                imgMenu.Click += ImgMenu_Click;
                btnCourseSearch.SetOnClickListener(this);
                btnSuperIntendantSearch.SetOnClickListener(this);
                imgBack.SetOnClickListener(this);
                autoCompleteTmCourseName.TextChanged += AutocompleteTmCoursename_TextChanged;
                autoCompleteTmCourseName.ItemClick += AutocompleteTmCoursename_ItemClick;
                autoCompleteTmSuperIntendantName.TextChanged += AutocompleteTmSuperIntendantname_TextChanged;
                autoCompleteTmSuperIntendantName.ItemClick += AutocompleteTmSuperIntendantname_ItemClick;
            }
            #endregion}
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.FindControlsById), Resources.GetString(Resource.String.TMCourseHomeActivity), null);
                }
            }
        }

        /// <summary>
        /// Raised on Menu icon click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgMenu_Click(object sender, EventArgs e)
        {
            try
            {
                Android.Support.V7.Widget.PopupMenu menu = new Android.Support.V7.Widget.PopupMenu(this, imgMenu);
                menu.Inflate(Resource.Menu.OptionMenu);
                menu.Show();
                menu.MenuItemClick += (s1, arg1) =>
                {
                    Intent intent;
                    switch (arg1.Item.ItemId)
                    {
                        case Resource.Id.logoutID:
                            intent = new Intent(this, typeof(LoginActivity));
                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.token), null).Commit();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.UserId), null).Commit();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.isApproved), null).Commit();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerritory), null).Commit();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.Role), null).Commit();
                            StartActivity(intent);
                            Finish();
                            break;
                        case Resource.Id.termsID:
                            intent = new Intent(this, typeof(TermsWebviewActivity));
                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerms), Resources.GetString(Resource.String.yes)).Commit();
                            this.StartActivity(intent);
                            break;
                        case Resource.Id.privacyID:
                            intent = new Intent(this, typeof(TermsWebviewActivity));
                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerms), Resources.GetString(Resource.String.no)).Commit();
                            this.StartActivity(intent);
                            break;
                        case Resource.Id.FAQ:
                            intent = new Intent(this, typeof(TermsWebviewActivity));
                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerms), Resources.GetString(Resource.String.FAQ)).Commit();
                            this.StartActivity(intent);
                            break;
                        case Resource.Id.SupportID:
                            intent = new Intent(this, typeof(TermsWebviewActivity));
                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerms), Resources.GetString(Resource.String.support)).Commit();
                            this.StartActivity(intent);
                            break;
                        case Resource.Id.About:
                            intent = new Intent(this, typeof(AboutAppActivity));
                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.about), Resources.GetString(Resource.String.about)).Commit();
                            this.StartActivity(intent);
                            break;
                        default:
                            break;
                    }
                };
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.ImgMenu_Click), Resources.GetString(Resource.String.TMCourseHomeActivity), null);
                }
            }
        }

        /// <summary>
        /// Item click event for AutocompleteTmSuperIntendantname textview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutocompleteTmSuperIntendantname_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                AutoCompleteTextView autoText = (AutoCompleteTextView)sender;
                if (lstTmSuperIntendant != null)
                {
                    string[] item_test = autoText.Text.Split("(", StringSplitOptions.None);
                    string email = autoText.Text;
                    string[] eh = email.Split(':');
                    string[] email1 = eh[1].Split(')');
                    autoText.Text = item_test[0];
                    tmSuperIntendantId = lstTmSuperIntendant.Where(x => x.userName == email1[0].Trim()).Select(x => x.id).FirstOrDefault();
                    if (string.IsNullOrEmpty(tmSuperIntendantId))
                    {
                        tmSuperIntendantId = string.Empty;
                    }
                    else
                    {
                        if (lstTmSuperIntendant != null)
                        {
                            tmSuperIntendantId = lstTmSuperIntendant.Where(x => x.userName == email1[0].Trim()).Select(x => x.id).FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.AutocompleteTmSuperIntendantname_ItemClick), Resources.GetString(Resource.String.TMCourseHomeActivity), null);
                }
            }
        }

        /// <summary>
        /// Text changed event for AutocompleteTmSuperIntendantname textview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutocompleteTmSuperIntendantname_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(autoCompleteTmSuperIntendantName.Text))
                {
                    tmSuperIntendantId = string.Empty;
                }
                else
                {
                    if (lstTmSuperIntendant != null)
                    {
                        AutoCompleteTextView autoText = (AutoCompleteTextView)sender;
                        tmSuperIntendantId = lstTmSuperIntendant.Where(x => x.userName == autoCompleteTmCourseName.Text || (x.firstName + " " + x.lastName).ToLower() == autoText.Text.ToLower()).Select(x => x.id).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.AutocompleteTmSuperIntendantname_TextChanged), Resources.GetString(Resource.String.TMCourseHomeActivity), null);
                }
            }
        }

        /// <summary>
        /// Item click event for AutocompleteTmCoursename textview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutocompleteTmCoursename_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                AutoCompleteTextView autoText = (AutoCompleteTextView)sender;
                if (lstTmCourse != null)
                {
                    string[] item = autoText.Text.Split("(", StringSplitOptions.None);
                    tmCourseId = lstTmCourse.Where(x => x.courseName == item[0].Trim()).Select(x => x.cId).FirstOrDefault();
                    if (string.IsNullOrEmpty(tmCourseId))
                    {
                        tmCourseId = string.Empty;
                    }
                    else
                    {
                        if (lstTmCourse != null)
                        {
                            tmCourseId = lstTmCourse.Where(x => x.courseName == item[0].Trim()).Select(x => x.cId).FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.AutocompleteTmCoursename_ItemClick), Resources.GetString(Resource.String.TMCourseHomeActivity), null);
                }
            }
        }

        /// <summary>
        /// Text changed event for AutocompleteTmCoursename textview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutocompleteTmCoursename_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(autoCompleteTmCourseName.Text))
                {
                    tmCourseId = string.Empty;
                }
                else
                {
                    if (lstTmCourse != null)
                    {
                        tmCourseId = lstTmCourse.Where(x => x.courseName == autoCompleteTmCourseName.Text).Select(x => x.cId).FirstOrDefault();

                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.AutocompleteTmCoursename_TextChanged), Resources.GetString(Resource.String.TMCourseHomeActivity), null);
                }
            }
        }

        /// <summary>
        /// Method to get All courses that belongs to loggedIn Territory Manager user
        /// </summary>
        public async Task GetTerritoryCourseList()
        {
            Show_Overlay();
            try
            {
                using (Utility util = new Utility(this))
                {
                    bool internetStatus = util.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this, Resource.String.NoInternet, ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        using (Utility utility = new Utility(this))
                        {
                            lstTmCourse = await utility.GetTmCourseList(token);
                            if (lstTmCourse != null)
                            {
                                List<string> territoryCourseName = new List<string>(); ;
                                foreach (var item in lstTmCourse)
                                {
                                    string coursename = item.courseName + " (" + Resources.GetString(Resource.String.zipcode) + " : " + item.zip + ")";
                                    territoryCourseName.Add(coursename);
                                }

                                var TmCourseAdapter = new ArrayAdapter<String>(this, Resource.Layout.list_item, territoryCourseName);
                                autoCompleteTmCourseName.Adapter = TmCourseAdapter;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetTerritoryCourseList), Resources.GetString(Resource.String.TMCourseHomeActivity), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// Method to get All courses that belongs to loggedIn Territory Manager user
        /// </summary>
        public async Task GetTerritorySuperIntendantList()
        {
            Show_Overlay();
            try
            {
                using (Utility util = new Utility(this))
                {
                    bool internetStatus = util.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this, Resource.String.NoInternet, ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        using (Utility utility = new Utility(this))
                        {
                            lstTmSuperIntendant = await utility.GetTmSuperIntendantList(token);
                            if (lstTmSuperIntendant != null)
                            {
                                List<string> territorySuperIntendantName = new List<string>(); ;
                                foreach (var item in lstTmSuperIntendant)
                                {
                                    string coursename = item.firstName + " " + item.lastName + " (" + "Email" + " : " + item.userName + ")";
                                    territorySuperIntendantName.Add(coursename);
                                }
                                var TmSuperIntendantAdapter = new ArrayAdapter<String>(this, Resource.Layout.list_item, territorySuperIntendantName);
                                autoCompleteTmSuperIntendantName.Adapter = TmSuperIntendantAdapter;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetTerritorySuperIntendantList), Resources.GetString(Resource.String.TMCourseHomeActivity), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// This method shows an overlay dialog.
        /// </summary>
        OverlayActivity overlay;
        public void Show_Overlay()
        {
            try
            {
                overlay = new OverlayActivity(this);
                overlay.Show();
                overlay.SetCanceledOnTouchOutside(false);
                overlay.SetCancelable(false);
            }
            catch (System.Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.TMCourseHomeActivity), null);
                }
            }
        }

        /// <summary>
        /// Click event handler for Button
        /// </summary>
        /// <param name="v"></param>
        public void OnClick(View v)
        {
            try
            {
                switch (v.Id)
                {
                    case Resource.Id.btnCourseSearch:
                        if (string.IsNullOrEmpty(autoCompleteTmCourseName.Text))
                        {
                            txtInputLayoutCoursename.Error = Resources.GetString(Resource.String.Required);
                        }
                        else if (!string.IsNullOrEmpty(tmCourseId))
                        {
                            txtInputLayoutCoursename.ErrorEnabled = false;
                            Intent intent = new Intent(this, typeof(TmCourseResultActivity));
                            intent.PutExtra(Resources.GetString(Resource.String.tmCourseId), tmCourseId);
                            StartActivity(intent);
                        }
                        else
                        {
                            txtInputLayoutCoursename.ErrorEnabled = false;
                            Toast.MakeText(this, Resource.String.NoRecord, ToastLength.Long).Show();
                        }
                        break;
                    case Resource.Id.btnSuperIntendantSearch:
                        if (string.IsNullOrEmpty(autoCompleteTmSuperIntendantName.Text))
                        {
                            txtInputLayoutSuperIntendantname.Error = Resources.GetString(Resource.String.Required);
                        }
                        else if (!string.IsNullOrEmpty(tmSuperIntendantId))
                        {
                            txtInputLayoutSuperIntendantname.ErrorEnabled = false;
                            Intent intent = new Intent(this, typeof(TmCourseResultActivity));
                            intent.PutExtra(Resources.GetString(Resource.String.tmUserId), tmSuperIntendantId);
                            StartActivity(intent);
                        }
                        else
                        {
                            txtInputLayoutSuperIntendantname.ErrorEnabled = false;
                            Toast.MakeText(this, Resource.String.NoRecord, ToastLength.Long).Show();
                        }
                        break;
                    case Resource.Id.imgBack:
                        Finish();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnClick), Resources.GetString(Resource.String.TMCourseHomeActivity), null);
                }
            }
        }
    }
}