

using System.Net.Http;
using System.Windows;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using LibVLCSharp.Shared;
using System.Windows.Input;
namespace Cinema_Platform_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {

        List<Movie> moviesList = new List<Movie>();

        List<int> randomList = GenerateUniqueRandomList(8, 0, 50);
        List <Movie> SearchList = new List<Movie>();


        public MainWindow()
        {

            InitializeComponent();
            Core.Initialize();
            loadmovies("action");



        }
        public class Movie
        {
            public string Title { get; set; }
            public int Year { get; set; }
            public double Rating { get; set; }
            public string ImageUrl { get; set; }
            public string Description { get; set; }
            public List<string> Genere { get; set; }
            public string YouTubeTrailerUrl { get; set; }
            public string MagnetUrl { get; set; }
            public string ImdbId { get; set; }

        }


    
        public async Task loadmovies(string genere)
        {

            string apiUrl = "https://yts.mx/api/v2/list_movies.json?&genre=" + genere + "&limit=50";


            using (HttpClient client = new HttpClient())
            {

                try
                {
                    string response = await client.GetStringAsync(apiUrl);

                    JObject jsonResponse = JObject.Parse(response);
                    var movies = jsonResponse["data"]["movies"];

                    if (movies != null)
                    {
                        foreach (var movie in movies)
                        {
                            var torrents = movie["torrents"];
                            string magnetUrl = torrents != null && torrents.HasValues
                                ? $"magnet:?xt=urn:btih:{torrents[0]["hash"]}" // Construct magnet URL
                                : null;
                            var movieData = new Movie
                            {
                                Title = movie["title"]?.ToString(),
                                Year = movie["year"]?.ToObject<int>() ?? 0,
                                Rating = movie["rating"]?.ToObject<double>() ?? 0.0,
                                ImageUrl = movie["medium_cover_image"]?.ToString(),
                                Description = movie["summary"]?.ToString(),
                                Genere = movie["genres"]?.ToObject<List<string>>() ?? new List<string>(),
                                YouTubeTrailerUrl = movie["yt_trailer_code"] != null
                               ? $"https://www.youtube.com/embed/{movie["yt_trailer_code"]}"
                                : null,
                                MagnetUrl = magnetUrl,
                                ImdbId = movie["imdb_code"]?.ToString()

                            };
                            moviesList.Add(movieData);
                        }



                        Storyboard fadeStoryboard = (Storyboard)FindResource("FadeAnimation");
                        fadeStoryboard.Completed += (s, _) =>
                        {
                            try
                            {
                                ElementSerie.Content = new Image
                                {
                                    Source = new BitmapImage(new Uri(moviesList[randomList[0]].ImageUrl, UriKind.Absolute)),
                                    Stretch = System.Windows.Media.Stretch.Fill
                                };

                                Element2.Content = new Image
                                {
                                    Source = new BitmapImage(new Uri(moviesList[randomList[1]].ImageUrl, UriKind.Absolute)),
                                    Stretch = System.Windows.Media.Stretch.Fill
                                };

                                
                                Element3.Content = new Image
                                {
                                    Source = new BitmapImage(new Uri(moviesList[randomList[2]].ImageUrl, UriKind.Absolute)),
                                    Stretch = System.Windows.Media.Stretch.Fill
                                };

                                Element4.Content = new Image
                                {
                                    Source = new BitmapImage(new Uri(moviesList[randomList[3]].ImageUrl, UriKind.Absolute)),
                                    Stretch = System.Windows.Media.Stretch.Fill
                                };
                                Element5.Content = new Image
                                {
                                    Source = new BitmapImage(new Uri(moviesList[randomList[4]].ImageUrl, UriKind.Absolute)),
                                    Stretch = System.Windows.Media.Stretch.Fill
                                };

                                Element6.Content = new Image
                                {
                                    Source = new BitmapImage(new Uri(moviesList[randomList[5]].ImageUrl, UriKind.Absolute)),
                                    Stretch = System.Windows.Media.Stretch.Fill
                                };

                                Element7.Content = new Image
                                {
                                    Source = new BitmapImage(new Uri(moviesList[randomList[6]].ImageUrl, UriKind.Absolute)),
                                    Stretch = System.Windows.Media.Stretch.Fill
                                };

                                Element8.Content = new Image
                                {
                                    Source = new BitmapImage(new Uri(moviesList[randomList[7]].ImageUrl, UriKind.Absolute)),
                                    Stretch = System.Windows.Media.Stretch.Fill
                                };


                            }
                            catch (Exception ex)
                            {


                            }
                        };
                        fadeStoryboard.Begin(ElementSerie);
                        fadeStoryboard.Begin(Element2);
                        fadeStoryboard.Begin(Element3);
                        fadeStoryboard.Begin(Element4);
                        fadeStoryboard.Begin(Element5);
                        fadeStoryboard.Begin(Element6);
                        fadeStoryboard.Begin(Element7);
                        fadeStoryboard.Begin(Element8);


                    }
                }
                catch (Exception ex)
                {

                    Errorpagenav.Navigate(new Errorpage());
                    Errorpagenav.Visibility = Visibility.Visible;
                    MovieContent.Visibility = Visibility.Hidden;
                }
            }
        }



        static List<int> GenerateUniqueRandomList(int size, int minVal, int maxVal)
        {
            if (size > (maxVal - minVal + 1))
            {
                throw new ArgumentException("The range is too small for the requested number of unique values.");
            }

            Random random = new Random();
            HashSet<int> uniqueNumbers = new HashSet<int>();

            while (uniqueNumbers.Count < size)
            {
                int num = random.Next(minVal, maxVal + 1);
                uniqueNumbers.Add(num);
            }

            return new List<int>(uniqueNumbers);
        }

        private async void clickcat(object sender, RoutedEventArgs e)
        {

            moviesList.Clear();
            await loadmovies("action");
            randomList.Clear();
            randomList = GenerateUniqueRandomList(8, 0, 50);
            Text.Content = "Top Of The Month";




        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            moviesList.Clear();
            await loadmovies("adventure");
            randomList.Clear();
            randomList = GenerateUniqueRandomList(8, 0, 50);
            Text.Content = "Top Of The Month";
        }


        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            moviesList.Clear();
            await loadmovies("comedy");
            randomList.Clear();
            randomList = GenerateUniqueRandomList(8, 0, 50);
            Text.Content = "Top Of The Month";
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            moviesList.Clear();
            await loadmovies("drama");
            randomList.Clear();
            randomList = GenerateUniqueRandomList(8, 0, 50);
            Text.Content = "Top Of The Month";
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            moviesList.Clear();
            await loadmovies("horror");
            randomList.Clear();
            randomList = GenerateUniqueRandomList(8, 0, 50);
            Text.Content = "Top Of The Month";
        }

        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            moviesList.Clear();
            await loadmovies("fantasy");
            randomList.Clear();
            randomList = GenerateUniqueRandomList(8, 0, 50);
            Text.Content = "Top Of The Month";
        }

        private async void Button_Click_6(object sender, RoutedEventArgs e)
        {
            moviesList.Clear();
            await loadmovies("thriller");
            randomList.Clear();
            randomList = GenerateUniqueRandomList(8, 0, 50);
            Text.Content = "Top Of The Month";
        }

        private async void Button_Click_7(object sender, RoutedEventArgs e)
        {
            moviesList.Clear();
            await loadmovies("romance");
            randomList.Clear();
            randomList = GenerateUniqueRandomList(8, 0, 50);
            Text.Content = "Top Of The Month";
        }

        private async void Button_Click_8(object sender, RoutedEventArgs e)
        {
            moviesList.Clear();
            await loadmovies("animation");
            randomList.Clear();
            randomList = GenerateUniqueRandomList(8, 0, 50);
            Text.Content = "Top Of The Month";
        }

        private async void Button_Click_9(object sender, RoutedEventArgs e)
        {
            moviesList.Clear();
            await loadmovies("documentary");
            randomList.Clear();
            randomList = GenerateUniqueRandomList(8, 0, 50);
            Text.Content = "Top Of The Month";
        }


        private async void Button_Click_11(object sender, RoutedEventArgs e)
        {
            moviesList.Clear();
            await loadmovies("crime");
            randomList.Clear();
            randomList = GenerateUniqueRandomList(8, 0, 50);
            Text.Content = "Top Of The Month";
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            moviesList.Clear();
            await loadmovies("war");
            randomList.Clear();
            randomList = GenerateUniqueRandomList(8, 0, 50);
            Text.Content = "Top Of The Month";
        }

        private async void Button_Click_10(object sender, RoutedEventArgs e)
        {
            moviesList.Clear();
            await loadmovies("western");
            randomList.Clear();
            randomList = GenerateUniqueRandomList(8, 0, 50);
            Text.Content = "Top Of The Month";
        }

        private void Errorpagenav_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {


        }

        private void ElementSerie_Click(object sender, RoutedEventArgs e)
        {
            Get8info(0);



        }

        private void Element2_Click(object sender, RoutedEventArgs e)
        {
            Get8info(1);


        }

        private void Element3_Click(object sender, RoutedEventArgs e)
        {
            Get8info(2);
        }

        private void Element4_Click(object sender, RoutedEventArgs e)
        {
            Get8info(3);
        }

        private void Element5_Click(object sender, RoutedEventArgs e)
        {
            Get8info(4);

        }

        private void Element6_Click(object sender, RoutedEventArgs e)
        {
            Get8info(5);

        }

        private void Element7_Click(object sender, RoutedEventArgs e)
        {
            Get8info(6);
        }
        private void Element8_Click(object sender, RoutedEventArgs e)
        {
            Get8info(7);
        }


        private void videoplayer_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        private void Get8info(int Index)
        {
            var window = new MovieContent();

            try
            {
                window.Movie_Img.Content = new Image
                {

                    Source = new BitmapImage(new Uri(moviesList[randomList[Index]].ImageUrl, UriKind.Absolute)),
                    Stretch = System.Windows.Media.Stretch.Fill

                };
                window.Title.Content += moviesList[randomList[Index]].Title;
                window.Rating.Content += moviesList[randomList[Index]].Rating.ToString();
                window.Year.Content += moviesList[randomList[Index]].Year.ToString();


                window.Genere.Content += string.Join(",", moviesList[randomList[Index]].Genere);

                Paragraph Summary = new Paragraph(new Run(moviesList[randomList[Index]].Description));
                window.Description.Document.Blocks.Add(Summary);
                window.Url_trailer = moviesList[randomList[Index]].YouTubeTrailerUrl;
                window.magnetUrl = moviesList[randomList[Index]].MagnetUrl;
                window.imdbId = moviesList[randomList[Index]].ImdbId;
            }
            catch (Exception ex)
            {
                Errorpagenav.Navigate(new Errorpage());
                Errorpagenav.Visibility = Visibility.Visible;
            }

            if (Errorpagenav.Visibility == Visibility.Visible)
            {
                MovieContent.Visibility = Visibility.Hidden;
            }
            else
            {
                MovieContent.Navigate(window);
                MovieContent.Visibility = Visibility.Visible;

            }
        }

        private void FullScreen(object sender, RoutedEventArgs e)
        {
            if ((WindowState == WindowState.Normal || WindowState == WindowState.Maximized) && WindowStyle == WindowStyle.SingleBorderWindow)
            {
                WindowState = WindowState.Maximized;
                WindowStyle = WindowStyle.None;
                Visibility = Visibility.Collapsed;
                ResizeMode = ResizeMode.NoResize;
                Visibility = Visibility.Visible;

            }
            else
            {
                WindowState = WindowState.Normal;
                WindowStyle = WindowStyle.SingleBorderWindow;
                ResizeMode = ResizeMode.CanResize;
            }


        }

        public async Task LoadMovieSearch(string Queary)
        {

            string apiUrl = $"https://yts.mx/api/v2/list_movies.json?query_term={Queary}&limit=8&sort_by=rating&order_by=desc";


            using (HttpClient client = new HttpClient())
            {

                try
                {
                    string response = await client.GetStringAsync(apiUrl);

                    JObject jsonResponse = JObject.Parse(response);
                    var movies = jsonResponse["data"]["movies"];

                    if (movies != null)
                    {
                        foreach (var movie in movies)
                        {
                            var torrents = movie["torrents"];
                            string magnetUrl = torrents != null && torrents.HasValues
                                ? $"magnet:?xt=urn:btih:{torrents[0]["hash"]}" // Construct magnet URL
                                : null;
                            var movieData = new Movie
                            {
                                Title = movie["title"]?.ToString(),
                                Year = movie["year"]?.ToObject<int>() ?? 0,
                                Rating = movie["rating"]?.ToObject<double>() ?? 0.0,
                                ImageUrl = movie["medium_cover_image"]?.ToString(),
                                Description = movie["summary"]?.ToString(),
                                Genere = movie["genres"]?.ToObject<List<string>>() ?? new List<string>(),
                                YouTubeTrailerUrl = movie["yt_trailer_code"] != null
                               ? $"https://www.youtube.com/embed/{movie["yt_trailer_code"]}"
                                : null,
                                MagnetUrl = magnetUrl,
                                ImdbId = movie["imdb_code"]?.ToString()

                            };
                            SearchList.Add(movieData);
                        }



                        Storyboard fadeStoryboard = (Storyboard)FindResource("FadeAnimation");
                        fadeStoryboard.Completed += (s, _) =>
                        {
                            try
                            {
                                Element2.Content = new Image
                                {
                                    Source = new BitmapImage(new Uri(SearchList[1].ImageUrl, UriKind.Absolute)),
                                    Stretch = System.Windows.Media.Stretch.Fill
                                };
                                

                                ElementSerie.Content = new Image
                                {
                                    Source = new BitmapImage(new Uri(SearchList[0].ImageUrl, UriKind.Absolute)),
                                    Stretch = System.Windows.Media.Stretch.Fill
                                };
                                
                                Element3.Content = new Image
                                {
                                    Source = new BitmapImage(new Uri(SearchList[2].ImageUrl, UriKind.Absolute)),
                                    Stretch = System.Windows.Media.Stretch.Fill
                                };
                                

                                Element4.Content = new Image
                                {
                                    Source = new BitmapImage(new Uri(SearchList[3].ImageUrl, UriKind.Absolute)),
                                    Stretch = System.Windows.Media.Stretch.Fill
                                };
                               
                                Element5.Content = new Image
                                {
                                    Source = new BitmapImage(new Uri(SearchList[4].ImageUrl, UriKind.Absolute)),
                                    Stretch = System.Windows.Media.Stretch.Fill
                                };
                                

                                Element6.Content = new Image
                                {
                                    Source = new BitmapImage(new Uri(SearchList[5].ImageUrl, UriKind.Absolute)),
                                    Stretch = System.Windows.Media.Stretch.Fill
                                };
                               
                                Element7.Content = new Image
                                {
                                    Source = new BitmapImage(new Uri(SearchList[6].ImageUrl, UriKind.Absolute)),
                                    Stretch = System.Windows.Media.Stretch.Fill
                                };
                               

                                Element8.Content = new Image
                                {
                                    Source = new BitmapImage(new Uri(SearchList[7].ImageUrl, UriKind.Absolute)),
                                    Stretch = System.Windows.Media.Stretch.Fill
                                };
                              
                                

                            }
                            catch (Exception ex)
                            {


                            }
                        };
                        fadeStoryboard.Begin(ElementSerie);
                        fadeStoryboard.Begin(Element2);
                        fadeStoryboard.Begin(Element3);
                        fadeStoryboard.Begin(Element4);
                        fadeStoryboard.Begin(Element5);
                        fadeStoryboard.Begin(Element6);
                        fadeStoryboard.Begin(Element7);
                        fadeStoryboard.Begin(Element8);


                    }
                }
                catch (Exception ex)
                {

                    Errorpagenav.Navigate(new Errorpage());
                    Errorpagenav.Visibility = Visibility.Visible;
                    MovieContent.Visibility = Visibility.Hidden;
                }
            }
        }

       

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            
             if (searchbox.Text != null&&e.Key==Key.Enter)
            {
                SearchList.Clear();
                await LoadMovieSearch(searchbox.Text);
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        if (SearchList[i] != null)
                        {
                            moviesList[randomList[i]] = SearchList[i];
                            Text.Content = $"Resualt For : {searchbox.Text}";
                        }
                    } catch (Exception ex) { }
                    

                }

            }
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
    }

  
