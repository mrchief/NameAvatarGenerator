using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using NameAvatarGenerator.ColorGenerators;

namespace NameAvatarGenerator
{
    public class AvatarGenerator
    {
        private readonly IColorGenerator _colorGenerator;

        public ImageFormat ImageFormat { get; set; }

        public AvatarGenerator(IColorGenerator colorGenerator)
        {
            _colorGenerator = colorGenerator;
            ImageFormat = ImageFormat.Jpeg;
        }

        public MemoryStream Generate(string firstName, string lastName, int height = 64, int width = 64, int fontSize = 28)
        {
            var avatarString = $"{firstName[0]}{lastName[0]}".ToUpper();

            var bgColour = _colorGenerator.GetNextColor();

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
            bmp.Save(ms, ImageFormat);

            return ms;
        }
    }
}
