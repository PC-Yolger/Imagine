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

namespace Imagine.View
{
    public partial class Scan : PhoneApplicationPage
    {
        public Scan()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            using (var filter = new FilterEffect(MainPage.source))
            {
                var sampleFilter = new NegativeFilter();
                filter.Filters = new IFilter[] { sampleFilter };
                var target = new WriteableBitmap((int)image.ActualWidth, (int)image.ActualHeight);
                using (var render = new WriteableBitmapRenderer(filter, target))
                {
                    await render.RenderAsync();
                    image.Source = target;
                }
            }
        }
    }
}