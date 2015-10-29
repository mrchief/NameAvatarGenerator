using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using NameAvatarGenerator.ColorHelper;

namespace NameAvatarGenerator
{
    public class AvatarGenerator
    {
        public MemoryStream Generate(string firstName, string lastName, int height = 64, int width = 64, int fontSize = 28)
        {
            var avatarString = $"{firstName[0]}{lastName[0]}".ToUpper();

            var bgColour = RandomColor.GetColor(ColorScheme.Random, Luminosity.Bright);
            //var bgColour = GetNextColor();

            var bmp = new Bitmap(height, width);
            var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var font = new Font("Arial", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
            var graphics = Graphics.FromImage(bmp);

            graphics.Clear(bgColour);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            graphics.DrawString(avatarString, font, new SolidBrush(Color.WhiteSmoke), new RectangleF(0, 0, height, width), sf);
            graphics.Flush();

            var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);

            return ms;
        }

        private Color GetNextColor()
        {
            var colorGenerator = new ColorGenerator {Accuracy = 50};

            // Faster, little bit more color with less distance 

            // My background, 4 to stay far away 
            colorGenerator.UsedColors.Add(new ColorRatio(Color.Black, 4));
            // No White
            colorGenerator.UsedColors.Add(new ColorRatio(Color.White, 1.2));
            colorGenerator.UsedColors.Add(new ColorRatio(Color.LightGray, 1));

            
            return colorGenerator.GetNextColor();
        }
    }
}
