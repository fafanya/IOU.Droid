using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace share
{
    public class UNObject
    {
        public int Id { get; set; }
    }

    public class TotalDebt : UNObject
    {
        public string DebtorName { get; set; }
        public string LenderName { get; set; }
        public double Amount { get; set; }
    }
}