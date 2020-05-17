using System;
using System.Linq;
using System.Drawing.Imaging;
using System.Drawing;
using System.Reflection;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Interop;

namespace TowerDefenseGame.Renderer
{
    /// <summary>
    /// 
    /// </summary>
    public class TransformBitmap
    {
        /*
        TransformGroup tg = new TransformGroup();
        tg.Children.Add(new TranslateTransform(Centre().X, Centre().Y));
        tg.Children.Add(new RotateTransform(Math.PI / 2, Centre().X, Centre().Y));
        DynamicArea.Transform = tg;
        */
        /*
        public Geometry DynamicArea
        {
            get
            {
                _dynamicArea = new RectangleGeometry(Area);
                return _dynamicArea.GetFlattenedPathGeometry();
            }
            set => _dynamicArea = value;
        }
        */

        public static Bitmap RotateImg(Stream stream, float angle)
        {
            Bitmap bmp;
            if (stream == null)
            {
                bmp = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(GetEmbendedResourceInFolder("TowerDefenseGame.Image.Path.s200n802.bmp")[0]));
            }
            else
            {
                bmp = new Bitmap(stream);
            }
            
            int w = bmp.Width;
            int h = bmp.Height;
            
            Bitmap tempImg = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(tempImg);
            g.Clear(Color.Empty);
            g.DrawImageUnscaled(bmp, 1, 1);
            g.Dispose();

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix mtrx = new Matrix();
            //Using System.Drawing.Drawing2D.Matrix class 
            mtrx.Rotate(angle);
            RectangleF rct = path.GetBounds(mtrx);
            Bitmap newImg = new Bitmap(Convert.ToInt32(rct.Width), Convert.ToInt32(rct.Height));
            g = Graphics.FromImage(newImg);
            g.Clear(Color.Empty);
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(tempImg, 0, 0);
            g.Dispose();
            tempImg.Dispose();
            return newImg;
        }

        private static string[] GetEmbendedResourceInFolder(string folder)
        {
            var assembly = Assembly.GetCallingAssembly().GetManifestResourceNames();
            string[] res = assembly.Where(x => x.Contains(folder)).Select(x => x).ToArray();
            return res;
        }

        public static System.Windows.Media.Imaging.BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            System.Windows.Media.Imaging.BitmapImage retval;

            try
            {
                retval = (System.Windows.Media.Imaging.BitmapImage)Imaging.CreateBitmapSourceFromHBitmap(
                             hBitmap,
                             IntPtr.Zero,
                             System.Windows.Int32Rect.Empty,
                             System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Exception)
            {
                retval = null;
            }

            return retval;
        }
    }
}
