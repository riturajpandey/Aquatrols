using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.Content;
using Android.Widget;
using Aquatrols.Droid.Adapter;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Aquatrols.Droid.Activities
{
    /// <summary>
    /// This class is used for snapshot screen
    /// </summary>
    [Activity]
    public class TmSnapshotActivity : Activity
    {
        private ListView lvTotalSignups, lvCommitmentsPerDistributor, lvTotalCommitments;
        private Button btnDownload, btnShare;
        private ImageView imgBack;
        private string token;

        /// <summary>
        /// This method is used to initialize page load value and show TotalSignupsandCommitments, TotalCommitmentsPerDistributor and TotalCommitmentsPerCountry data
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.TerritorySnapshotLayout);
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
                await GetTotalSignupsandCommitments();
                await GetTotalCommitmentsPerDistributor();
                await GetTotalCommitmentsPerCountry();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreate), Resources.GetString(Resource.String.TmSnapshotActivity), null);
                }
            }
        }
        /// <summary>
        /// Getting the reference of controls
        /// </summary>
        public void FindControlsById()
        {
            try
            {
                lvTotalSignups = FindViewById<ListView>(Resource.Id.lvTotalSignups);
                lvCommitmentsPerDistributor = FindViewById<ListView>(Resource.Id.lvCommitmentsPerDistributor);
                lvTotalCommitments = FindViewById<ListView>(Resource.Id.lvTotalCommitments);
                btnDownload = FindViewById<Button>(Resource.Id.btnDownload);
                btnShare = FindViewById<Button>(Resource.Id.btnShare);
                imgBack = FindViewById<ImageView>(Resource.Id.imgBack);
                btnDownload.Click += BtnDownload_Click;
                btnShare.Click += BtnShare_Click;
                imgBack.Click += ImgBack_Click;
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.FindControlsById), Resources.GetString(Resource.String.TmSnapshotActivity), null);
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
        /// Back button click to go back
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgBack_Click(object sender, EventArgs e)
        {
            Finish();
        }

        /// <summary>
        /// download button click handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDownload_Click(object sender, EventArgs e)
        {
            GetTerritorySnapshotExcelUrl();
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
                Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.TmSnapShotFileName), Path.GetFileNameWithoutExtension(url) + Path.GetExtension(url).Replace("\"", string.Empty)).Commit();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.StartNewDownload), Resources.GetString(Resource.String.TmSnapshotActivity), null);
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
                        string data = await utility.GetTerritorySnapshotExcelUrl(token, snapShotRequestEntity);
                        if (!string.IsNullOrEmpty(data))
                        {
                            // download code
                            string newUrl = data.Remove(0, 1);
                            newUrl = newUrl.Remove(newUrl.Length - 1, 1);
                            Java.IO.File file = new Java.IO.File(Android.OS.Environment.DirectoryDownloads);
                            DownloadManager.Request request = new DownloadManager.Request(Android.Net.Uri.Parse(newUrl)); /*init a request*/
                            request.SetTitle(Path.GetFileNameWithoutExtension(data));//this description apears in the android notification   
                            request.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
                            request.SetDestinationInExternalPublicDir(
                                 file.Path,
                                    Path.GetFileNameWithoutExtension(data) + Path.GetExtension(data).Replace("\"", string.Empty)); //set destination
                                                                                                                                   // OR
                            DownloadManager manager = (DownloadManager)this.GetSystemService(DownloadService);
                            long downloadId = manager.Enqueue(request); //start the download and return the id of the download. this id can be used to get info about the file (the size, the download progress ...) you can also stop the download by using this id                 
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.TmSnapShotFileName), Path.GetFileNameWithoutExtension(data) + Path.GetExtension(data).Replace("\"", string.Empty)).Commit();
                            // share code
                            await Task.Delay(2000);
                            string fileName = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.TmSnapShotFileName), null);
                            var filep = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads + "/" + fileName);
                            var fileToAttach = FileProvider.GetUriForFile(this, this.PackageName + Resources.GetString(Resource.String.fileprovider) , filep);
                            Intent emailIntent = new Intent(Intent.ActionSend);
                            emailIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
                            emailIntent.AddFlags(ActivityFlags.GrantWriteUriPermission);
                            // set the type to 'email'
                            emailIntent.SetType("application/excel");
                            emailIntent.PutExtra(Intent.ExtraEmail, string.Empty);
                            // the attachment
                            emailIntent.PutExtra(Intent.ExtraStream, fileToAttach);
                            // the mail subject
                            emailIntent.PutExtra(Intent.ExtraSubject, string.Empty);
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.AttachFile), Resources.GetString(Resource.String.TmSnapshotActivity), null);
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
        public async void GetTerritorySnapshotExcelUrl()
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
                            string data = await utility.GetTerritorySnapshotExcelUrl(token, snapShotRequestEntity);
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetTerritorySnapshotExcelUrl), Resources.GetString(Resource.String.TmSnapshotActivity), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// Method to get total number of signups and commitments
        /// </summary>
        public async Task GetTotalSignupsandCommitments()
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
                            List<TmSnapshotSignUpCountVm> lstTotalSignups = await utility.GetTmTotalSignupAndCommitments(token);
                            if (lstTotalSignups != null)
                            {
                                lvTotalSignups.Adapter = new TerritoryTotalSignupAdapter(this, lstTotalSignups);
                            }
                            else
                            {
                                Toast.MakeText(this, Resources.GetString(Resource.String.NoRecord), ToastLength.Long).Show();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetTotalSignupsandCommitments), Resources.GetString(Resource.String.TmSnapshotActivity), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// Method to get total number of commitments per country
        /// </summary>
        public async Task GetTotalCommitmentsPerCountry()
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
                            List<TmTotalCommitments> tmTotalCommitments = await utility.GetTmTotalCommitmentsPerRegion(token);
                            if (tmTotalCommitments != null)
                            {
                                var adapter = new TerritoryTotalCommitAdapter(this, tmTotalCommitments);
                                lvTotalCommitments.Adapter = adapter;
                            }
                            else
                            {
                                Toast.MakeText(this, Resources.GetString(Resource.String.NoRecord), ToastLength.Long).Show();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetTotalCommitmentsPerCountry), Resources.GetString(Resource.String.TmSnapshotActivity), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// Method to get total number of commitments per Distributor
        /// </summary>
        public async Task GetTotalCommitmentsPerDistributor()
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
                            List<TmCommitmentsPerDistributor> tmCommitmentsPerDistributors = await utility.GetTmTotalCommitmentsPerDistributor(token);
                            if (tmCommitmentsPerDistributors != null)
                            {
                                var adapter = new TerritoryTotalCommitPerDistributorAdapter(this, tmCommitmentsPerDistributors);
                                lvCommitmentsPerDistributor.Adapter = adapter;
                            }
                            else
                            {
                                Toast.MakeText(this, Resources.GetString(Resource.String.NoRecord), ToastLength.Long).Show();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetTotalCommitmentsPerDistributor), Resources.GetString(Resource.String.TmSnapshotActivity), null);
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.TmSnapshotActivity), null);
                }
            }
        }
    }
}