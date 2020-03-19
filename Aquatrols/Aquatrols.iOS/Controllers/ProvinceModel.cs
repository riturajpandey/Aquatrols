using System;
using System.Collections.Generic;
using UIKit;

namespace Aquatrols.iOS.Controllers
{
    public class ProvinceModel: UIPickerViewModel
    {  
        List<string> Provinces;
        public EventHandler ValueChanged;
        public string SelectedValue;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.Controllers.StateModel"/> class.
        /// </summary>
        /// <param name="Provinces">State.</param>
        public ProvinceModel(List<string> Provinces)
        {
            this.Provinces = Provinces;
        }
        /// <summary>
        /// Gets the rows in component.
        /// </summary>
        /// <returns>The rows in component.</returns>
        /// <param name="pickerView">Picker view.</param>
        /// <param name="component">Component.</param>
        public override nint GetRowsInComponent(UIPickerView pickerView, nint component) {  
            return Provinces.Count;  
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
            return Provinces[(int) row];  
        } 
        /// <summary>
        /// Selected the specified pickerView, row and component.
        /// </summary>
        /// <returns>The selected.</returns>
        /// <param name="pickerView">Picker view.</param>
        /// <param name="row">Row.</param>
        /// <param name="component">Component.</param>
        public override void Selected(UIPickerView pickerView, nint row, nint component) {  
            var provinces = Provinces[(int) row];  
            SelectedValue = provinces;  
            ValueChanged ? .Invoke(null, null);  
        } 
    }
}
