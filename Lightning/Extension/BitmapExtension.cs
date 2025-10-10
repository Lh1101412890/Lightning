using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Lightning.Extension
{
    public static class BitmapExtension
    {
        /// <summary>
        /// 指定imageSource的尺寸
        /// </summary>
        /// <param name="imageSource"></param>
        /// <param name="size">size * size</param>
        /// <returns></returns>
        public static ImageSource Resize(this ImageSource imageSource, int size)
        {
            var rect = new Rect(0, 0, size, size);
            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawImage(imageSource, rect);
            }
            var resizedImage = new RenderTargetBitmap((int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Default);
            resizedImage.Render(drawingVisual);
            return resizedImage;
        }

        /// <summary>
        /// 指定imageSource的宽度，高度随比例调整
        /// </summary>
        /// <param name="imageSource"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static ImageSource ResizeOfWidth(this ImageSource imageSource, int width)
        {
            int height = (int)(imageSource.Height * width / imageSource.Width);
            var rect = new Rect(0, 0, width, height);
            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawImage(imageSource, rect);
            }
            var resizedImage = new RenderTargetBitmap((int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Default);
            resizedImage.Render(drawingVisual);
            return resizedImage;
        }

        /// <summary>
        /// 指定imageSource的高度，宽度随比例调整
        /// </summary>
        /// <param name="imageSource"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static ImageSource ResizeOfHeight(this ImageSource imageSource, int height)
        {
            int width = (int)(imageSource.Width * height / imageSource.Height);
            var rect = new Rect(0, 0, width, height);
            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawImage(imageSource, rect);
            }
            var resizedImage = new RenderTargetBitmap((int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Default);
            resizedImage.Render(drawingVisual);
            return resizedImage;
        }

        /// <summary>
        /// 将Bitmap转换为BitmapImage
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            Stream stream = new MemoryStream();
            new Bitmap(bitmap).Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            return bitmapImage;
        }

    }
}