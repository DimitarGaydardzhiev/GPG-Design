using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;

namespace GPGDesign.ImageUtils
{
    public static class ImageWorker
    {
        private const double MaxImageWidth = 720d;

        public static string ResizeImage(MemoryStream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using (Image<Rgba32> image = Image.Load(stream))
            {
                var ratio = (double)image.Width / (double)image.Height;
                var newImageWidth = MaxImageWidth;
                var newImageHeight = newImageWidth / ratio;

                image.Mutate(x => x
                     .Resize((int)newImageWidth, (int)newImageHeight)
                );
                
                return image.ToBase64String(ImageFormats.Jpeg);
            }
        }

        public static bool IsImage(IFormFile file)
        {
            if (!string.Equals(file.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}
