using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NameAvatarGenerator.Tests
{
    [TestClass]
    public class AvatarGeneratorTests
    {
        [TestMethod]
        public void ShouldGenerateAvatar()
        {
            var ag = new AvatarGenerator();
            var imageList = new List<string>();
            for (int i = 0; i < 25; i++)
            {
                var stream = ag.Generate("Hello", "World");
                var base64Data = Convert.ToBase64String(stream.ToArray());

                var imgTag = $"<img src='data:image/png;base64,{base64Data} />";
                imageList.Add(imgTag);
            }

            var images = string.Join("<br/>", imageList);

            Assert.IsFalse(string.IsNullOrEmpty(images));
            
        }
    }
}
