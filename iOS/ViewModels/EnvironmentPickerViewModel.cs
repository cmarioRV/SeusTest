using System;
using System.Collections.Generic;
using demoseusapp.Models;
using UIKit;

namespace demoseusapp.iOS.ViewModels
{
    public partial class EnvironmentPickerViewModel : UIPickerViewModel
    {
        List<SeusEnvironment> environments;
        public Action<SeusEnvironment> ValueChanged { get; set; }

        public EnvironmentPickerViewModel(List<SeusEnvironment> list) : base()
        {
            environments = list;
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return environments.Count;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return environments[(int)row].ToString();
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            ValueChanged?.Invoke(environments[(int)row]);
        }
    }
}