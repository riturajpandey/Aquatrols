using System;
using System.Collections.Generic;
using Aquatrols.Models;
using UIKit;
using Foundation;

namespace Aquatrols.iOS.Controllers
{
    public class UIUserCategoryPicker:UIPickerViewModel
    {
        List<UserCategory> lstUserCategory;
        public EventHandler ValueChanged;
        public string SelectedValue;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.Controllers.UIUserCategoryPicker"/> class.
        /// </summary>
        /// <param name="lstUserCategory">Lst user category.</param>
        public UIUserCategoryPicker(List<UserCategory> lstUserCategory)
        {
            this.lstUserCategory = lstUserCategory;
        }
        /// <summary>
        /// Gets the rows in component.
        /// </summary>
        /// <returns>The rows in component.</returns>
        /// <param name="pickerView">Picker view.</param>
        /// <param name="component">Component.</param>
        public override nint GetRowsInComponent(UIPickerView pickerView, nint component) {  
            return lstUserCategory.Count;  
        }
        /// <summary>
        /// Gets the component count.
        /// </summary>
        /// <returns>The component count.</returns>
        /// <param name="pickerView">Picker view.</param>
        public override nint GetComponentCount(UIPickerView pickerView) {  
            return 1;  
        } 
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <returns>The title.</returns>
        /// <param name="pickerView">Picker view.</param>
        /// <param name="row">Row.</param>
        /// <param name="component">Component.</param>
        public override string GetTitle(UIPickerView pickerView, nint row, nint component) {  
            return lstUserCategory[(int) row].categoryName;  
        } 
        /// <summary>
        /// Selected the specified pickerView, row and component.
        /// </summary>
        /// <returns>The selected.</returns>
        /// <param name="pickerView">Picker view.</param>
        /// <param name="row">Row.</param>
        /// <param name="component">Component.</param>
        public override void Selected(UIPickerView pickerView, nint row, nint component) {
            var usercategory = lstUserCategory[(int)row];    
            SelectedValue = usercategory.categoryName;
            NSUserDefaults.StandardUserDefaults.SetString(usercategory.categoryId,"UICategorySelectedValue");

            ValueChanged ? .Invoke(null, null);  
        }
    }

}
