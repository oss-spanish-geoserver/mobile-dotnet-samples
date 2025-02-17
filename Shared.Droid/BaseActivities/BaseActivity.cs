﻿using System;
using Android.App;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Carto.Utils;

namespace Shared.Droid
{
	public class BaseActivity : Activity
	{
        public const string MapTitle = "map_title";
        public const string MapDescription = "map_description";

		protected const int RequestCode = 1;
		protected const int Marshmallow = 23;

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			if (ActionBar != null)
			{
				ActionBar.SetDisplayHomeAsUpEnabled(true);
				ActionBar.SetBackgroundDrawable(new ColorDrawable { Color = Colors.ActionBar });
			}

            string title = Intent.GetStringExtra(MapTitle);
            if (title != "")
            {
				ActionBar.Title = title;
				ActionBar.Subtitle = Intent.GetStringExtra(MapDescription);    
            }
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if (item.ItemId == Android.Resource.Id.Home)
			{
				OnBackPressed();
				return true;
			}

			return base.OnOptionsItemSelected(item);
		}

		protected void Alert(string message)
		{
			RunOnUiThread(delegate
			{
				Toast.MakeText(this, message, ToastLength.Short).Show();
			});
		}

		public void RunOnBackgroundThread(Action action)
		{
			System.Threading.Tasks.Task.Run(action);
		}

        protected Carto.Graphics.Bitmap CreateBitmap(int resource)
        {
            return BitmapUtils.CreateBitmapFromAndroidBitmap(Android.Graphics.BitmapFactory.DecodeResource(Resources, resource));
        }

	}
}

