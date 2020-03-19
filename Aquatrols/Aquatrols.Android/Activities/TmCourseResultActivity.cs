using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Adapter;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Aquatrols.Droid.Activities
{
    /// <summary>
    ///  This class is used for course result screen
    /// </summary>
    [Activity]
    public class TmCourseResultActivity : Activity, View.IOnClickListener
    {
        private TmCourseResultResponseEntity tmCourseResultResponseEntity;
        private TmDistributorResultResponseEntity TmDistributorResultResponse;
        private string token, tmCourseId, tmUserId, tmDistributorId, tmState;
        private ListView lvTmProductlist;
        private ImageView imgBack;
        private Button btnDownload, btnShare;
        private LinearLayout LinearDownload;
        private View ViewAddressLine;
        private RelativeLayout rllcountry, rllzipcode, rllState, rllcity, rllAddress;
        private TextView txtTmCourseName, txtNorecord, txtViewAddressValue, txtViewCityValue, txtViewCountryValue, txtViewZipValue, txtViewStateValue, txtHeaderCourse, txtHeaderCommitments, txtUserTiedToCourse, txtHeading;

        /// <summary>
        /// This method is used to initialize page load value and show CourseResultData, SuperintendentResultData and DistributorResultData
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.TmCourseResultLayout);
                RequestedOrientation = ScreenOrientation.Portrait;      //disabling Landscape mode. 
                if (Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null) != null)
                {
                    using (Utility utility = new Utility(this))
                    {
                        utility.SaveExceptionHandling(null, null, null, Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null));
                    }
                }
                if (Intent.Extras != null)
                {
                    Bundle bundle = Intent.Extras;
                    tmCourseId = bundle.GetString(Resources.GetString(Resource.String.tmCourseId));
                    tmUserId = bundle.GetString(Resources.GetString(Resource.String.tmUserId));
                    tmDistributorId = bundle.GetString(Resources.GetString(Resource.String.tmDistributorId));
                    tmState = bundle.GetString(Resources.GetString(Resource.String.tmState));
                }
                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null)))
                {
                    token = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null);
                }
                FindControlsById();
                if (!string.IsNullOrEmpty(tmCourseId))
                {
                    rllAddress.Visibility = ViewStates.Visible;
                    rllcity.Visibility = ViewStates.Visible;
                    rllcountry.Visibility = ViewStates.Visible;
                    rllState.Visibility = ViewStates.Visible;
                    rllzipcode.Visibility = ViewStates.Visible;
                    ViewAddressLine.Visibility = ViewStates.Visible;
                    await GetTmCourseResultData(tmCourseId);
                }
                if (!string.IsNullOrEmpty(tmUserId))
                {
                    rllAddress.Visibility = ViewStates.Visible;
                    rllcity.Visibility = ViewStates.Visible;
                    rllcountry.Visibility = ViewStates.Visible;
                    rllState.Visibility = ViewStates.Visible;
                    rllzipcode.Visibility = ViewStates.Visible;
                    ViewAddressLine.Visibility = ViewStates.Visible;
                    await GetTmSuperintendentResultData(tmUserId);
                }
                if (!string.IsNullOrEmpty(tmDistributorId))
                {
                    txtHeaderCourse.Text = Resources.GetString(Resource.String.distributor);
                    await GetTmDistributorResultData(tmDistributorId, tmState);
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreate), Resources.GetString(Resource.String.TmCourseResultActivity), null);
                }
            }
        }

        /// <summary>
        /// Mehtod to find controls on activity
        /// </summary>
        public void FindControlsById()
        {
            try
            {
                lvTmProductlist = FindViewById<ListView>(Resource.Id.lvTmProductlist);
                txtViewAddressValue = FindViewById<TextView>(Resource.Id.txtAddressValue);
                txtViewCityValue = FindViewById<TextView>(Resource.Id.txtViewCityValue);
                txtViewCountryValue = FindViewById<TextView>(Resource.Id.txtViewCountryValue);
                txtViewZipValue = FindViewById<TextView>(Resource.Id.txtViewZipValue);
                txtViewStateValue = FindViewById<TextView>(Resource.Id.txtViewStateValue);
                txtTmCourseName = FindViewById<TextView>(Resource.Id.txtTmCourseName);
                txtHeaderCourse = FindViewById<TextView>(Resource.Id.txtHeaderForCourse);
                txtHeaderCommitments = FindViewById<TextView>(Resource.Id.txtHeaderCommitments);
                txtNorecord = FindViewById<TextView>(Resource.Id.txtNorecord);
                txtUserTiedToCourse = FindViewById<TextView>(Resource.Id.txtUserTiedToCourse);
                btnDownload = FindViewById<Button>(Resource.Id.btnDownload);
                btnShare = FindViewById<Button>(Resource.Id.btnShare);
                imgBack = FindViewById<ImageView>(Resource.Id.imgBack);
                ViewAddressLine = FindViewById<View>(Resource.Id.ViewAddressLine);
                rllcountry = FindViewById<RelativeLayout>(Resource.Id.rllcountry);
                rllzipcode = FindViewById<RelativeLayout>(Resource.Id.rllzipcode);
                rllState = FindViewById<RelativeLayout>(Resource.Id.rllState);
                rllcity = FindViewById<RelativeLayout>(Resource.Id.rllcity);
                rllAddress = FindViewById<RelativeLayout>(Resource.Id.rllAddress);
                LinearDownload = FindViewById<LinearLayout>(Resource.Id.LinearDownload);
                txtHeading = FindViewById<TextView>(Resource.Id.txtHeading);
                btnDownload.Click += BtnDownload_Click;
                btnShare.Click += BtnShare_Click;
                imgBack.SetOnClickListener(this);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.FindControlsById), Resources.GetString(Resource.String.TmCourseResultActivity), null);
                }
            }
        }

        /// <summary>
        /// Method to handle share button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShare_Click(object sender, EventArgs e)
        {
            AttachFile();
        }

        /// <summary>
        /// download button click handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDownload_Click(object sender, EventArgs e)
        {
            GetTerritoryDistributorExcelUrl();
        }

        /// <summary>
        /// Method to download a file
        /// </summary>
        /// <param name="url"></param>
        public void StartNewDownload(string url)
        {
            try
            {
                var territoryFile = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads + "/" + Path.GetFileNameWithoutExtension(url) + Path.GetExtension(url).Replace("\"", string.Empty));
                if (territoryFile.Exists())
                {
                    territoryFile.Delete();
                }
                string newUrl = url.Remove(0, 1);
                newUrl = newUrl.Remove(newUrl.Length - 1, 1);
                Java.IO.File file = new Java.IO.File(Android.OS.Environment.DirectoryDownloads);
                DownloadManager.Request request = new DownloadManager.Request(Android.Net.Uri.Parse(newUrl)); /*init a request*/
                request.SetTitle(Path.GetFileNameWithoutExtension(url));//this description apears in the android notification   
                request.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
                request.SetDestinationInExternalPublicDir(
                     file.Path,
                        Path.GetFileNameWithoutExtension(url) + Path.GetExtension(url).Replace("\"", string.Empty)); //set destination
                                                                                                                     // OR
                DownloadManager manager = (DownloadManager)this.GetSystemService(DownloadService);
                long downloadId = manager.Enqueue(request); //start the download and return the id of the download. this id can be used to get info about the file (the size, the download progress ...) you can also stop the download by using this id                 
                Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.TmDistributerFileName), Path.GetFileNameWithoutExtension(url) + Path.GetExtension(url).Replace("\"", string.Empty)).Commit();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.StartNewDownload), Resources.GetString(Resource.String.TmCourseResultActivity), null);
                }
            }
        }

        /// <summary>
        /// Method to attach downloaded excel file
        /// </summary>
        public async void AttachFile()
        {
            Show_Overlay();
            try
            {
                using (Utility utility = new Utility(this))
                {
                    bool internetStatus = utility.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this, Resource.String.NoInternet, ToastLength.Long).Show();
                    }
                    else
                    {
                        SnapShotRequestEntity snapShotRequestEntity = new SnapShotRequestEntity();
                        snapShotRequestEntity.distributorName = tmDistributorId;
                        snapShotRequestEntity.state = tmState;
                        string data = await utility.GetTerritoryDistributorExcelUrl(token, snapShotRequestEntity);
                        if (!string.IsNullOrEmpty(data))
                        {
                            //download code
                            string newUrl = data.Remove(0, 1);
                            newUrl = newUrl.Remove(newUrl.Length - 1, 1);
                            Java.IO.File file = new Java.IO.File(Android.OS.Environment.DirectoryDownloads);
                            DownloadManager.Request request = new DownloadManager.Request(Android.Net.Uri.Parse(newUrl)); /*init a request*/
                            request.SetTitle(Path.GetFileNameWithoutExtension(data));//this description apears in the android notification   
                            request.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
                            request.SetDestinationInExternalPublicDir(
                                 file.Path,
                                    Path.GetFileNameWithoutExtension(data) + Path.GetExtension(data).Replace("\"", string.Empty)); //set destination
                            DownloadManager manager = (DownloadManager)this.GetSystemService(DownloadService);
                            long downloadId = manager.Enqueue(request);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.TmDistributerFileName), Path.GetFileNameWithoutExtension(data) + Path.GetExtension(data).Replace("\"", string.Empty)).Commit();
                            //share code
                            await Task.Delay(2000);
                            var fileName = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.TmDistributerFileName), null);
                            var filep = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads + "/" + fileName);
                            var fileToAttach = FileProvider.GetUriForFile(this, this.PackageName + ".fileprovider", filep);
                            Intent emailIntent = new Intent(Intent.ActionSend);
                            emailIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
                            emailIntent.AddFlags(ActivityFlags.GrantWriteUriPermission);
                            emailIntent.SetType("application/excel");// set the type to 'email'
                            emailIntent.PutExtra(Intent.ExtraEmail, string.Empty);
                            emailIntent.PutExtra(Intent.ExtraStream, fileToAttach);// the attachment
                            emailIntent.PutExtra(Intent.ExtraSubject, string.Empty);// the mail subject
                            StartActivity(Intent.CreateChooser(emailIntent, Resources.GetString(Resource.String.sendmail)));
                        }
                        else
                        {
                            Toast.MakeText(this, Resources.GetString(Resource.String.NofileToDownoad), ToastLength.Long).Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.AttachFile), Resources.GetString(Resource.String.TmCourseResultActivity), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// Method to get url of Excel file from server
        /// </summary>
        /// <returns></returns>
        public async void GetTerritoryDistributorExcelUrl()
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
                            SnapShotRequestEntity snapShotRequestEntity = new SnapShotRequestEntity();
                            snapShotRequestEntity.distributorName = tmDistributorId;
                            snapShotRequestEntity.state = tmState;
                            string data = await utility.GetTerritoryDistributorExcelUrl(token, snapShotRequestEntity);
                            if (!string.IsNullOrEmpty(data))
                            {
                                StartNewDownload(data);
                            }
                            else
                            {
                                Toast.MakeText(this, Resources.GetString(Resource.String.NofileToDownoad), ToastLength.Long).Show();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetTerritoryDistributorExcelUrl), Resources.GetString(Resource.String.TmCourseResultActivity), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// Method implementation to get Territory course search result data
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public async Task GetTmCourseResultData(string CourseId)
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
                    }
                    else
                    {
                        tmCourseResultResponseEntity = await util.GetTmCourseSearchResult(token, tmCourseId);
                        if (tmCourseResultResponseEntity != null)
                        {
                            txtUserTiedToCourse.Visibility = ViewStates.Visible;
                            if (tmCourseResultResponseEntity.isUserTiedToCourse.ToUpper() == Resources.GetString(Resource.String.TRUE))
                            {
                                txtUserTiedToCourse.Text = Resources.GetString(Resource.String.UserTiedToCourseTxtValueYes);
                            }
                            else
                            {
                                txtUserTiedToCourse.Text = Resources.GetString(Resource.String.UserTiedToCourseTxtValueNo);
                            }
                            txtTmCourseName.Text = tmCourseResultResponseEntity.tmCoursesVm.courseName;
                            txtViewAddressValue.Text = tmCourseResultResponseEntity.tmCoursesVm.address;
                            txtViewCityValue.Text = tmCourseResultResponseEntity.tmCoursesVm.city;
                            txtViewStateValue.Text = tmCourseResultResponseEntity.tmCoursesVm.state;
                            txtViewZipValue.Text = tmCourseResultResponseEntity.tmCoursesVm.zip;
                            txtViewCountryValue.Text = tmCourseResultResponseEntity.tmCoursesVm.country;
                            txtHeading.Text = "Course Search Result";
                            if (tmCourseResultResponseEntity.tmDistributorProductVm != null)
                            {
                                // removing this functionality because of business requirements 
                                //if (tmCourseResultResponseEntity.tmDistributorProductVm.Count != 0)
                                //{
                                //    txtNorecord.Visibility = ViewStates.Gone;
                                //    lvTmProductlist.Visibility = ViewStates.Visible;
                                //    TmCourseResultAdapter adapter = new TmCourseResultAdapter(this, tmCourseResultResponseEntity.tmDistributorProductVm);
                                //    lvTmProductlist.Adapter = adapter;
                                //}
                                //else
                                //{
                                //    txtNorecord.Visibility = ViewStates.Visible;
                                //    lvTmProductlist.Visibility = ViewStates.Gone;
                                //}
                            }
                            else
                            {
                                txtNorecord.Visibility = ViewStates.Visible;
                                lvTmProductlist.Visibility = ViewStates.Gone;
                            }
                        }
                        else
                        {
                            txtNorecord.Visibility = ViewStates.Visible;
                            lvTmProductlist.Visibility = ViewStates.Gone;
                            Toast.MakeText(this, Resource.String.NoRecord, ToastLength.Long).Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetTmCourseResultData), Resources.GetString(Resource.String.TmCourseResultActivity), null);
                }
            }
            finally
            {
                if (overlay != null)
                {
                    overlay.Dismiss();
                }
            }
        }

        /// <summary>
        /// Method implementation to get Territory Superintendent search result data
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public async Task GetTmSuperintendentResultData(string tmUserId)
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
                    }
                    else
                    {
                        tmCourseResultResponseEntity = await util.GetTmSuperIntendantSearchResult(token, tmUserId);
                        if (tmCourseResultResponseEntity != null)
                        {
                            txtTmCourseName.Text = tmCourseResultResponseEntity.tmCoursesVm.courseName;
                            txtViewAddressValue.Text = tmCourseResultResponseEntity.tmCoursesVm.address;
                            txtViewCityValue.Text = tmCourseResultResponseEntity.tmCoursesVm.city;
                            txtViewStateValue.Text = tmCourseResultResponseEntity.tmCoursesVm.state;
                            txtViewZipValue.Text = tmCourseResultResponseEntity.tmCoursesVm.zip;
                            txtViewCountryValue.Text = tmCourseResultResponseEntity.tmCoursesVm.country;
                            txtHeading.Text = "Course Search Result";
                            if (tmCourseResultResponseEntity.tmDistributorProductVm != null)
                            {
                                // removing this functionality because of business requirements 

                                //if (tmCourseResultResponseEntity.tmDistributorProductVm.Count != 0)
                                //{
                                //    txtNorecord.Visibility = ViewStates.Gone;
                                //    lvTmProductlist.Visibility = ViewStates.Visible;
                                //    TmCourseResultAdapter adapter = new TmCourseResultAdapter(this, tmCourseResultResponseEntity.tmDistributorProductVm);
                                //    lvTmProductlist.Adapter = adapter;
                                //}
                                //else
                                //{
                                //    txtNorecord.Visibility = ViewStates.Visible;
                                //    lvTmProductlist.Visibility = ViewStates.Gone;
                                //}
                            }
                            else
                            {
                                txtNorecord.Visibility = ViewStates.Visible;
                                lvTmProductlist.Visibility = ViewStates.Gone;
                            }
                        }
                        else
                        {
                            txtNorecord.Visibility = ViewStates.Visible;
                            lvTmProductlist.Visibility = ViewStates.Gone;
                            Toast.MakeText(this, Resource.String.NoRecord, ToastLength.Long).Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetTmSuperintendentResultData), Resources.GetString(Resource.String.TmCourseResultActivity), null);
                }
            }
            finally
            {
                if (overlay != null)
                {
                    overlay.Dismiss();
                }
            }
        }

        /// <summary>
        /// Method implementation to get Territory Distributor search result data
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public async Task GetTmDistributorResultData(string tmDistributorID, string tmState)
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
                    }
                    else
                    {
                        TmDistributorResultRequestEntity TmDistributorResultRequestEntity = new TmDistributorResultRequestEntity();
                        TmDistributorResultRequestEntity.distributorName = tmDistributorId;
                        TmDistributorResultRequestEntity.state = tmState;
                        TmDistributorResultResponse = await util.GetTmDistributorSearchResultData(TmDistributorResultRequestEntity, token);
                        txtHeading.Text = Resources.GetString(Resource.String.HeadingtxtValue);
                        if (TmDistributorResultResponse != null)
                        {
                            // removing this functionality because of business requirements 

                            //if (TmDistributorResultResponse.tmCourseProductVm.Count > 0)
                            //{
                            //    LinearDownload.Visibility = ViewStates.Visible;
                            //    if (TmDistributorResultResponse.tmDistributors != null)
                            //    {
                            //        txtTmCourseName.Text = TmDistributorResultResponse.tmDistributors.distributorName;
                            //        txtViewAddressValue.Text = TmDistributorResultResponse.tmDistributors.address;
                            //        txtViewCityValue.Text = TmDistributorResultResponse.tmDistributors.city;
                            //        txtViewStateValue.Text = TmDistributorResultResponse.tmDistributors.state;
                            //        txtViewZipValue.Text = TmDistributorResultResponse.tmDistributors.zip;
                            //        txtViewCountryValue.Text = TmDistributorResultResponse.tmDistributors.country;
                            //    }
                            //    txtNorecord.Visibility = ViewStates.Gone;
                            //    lvTmProductlist.Visibility = ViewStates.Visible;
                            //    TmDistributorResultAdapter adapter = new TmDistributorResultAdapter(this, TmDistributorResultResponse.tmCourseProductVm);
                            //    lvTmProductlist.Adapter = adapter;
                            //}
                            //else
                            //{
                            //    LinearDownload.Visibility = ViewStates.Gone;
                            //    txtNorecord.Visibility = ViewStates.Visible;
                            //    lvTmProductlist.Visibility = ViewStates.Gone;
                            //}
                        }
                        else
                        {
                            LinearDownload.Visibility = ViewStates.Gone;
                            txtNorecord.Visibility = ViewStates.Visible;
                            lvTmProductlist.Visibility = ViewStates.Gone;
                            Toast.MakeText(this, Resource.String.NoRecord, ToastLength.Long).Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetTmDistributorResultData), Resources.GetString(Resource.String.TmCourseResultActivity), null);
                }
            }
            finally
            {
                if (overlay != null)
                {
                    overlay.Dismiss();
                }
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.TmCourseResultActivity), null);
                }
            }
        }

        /// <summary>
        /// On click listener
        /// </summary>
        /// <param name="v"></param>
        public void OnClick(View v)
        {
            try
            {
                switch (v.Id)
                {
                    case Resource.Id.imgBack:
                        Finish();
                        break;
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnClick), Resources.GetString(Resource.String.TmCourseResultActivity), null);
                }
            }
        }
    }
}