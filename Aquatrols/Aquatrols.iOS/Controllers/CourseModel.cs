using System;
using System.Collections.Generic;
using Aquatrols.Models;
using Foundation;
using UIKit;

namespace Aquatrols.iOS.Controllers
{
    public class CourseModel: UIPickerViewModel
    {
        List<CourseEntity> courseEntity;
        public EventHandler ValueChanged;
        public string SelectedValue;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.Controllers.CourseModel"/> class.
        /// </summary>
        public CourseModel(List<CourseEntity> courseEntity)
        {
            this.courseEntity = courseEntity;
        }
        /// <summary>
        /// Gets the rows in component.
        /// </summary>
        /// <returns>The rows in component.</returns>
        /// <param name="pickerView">Picker view.</param>
        /// <param name="component">Component.</param>
        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return courseEntity.Count;
        }
        /// <summary>
        /// Gets the component count.
        /// </summary>
        /// <returns>The component count.</returns>
        /// <param name="pickerView">Picker view.</param>
        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
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
            return courseEntity[(int)row].courseName;
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
            var course = courseEntity[(int)row];
            SelectedValue = course.courseName;
           // NSUserDefaults.StandardUserDefaults.SetString(Convert.ToString(course.courseId), "courseid");
            ValueChanged?.Invoke(null, null);
        }
    }
}
