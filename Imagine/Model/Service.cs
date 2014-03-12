using Microsoft.Phone.Tasks;
using Nokia.Graphics.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Windows.Foundation;

namespace Imagine.Model
{
    public class Service
    {
        private static Service service;
        public static Service Instance
        {
            get
            {
                if (service == null)
                {
                    service = new Service();
                }
                return service;
            }
            set { service = value; }
        }

        private WriteableBitmap selectedimage;
        public WriteableBitmap SelectedImage
        {
            get { return selectedimage; }
            set { selectedimage = value; }
        }

        public Task<Stream> GetImageFromSystemAsyncLibrary()
        {
            TaskCompletionSource<Stream> source = new TaskCompletionSource<Stream>();
            PhotoChooserTask task = new PhotoChooserTask();
            task.ShowCamera = true;
            task.Completed += (s, e) =>
            {
                if (e.TaskResult == TaskResult.OK)
                {
                    source.SetResult(e.ChosenPhoto);
                }
                else
                {
                    source.SetResult(null);
                }
            };
            task.Show();
            return source.Task;
        }
        public Task<Stream> GetImageFromSystemAsyncCamera()
        {
            TaskCompletionSource<Stream> source = new TaskCompletionSource<Stream>();
            CameraCaptureTask task = new CameraCaptureTask();
            task.Completed += (s, e) =>
            {
                if (e.TaskResult == TaskResult.OK)
                {
                    source.SetResult(e.ChosenPhoto);
                }
                else
                {
                    source.SetResult(null);
                }
            };
            task.Show();
            return source.Task;
        }

        public async Task<WriteableBitmap> ApplyEffectFilter(StreamImageSource source, WriteableBitmap image, Rect rect)
        {
            var effect = new FilterEffect(source);
            var renderer = new WriteableBitmapRenderer(effect, image);
            var filters = new List<IFilter>();
            filters.Add(new NegativeFilter());
            filters.Add(new CropFilter(rect));
            effect.Filters = filters;
            await renderer.RenderAsync();
            return image;
        }

    }
}
