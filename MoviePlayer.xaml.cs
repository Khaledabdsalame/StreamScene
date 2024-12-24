
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using LibVLCSharp.Shared;
using Microsoft.Win32;





namespace Cinema_Platform_Application
{

    public partial class MoviePlayer : Page
    {

        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        private Process process = new Process();
        private DispatcherTimer _timer;
        private string imdbId;

        // Start Peerflix Process
       
       
        public MoviePlayer(string magneturl,string imdbcode)
        {
            InitializeComponent();
            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);
            videoView.MediaPlayer = _mediaPlayer;
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            _timer.Tick += Timer_Tick;

            _mediaPlayer.Volume = (int)VolumeSlider.Value;
            PlayMagnet(magneturl);
            this.imdbId = imdbcode;
            _timer.Start();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            process.Kill();
            process.Dispose();
            process = null;
            var window = (MainWindow)Application.Current.MainWindow;
            window.videoplayer.Visibility = Visibility.Hidden;
            window.Space.IsEnabled = true;
            window.Space.Effect = null;
            _mediaPlayer.Stop();
            _timer.Stop();
            _mediaPlayer.Dispose();
            _libVLC.Dispose();



        }

        private void PlayMagnet(string Magnet)
        {
            var options = new string[]
                    {
                    ":network-caching=1500",
                    ":clock-jitter=0",
                    ":clock-synchro=0",
                   
                    };
            try
            {
                process.StartInfo.FileName = "cmd.exe";  // Run in cmd.exe to execute WebTorrent
                process.StartInfo.Arguments = $"/k webtorrent {Magnet} --keep-seeding --out ./Downloads/ ";  // Command to run WebTorrent CLI
                process.StartInfo.RedirectStandardOutput = true;  // Capture standard output to get stream URL
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                // Capture output data (WebTorrent CLI stream URL)
                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data != null && (e.Data.Contains("fetching torrent metadata from") || e.Data.Contains("verifying existing torrent data...")))
                    {
                        Dispatcher.Invoke(() =>
                        {

                            Message.Content = e.Data;

                        });
                    }
                   else if (e.Data != null && e.Data.Contains("http://localhost:8000"))
                    {
                        Dispatcher.Invoke(() =>
                        {
                           
                            var media = new Media(_libVLC, new Uri(e.Data.Replace("Server running at: ", "", StringComparison.OrdinalIgnoreCase)));
                           
                            Play.Content = FindResource("Stop");
                            
                            _mediaPlayer.Play(media);

                        });
                        process.CancelOutputRead();
                    }

                };

                process.Start();
                process.BeginOutputReadLine();

            }
            catch (Exception ex)
            {
                Message.Content = ex.Message;
            }

        }


        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Volume = (int)e.NewValue;
            }

        }

        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_mediaPlayer != null && _mediaPlayer.IsSeekable && ProgressSlider.IsFocused)
            {
                double currentPosition = _mediaPlayer.Time / 1000.0;
                double newPosition = e.NewValue;

                if (Math.Abs(newPosition - currentPosition) > 1)
                {
                    _mediaPlayer.Time = (long)(newPosition * 1000);
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Play.Content == FindResource("Play") && !_mediaPlayer.IsPlaying)
            {
                Play.Content = FindResource("Stop");
                _mediaPlayer.Play();

            }
            else if (Play.Content == FindResource("Stop") && _mediaPlayer.IsPlaying)
            {
                Play.Content = FindResource("Play");
                _mediaPlayer.Pause();
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            var currentTime = TimeSpan.FromMilliseconds(_mediaPlayer.Time);
            var totalTime = TimeSpan.FromMilliseconds(_mediaPlayer.Length);

            if (_mediaPlayer != null && _mediaPlayer.Media != null)
            {
                if (_mediaPlayer.IsSeekable) // Check if the stream is seekable
                {
                    ProgressSlider.IsEnabled = true;
                    ProgressSlider.Maximum = _mediaPlayer.Length / 1000; // Convert from ms to seconds 
                    ProgressSlider.Value = _mediaPlayer.Time / 1000;
                    timerDisplay.Text = $"{FormatTime(currentTime)} / {FormatTime(totalTime)}";

                }
                else
                {
                    ProgressSlider.IsEnabled = false;
                }
            }
           
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (Play.Content == FindResource("Play") && !_mediaPlayer.IsPlaying)
                {
                    Play.Content = FindResource("Stop");
                    _mediaPlayer.Play();

                }
                else if (Play.Content == FindResource("Stop") && _mediaPlayer.IsPlaying)
                {
                    Play.Content = FindResource("Play");
                    _mediaPlayer.Pause();
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Btn1.Visibility == Visibility.Visible&& Btn2.Visibility == Visibility.Visible && text.Visibility == Visibility.Visible && text2.Visibility == Visibility.Visible)
            {
                Btn1.Visibility = Visibility.Hidden;
                Btn2.Visibility = Visibility.Hidden;
                text.Visibility = Visibility.Hidden;
                text2.Visibility = Visibility.Hidden;
            }
            else
            {
                Btn1.Visibility = Visibility.Visible;
                Btn2.Visibility = Visibility.Visible;
                text.Visibility = Visibility.Visible;
                text2.Visibility = Visibility.Visible;
            }

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string sub;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Subtitle Files (*.srt)|*.srt|All Files (*.*)|*.*";
            openFileDialog.Title = "Select a Subtitle File";
            if (openFileDialog.ShowDialog() == true)
            {
                sub = openFileDialog.FileName;
                _mediaPlayer.AddSlave(MediaSlaveType.Subtitle, new Uri(sub).ToString(), true);
            }
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = $"https://yifysubtitles.ch//movie-imdb/{imdbId}",
                    UseShellExecute = true 
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                
            }
        }

        private void FullScreen(object sender, RoutedEventArgs e)
        {
            var window=(MainWindow)Application.Current.MainWindow;
            if ((window.WindowState == WindowState.Normal || window.WindowState == WindowState.Maximized) && window.WindowStyle == WindowStyle.SingleBorderWindow)
            {
                window.WindowState = WindowState.Maximized;
                window.WindowStyle = WindowStyle.None;
                window.Visibility = Visibility.Collapsed;
                window.ResizeMode = ResizeMode.NoResize;
                window.Visibility = Visibility.Visible;

            }
            else
            {
                window.WindowState = WindowState.Normal;
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.ResizeMode = ResizeMode.CanResize;
            }

        }

        private string FormatTime(TimeSpan time)
        {
            return time.ToString(@"hh\:mm\:ss");
        }
    }
}





