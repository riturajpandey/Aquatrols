using System;
using System.Collections.Generic;
using UIKit;

namespace Aquatrols.iOS.Picker
{
    public class CabelaGiftCardModel: UIPickerViewModel
    {
        List<string> CabelaGiftCard;
        public EventHandler ValueChanged;
        public string SelectedValue;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.Picker.MarketModel"/> class.
        /// </summary>
        /// <param name="CabelaGiftCard">Market.</param>
        public CabelaGiftCardModel(List<string> CabelaGiftCard)
        {
            this.CabelaGiftCard = CabelaGiftCard;
        }
        /// <summary>
        /// Gets the rows in component.
        /// </summary>
        /// <returns>The rows in component.</returns>
        /// <param name="pickerView">Picker view.</param>
        /// <param name="component">Component.</param>
        public override nint GetRowsInComponent(UIPickerView pickerView, nint component) {  
            return CabelaGiftCard.Count;  
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
            return CabelaGiftCard[(int) row];  
        } 
        /// <summary>
        /// Selected the specified pickerView, row and component.
        /// </summary>
        /// <returns>The selected.</returns>
        /// <param name="pickerView">Picker view.</param>
        /// <param name="row">Row.</param>
        /// <param name="component">Component.</param>
        public override void Selected(UIPickerView pickerView, nint row, nint component) {  
            var cabelaGiftCard = CabelaGiftCard[(int) row];  
            SelectedValue = cabelaGiftCard;  
            ValueChanged ? .Invoke(null, null);  
        } 
    }
}
