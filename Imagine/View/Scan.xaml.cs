using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Threading.Tasks;
using Imagine.Model;
using Windows.Foundation;

namespace Imagine.View
{
    public partial class Scan : PhoneApplicationPage
    {
        WriteableBitmap img;
        Stream stream;

        public Scan()
        {
            InitializeComponent();
        }

        public void SaveToMediaLibrary(WriteableBitmap bitmap, string name, int quality)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.SaveJpeg(stream, (int)image.ActualWidth, (int)image.ActualHeight, 0, quality);
                stream.Seek(0, SeekOrigin.Begin);
                new MediaLibrary().SavePicture(name, stream);
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("option"))
            {
                try
                {
                    switch (int.Parse(NavigationContext.QueryString["option"]))
                    {
                        case 0:
                            if (img == null)
                            {
                                img = new WriteableBitmap(100, 100);
                                stream = await Service.Instance.GetImageFromSystemAsyncLibrary();
                                img.SetSource(stream);
                                image.Source = img;
                            }
                            break;
                        case 1:
                            if (img == null)
                            {
                                img = new WriteableBitmap(100, 100);
                                stream = await Service.Instance.GetImageFromSystemAsyncCamera();
                                img.SetSource(stream);
                                image.Source = img;
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    NavigationService.Navigate(new Uri("/View/MainPage.xaml", UriKind.Relative));
                }
            }
        }

        private void InterationCanvas_manipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (e.CumulativeManipulation.Translation.X >= 0)
                brdCrop.Width = e.CumulativeManipulation.Translation.X;
            if (e.CumulativeManipulation.Translation.Y >= 0)
                brdCrop.Height = e.CumulativeManipulation.Translation.Y;
        }

        private void InteractionCanvas_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            Canvas.SetTop(brdCrop, e.ManipulationOrigin.Y);
            Canvas.SetLeft(brdCrop, e.ManipulationOrigin.X);
        }

        private async void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            Rect r = new Rect(Canvas.GetLeft(brdCrop), Canvas.GetTop(brdCrop), brdCrop.Width, brdCrop.Height);
            StreamImageSource _source;
            using (var memoryStream = new MemoryStream())
            {
                stream.Position = 0;
                stream.CopyTo(memoryStream);
                try
                {
                    stream.Flush();
                }
                catch (Exception ex)
                {
                    throw;
                }
                memoryStream.Position = 0;
                _source = new StreamImageSource(memoryStream);
                image.Source = await Service.Instance.ApplyEffectFilter(_source, img, r);
            }
            Canvas.SetLeft(brdCrop, 0);
            Canvas.SetTop(brdCrop, 0);
            brdCrop.Height = 0;
            brdCrop.Width = 0;
        }

    }
}