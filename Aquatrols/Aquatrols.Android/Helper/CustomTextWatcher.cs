using Android.Content;
using Android.Text;
using Android.Widget;
using Aquatrols.Droid.Activities;
using System;
using System.Linq;

namespace Aquatrols.Droid.Helper
{
    /// <summary>
    /// This class is used for text changed event
    /// </summary>
    public class CustomTextWatcher : Java.Lang.Object, ITextWatcher
    {
        private EditText view;
        private Context context;
        private String lastValue, tempValue,tempFirstName,tempLastName, tempCourseName = "";
        private bool isLengthEight, isLengthFour, isLengthOne = false;

        /// <summary>
        /// This method is used to view text to be changed
        /// </summary>
        /// <param name="view"></param>
        /// <param name="context"></param>
        public CustomTextWatcher(EditText view, Context context)
        {
            this.view = view;
            this.context = context;
        }

        /// <summary>
        /// This method is used after text get changed
        /// </summary>
        /// <param name="s"></param>
        public void AfterTextChanged(IEditable s)
        {

        }

        /// <summary>
        /// This method is used before text change
        /// </summary>
        /// <param name="s"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="after"></param>
        public void BeforeTextChanged(Java.Lang.ICharSequence s, int start, int count, int after)
        {
        }

        /// <summary>
        /// This method is used on text change
        /// </summary>
        /// <param name="s"></param>
        /// <param name="start"></param>
        /// <param name="before"></param>
        /// <param name="count"></param>
        public void OnTextChanged(Java.Lang.ICharSequence s, int start, int before, int count)
        {
            switch (view.Id)
            {
                case Resource.Id.editAmount:
                    String newValuefairways = view.Text;
                    if (!newValuefairways.Equals(lastValue) && (!string.IsNullOrEmpty(view.Text)))
                    {
                        if (newValuefairways.Contains(","))
                        {
                            newValuefairways = newValuefairways.Replace(",", "");
                        }
                        long number = Convert.ToInt64(newValuefairways);
                        newValuefairways = number.ToString("#,##0");
                        lastValue = newValuefairways;
                        view.Text = newValuefairways;
                        view.SetSelection(view.Text.Length);
                    }
                    break;
                //case Resource.Id.autocompleteCoursename:
                //    if (!view.Text.Equals(tempCourseName))
                //    {
                //        char[] array = view.Text.ToCharArray();
                //        // Handle the first letter in the string.
                //        if (array.Length >= 1)
                //        {
                //            if (char.IsLower(array[0]))
                //            {
                //                array[0] = char.ToUpper(array[0]);
                //            }
                //        }
                //        // Scan through the letters, checking for spaces.
                //        // ... Uppercase the lowercase letters following spaces.
                //        for (int i = 1; i < array.Length; i++)
                //        {
                //            if (array[i - 1] == ' ')
                //            {
                //                if (char.IsLower(array[i]))
                //                {
                //                    array[i] = char.ToUpper(array[i]);
                //                }
                //            }
                //        }
                //        tempCourseName = new string(array);
                //        view.Text = tempCourseName;
                //        view.SetSelection(view.Text.Length);
                //    }
                //    break;
                //case Resource.Id.editPhone:                  
                //    if (!view.Text.Equals(tempValue) && (!string.IsNullOrEmpty(view.Text)))
                //    {
                //        if (view.Text.Length == 1 && isLengthOne==false && (!view.Text.Contains("(")))
                //        {
                //            string str = "(" + view.Text;
                //            view.Text = str;
                //            tempValue = view.Text;
                //            view.SetSelection(view.Text.Length);
                //            isLengthOne = true;
                //            isLengthFour = false;
                //            isLengthEight = false;
                //        }
                //        else if (view.Text.Length == 4 && isLengthFour==false)
                //        {
                //            view.Text = view.Text + ")";
                //            tempValue = view.Text;
                //            view.SetSelection(view.Text.Length);
                //            isLengthFour = true;
                //            isLengthEight = false;
                //            isLengthOne = false;
                //        }
                //        else if (view.Text.Length == 8 && isLengthEight==false)
                //        {
                //            isLengthEight = true;
                //            isLengthFour = false;
                //            isLengthOne = false;
                //            view.Text = view.Text + "-";
                //            tempValue = view.Text;
                //            view.SetSelection(view.Text.Length);
                //        }                       
                //    }
                //    else
                //    {
                //        isLengthOne = false;
                //    }
                //    break;
                //case Resource.Id.editFirstname:
                //    if (!view.Text.Equals(tempFirstName))
                //    {
                //        char[] array = view.Text.ToCharArray();
                //        // Handle the first letter in the string.
                //        if (array.Length >= 1)
                //        {
                //            if (char.IsLower(array[0]))
                //            {
                //                array[0] = char.ToUpper(array[0]);
                //            }
                //        }
                //        // Scan through the letters, checking for spaces.
                //        // ... Uppercase the lowercase letters following spaces.
                //        for (int i = 1; i < array.Length; i++)
                //        {
                //            if (array[i - 1] == ' ')
                //            {
                //                if (char.IsLower(array[i]))
                //                {
                //                    array[i] = char.ToUpper(array[i]);
                //                }
                //            }
                //        }
                //        tempFirstName = new string(array);
                //        view.Text = tempFirstName;
                //        view.SetSelection(view.Text.Length);
                //    }
                //    break;
                //case Resource.Id.editLastname:
                    //if (!view.Text.Equals(tempLastName))
                    //{
                    //    char[] array = view.Text.ToCharArray();
                    //    // Handle the first letter in the string.
                    //    if (array.Length >= 1)
                    //    {
                    //        if (char.IsLower(array[0]))
                    //        {
                    //            array[0] = char.ToUpper(array[0]);
                    //        }
                    //    }
                    //    // Scan through the letters, checking for spaces.
                    //    // ... Uppercase the lowercase letters following spaces.
                    //    for (int i = 1; i < array.Length; i++)
                    //    {
                    //        if (array[i - 1] == ' ')
                    //        {
                    //            if (char.IsLower(array[i]))
                    //            {
                    //                array[i] = char.ToUpper(array[i]);
                    //            }
                    //        }
                    //    }
                    //    tempLastName = new string(array);
                    //    view.Text = tempLastName;
                    //    view.SetSelection(view.Text.Length);
                    //}
                    //break;
                case Resource.Id.editPassword:         //password on signup screen
                    {
                        if (s.ToArray().Count() > 0)
                        {
                            if (!LoginActivity.canSeePassword)
                            {
                                view.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconVisible, 0);
                            }
                            else
                            {
                                view.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconInvisible, 0);
                            }
                        }
                        else
                        {
                            view.SetCompoundDrawablesWithIntrinsicBounds(0, 0, 0, 0);
                            LoginActivity.canSeePassword = false;
                        }
                    }
                    break;
                //case Resource.Id.editCPassword:         //password on signup screen
                //    {
                //        if (s.ToArray().Count() > 0)
                //        {
                //            if (!LoginActivity.canSeePassword)
                //            {
                //                view.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconVisible, 0);
                //            }
                //            else
                //            {
                //                view.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconInvisible, 0);
                //            }
                //        }
                //        else
                //        {
                //            view.SetCompoundDrawablesWithIntrinsicBounds(0, 0, 0, 0);
                //            LoginActivity.canSeePassword = false;
                //        }
                //    }
                //    break;
                case Resource.Id.editConPassword:         //password on signup screen
                    {
                        if (s.ToArray().Count() > 0)
                        {
                            if (!MyAccountActivity.canseeOldpassword)
                            {
                                view.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconVisibleBlue, 0);
                            }
                            else
                            {
                                view.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconInvisibleBlue, 0);
                            }
                        }
                        else
                        {
                            view.SetCompoundDrawablesWithIntrinsicBounds(0, 0, 0, 0);
                            MyAccountActivity.canseeOldpassword = false;
                        }
                    }
                    break;
                case Resource.Id.editNewPassword:         //password on signup screen
                    {
                        if (s.ToArray().Count() > 0)
                        {
                            if (!MyAccountActivity.canseeNewpassword)
                            {
                                view.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconVisibleBlue, 0);
                            }
                            else
                            {
                                view.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconInvisibleBlue, 0);
                            }
                        }
                        else
                        {
                            view.SetCompoundDrawablesWithIntrinsicBounds(0, 0, 0, 0);
                            MyAccountActivity.canseeNewpassword = false;
                        }
                    }
                    break;
                case Resource.Id.editOldPassword:         //password on signup screen
                    {
                        if (s.ToArray().Count() > 0)
                        {
                            if (!MyAccountActivity.canseeConpassword)
                            {
                                view.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconVisibleBlue, 0);
                            }
                            else
                            {
                                view.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconInvisibleBlue, 0);
                            }
                        }
                        else
                        {
                            view.SetCompoundDrawablesWithIntrinsicBounds(0, 0, 0, 0);
                            MyAccountActivity.canseeConpassword = false;
                        }
                    }
                    break;               
                default:
                    break;
            }
        }
    }
}