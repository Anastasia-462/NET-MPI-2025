using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GameOfLife
{
    class AdWindow : Window
    {
        private readonly DispatcherTimer adTimer;
        private readonly ImageBrush myBrush = new ImageBrush();
        private readonly BitmapImage[] preloadedImages;
        private string[] adImages = new string[] { "ad1.jpg", "ad2.jpg", "ad3.jpg" };

        private int imgNmb;     // the number of the image currently shown
        private string link;    // the URL where the currently shown ad leads to

        public AdWindow(Window owner)
        {
            var rnd = new Random();
            Owner = owner;
            Width = 350;
            Height = 100;
            ResizeMode = ResizeMode.NoResize;
            WindowStyle = WindowStyle.ToolWindow;
            Title = "Support us by clicking the ads";
            Cursor = Cursors.Hand;
            ShowActivated = false;
            MouseDown += OnClick;

            imgNmb = rnd.Next(1, 3);

            preloadedImages = new BitmapImage[adImages.Length];

            for (int i = 0; i < adImages.Length; i++)
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(adImages[i], UriKind.Relative);
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                preloadedImages[i] = bitmapImage;
            }

            ChangeAds(this, new EventArgs());

            // Run the timer that changes the ad's image 
            adTimer = new DispatcherTimer();
            adTimer.Interval = TimeSpan.FromSeconds(3);
            adTimer.Tick += ChangeAds;
            adTimer.Start();
        }

        private void OnClick(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(link);
            Close();
        }
        
        protected override void OnClosed(EventArgs e)
        {
            Unsubscribe();
            base.OnClosed(e);
        } 

        public void Unsubscribe()
        {
            adTimer.Stop();
            adTimer.Tick -= ChangeAds;
            MouseDown -= OnClick;
        }

        private void ChangeAds(object sender, EventArgs eventArgs)
        {
            Background = myBrush;
            link = "http://example.com";

            switch (imgNmb)
            {
                case 1:
                    myBrush.ImageSource = preloadedImages[0];
                    imgNmb++;
                    break;
                case 2:
                    myBrush.ImageSource = preloadedImages[1];
                    imgNmb++;
                    break;
                case 3:
                    myBrush.ImageSource = preloadedImages[2];
                    imgNmb = 1;
                    break;
            }
            
        }
    }
}