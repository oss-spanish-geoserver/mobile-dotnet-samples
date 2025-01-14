﻿using System;
using AdvancedMap.Droid.Sections.BaseMap.Subviews;
using Android.Content;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Utils;
using Carto.VectorTiles;
using Shared;
using Shared.Droid;
using Shared.Model;

namespace AdvancedMap.Droid.Sections.BaseMap.Views
{
	public class BaseMapsView : MapBaseView
	{
        public ActionButton BasemapButton;
        public StylePopupContent StyleContent;

        public ActionButton LanguageButton;
        public LanguagePopupContent LanguageContent;

		public BaseMapsView(Context context) : base(context,
                                                    Resource.Drawable.icon_info_blue,
                                                    Resource.Drawable.icon_back_blue,
                                                    Resource.Drawable.icon_close,
                                                    Resource.Drawable.icon_info_white)
		{
            BasemapButton = new ActionButton(context, Resource.Drawable.icon_basemap);
            AddButton(BasemapButton);
            StyleContent = new StylePopupContent(context);

            LanguageButton = new ActionButton(context, Resource.Drawable.icon_language);
            AddButton(LanguageButton);
            LanguageContent = new LanguagePopupContent(context);

            Frame = new CGRect(0, 0, Metrics.WidthPixels, UsableHeight);
		}

		string currentOSM;
		string currentSelection;
        public TileLayer CurrentLayer { get; set; }

		public void UpdateBaseLayer(string selection, string source)
        {
            if (source.Equals(StylePopupContent.CartoVectorSource))
            {
                // Nutiteq styles are bundled with the SDK, we can initialize them via constuctor
                if (selection.Equals(StylePopupContent.Voyager))
                {
                    CurrentLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);
                }
                else if (selection.Equals(StylePopupContent.Positron))
                {
                    CurrentLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStylePositron);
                }
                else
                {
                    CurrentLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDarkmatter);
                }
            }
            else if (source.Equals(StylePopupContent.CartoRasterSource))
            {
                if (selection.Equals(StylePopupContent.HereSatelliteDaySource))
				{
					CurrentLayer = new CartoOnlineRasterTileLayer("here.satellite.day@2x");
				}
                else if (selection.Equals(StylePopupContent.HereNormalDaySource))
				{
					CurrentLayer = new CartoOnlineRasterTileLayer("here.normal.day@2x");
				}
            }

            if (source.Equals(StylePopupContent.CartoRasterSource))
            {
                LanguageButton.Disable();
            }
            else
            {
                LanguageButton.Enable();
            }

            MapView.Layers.Clear();
            MapView.Layers.Add(CurrentLayer);

            InitializeVectorTileListener();
        }

        public void UpdateLanguage(Language language)
        {
            if (CurrentLayer is VectorTileLayer)
            {
                var decoder = (CurrentLayer as VectorTileLayer).TileDecoder as MBVectorTileDecoder;
                decoder.SetStyleParameter("lang", language.Value);
            }
        }

        public void InitializeVectorTileListener()
        {
			if (CurrentLayer is VectorTileLayer)
			{
				MapView.InitializeVectorTileListener(CurrentLayer as VectorTileLayer);
			}
        }
    }
}
