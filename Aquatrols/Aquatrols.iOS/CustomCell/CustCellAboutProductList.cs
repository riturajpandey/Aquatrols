using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using Foundation;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellAboutProductList : UITableViewCell
    {
        private ProductListEntity productListEntity;
        private AboutController aboutcontroller;
        private NSData nsDataProductLogo;
        private string productId;
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.CustCellAboutProductList"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustCellAboutProductList (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Update the cell Product List.
        /// bind the data in cell
        /// </summary>
        /// <param name="p">P.</param>
        /// <param name="controller">Controller.</param>
        internal void UpdateCell(ProductListEntity p,NSData data, AboutController controller)
        {
            aboutcontroller = controller;
            productListEntity = p;
            nsDataProductLogo = data;
            try
            {
                if (!String.IsNullOrEmpty(productListEntity.productLogo))
                {
                    if(nsDataProductLogo!=null)
                    {
                        imgProductlist.Image = UIImage.LoadFromData(nsDataProductLogo); 
                    }
                    productId = productListEntity.productId;;
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateCell, AppResources.CustCellAboutProductList, null);
            }
        }
        /// <summary>
        /// Gesture implementation for productlist click to show product detail based on product id.
        /// </summary>
        public void ImgProductlistClick()
        {
            UITapGestureRecognizer imgProductlistClicked = new UITapGestureRecognizer(() =>
            {
                aboutcontroller.ProductDetail(productId);
            });
            imgProductlist.UserInteractionEnabled = true;
            imgProductlist.AddGestureRecognizer(imgProductlistClicked);
        }
    }
}