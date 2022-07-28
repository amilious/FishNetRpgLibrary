using UnityEngine;

namespace Amilious.Core.Extensions {
    public static class Texture2DExtensions {
        
        /// <summary>
        /// This method is used to add a watermark to another texture in the bottom-right-hand corner.
        /// </summary>
        /// <param name="mainTexture">The texture that you want to add the watermark to.</param>
        /// <param name="watermark">The watermark that you want to add to the main texture.</param>
        /// <param name="offset">The offset from the bottom-right of the main texture.</param>
        /// <returns>A texture with the watermark applied.</returns>
        public static Texture2D AddWatermark(this Texture2D mainTexture, Texture2D watermark, int offset = 0) {
            if(watermark == null) return mainTexture;
            if(!watermark.isReadable) {
                Debug.LogError("The watermark must be readable!");
                return mainTexture;
            }
            // Create a new writable texture.
            var result = new Texture2D(mainTexture.width, mainTexture.height);

            // Draw watermark at bottom right corner.
            var startX = mainTexture.width - watermark.width - offset;
            var endY = watermark.height + offset;

            for (var x = 0; x < mainTexture.width; x++) {
                for (var y = 0; y < mainTexture.height; y++) {
                    var bgColor = mainTexture.GetPixel(x, y);
                    var wmColor = new Color(0, 0, 0, 0);

                    // Change this test if no longer drawing at the bottom right corner.
                    if (x >= startX && y <= endY &&y>=offset)
                        wmColor = watermark.GetPixel(x-startX, y-offset);

                    switch(wmColor.a) {
                        case 0:
                            result.SetPixel(x, y, bgColor);
                            break;
                        case 1:
                            result.SetPixel(x, y, wmColor);
                            break;
                        default:
                            var blended = bgColor * (1.0f - wmColor.a) + wmColor;
                            blended.a = 1.0f;
                            result.SetPixel(x, y, blended);
                            break;
                    }
                }
            }
            result.Apply();
            return result;
        }
        
        
    }
}