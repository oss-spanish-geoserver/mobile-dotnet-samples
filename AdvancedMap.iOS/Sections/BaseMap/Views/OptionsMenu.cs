﻿
using System;
using System.Collections.Generic;
using CoreGraphics;
using Shared;
using UIKit;

namespace AdvancedMap.iOS
{
	public class OptionsMenu : UIView
	{
		public EventHandler<OptionEventArgs> OptionTapped;

		static nfloat SmallBoxPadding = 5;

		static nfloat LargeBoxPadding = 20;

		const double animationDuration = 0.2;

		public bool IsVisible { get { return Alpha == 1; } }

		List<OptionsMenuItem> views;

		List<Section> items;
		public List<Section> Items
		{
			get { return items; }
			set {

				items = value;

				foreach (Section section in items)
				{
					OptionsMenuItem view = new OptionsMenuItem();
					view.Section = section;
					view.AddGestureRecognizer(new UITapGestureRecognizer(OnSubviewTap));
					views.Add(view);
					AddSubview(view);

					view.OptionTapped += OnOptionTap;
				}
			}
		}

		public OptionsMenu()
		{
			BackgroundColor = UIColor.FromRGBA(0, 0, 0, 150);

			views = new List<OptionsMenuItem>();

			Alpha = 0;

			AddGestureRecognizer(new UITapGestureRecognizer(OnBackgroundTap));

			Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Size.Width, UIScreen.MainScreen.Bounds.Size.Height);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			nfloat x = SmallBoxPadding;
			nfloat y = LargeBoxPadding;
			nfloat w = Frame.Width - 2 * SmallBoxPadding;
			nfloat h = 0;

			int counter = 0;

			foreach (OptionsMenuItem view in views)
			{
				// Trivial heights
				h = view.IsMultiLine ? 120 : 80;
				view.Frame = new CGRect(x, y, w, h);

				if (counter == 2)
				{
					// Extra padding so language menu would be more separate
					y += h + LargeBoxPadding;
				}
				else {
					y += h + SmallBoxPadding;
				}

				counter++;
			}
		}

		public void Show()
		{
			UIApplication.SharedApplication.KeyWindow.AddSubview(this);

			Animate(animationDuration, delegate { Alpha = 1; });
		}

		public void Hide()
		{
			Animate(animationDuration, delegate { Alpha = 0; }, delegate { RemoveFromSuperview(); });
		}

		void OnBackgroundTap()
		{
			Hide();
		}

		void OnSubviewTap()
		{
			// Do nothing. Just catch taps
			Console.WriteLine("Subview (box) tapped");
		}

		OptionLabel current;
		OptionLabel currentLanguage;

		void OnOptionTap(object sender, OptionEventArgs e)
		{
			OptionLabel option = e.Option;

			if (e.Section.Type == SectionType.Language)
			{
				if (currentLanguage != null)
				{
					currentLanguage.Normalize();
				}

				option.Highlight();

				currentLanguage = option;
			}
			else 
			{
				if (current != null)
				{
					current.Normalize();
				}

				option.Highlight();

				current = option;
			}

			if (OptionTapped != null)
			{
				OptionTapped(null, e);
			}
		}

		public void SetInitialItem(Section section)
		{
			foreach (OptionsMenuItem view in views)
			{
				if (view.Section.Equals(section))
				{
					if (section.Type == SectionType.Language)
					{
						currentLanguage = view.SetFirstItemActive();
					}
					else
					{
						current = view.SetFirstItemActive();
					}
				}
			}
		}
	}

	public class OptionEventArgs : EventArgs
	{
		public Section Section { get; set; }

		public OptionLabel Option { get; set; }
	}
}
