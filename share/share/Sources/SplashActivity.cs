using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using System.Collections.Generic;

namespace share
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true, Label = "Общак", Icon = "@drawable/icon")]
    public class SplashActivity : AppCompatActivity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        protected override void OnResume()
        {
            base.OnResume();

            Task startupWork = new Task(() =>
            {
                List<string> example = new List<string>() {
                    Resources.GetText(Resource.String.exam_group_name),
                    Resources.GetText(Resource.String.exam_event1_name),
                    Resources.GetText(Resource.String.exam_event2_name),
                    Resources.GetText(Resource.String.exam_event3_name),
                    Resources.GetText(Resource.String.exam_guy1_name),
                    Resources.GetText(Resource.String.exam_guy2_name),
                    Resources.GetText(Resource.String.exam_guy3_name),
                    Resources.GetText(Resource.String.exam_debt_desc)
                };
                LocalDBController.Initialize(example);
            });

            startupWork.ContinueWith(t =>
            {
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }, TaskScheduler.FromCurrentSynchronizationContext());

            startupWork.Start();
        }
    }
}