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

namespace TheaterMenager
{
    class MyImageButton : ImageButton
    {
        public ImageButton ImgBtn { get; set; }
        public int i { get; set; }
        public int k { get; set; }
        public string Loc { get; set; }

        public MyImageButton(Context con, int I, int K) : base (con)
        {
            this.i = I;
            this.k = K;
            this.Loc = string.Format(this.i + "," + this.k);
        }
    }
}