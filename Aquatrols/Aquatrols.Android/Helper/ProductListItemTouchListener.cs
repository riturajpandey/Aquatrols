using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Aquatrols.Droid.Adapter;

namespace Aquatrols.Droid.Helper
{
    /// <summary>
    /// This class file product list item touch listner is used for product list
    /// </summary>
    class ProductListItemTouchListener: ItemTouchHelper.SimpleCallback
    {
        private RecyclerItemTouchHelperListener listener;
        
        public ProductListItemTouchListener(int dragDirs, int swipeDirs, RecyclerItemTouchHelperListener listener) : base(dragDirs, swipeDirs)
        {
            this.listener = listener;
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            return true;
        }
        
        public override void OnSelectedChanged(RecyclerView.ViewHolder viewHolder, int actionState)
        {
            if (viewHolder != null)
            {
                View foregroundView = ((CartProductListAdapterViewHolder)viewHolder).mview_foreground;
                DefaultUIUtil.OnSelected(foregroundView);
            }
        }
        
        public override void OnChildDrawOver(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX, float dY, int actionState, bool isCurrentlyActive)
        {
            View foregroundView = ((CartProductListAdapterViewHolder)viewHolder).mview_foreground;
            DefaultUIUtil.OnDrawOver(c, recyclerView, foregroundView, dX, dY, actionState, isCurrentlyActive);
        }
        
        public override void ClearView(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            View foregroundView = ((CartProductListAdapterViewHolder)viewHolder).mview_foreground;
            DefaultUIUtil.ClearView(foregroundView);
        }
        
        public override void OnChildDraw(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX, float dY, int actionState, bool isCurrentlyActive)
        {
            View foregroundView = ((CartProductListAdapterViewHolder)viewHolder).mview_foreground;
            DefaultUIUtil.OnDraw(c, recyclerView, foregroundView, dX, dY, actionState, isCurrentlyActive);
        }
        
        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {         
            listener.OnSwiped(viewHolder, direction, viewHolder.AdapterPosition);
        }
 
        public override int ConvertToAbsoluteDirection(int flags, int layoutDirection)
        {
            return base.ConvertToAbsoluteDirection(flags, layoutDirection);
        }
    }

    public interface RecyclerItemTouchHelperListener
    {
        void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction, int position);
    }
}