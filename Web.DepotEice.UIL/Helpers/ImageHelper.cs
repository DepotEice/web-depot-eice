using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace Web.DepotEice.UIL.Helpers
{
    public enum ImageFormat
    {
        JPG,
        JPEG,
        PNG,
        GIF
    }

    public static class ImageHelper
    {
        /// <summary>
        /// Crop an image with the given coordinates and return it
        /// </summary>
        /// <param name="bytes">The array of bytes containing the image data</param>
        /// <param name="x">The position on the X (horizontal) axis from where the image must be cropped</param>
        /// <param name="y">The position on the Y (vertical) axis from where the image must be cropped</param>
        /// <param name="width">The width to crop</param>
        /// <param name="height">The height to crop</param>
        /// <returns></returns>
        public static Image Crop(byte[] bytes, int x, int y, int width, int height)
        {
            using (var image = Image.Load(bytes))
            {
                image.Mutate(img => img.Crop(new Rectangle(x, y, width, height)));

                return image.Clone(ctx => { });
            }
        }

        /// <summary>
        /// Return an array of bytes from <paramref name="image"/>
        /// </summary>
        /// <param name="image">The image to convert</param>
        /// <param name="imageFormat">The format of the image</param>
        /// <returns>
        /// An array of bytes <see cref="byte[]"/> from the image <paramref name="image"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static byte[] GetBytes(Image image, ImageFormat imageFormat)
        {
            if (image is null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            IImageEncoder imageEncoder = imageFormat switch
            {
                ImageFormat.JPG or ImageFormat.JPEG => new JpegEncoder(),
                ImageFormat.PNG => new PngEncoder(),
                ImageFormat.GIF => new GifEncoder(),
                _ => throw new ArgumentException("The provided format is incorrect"),
            };

            if (imageEncoder is null)
            {
                throw new NullReferenceException($"{nameof(imageEncoder)} should never be null!");
            }

            byte[]? bytes = null;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, imageEncoder);

                bytes = memoryStream.ToArray();
            }

            if (bytes is null)
            {
                throw new NullReferenceException($"{nameof(bytes)} should never be null.");
            }

            return bytes;
        }

        /// <summary>
        /// Get the encoder for the given <paramref name="imageFormat"/>
        /// </summary>
        /// <param name="imageFormat">The format of the image</param>
        /// <returns>An instance of the correct Encoder depending on <paramref name="imageFormat"/></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static IImageEncoder GetEncoder(ImageFormat imageFormat)
        {
            switch (imageFormat)
            {
                case ImageFormat.JPG:
                case ImageFormat.JPEG:
                    return new JpegEncoder();

                case ImageFormat.PNG:
                    return new PngEncoder();

                case ImageFormat.GIF:
                    return new GifEncoder();

                default:
                    throw new NotSupportedException($"Unsupported image format '{imageFormat}'.");
            }
        }
    }
}

