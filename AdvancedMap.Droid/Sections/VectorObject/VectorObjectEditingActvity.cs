﻿
using System;
using Android.App;
using Android.Util;
using Carto.Core;
using Carto.DataSources;
using Carto.Geometry;
using Carto.Graphics;
using Carto.Layers;
using Carto.Projections;
using Carto.Styles;
using Carto.Ui;
using Carto.VectorElements;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "Vector object editing", Description = "Shows usage of an editable vector layer")]
	public class VectorObjectEditingActvity : MapBaseActivity
	{
		LocalVectorDataSource source;

		EditableVectorLayer editLayer;

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

            AddOnlineBaseLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);

			// Initialize source and Edit layer, add it to the map
			source = new LocalVectorDataSource(MapView.Options.BaseProjection);

			editLayer = new EditableVectorLayer(source);
			MapView.Layers.Add(editLayer);

			// Convenience methods to add elements to the map, cf. LocalVectorDataSourceExtensions
			source.AddPoint(new MapPos(-5000000, -900000));

			source.AddLine(new MapPosVector { 
				new MapPos(-6000000, -500000), new MapPos(-9000000, -500000) 
			});

			source.AddPolygon(new MapPosVector { 
				new MapPos(-5000000, -5000000), new MapPos(5000000, -5000000), new MapPos(0, 10000000) 
			});

			// Add a vector element event listener to select elements (on element click)
			editLayer.VectorElementEventListener = new VectorElementSelectEventListener(editLayer);

			// Add a map event listener to deselect element (on map click)
			MapView.MapEventListener = new VectorElementDeselectEventListener(editLayer);

			// Add the vector element edit event listener
			editLayer.VectorEditEventListener = new BasicEditEventListener(source);

			Alert("Click on object to modify or move it");
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			editLayer.VectorElementEventListener = null;

			MapView.MapEventListener = null;

			editLayer.VectorEditEventListener = null;
		}
	}
}

