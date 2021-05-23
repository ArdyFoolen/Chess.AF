using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.ChessForm.Extensions
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Creates color with corrected brightness.
        /// </summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1. 
        /// Negative values produce darker colors.</param>
        /// <returns>
        /// Corrected <see cref="Color"/> structure.
        /// </returns>
        public static Color ChangeColorBrightness(this Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            red = correctWithFactor(red, correctionFactor);
            green = correctWithFactor(green, correctionFactor);
            blue = correctWithFactor(blue, correctionFactor);

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        private static float correctWithFactor(float color, float factor)
        {
            var newValue = color * (factor < 0 ? 0 : factor);
            return newValue > 255 ? 255f : newValue;
        }
    }
}
