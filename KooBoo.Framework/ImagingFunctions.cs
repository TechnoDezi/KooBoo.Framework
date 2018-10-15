using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KooBoo.Framework
{
    public class ImagingFunctions
    {
        public MemoryStream ResizeImagePreportional(string fileUrl, int maxWidth, int maxHeight)
        {
            //Resize Image
            ImageResizer.ResizeSettings settings = new ImageResizer.ResizeSettings();
            settings.MaxHeight = maxHeight;
            settings.MaxWidth = maxWidth;
            settings.Mode = ImageResizer.FitMode.Max;

            MemoryStream outputStream = new MemoryStream();
            ImageResizer.ImageBuilder.Current.Build(fileUrl, outputStream, settings);

            return outputStream;
        }

        public MemoryStream ResizeImagePreportional(Stream blobStream, int maxWidth, int maxHeight)
        {
            //Resize Image
            ImageResizer.ResizeSettings settings = new ImageResizer.ResizeSettings();
            settings.MaxHeight = maxHeight;
            settings.MaxWidth = maxWidth;
            settings.Mode = ImageResizer.FitMode.Max;

            MemoryStream outputStream = new MemoryStream();
            ImageResizer.ImageBuilder.Current.Build(blobStream, outputStream, settings);

            return outputStream;
        }
    }
}
