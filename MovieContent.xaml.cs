

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;



namespace Cinema_Platform_Application
{
    /// <summary>
    /// Interaction logic for MovieContent.xaml
    /// </summary>
    public partial class MovieContent : Page
    {
        public string Url_trailer;
        public string magnetUrl;
        public string imdbId;
        public MovieContent()
        {
            InitializeComponent();
          

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            var window = (MainWindow)Application.Current.MainWindow;
            window.MovieContent.Visibility = Visibility.Hidden;
          

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var window = (MainWindow)Application.Current.MainWindow;
            var window2 = new VideoPlayer();
            
            window2.Web.Source = new Uri(Url_trailer, UriKind.Absolute);
            window.videoplayer.Navigate(window2);
            window.videoplayer.Visibility = Visibility.Visible;
            window.Space.Effect = new BlurEffect();
            window.Space.IsEnabled = false;








        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var window = new MoviePlayer(magnetUrl,imdbId);
            var mainwindow = (MainWindow)Application.Current.MainWindow;
            mainwindow.videoplayer.Navigate(window);
            mainwindow.videoplayer.Visibility = Visibility.Visible;
            mainwindow.Space.Effect = new BlurEffect();
            mainwindow.Space.IsEnabled = false;
           

        }

       


    }
}
