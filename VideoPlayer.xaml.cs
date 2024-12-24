
using System.Windows;
using System.Windows.Controls;
namespace Cinema_Platform_Application
{
    /// <summary>
    /// Interaction logic for VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : Page
    {
       
        public VideoPlayer()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var window = (MainWindow)Application.Current.MainWindow;

            window.videoplayer.Visibility = Visibility.Hidden ;
            Web.Dispose();
            window.Space.IsEnabled = true;
            window.Space.Effect = null;
            

        }
    }
}
