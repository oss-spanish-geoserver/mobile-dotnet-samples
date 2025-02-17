﻿using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Ui;
using Carto.VectorElements;

namespace Shared
{
	public class MyUTFGridEventListener : UTFGridEventListener
	{
		VectorLayer vectorLayer;
		LocalVectorDataSource vectorDataSource;

		public MyUTFGridEventListener(VectorLayer vectorLayer, Variant infoWindow = null)
		{
			this.vectorLayer = vectorLayer;
		}

		public MyUTFGridEventListener(LocalVectorDataSource source)
		{
			vectorDataSource = source;
		}

		public override bool OnUTFGridClicked(UTFGridClickInfo clickInfo)
		{
			if (vectorDataSource == null)
			{
				vectorDataSource = (LocalVectorDataSource)vectorLayer.DataSource;
			}

			// Clear previous popups
			vectorDataSource.Clear();

			// Multiple vector elements can be clicked at the same time, we only care about the one
			// Check the type of vector element
			BalloonPopup clickPopup = null;
			BalloonPopupStyleBuilder styleBuilder = new BalloonPopupStyleBuilder();

			// Configure style
			styleBuilder.LeftMargins = new BalloonPopupMargins(0, 0, 0, 0);
			styleBuilder.TitleMargins = new BalloonPopupMargins(6, 3, 6, 3);

			// Make sure this label is shown on top all other labels
			styleBuilder.PlacementPriority = 10;

			// Show clicked element variant as JSON string
			string desc = clickInfo.ElementInfo.ToString();

			clickPopup = new BalloonPopup(clickInfo.ClickPos, styleBuilder.BuildStyle(), "", desc);

			vectorDataSource.Add(clickPopup);

			return true;
		}
	}

}

