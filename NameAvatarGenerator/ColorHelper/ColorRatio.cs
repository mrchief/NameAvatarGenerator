using System.Drawing;

namespace NameAvatarGenerator.ColorHelper
{
	public class ColorRatio
	{
		public Color Color { get; set; }
		public double KeepAwayRatio { get; set; } // Usually 1, it could be ~ 4 for background color and 32 is extremely high. It define the distance between this color and the new generated one. Higher = greater distance. 

		public ColorRatio(Color color)
		{
			Color = color;
			KeepAwayRatio = 1;
		}

		public ColorRatio(Color color, double keepAwayRatio)
		{
			Color = color;
			KeepAwayRatio = keepAwayRatio;
		}

		public static implicit operator Color(ColorRatio coloRatio)
		{
			return coloRatio.Color;
		}
	}
}
