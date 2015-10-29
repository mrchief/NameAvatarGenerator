using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Devcorp.Controls.Design;

/* Main usage:
 			ColorGenerator colorGenerator = new ColorGenerator();
			colorGenerator.Accuracy = 50; // Faster, little bit more color with less distance 
			colorGenerator.UsedColors.Add(new ColorGenerator.ColorRatio(System.Windows.Media.Colors.Black, 4)); // My background, 4 to stay far away 
			colorGenerator.UsedColors.Add(new ColorGenerator.ColorRatio(System.Windows.Media.Colors.White, 1.2)); // No White
			colorGenerator.UsedColors.Add(new ColorGenerator.ColorRatio(System.Windows.Media.Colors.LightGray, 1)); // Grid Color

			System.Windows.Media.Color c1 = colorGenerator.GetNextColor();
			colorGenerator.RemoveColor(c1);
			System.Windows.Media.Color c2 = colorGenerator.GetNextColor();
			System.Windows.Media.Color c3 = colorGenerator.GetNextColor();
			System.Windows.Media.Color c4 = colorGenerator.GetNextColor();
*/

namespace Devcorp.ColorSpaceSample.utils
{
	public class ColorGenerator
	{
		// ******************************************************************
		private ObservableCollection<ColorRatio> _usedColors = new ObservableCollection<ColorRatio>();

		// ******************************************************************
		// The color zero is usually the backgroundColor.
		// the color zero is the one used to determine if the luminance with 
		// You can remove or add any colors directly.
		// It is a lot prefereable to keep ColorRatio with higher "KeepAwayRatio" in first elements (near index 0)
		public ObservableCollection<ColorRatio> UsedColors
		{
			get { return _usedColors; }
		}

		// ******************************************************************
		// For better performance, lower this value. (50 is good for hi speed, less diff)
		// For better differences, raise this value. (200 is good hi diff but low speed) I prefer 200.
		public int Accuracy { get; set; }

		// ******************************************************************
		public ColorGenerator()
		{
			DistanceMin = MaximumDistanceMin;
			_usedColors.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(_usedColors_CollectionChanged);
			Accuracy = 200;
		}

		// ******************************************************************
		// Only to help a bit keeping good diff. It should re-adjust itself.
		void _usedColors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				DistanceMin = MaximumDistanceMin;
			}

			if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				if (e.OldItems.Count <= 2)
				{
					DistanceMin += .1;
				}
				else
				{
					DistanceMin = MaximumDistanceMin;
				}
			}
		}

		// ******************************************************************
		public void RemoveColor(Color c)
		{
			for (int n = _usedColors.Count - 1; n >= 0; n-- )
			{
				if (_usedColors[n].Color == c)
				{
					_usedColors.RemoveAt(n);
				}
			}
		}

		// ******************************************************************
		private double _distanceMin;
		private double DistanceMin
		{
			get { return _distanceMin; }
			set
			{
				if (value > MaximumDistanceMin)
				{
					_distanceMin = MaximumDistanceMin;
				}
				else if (value < MinimumDistanceMin)
				{
					_distanceMin = MinimumDistanceMin;
				}
				else
				{
					_distanceMin = value;
				}

				// Debug.Print(_distanceMin.ToString()); // To uncomment while playing with performance 
			}
		}

		// ******************************************************************
		private const double MaximumDistanceMin = 1.0;
		private const double MinimumDistanceMin = .01;
		private int _badTryCount = 0;
		private Random _random = new Random();

		// ******************************************************************
		public Color GetNextColor()
		{
			while (true)
			{
				double hue = _random.NextDouble() * 360;
				double saturation;
				double luminance;

				// To go quicker and darker for white background
				// saturation = Math.Sqrt(_random.NextDouble()) ; 
				// luminance = Math.Sqrt(_random.NextDouble());

				// To go quicker and lighter for dark background
				//saturation = Math.Pow(_random.NextDouble(), 2.0); 
				//luminance = Math.Pow(_random.NextDouble(), 2.0);

				// Less performance but higher compatibility
				saturation = _random.NextDouble(); 
				luminance = _random.NextDouble();

				HSL hsl = new HSL(hue, saturation, luminance);
				Color c = hsl.ToColor();

				if (IsFarEnoughFromExistingColor(c, DistanceMin))
				{
					UsedColors.Add(new ColorRatio(c));
					DistanceMin += .02;
					_badTryCount = 0;
					return c;
				}

				_badTryCount++;
				if (_badTryCount > Accuracy)
				{
					_badTryCount = 0;
					DistanceMin -= .002;
				}
			}
		}

		// ******************************************************************
		private bool IsFarEnoughFromExistingColor(Color c, double distanceMin)
		{
			foreach (ColorRatio coloRatio in UsedColors)
			{
				// double distance = ColorSpaceHelper.GetColorDistance(c, coloRatio.Color);

				// This is a lot better differences between color with CIELab calc.
				double distance = ColorSpaceHelper.GetColorDistanceCIELab(c, coloRatio.Color) / 100;

				if (distance / coloRatio.KeepAwayRatio < distanceMin)
				{
					return false; // Too close
				}
			}

			return true;
		}

		// ******************************************************************

	}
}
