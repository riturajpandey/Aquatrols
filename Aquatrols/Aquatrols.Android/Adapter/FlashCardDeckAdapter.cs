using Android.Content.Res;
using Android.Support.V4.App;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Fragments;
using Aquatrols.Models;
using System.Collections.Generic;

namespace Aquatrols.Droid.Adapter
{
    /// <summary>
    /// This Class is used to bind Product List data in FlashCardDeckAdapter
    /// </summary>
    class FlashCardDeckAdapter : FragmentPagerAdapter
    {
        public List<ProductListEntity> flashCardDeck;
        MainActivity mainActivity;

        /// <summary>
        /// This method is used to get detail of main activity to add product list detail
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="flashCards"></param>
        /// <param name="mainActivity"></param>
        public FlashCardDeckAdapter(Android.Support.V4.App.FragmentManager fm, List<ProductListEntity> flashCards,MainActivity mainActivity)
            : base(fm)
        {
            this.mainActivity = mainActivity;
            this.flashCardDeck = flashCards;
        }

        /// <summary>
        /// Fill in cound here, currently 0
        /// </summary>
        public override int Count
        {
            get { return flashCardDeck.Count; }
        }

        /// <summary>
        /// This method is used to get the position of product
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return (Android.Support.V4.App.Fragment)
                FlashCardFragment.newInstance(new string[] { this.flashCardDeck[position].productDescription, this.flashCardDeck[position].sds, this.flashCardDeck[position].labelPermaLink, this.flashCardDeck[position].productWebpage, this.flashCardDeck[position].productImage, this.flashCardDeck[position].productName,this.flashCardDeck[position].rates , this.flashCardDeck[position].productLogo },this.mainActivity);
        }

        /// <summary>
        /// This method is used to get the particular position of product
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(this.mainActivity.Resources.GetString(Resource.String.Problem) + (position + 1));
        }
    }
}