using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using Nokia.Graphics.Imaging;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using Microsoft.Devices;
using System.IO;
using Microsoft.Xna.Framework.Media;

namespace Imagine.View
{
    public partial class Scan : PhoneApplicationPage
    {
        int w, h;
        public Scan()
        {
            InitializeComponent();
            Loaded += Scan_Loaded;
        }

        private async void TakeScreenShot()
        {
            var screenshotname = String.Format("Photo-", DateTime.Now.Ticks);
            var filters = new FilterEffect(MainPage.source);
            Windows.Foundation.Rect r = new Windows.Foundation.Rect(Canvas.GetLeft(photo), Canvas.GetTop(photo), photo.ActualWidth, photo.ActualHeight);
            var effect = new CropFilter(r);
            var effect1 = new NegativeFilter();
            filters.Filters = new IFilter[] { effect, effect1 };
            var target = new WriteableBitmap((int)photo.ActualWidth, (int)photo.ActualHeight);
            var render = new WriteableBitmapRenderer(filters, target);
            await render.RenderAsync();
            BitmapImage bmp = new BitmapImage();
            using (var ms = new MemoryStream())
            {
                target.SaveJpeg(ms, w, h, 0, 100);
                bmp.SetSource(ms);
            }
            target = null;

            SaveToMediaLibrary(render.WriteableBitmap, screenshotname, 100);
            MessageBox.Show(String.Format("Image saved ", screenshotname));
        }

        public void SaveToMediaLibrary(WriteableBitmap bitmap, string name, int quality)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.SaveJpeg(stream, (int)photo.ActualWidth, (int)photo.ActualHeight, 0, quality);
                stream.Seek(0, SeekOrigin.Begin);
                new MediaLibrary().SavePicture(name, stream);
            }
        }

        async void Scan_Loaded(object sender, RoutedEventArgs e)
        {
            var filter = new FilterEffect(MainPage.source);
            var sampleFilter = new NegativeFilter();
            filter.Filters = new IFilter[] { sampleFilter };
            w = (int)grid.ActualWidth - 10;
            h = (int)grid.ActualHeight - 10;
            var target = new WriteableBitmap(w, h);
            var render = new WriteableBitmapRenderer(filter, target);
            await render.RenderAsync();
            target.Invalidate();
            BitmapImage bmp = new BitmapImage();
            using (var ms = new MemoryStream())
            {
                target.SaveJpeg(ms, w, h, 0, 100);
                bmp.SetSource(ms);
            }
            target = null;
            image.Source = bmp;
        }

        private void Event_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (e.CumulativeManipulation.Translation.X >= 0)
                photo.Width = e.CumulativeManipulation.Translation.X;
            if (e.CumulativeManipulation.Translation.Y >= 0)
                photo.Height = e.CumulativeManipulation.Translation.Y;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            TakeScreenShot();
        }

        private void Event_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            Canvas.SetTop(photo, e.ManipulationOrigin.Y);
            Canvas.SetLeft(photo, e.ManipulationOrigin.X);
        }
    }
}