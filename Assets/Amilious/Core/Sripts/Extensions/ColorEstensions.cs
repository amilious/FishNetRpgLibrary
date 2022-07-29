using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add methods to the color class.
    /// </summary>
    public static class ColorExtensions {

        /// <summary>
        /// This method is used to get the RGBA hex value of the color.
        /// </summary>
        /// <param name="color">The color that you want to get the hex value for.</param>
        /// <returns>The RGBA hex value of the color.</returns>
        // ReSharper disable once InconsistentNaming
        public static string HtmlRGBA(this Color color) => ColorUtility.ToHtmlStringRGBA(color);

        /// <summary>
        /// This method is used to get the RGB hex value of the color.
        /// </summary>
        /// <param name="color">The color that you want to get the hex value for.</param>
        /// <returns>The RGB hex value of the color.</returns>
        public static string HtmlRGB(this Color color) => ColorUtility.ToHtmlStringRGB(color);

        /// <summary>
        /// This method is used to create a texture filled with the color.
        /// </summary>
        /// <param name="color">The color you want to use to generate a texture.</param>
        /// <param name="width">The width of the generated texture.</param>
        /// <param name="height">The height of the generated texture.</param>
        /// <returns>A texture generated from the given color.</returns>
        public static Texture2D MakeTexture(this Color color, int width, int height) {
            var pix = new Color[width * height];
            Array.Fill(pix,color);
            var result = new Texture2D( width, height );
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        /// <summary>
        /// This method is used to create a texture filled with the color.
        /// </summary>
        /// <param name="color">The color you want to use to generate a texture.</param>
        /// <param name="width">The width of the generated texture.</param>
        /// <param name="height">The height of the generated texture.</param>
        /// <param name="format">The format of the generated texture.</param>
        /// <param name="flags">The texture creation flags to use when generating the texture.</param>
        /// <returns>A texture generated from the given color.</returns>
        public static Texture2D MakeTexture(this Color color, int width, int height, DefaultFormat format, 
            TextureCreationFlags flags) {
            var pix = new Color[width * height];
            Array.Fill(pix,color);
            var result = new Texture2D(width, height, format, flags );
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
        
        /// <summary>
        /// This method is used to create a texture filled with the color.
        /// </summary>
        /// <param name="color">The color you want to use to generate a texture.</param>
        /// <param name="width">The width of the generated texture.</param>
        /// <param name="height">The height of the generated texture.</param>
        /// <param name="format">The format of the generated texture.</param>
        /// <param name="flags">The texture creation flags to use when generating the texture.</param>
        /// <returns>A texture generated from the given color.</returns>
        public static Texture2D MakeTexture(this Color color, int width, int height, GraphicsFormat format, 
            TextureCreationFlags flags) {
            var pix = new Color[width * height];
            Array.Fill(pix,color);
            var result = new Texture2D(width, height, format, flags );
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        /// <summary>
        /// This method is used to create a texture filled with the color.
        /// </summary>
        /// <param name="color">The color you want to use to generate a texture.</param>
        /// <param name="width">The width of the generated texture.</param>
        /// <param name="height">The height of the generated texture.</param>
        /// <param name="format">The format of the generated texture.</param>
        /// <param name="mipCount">The mipMap count for the generated texture.</param>
        /// <param name="flags">The texture creation flags to use when generating the texture.</param>
        /// <returns>A texture generated from the given color.</returns>
        public static Texture2D MakeTexture(this Color color, int width, int height, GraphicsFormat format, 
            int mipCount, TextureCreationFlags flags) {
            var pix = new Color[width * height];
            Array.Fill(pix,color);
            var result = new Texture2D(width, height, format, mipCount, flags );
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
        
        /// <summary>
        /// This method is used to create a texture filled with the color.
        /// </summary>
        /// <param name="color">The color you want to use to generate a texture.</param>
        /// <param name="width">The width of the generated texture.</param>
        /// <param name="height">The height of the generated texture.</param>
        /// <param name="textureFormat">The format of the generated texture.</param>
        /// <param name="mipChain">If true a mip chain will be generated.</param>
        /// <returns>A texture generated from the given color.</returns>
        public static Texture2D MakeTexture(this Color color, int width, int height, TextureFormat textureFormat, 
            bool mipChain) {
            var pix = new Color[width * height];
            Array.Fill(pix,color);
            var result = new Texture2D(width, height,textureFormat, mipChain);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        /// <summary>
        /// This method is used to create a texture filled with the color.
        /// </summary>
        /// <param name="color">The color you want to use to generate a texture.</param>
        /// <param name="width">The width of the generated texture.</param>
        /// <param name="height">The height of the generated texture.</param>
        /// <param name="textureFormat">The format of the generated texture.</param>
        /// <param name="mipChain">If true a mip chain will be generated.</param>
        /// <param name="linear">If the texture should be linearly generated.</param>
        /// <returns>A texture generated from the given color.</returns>
        public static Texture2D MakeTexture(this Color color, int width, int height, TextureFormat textureFormat, 
            bool mipChain, bool linear) {
            var pix = new Color[width * height];
            Array.Fill(pix,color);
            var result = new Texture2D(width, height,textureFormat,mipChain,linear);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
        
        /// <summary>
        /// This method is used to create a texture filled with the color.
        /// </summary>
        /// <param name="color">The color you want to use to generate a texture.</param>
        /// <param name="width">The width of the generated texture.</param>
        /// <param name="height">The height of the generated texture.</param>
        /// <param name="textureFormat">The format of the generated texture.</param>
        /// <param name="mipCount">The mipMap count for the generated texture.</param>
        /// <param name="linear">If the texture should be linearly generated.</param>
        /// <returns>A texture generated from the given color.</returns>
        public static Texture2D MakeTexture(this Color color, int width, int height, TextureFormat textureFormat, 
            int mipCount, bool linear) {
            var pix = new Color[width * height];
            Array.Fill(pix,color);
            var result = new Texture2D(width, height,textureFormat,mipCount,linear);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
        
    }
}