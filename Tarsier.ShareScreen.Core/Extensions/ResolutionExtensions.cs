using System;
using System.Drawing;
using Tarsier.ShareScreen.Core.Enumerations;

namespace Tarsier.ShareScreen.Core.Extensions
{
    internal static class ResolutionExtensions
    {
        #region Screen resolutions

        private const int OneThousandAndEightyPWidth = 1920;
        private const int OneThousandAndEightyPHeight = 1080;

        private const int SevenHundredAndTwentyPWidth = 1080;
        private const int SevenHundredAndTwentyPHeight = 720;

        private const int FourHundredAndEightyPWidth = 854;
        private const int FourHundredAndEightyPHeight = 480;

        private const int ThreeHundredAndSixtyPWidth = 480;
        private const int ThreeHundredAndSixtyPHeight = 360;

        private const int TwoHundredAndFortyPWidth = 352;
        private const int TwoHundredAndFortyPHeight = 240;

        #endregion
        /// <summary>
        /// Provides width and height for the required screen resolution.
        /// </summary>
        /// <param name="resolution">The resolution of the screen whose size you need to know.</param>
        /// <returns>Width and height of resolution.</returns>
        public static Size GetResolutionSize(this ScreenResolution resolution) {
            switch (resolution) {
                case ScreenResolution.OneThousandAndEightyP:
                    return new Size(OneThousandAndEightyPWidth, OneThousandAndEightyPHeight);
                case ScreenResolution.SevenHundredAndTwentyP:
                    return new Size(SevenHundredAndTwentyPWidth, SevenHundredAndTwentyPHeight);
                case ScreenResolution.FourHundredAndEightyP:
                    return new Size(FourHundredAndEightyPWidth, FourHundredAndEightyPHeight);
                case ScreenResolution.ThreeHundredAndSixtyP:
                    return new Size(ThreeHundredAndSixtyPWidth, ThreeHundredAndSixtyPHeight);
                case ScreenResolution.TwoHundredAndFortyP:
                    return new Size(TwoHundredAndFortyPWidth, TwoHundredAndFortyPHeight);
                default:
                    throw new ArgumentOutOfRangeException(nameof(resolution), resolution, null);
            }
        }
    }
}