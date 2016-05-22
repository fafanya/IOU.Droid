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
    public class UGObject
    {
        
    }

    public class UGGroup
    {
        public int Id { get; set; }
        public int Name { get; set; }

        public List<UEvent> UEvents { get; set; }
        public List<UMember> UMembers { get; set; }
        public List<UDebt> UDebts { get; set; }
    }

    public class UGEvent
    {
    }

    public class UGMember
    {

    }

    public class UGDebt
    {

    }
}