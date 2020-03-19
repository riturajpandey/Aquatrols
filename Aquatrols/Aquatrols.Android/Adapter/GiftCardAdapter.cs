using Android.App;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Fragments;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using Square.Picasso;
using System;
using System.Collections.Generic;

namespace Aquatrols.Droid.Adapter
{
    /// <summary>
    /// This Class is used to show redeem screen
    /// </summary>
    public class GiftCardAdapter : RecyclerView.Adapter
    {
        public MainActivity Context;
        public List<RedeemGiftCardEntity> lstGiftCard;
        public TextView txtCount;
        public string userId = string.Empty, country = string.Empty;
        public int count = 0;

        /// <summary>
        /// This method is used to hod redeem detail
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mlstGiftCard"></param>
        public GiftCardAdapter(MainActivity context, List<RedeemGiftCardEntity> mlstGiftCard)
        {
            Context = context;
            lstGiftCard = mlstGiftCard;
        }

        /// <summary>
        /// Fill in cound here, currently 0
        /// </summary>
        public override int ItemCount
        {
            get { return lstGiftCard.Count; }
        }

        /// <summary>
        /// This method is used to show redeem deatil in view
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View row = LayoutInflater.FromContext(Context).Inflate(Resource.Layout.RedeemRowLayout, null, false);
            GiftCardAdapterViewHolder myViewHolder = new GiftCardAdapterViewHolder(row, this);
            if (count == 0)
            {
                count++;
                myViewHolder.CheckUser_LoggedInAsManager();
            }
            return myViewHolder;
        }

        /// <summary>
        /// To ind redeem record in view
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="position"></param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            GiftCardAdapterViewHolder viewHolder = (GiftCardAdapterViewHolder)holder;
            viewHolder.Bind(position);
        }
    }

    /// <summary>
    /// This class is used to hold detail of redeem
    /// </summary>
    public class GiftCardAdapterViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
    {
        GiftCardAdapter adapter;
        private int position;
        string userId = string.Empty;
        string token = string.Empty;
        string point = string.Empty;
        public ImageView imgGiftCardLogo { get; set; }
        public TextView txtProductInfo { get; set; }
        public TextView txtProductDesc { get; set; }
        public TextView txtAmounttext { get; set; }
        public TextView txtInfotext { get; set; }
        public Button btnRedeem { get; set; }
        public EditText editAmount { get; set; }
        public TextInputLayout txtInputLayoutAmount { get; set; }
        public LinearLayout llContainer { get; set; }

        /// <summary>
        /// Getting the reference of controls
        /// </summary>
        /// <param name="view"></param>
        /// <param name="adapter"></param>
        public GiftCardAdapterViewHolder(View view, GiftCardAdapter adapter) : base(view)
        {
            try
            {
                this.adapter = adapter;
                imgGiftCardLogo = view.FindViewById<ImageView>(Resource.Id.imgGiftCardLogo);
                txtProductInfo = view.FindViewById<TextView>(Resource.Id.txtProductInfo);
                txtProductDesc = view.FindViewById<TextView>(Resource.Id.txtProductDesc);
                txtInfotext = view.FindViewById<TextView>(Resource.Id.txtInfotext);
                txtAmounttext = view.FindViewById<TextView>(Resource.Id.txtAmounttext);
                btnRedeem = view.FindViewById<Button>(Resource.Id.btnRedeem);
                editAmount = view.FindViewById<EditText>(Resource.Id.editAmount);
                llContainer = view.FindViewById<LinearLayout>(Resource.Id.llContainer);
                txtInputLayoutAmount = view.FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutAmount);
                btnRedeem.SetOnClickListener(this);
                editAmount.AddTextChangedListener(new CustomTextWatcher(editAmount, adapter.Context));
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(adapter.Context))
                {
                    utility.SaveExceptionHandling(ex, adapter.Context.Resources.GetString(Resource.String.GiftCardAdapterViewHolder), adapter.Context.Resources.GetString(Resource.String.GiftCardAdapter), null);
                }
            }
        }

        /// <summary>
        /// Preventing user to Redeem Points if not loggedIn as manager
        /// </summary>
        public void CheckUser_LoggedInAsManager()
        {
            try
            {
                string role = Utility.sharedPreferences.GetString(adapter.Context.Resources.GetString(Resource.String.Role), null);
                if (!string.IsNullOrEmpty(role))
                {
                    if (!role.ToLower().Equals(adapter.Context.Resources.GetString(Resource.String.Manager)))
                    {
                        editAmount.Visibility = ViewStates.Gone;
                        btnRedeem.Visibility = ViewStates.Gone;
                        txtAmounttext.Visibility = ViewStates.Gone;
                        Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(adapter.Context, Android.Resource.Style.ThemeDeviceDefaultDialogAlert);
                        alert.SetTitle(adapter.Context.Resources.GetString(Resource.String.message));
                        alert.SetMessage(adapter.Context.Resources.GetString(Resource.String.usercannotbook));
                        alert.SetPositiveButton(adapter.Context.Resources.GetString(Resource.String.Ok), (senderAlert, args) =>
                        {
                        });
                        Dialog dialog = alert.Create();
                        dialog.Show();
                    }
                    else
                    {
                        editAmount.Visibility = ViewStates.Visible;
                        btnRedeem.Visibility = ViewStates.Visible;
                        txtAmounttext.Visibility = ViewStates.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(adapter.Context))
                {
                    utility.SaveExceptionHandling(ex, adapter.Context.Resources.GetString(Resource.String.CheckUser_LoggedInAsManager), adapter.Context.Resources.GetString(Resource.String.GiftCardAdapter), null);
                }
            }
        }

        /// <summary>
        /// Binding Data to view holder
        /// </summary>
        /// <param name="position"></param>
        public void Bind(int position)
        {
            try
            {
                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(adapter.Context.Resources.GetString(Resource.String.UserId), null)))
                {
                    userId = Utility.sharedPreferences.GetString(adapter.Context.Resources.GetString(Resource.String.UserId), null);
                }
                point = Utility.sharedPreferences.GetString(adapter.Context.Resources.GetString(Resource.String.BalancePoint), null);
                this.position = position;
                string image = adapter.lstGiftCard[position].itemImage.Replace("\\", "//");
                Android.Net.Uri url = Android.Net.Uri.Parse(adapter.Context.Resources.GetString(Resource.String.baseurl) + image);
                Picasso.With(adapter.Context).Load(url).Into(imgGiftCardLogo);
                txtProductInfo.Text = adapter.lstGiftCard[position].rewardItem;
                txtProductDesc.Text = adapter.lstGiftCard[position].description;
                txtInfotext.Text = adapter.lstGiftCard[position].minimumPoints + " Points = $" + adapter.lstGiftCard[position].pointPricePerUnit + " " + adapter.lstGiftCard[position].itemType;
                txtAmounttext.Text = adapter.lstGiftCard[position].labelForAmount;
                if (position % 2 == 0)
                {
                    llContainer.SetBackgroundResource(Resource.Color.white);
                }
                else
                {
                    llContainer.SetBackgroundResource(Resource.Color.lightgrey);
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(adapter.Context))
                {
                    utility.SaveExceptionHandling(ex, adapter.Context.Resources.GetString(Resource.String.Bind), adapter.Context.Resources.GetString(Resource.String.GiftCardAdapter), null);
                }
            }
        }

        /// <summary>
        /// Method to handle Redeem button click
        /// </summary>
        public void HandleRedeemButtonclick()
        {
            try
            {
                if (!string.IsNullOrEmpty(editAmount.Text))
                {
                    int enteredPoint = 0;
                    if (!string.IsNullOrEmpty(point))
                    {
                        bool flag = int.TryParse(editAmount.Text.Replace(",", ""), out int amount);
                        if (flag == true)
                        {
                            enteredPoint = amount;
                            if (enteredPoint > 0)
                            {
                                int availablePoint = Convert.ToInt32(point);
                                if (enteredPoint <= availablePoint)
                                {
                                    if (enteredPoint % adapter.lstGiftCard[position].minimumPoints == 0)
                                    {
                                        txtInputLayoutAmount.ErrorEnabled = false;
                                        HitRedeemPointsAPI(userId, adapter.lstGiftCard[position].rewardId, adapter.lstGiftCard[position].rewardItem, adapter.lstGiftCard[position].minimumPoints, adapter.lstGiftCard[position].pointPricePerUnit, enteredPoint);
                                    }
                                    else
                                    {
                                        Toast.MakeText(adapter.Context, adapter.Context.Resources.GetString(Resource.String.enterMultiplierpoints) + " " + adapter.lstGiftCard[position].minimumPoints, ToastLength.Long).Show();
                                    }
                                }
                                else
                                {
                                    txtInputLayoutAmount.ErrorEnabled = false;
                                    Toast.MakeText(adapter.Context, adapter.Context.Resources.GetString(Resource.String.notEnoughPoints), ToastLength.Long).Show();
                                }
                            }
                            else
                            {
                                Toast.MakeText(adapter.Context, adapter.Context.Resources.GetString(Resource.String.enterMultiplierpoints) + " " + adapter.lstGiftCard[position].minimumPoints, ToastLength.Long).Show();
                            }
                        }
                        else
                        {
                            Toast.MakeText(adapter.Context, adapter.Context.Resources.GetString(Resource.String.enterValidAmount), ToastLength.Long).Show();
                        }
                    }
                }
                else
                {
                    txtInputLayoutAmount.Error = adapter.Context.Resources.GetString(Resource.String.Required);
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(adapter.Context))
                {
                    utility.SaveExceptionHandling(ex, adapter.Context.Resources.GetString(Resource.String.HandleRedeemButtonclick), adapter.Context.Resources.GetString(Resource.String.GiftCardAdapter), null);
                }
            }
        }

        /// <summary>
        /// Method to call Redeem points API.
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="pointsSpent"></param>
        public async void HitRedeemPointsAPI(string UserId, string rewardItemId, string rewardItemName, int minBal, int pointePerUnit, int totalPointsSPent)
        {
            try
            {
                Show_Overlay();
                using (Utility utility = new Utility(adapter.Context))
                {
                    bool internetStatus = utility.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(adapter.Context, Resource.String.NoInternet, ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        string token = string.Empty;
                        if (Utility.sharedPreferences.GetString(adapter.Context.Resources.GetString(Resource.String.token), null) != null)
                        {
                            token = Utility.sharedPreferences.GetString(adapter.Context.Resources.GetString(Resource.String.token), null);
                        }
                        RedeemPointEntity redeemPointEntity = new RedeemPointEntity();
                        redeemPointEntity.pointsSpentBy = UserId;
                        redeemPointEntity.minimumBalance = minBal;
                        redeemPointEntity.pointsPricePerUnit = pointePerUnit;
                        redeemPointEntity.rewardItem = rewardItemName;
                        redeemPointEntity.rewardItemId = rewardItemId;
                        redeemPointEntity.totalPointsSpent = totalPointsSPent;
                        RedeemPointResponseEntity redeemPointResponse = await utility.RedeemPoints(redeemPointEntity, token);
                        if (redeemPointResponse.operationStatus.ToLower().Equals(adapter.Context.Resources.GetString(Resource.String.success)))
                        {
                            Android.Support.V4.App.Fragment fragment = new RedeemConfirmFragment(adapter.Context);
                            adapter.Context.SupportFragmentManager.BeginTransaction()
                            .Replace(Resource.Id.content_frame, fragment)
                            .Commit();
                        }
                        else
                        {
                            Toast.MakeText(adapter.Context, redeemPointResponse.operationMessage, ToastLength.Long).Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(adapter.Context))
                {
                    utility.SaveExceptionHandling(ex, adapter.Context.Resources.GetString(Resource.String.HitRedeemPointsAPI), adapter.Context.Resources.GetString(Resource.String.GiftCardAdapter), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        OverlayActivity overlay;
        /// <summary>
        /// Showing overlay screen
        /// </summary>
        public void Show_Overlay()
        {
            try
            {
                overlay = new OverlayActivity(adapter.Context);
                overlay.Show();
                overlay.SetCanceledOnTouchOutside(false);
                overlay.SetCancelable(false);
            }
            catch (System.Exception ex)
            {
                using (Utility utility = new Utility(adapter.Context))
                {
                    utility.SaveExceptionHandling(ex, adapter.Context.Resources.GetString(Resource.String.Show_Overlay), adapter.Context.Resources.GetString(Resource.String.GiftCardAdapter), null);
                }
            }
        }

        /// <summary>
        /// On Click Events
        /// </summary>
        /// <param name="v"></param>
        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.btnRedeem:
                    HandleRedeemButtonclick();
                    break;
                default:
                    break;
            }
        }
    }
}