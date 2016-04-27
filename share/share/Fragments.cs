using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace share
{
    public class Fragments : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }

    public class SpeakersFragment : ListFragment
    {
        string[] items;
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            items = new string[] { "Vegetables", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };
            ListAdapter = new ArrayAdapter<String>(Activity, Android.Resource.Layout.SimpleListItem1, items);
        }
            /*public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                View view = inflater.Inflate(Resource.Layout.simple_fragment, null);
                view.FindViewById<TextView>(Resource.Id.textView1).SetText(Resource.String.speakers_tab_label);
                view.FindViewById<ImageView>(Resource.Id.imageView1).SetImageResource(Resource.Drawable.ic_action_speakers);
                return view;
            }*/
    }

    public class SessionsFragment : ListFragment
    {
        /*public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.simple_fragment, null);
            view.FindViewById<TextView>(Resource.Id.textView1).SetText(Resource.String.sessions_tab_label);
            view.FindViewById<ImageView>(Resource.Id.imageView1).SetImageResource(Resource.Drawable.ic_action_sessions);
            return view;
        }*/
    }

    public class WhatsOnFragment : ListFragment
    {
        /*public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.simple_fragment, null);
            view.FindViewById<TextView>(Resource.Id.textView1).SetText(Resource.String.whatson_tab_label);
            view.FindViewById<ImageView>(Resource.Id.imageView1).SetImageResource(Resource.Drawable.ic_action_whats_on);
            return view;
        }*/
    }
}