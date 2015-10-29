using System;

namespace NameAvatarGenerator.ColorHelper
{
	/// <summary>
	/// Structure to define CIE L*a*b*.
	/// </summary>
	public struct CIELab
	{
		/// <summary>
		/// Gets an empty CIELab structure.
		/// </summary>
		public static readonly CIELab Empty = new CIELab();

		#region Fields
		private double l;
		private double a;
		private double b;

		#endregion

		#region Operators
		public static bool operator ==(CIELab item1, CIELab item2)
		{
			return (
				item1.L == item2.L 
				&& item1.A == item2.A 
				&& item1.B == item2.B
				);
		}

		public static bool operator !=(CIELab item1, CIELab item2)
		{
			return (
				item1.L != item2.L 
				|| item1.A != item2.A 
				|| item1.B != item2.B
				);
		}

		#endregion

		#region Accessors
		/// <summary>
		/// Gets or sets L component.
		/// </summary>
		public double L
		{
			get
			{
				return l;
			}
			set
			{
				l = value;
			}
		}

		/// <summary>
		/// Gets or sets a component.
		/// </summary>
		public double A
		{
			get
			{
				return a;
			}
			set
			{
				a = value;
			}
		}

		/// <summary>
		/// Gets or sets a component.
		/// </summary>
		public double B
		{
			get
			{
				return b;
			}
			set
			{
				b = value;
			}
		}

		#endregion

		public CIELab(double l, double a, double b) 
		{
			this.l = l;
			this.a = a;
			this.b = b;
		}

		#region Methods
		public override bool Equals(Object obj) 
		{
			if(obj==null || GetType()!=obj.GetType()) return false;

			return (this == (CIELab)obj);
		}

		public override int GetHashCode() 
		{
			return L.GetHashCode() ^ a.GetHashCode() ^ b.GetHashCode();
		}

		// ******************************************************************
		// EO: Based on: http://www.easyrgb.com/index.php?X=DELT&H=04#text4
		// Based on: Delta E 1994
		public static double GetDistanceBetweenCie1994(CIELab lab1, CIELab lab2)
		{
			const double whtL = 1;
			const double whtC = 1;
			const double whtH = 1;               //Weighting factors depending 
										   //on the application (1 = default)

			double xC1 = Math.Sqrt(Math.Pow(lab1.a, 2.0) + Math.Pow(lab1.b, 2.0));
			double xC2 = Math.Sqrt(Math.Pow(lab2.a, 2.0) + Math.Pow(lab2.b, 2.0));
			double xDL = lab2.l - lab1.l;
			double xDC = xC2 - xC1;
			double xDE = Math.Sqrt(
				((lab1.l - lab2.l) * (lab1.l - lab2.l))
				+ ((lab1.a - lab2.a) * (lab1.a - lab2.a))
				+ ((lab1.b - lab2.b) * (lab1.b - lab2.b)));

			double xDH;
			if (Math.Sqrt( xDE ) > ( Math.Sqrt( Math.Abs( xDL ) ) + Math.Sqrt( Math.Abs( xDC ) ) ) )
			{
				xDH = Math.Sqrt((xDE*xDE) - (xDL*xDL) - (xDC*xDC));
			}
			else
			{
				xDH = 0;
			}

			double xSC = 1 + (0.045*xC1);
			double xSH = 1 + (0.015*xC1);
			xDL /= whtL;
			xDC /= whtC*xSC;
			xDH /= whtH*xSH;
			double deltaE94 = Math.Sqrt(Math.Pow(xDL, 2.0) + Math.Pow(xDC, 2.0) + Math.Pow(xDH, 2.0));
			return deltaE94;
		}

		// ******************************************************************
        // Given by Rob2412 http://www.codeproject.com/KB/recipes/colorspace1.aspx?msg=4041550#xx4041550xx
        // and should be better then the previous one.
        /// <summary>
        /// Returns the color difference (distance) between a sample color CIELap(2) and a reference color CIELap(1)
        /// <para>in accorance with CIE 2000 alogorithm.</para>
        /// </summary>
        /// <param name="lab1">CIELap reference color.</param>
        /// <param name="lab2">CIELap sample color.</param>
        /// <returns>Color difference.</returns>
        public static double GetDistanceBetweenCie2000(CIELab lab1, CIELab lab2)
        {
            double p25 = Math.Pow(25, 7);

            double C1 = Math.Sqrt(lab1.A * lab1.A + lab1.B * lab1.B);
            double C2 = Math.Sqrt(lab2.A * lab2.A + lab2.B * lab2.B);
            double avgC = (C1 + C2) / 2F;

            double powAvgC = Math.Pow(avgC, 7);
            double G = (1 - Math.Sqrt(powAvgC / (powAvgC + p25))) / 2D;

            double a_1 = lab1.A * (1 + G);
            double a_2 = lab2.A * (1 + G);

            double C_1 = Math.Sqrt(a_1 * a_1 + lab1.B * lab1.B);
            double C_2 = Math.Sqrt(a_2 * a_2 + lab2.B * lab2.B);
            double avgC_ = (C_1 + C_2) / 2D;

            double h1 = (Atan(lab1.B, a_1) >= 0 ? Atan(lab1.B, a_1) : Atan(lab1.B, a_1) + 360F);
            double h2 = (Atan(lab2.B, a_2) >= 0 ? Atan(lab2.B, a_2) : Atan(lab2.B, a_2) + 360F);

            double H = (h1 - h2 > 180D ? (h1 + h2 + 360F) / 2D : (h1 + h2) / 2D);

            double T = 1;
            T -= 0.17 * Cos(H - 30);
            T += 0.24 * Cos(2 * H);
            T += 0.32 * Cos(3 * H + 6);
            T -= 0.20 * Cos(4 * H - 63);

            double deltah = 0;
            if (h2 - h1 <= 180)
                deltah = h2 - h1;
            else if (h2 <= h1)
                deltah = h2 - h1 + 360;
            else
                deltah = h2 - h1 - 360;

            double avgL = (lab1.L + lab2.L) / 2F;
            double deltaL_ = lab2.L - lab1.L;
            double deltaC_ = C_2 - C_1;
            double deltaH_ = 2 * Math.Sqrt(C_1 * C_2) * Sin(deltah / 2);

            double SL = 1 + (0.015 * Math.Pow(avgL - 50, 2)) / Math.Sqrt(20 + Math.Pow(avgL - 50, 2));
            double SC = 1 + 0.045 * avgC_;
            double SH = 1 + 0.015 * avgC_ * T;

            double exp = Math.Pow((H - 275) / 25, 2);
            double teta = Math.Pow(30, -exp);

            double RC = 2D * Math.Sqrt(Math.Pow(avgC_, 7) / (Math.Pow(avgC_, 7) + p25));
            double RT = -RC * Sin(2 * teta);

            double deltaE = 0;
            deltaE = Math.Pow(deltaL_ / SL, 2);
            deltaE += Math.Pow(deltaC_ / SC, 2);
            deltaE += Math.Pow(deltaH_ / SH, 2);
            deltaE += RT * (deltaC_ / SC) * (deltaH_ / SH);
            deltaE = Math.Sqrt(deltaE);

            return deltaE;
        }

        // ******************************************************************
        /// <summary>
        /// Returns the angle in degree whose tangent is the quotient of the two specified numbers.
        /// </summary>
        /// <param name="y">The y coordinate of a point.</param>
        /// <param name="x">The x coordinate of a point.</param>
        /// <returns>Angle in degree.</returns>
        private static double Atan(double y, double x)
        {
            return Math.Atan2(y, x) * 180D / Math.PI;
        }

        // ******************************************************************
        /// <summary>
        /// Returns the cosine of the specified angle in degree.
        /// </summary>
        /// <param name="d">Angle in degree</param>
        /// <returns>Cosine of the specified angle.</returns>
        private static double Cos(double d)
        {
            return Math.Cos(d * Math.PI / 180);
        }

        // ******************************************************************
        /// <summary>
        /// Returns the sine of the specified angle in degree.
        /// </summary>
        /// <param name="d">Angle in degree</param>
        /// <returns>Sine of the specified angle.</returns>
        private static double Sin(double d)
        {
            return Math.Sin(d * Math.PI / 180);
        }


		#endregion
	} 
}
