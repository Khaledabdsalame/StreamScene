
using System.Windows;
using System.Windows.Controls;



namespace Cinema_Platform_Application
{
    
    public partial class Errorpage : Page
    {
        public Errorpage()
        {
            InitializeComponent();
           

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = (MainWindow)Application.Current.MainWindow;
            window.Errorpagenav.Visibility = Visibility.Hidden;
            window.loadmovies("action");
        }
    }
}
