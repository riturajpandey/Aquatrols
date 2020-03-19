using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Helper;
using System;

namespace Aquatrols.Droid.Fragments
{
    /// <summary>
    /// This class is used to show Territory Manager detail
    /// </summary>
    public class TerritoryManagerFragment : Android.Support.V4.App.Fragment
    {
        private MainActivity mainActivity;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        /// <summary>
        /// This method is used to initialize page load value
        /// </summary>
        /// <param name="mainActivity"></param>
        public TerritoryManagerFragment(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        /// <summary>
        /// This method is used to initialize page load value
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View root = inflater.Inflate(Resource.Layout.TerritoryManagerLayout, container, false);
            try
            {
                LinearLayout iiUserInfo = mainActivity.FindViewById<LinearLayout>(Resource.Id.llUserInfo);
                LinearLayout llTextMessage = mainActivity.FindViewById<LinearLayout>(Resource.Id.llTextMessage);
                TextView txtMessage = mainActivity.FindViewById<TextView>(Resource.Id.txtMessage);
                llTextMessage.Visibility = ViewStates.Visible;
                iiUserInfo.Visibility = ViewStates.Visible;
                txtMessage.Text = this.mainActivity.Resources.GetString(Resource.String.territorrymanagerInfotext);
                TextView KMauserEmail = root.FindViewById<TextView>(Resource.Id.kmauserEmail);
                TextView JTurnerEmail = root.FindViewById<TextView>(Resource.Id.jturnerEmail);
                TextView RWilsonEmail = root.FindViewById<TextView>(Resource.Id.rwilsonEmail);
                TextView GLovellEmail = root.FindViewById<TextView>(Resource.Id.glovellEmail);
                TextView SPoynotEmail = root.FindViewById<TextView>(Resource.Id.spoynotEmail);
                TextView TValentineEmail = root.FindViewById<TextView>(Resource.Id.tvalentineEmail);
                TextView WHammEmail = root.FindViewById<TextView>(Resource.Id.whammEmail);
                TextView WDeaEmail = root.FindViewById<TextView>(Resource.Id.WDeaEmail);
                TextView txtKMauserMobile = root.FindViewById<TextView>(Resource.Id.txtKMauserMobile);
                TextView txtJTurnerEmail = root.FindViewById<TextView>(Resource.Id.txtJTurnerMobile);
                TextView txtRWilsonMobile = root.FindViewById<TextView>(Resource.Id.txtRWilsonMobile);
                TextView txtGLovellMobile = root.FindViewById<TextView>(Resource.Id.txtGLovellMobile);
                TextView txtSPoynotMobile = root.FindViewById<TextView>(Resource.Id.txtSPoynotMobile);
                TextView txtTValentineMobile = root.FindViewById<TextView>(Resource.Id.txtTValentineMobile);
                TextView txtWHammMobile = root.FindViewById<TextView>(Resource.Id.txtWHammMobile);
                TextView txtPhoneWDea = root.FindViewById<TextView>(Resource.Id.txtPhoneWDea);
                TextView txtMobileWDea = root.FindViewById<TextView>(Resource.Id.txtMobileWDea);

                #region //Textview Click Event handlers
                KMauserEmail.Click += KmauserEmail_Click;
                JTurnerEmail.Click += JturnerEmail_Click;
                RWilsonEmail.Click += RwilsonEmail_Click;
                GLovellEmail.Click += GlovellEmail_Click;
                SPoynotEmail.Click += SpoynotEmail_Click;
                TValentineEmail.Click += TvalentineEmail_Click;
                WHammEmail.Click += WhammEmail_Click;
                WDeaEmail.Click += WDeaEmail_Click;
                txtKMauserMobile.Click += TxtKMauserMobile_Click; ;
                txtJTurnerEmail.Click += TxtJTurnerEmail_Click;
                txtRWilsonMobile.Click += TxtRWilsonMobile_Click;
                txtGLovellMobile.Click += TxtGLovellMobile_Click;
                txtSPoynotMobile.Click += TxtSPoynotMobile_Click;
                txtTValentineMobile.Click += TxtTValentineMobile_Click;
                txtWHammMobile.Click += TxtWHammMobile_Click;
                txtPhoneWDea.Click += TxtPhoneWDea_Click;
                txtMobileWDea.Click += TxtMobileWDea_Click;
                #endregion
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreateView), Resources.GetString(Resource.String.TerritoryManagerFragment), null);
                }
            }
            return root;
        }

        /// <summary>
        /// On Wdea phone number link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtMobileWDea_Click(object sender, EventArgs e)
        {
            try
            {
                TextView txtMobileWDea = sender as TextView;
                Intent intent = new Intent(Intent.ActionDial, Android.Net.Uri.Parse("tel:" + txtMobileWDea.Text));
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.TxtMobileWDea_Click), Resources.GetString(Resource.String.TerritoryManagerFragment), null);
                }
            }
        }

        /// <summary>
        /// On Whamm phone number link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtWHammMobile_Click(object sender, EventArgs e)
        {
            try
            {
                TextView txtMobileWHamm = sender as TextView;
                Intent intent = new Intent(Intent.ActionDial, Android.Net.Uri.Parse("tel:" + txtMobileWHamm.Text));
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.TxtWHammMobile_Click), Resources.GetString(Resource.String.TerritoryManagerFragment), null);
                }
            }
        }

        /// <summary>
        /// On Tvalentine phone number link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtTValentineMobile_Click(object sender, EventArgs e)
        {
            try
            {
                TextView txtMobileWTVal = sender as TextView;
                Intent intent = new Intent(Intent.ActionDial, Android.Net.Uri.Parse(Resources.GetString(Resource.String.Tel) + txtMobileWTVal.Text));
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.TxtTValentineMobile_Click), Resources.GetString(Resource.String.TerritoryManagerFragment), null);
                }
            }
        }

        /// <summary>
        /// On SPynot phone number link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtSPoynotMobile_Click(object sender, EventArgs e)
        {
            try
            {
                TextView txtMobileSPynot = sender as TextView;
                Intent intent = new Intent(Intent.ActionDial, Android.Net.Uri.Parse(Resources.GetString(Resource.String.Tel) + txtMobileSPynot.Text));
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.TxtSPoynotMobile_Click), Resources.GetString(Resource.String.TerritoryManagerFragment), null);
                }
            }
        }

        /// <summary>
        /// On SPynot phone number link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtGLovellMobile_Click(object sender, EventArgs e)
        {
            try
            {
                TextView txtMobileGLovell = sender as TextView;
                Intent intent = new Intent(Intent.ActionDial, Android.Net.Uri.Parse(Resources.GetString(Resource.String.Tel) + txtMobileGLovell.Text));
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.TxtGLovellMobile_Click), Resources.GetString(Resource.String.TerritoryManagerFragment), null);
                }
            }
        }

        /// <summary>
        /// On RWilson phone number link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtRWilsonMobile_Click(object sender, EventArgs e)
        {
            try
            {
                TextView txtMobileRWilson = sender as TextView;
                Intent intent = new Intent(Intent.ActionDial, Android.Net.Uri.Parse(Resources.GetString(Resource.String.Tel) + txtMobileRWilson.Text));
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.TxtRWilsonMobile_Click), Resources.GetString(Resource.String.TerritoryManagerFragment), null);
                }
            }
        }

        /// <summary>
        /// On RWilson phone number link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtJTurnerEmail_Click(object sender, EventArgs e)
        {
            try
            {
                TextView txtMobileJTurner = sender as TextView;
                Intent intent = new Intent(Intent.ActionDial, Android.Net.Uri.Parse(Resources.GetString(Resource.String.Tel) + txtMobileJTurner.Text));
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.TxtJTurnerEmail_Click), Resources.GetString(Resource.String.TerritoryManagerFragment), null);
                }
            }
        }

        /// <summary>
        /// On Kmauser phone number link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtKMauserMobile_Click(object sender, EventArgs e)
        {
            try
            {
                TextView txtMobileKMaucer = sender as TextView;
                Intent intent = new Intent(Intent.ActionDial, Android.Net.Uri.Parse(Resources.GetString(Resource.String.Tel) + txtMobileKMaucer.Text));
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.TxtKMauserMobile_Click), Resources.GetString(Resource.String.TerritoryManagerFragment), null);
                }
            }
        }

        /// <summary>
        /// On DMacius phone number link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtDMaciusPhone_Click(object sender, EventArgs e)
        {
            try
            {
                TextView txtMobileDMacius = sender as TextView;
                Intent intent = new Intent(Intent.ActionDial, Android.Net.Uri.Parse("tel:" + txtMobileDMacius.Text));
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.TxtDMaciusPhone_Click), Resources.GetString(Resource.String.TerritoryManagerFragment), null);
                }
            }
        }

        /// <summary>
        /// On Wdea phone number link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtPhoneWDea_Click(object sender, EventArgs e)
        {
            try
            {
                TextView txtPhoneWDea = sender as TextView;
                Intent intent = new Intent(Intent.ActionDial, Android.Net.Uri.Parse(Resources.GetString(Resource.String.Tel) + txtPhoneWDea.Text));
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.TxtPhoneWDea_Click), Resources.GetString(Resource.String.TerritoryManagerFragment), null);
                }
            }
        }

        /// <summary>
        /// On Wdea Email link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WDeaEmail_Click(object sender, EventArgs e)
        {
            TextView txt = sender as TextView;
            OpenEmailApp(txt.Text);
        }

        /// <summary>
        /// On Whamm Email link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WhammEmail_Click(object sender, EventArgs e)
        {
            TextView txt = sender as TextView;
            OpenEmailApp(txt.Text);
        }

        /// <summary>
        /// On Tvalentine Email link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TvalentineEmail_Click(object sender, EventArgs e)
        {
            TextView txt = sender as TextView;
            OpenEmailApp(txt.Text);
        }

        /// <summary>
        /// On Spoynot Email link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpoynotEmail_Click(object sender, EventArgs e)
        {
            TextView txt = sender as TextView;
            OpenEmailApp(txt.Text);
        }

        /// <summary>
        /// On Glovell Email link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GlovellEmail_Click(object sender, EventArgs e)
        {
            TextView txt = sender as TextView;
            OpenEmailApp(txt.Text);
        }

        /// <summary>
        /// On Rwilson Email link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RwilsonEmail_Click(object sender, EventArgs e)
        {
            TextView txt = sender as TextView;
            OpenEmailApp(txt.Text);
        }

        /// <summary>
        /// On Jturner Email link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JturnerEmail_Click(object sender, EventArgs e)
        {
            TextView txt = sender as TextView;
            OpenEmailApp(txt.Text);
        }

        /// <summary>
        /// On Kmauser Email link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KmauserEmail_Click(object sender, EventArgs e)
        {
            TextView txt = sender as TextView;
            OpenEmailApp(txt.Text);
        }

        /// <summary>
        /// On Dmacius Email link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DmaciusEmail_Click(object sender, EventArgs e)
        {
            TextView txt = sender as TextView;
            OpenEmailApp(txt.Text);
        }

        /// <summary>
        /// Function to open Email when click on email address link
        /// </summary>
        /// <param name="text"></param>
        public void OpenEmailApp(string text)
        {
            try
            {
                Intent intent = new Intent(Intent.ActionSend);
                intent.SetType("text/html");
                intent.PutExtra(Intent.ExtraEmail, new String[] { text });
                intent.PutExtra(Intent.ExtraSubject, string.Empty);
                StartActivity(Intent.CreateChooser(intent, Resources.GetString(Resource.String.sendmail)));
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OpenEmailApp), Resources.GetString(Resource.String.TerritoryManagerFragment), null);
                }
            }
        }

    }
}