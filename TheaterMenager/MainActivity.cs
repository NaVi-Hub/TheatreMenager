using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase;
using static Android.Views.View;
using Android.Views;
using Android.Graphics;
using Java.Util;

namespace TheaterMenager
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText NameET, PhoneNumET;
        Button SendBtn;
        Dialog d;
        LinearLayout OverAllLayout, InsideSVLayout;
        ScrollView SV;
        FirebaseFirestore database;
        LinearLayout.LayoutParams matchParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
        LinearLayout.LayoutParams Imageparams = new LinearLayout.LayoutParams(90, 120);
        LinearLayout.LayoutParams WrapParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
        LinearLayout lay;
        int currI = -1;
        int currK = -1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            database = GetDataase();
            BuildScreen();
        }
        public void BuildDialog()
        {
            d = new Dialog(this);
            d.SetContentView(Resource.Layout.DialogLayout);
            NameET = d.FindViewById<EditText>(Resource.Id.NameET);
            PhoneNumET = d.FindViewById<EditText>(Resource.Id.PhoneNumET);
            SendBtn = d.FindViewById<Button>(Resource.Id.SendBtn);
            SendBtn.Click += this.Btn_Click;
            d.Show();
        }
        public void BuildScreen()
        {
            OverAllLayout = FindViewById<LinearLayout>(Resource.Id.Layout);
            InsideSVLayout = new LinearLayout(this);
            InsideSVLayout.LayoutParameters = new LinearLayout.LayoutParams(1320, 780);
            InsideSVLayout.Orientation = Orientation.Vertical;
            SV = new ScrollView(this);
            SV.LayoutParameters = matchParams;
            SV.CanScrollHorizontally(-1);
            MyImageButton[,] arr = new MyImageButton[13, 22];
            for (int i = 0; i < arr.GetLength(0) -1; i++)
            {
                lay = new LinearLayout(this);
                lay.LayoutParameters = WrapParams;
                lay.Orientation = Orientation.Horizontal;
                lay.SetGravity(GravityFlags.CenterHorizontal);
                for (int k = 0; k < arr.GetLength(1) -1; k++)
                {
                    arr[i, k] = new MyImageButton(this, i, k);
                    arr[i, k].SetImageResource(Resource.Drawable.Chair);
                    arr[i, k].SetBackgroundColor(Color.MediumSeaGreen);
                    Imageparams.SetMargins(5, 5, 5, 5);
                    arr[i, k].LayoutParameters = Imageparams;
                    arr[i, k].SetScaleType(ImageView.ScaleType.FitCenter);
                    arr[i, k].SetAdjustViewBounds(true);
                    arr[i, k].Click += this.MainActivity_Click;
                    lay.AddView(arr[i, k]);
                }
                InsideSVLayout.AddView(lay);
            }
            SV.AddView(InsideSVLayout);
            OverAllLayout.AddView(SV);
        }
        private void MainActivity_Click(object sender, System.EventArgs e)
        {
            MyImageButton imgbtn = (MyImageButton)sender;
            imgbtn.SetBackgroundColor(Color.Red);
            imgbtn.Clickable = false;
            currI = imgbtn.i + 1;
            currK = imgbtn.k + 1;
            BuildDialog();
        }
        private void Btn_Click(object sender, System.EventArgs e)
        {
            if (InputLegit(NameET.Text, PhoneNumET.Text))
            {
                HashMap map = new HashMap();
                map.Put("Name", NameET.Text);
                map.Put("PhoneNum", PhoneNumET.Text);
                map.Put("Taken", true);
                DocumentReference docref = database.Collection("Seats").Document(currI + "," + currK);
                docref.Set(map);
                Toast.MakeText(this, "Successful", ToastLength.Long).Show();
                d.Dismiss();
            }
            else
            {
                Toast.MakeText(this, "Input InValid", ToastLength.Short).Show();
                NameET.Text = "";
                PhoneNumET.Text = "05";
            }
        }
        public bool InputLegit(string name, string phonum)
        {
            if (name.Length < 3 || name.Length > 15)
                return false;
            List<string> lst = new List<string>();
            lst.Add("זין");
            lst.Add("כוס");
            lst.Add("הומו");
            lst.Add("69");
            lst.Add("420");
            lst.Add("שרמוטה");
            lst.Add("בן זונה");
            for(int i = 0; i<lst.Count; i++)
            {
                if (name.Contains(lst[i]))
                {
                    Toast.MakeText(this, "Input InValid", ToastLength.Long).Show();
                    return false;
                }
            }
            if (!(PhoneNumET.Text.Length == 10))
                return false;
            int x = -1;
            int.TryParse(PhoneNumET.Text, out x);
            if (x == -1)
                return false;
            return true;
        }
        public FirebaseFirestore GetDataase()
        {
            FirebaseFirestore database;
            var options = new FirebaseOptions.Builder()
                .SetProjectId("theatermenager")
                .SetApplicationId("theatermenager")
                .SetApiKey("AIzaSyD1dkOwe8aEXJ29-T09-Blib14lXE4hmJc")
                .SetDatabaseUrl("https://theatermenager.firebaseio.com")
                .SetStorageBucket("theatermenager.appspot.com")
                .Build();
            var app = FirebaseApp.InitializeApp(this, options);
            database = FirebaseFirestore.GetInstance(app);
            return database;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}