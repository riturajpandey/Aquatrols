﻿using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using System;
using System.Collections.Generic;
using UIKit;

namespace Aquatrols.iOS.Picker
{
    public class TmStateModel:UIPickerViewModel
    {
        public List<TmStateList> lstState;
        public EventHandler ValueChanged;
        public string SelectedValue;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.Controllers.CountryModel"/> class.
        /// </summary>
        /// <param name="lstState">Country.</param>
        public TmStateModel(List<TmStateList> lstState)
        {
            this.lstState = lstState;
        }
        /// <summary>
        /// Get the rows in component.
        /// </summary>
        /// <returns>The rows in component.</returns>
        /// <param name="pickerView">Picker view.</param>
        /// <param name="component">Component.</param>
        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return lstState.Count;
        }
        /// <summary>
        /// Get the component count.
        /// </summary>
        /// <returns>The component count.</returns>
        /// <param name="pickerView">Picker view.</param>
        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return Constant.lstDigit[1];
        }
        /// <summary>
        /// Get the title.
        /// </summary>
        /// <returns>The title.</returns>
        /// <param name="pickerView">Picker view.</param>
        /// <param name="row">Row.</param>
        /// <param name="component">Component.</param>
        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return lstState[(int)row].regionState;
        }
        /// <summary>
        /// Selected the specified pickerView, row and component.
        /// </summary>
        /// <returns>The selected.</returns>
        /// <param name="pickerView">Picker view.</param>
        /// <param name="row">Row.</param>
        /// <param name="component">Component.</param>
        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            var state = lstState[(int)row].regionState;
            SelectedValue = state;
            ValueChanged?.Invoke(null, null);
        }
    }
}
