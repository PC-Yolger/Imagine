using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Imagine.Resources;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using Imagine.View;
using Nokia.Graphics.Imaging;

namespace Imagine
{
    public partial class MainPage : PhoneApplicationPage
    {
        PhotoChooserTask pct = new PhotoChooserTask();
        CameraCaptureTask cct = new CameraCaptureTask();

        public static string url;
        public static StreamImageSource source;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            lstoptions.SelectedItem = null;
            pct.Completed += image_Completed;
            cct.Completed += image_Completed;
            // Código de ejemplo para traducir ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        void image_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                MainPage.source = new StreamImageSource(e.ChosenPhoto);
                NavigationService.Navigate(new Uri("/View/Scan.xaml", UriKind.Relative));
                //bmp = new BitmapImage();
                //bmp.SetSource(e.ChosenPhoto);
                //source = new StreamImageSource(e.ChosenPhoto);
                //effect = new FilterEffect(source);
                
            }
        }

        private void lstoptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstoptions.SelectedItem != null)
            {
                switch (lstoptions.SelectedIndex)
                {
                    case 0:
                        pct.Show();
                        break;
                    case 1:
                        cct.Show();
                        break;
                    default:
                        break;
                }
            }
        }

        // Código de ejemplo para compilar una ApplicationBar traducida
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Establecer ApplicationBar de la página en una nueva instancia de ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Crear un nuevo botón y establecer el valor de texto en la cadena traducida de AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Crear un nuevo elemento de menú con la cadena traducida de AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}