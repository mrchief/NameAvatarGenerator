using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NameAvatarGenerator.ColorGenerators;
using NameAvatarGenerator.ColorHelper;

namespace NameAvatarGenerator.Tests
{
    [TestClass]
    public class AvatarGeneratorTests
    {
        [TestMethod]
        public void ShouldGenerateAvatar()
        {
            var ag = new AvatarGenerator(new PaletteColorGenerator()) {ImageFormat = ImageFormat.Png};
            var imageList = new List<string>();
            for (int i = 0; i < 25; i++)
            {
                var stream = ag.Generate("Hello", "World");
                var base64Data = Convert.ToBase64String(stream.ToArray());

                var imgTag = $"<img src='data:image/png;base64,{base64Data}' />";
                imageList.Add(imgTag);
            }

            var images = string.Join("<br/>", imageList);

            Assert.IsFalse(string.IsNullOrEmpty(images));
            
        }

        [TestMethod]
        public void ShouldGenerateAvatarWithRandomColors()
        {
            var ag = new AvatarGenerator(
               new RandomColorGenerator { Luminosity = Luminosity.Bright }
               )
            { ImageFormat = ImageFormat.Png };

            var imageList = new List<string>();
            for (var i = 0; i < 25; i++)
            {
                var stream = ag.Generate("Hello", "World");
                var base64Data = Convert.ToBase64String(stream.ToArray());

                var imgTag = $"<img src='data:image/png;base64,{base64Data}' />";
                imageList.Add(imgTag);
            }

            var images = string.Join("<br/>", imageList);

            Assert.IsFalse(string.IsNullOrEmpty(images));

        }

        [TestMethod]
        public void ShouldGenerateAvatarWithSpacedColors()
        {
            var ag = new AvatarGenerator(
               new SpacedColorGenerator { Accuracy = 50 }
               )
            { ImageFormat = ImageFormat.Png };

            var imageList = new List<string>();
            for (var i = 0; i < 25; i++)
            {
                var stream = ag.Generate("Hello", "World");
                var base64Data = Convert.ToBase64String(stream.ToArray());

                var imgTag = $"<img src='data:image/png;base64,{base64Data}' />";
                imageList.Add(imgTag);
            }

            var images = string.Join("<br/>", imageList);

            Assert.IsFalse(string.IsNullOrEmpty(images));

        }
    }
}
