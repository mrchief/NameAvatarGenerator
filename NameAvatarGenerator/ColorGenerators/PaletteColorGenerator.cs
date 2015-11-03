using System;
using System.Collections.Generic;
using System.Drawing;

namespace NameAvatarGenerator.ColorGenerators
{
    public class PaletteColorGenerator : IColorGenerator
    {
        private readonly string[] _colors = {"#f15c75", "#ac707a", "#654982", "#654982", "#8eb021", "#84bbc6", "#4a6785", "#59afe1", "#d39c3f", "#f1a257", "#f691b2", "#f79232", "#815b3a" };

        private int _index;

        public PaletteColorGenerator()
        {
            Shuffle(_colors);
        }

        private void Shuffle(IList<string> list)
        {
            var random = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = random.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


        public Color GetNextColor()
        {
            // cycle thru colors. if we reach end, reset to beginning
            if (_index == _colors.Length) _index = 0;
            return ColorTranslator.FromHtml(_colors[_index++]);
        }
    }
}