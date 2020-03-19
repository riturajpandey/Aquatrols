using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using System;
using System.Collections.Generic;
using UIKit;

namespace Aquatrols.iOS.Picker
{
    public class CourseModel: UIPickerViewModel
    {
        public List<CourseEntity> lstCourseEntity;
        public EventHandler ValueChanged;
        public string SelectedValue;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.Controllers.CourseModel"/> class.
        /// </summary>
        public CourseModel(List<CourseEntity> lstCourseEntity)
        {
            this.lstCourseEntity = lstCourseEntity;
        }
        /// <summary>
        /// Gets the rows in component.
        /// </summary>
        /// <returns>The rows in component.</returns>
        /// <param name="pickerView">Picker view.</param>
        /// <param name="component">Component.</param>
        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return lstCourseEntity.Count;
        }
        /// <summary>
        /// Gets the component count.
        /// </summary>
        /// <returns>The component count.</returns>
        /// <param name="pickerView">Picker view.</param>
        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return Constant.lstDigit[1];
        }
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <returns>The title.</returns>
        /// <param name="pickerView">Picker view.</param>
        /// <param name="row">Row.</param>
        /// <param name="component">Component.</param>
        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return lstCourseEntity[(int)row].courseName;
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
            var course = lstCourseEntity[(int)row];
            SelectedValue = course.courseName;
            ValueChanged?.Invoke(null, null);
        }
    }
}
