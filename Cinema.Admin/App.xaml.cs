using Cinema.Admin.Model;
using Cinema.Admin.View;
using Cinema.Admin.ViewModel;
using Microsoft.Win32;
using System.Configuration;
using System.IO;
using System.Windows;

namespace Cinema.Admin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IDisposable
    {
        private CinemaAPIService _service = null!;
        private MainViewModel _mainViewModel = null!;
        private LoginViewModel _loginViewModel = null!;
        private MainWindow _mainView = null!;
        private LoginWindow _loginView = null!;
        private MovieEditorWindow _movieEditorWindow = null!;
        private ShowEditorWindow _showEditorWindow = null!;

        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(Object sender, StartupEventArgs e)
        {
            _service = new CinemaAPIService(
                new Uri(ConfigurationManager.AppSettings["baseAddress"]!));

            _loginViewModel = new LoginViewModel(_service);

            _loginViewModel.LoginSucceeded += ViewModel_LoginSucceeded;
            _loginViewModel.LoginFailed += ViewModel_LoginFailed;
            _loginViewModel.MessageApplication += ViewModel_MessageApplication;

            _loginView = new LoginWindow
            {
                DataContext = _loginViewModel
            };

            _mainViewModel = new MainViewModel(_service);
            _mainViewModel.LogoutSucceeded += ViewModel_LogoutSucceeded;
            _mainViewModel.MessageApplication += ViewModel_MessageApplication;
            _mainViewModel.StartingMovieEdit += ViewModel_StartingMovieEdit;
            _mainViewModel.FinishingMovieEdit += ViewModel_FinishingMovieEdit;
            _mainViewModel.StartingMovieImageChange += ViewModel_StartingMovieImageChange;
            _mainViewModel.StartingShowEdit += ViewModel_StartingShowEdit;
            _mainViewModel.FinishingShowEdit += ViewModel_FinishingShowEdit;

            _mainView = new MainWindow
            {
                DataContext = _mainViewModel
            };

            MainWindow = _mainView;
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            _loginView.Closed += LoginView_Closed;

            _loginView.Show();
        }

        private void LoginView_Closed(Object? sender, EventArgs e)
        {
            Shutdown();
        }

        private void ViewModel_LoginSucceeded(Object? sender, EventArgs e)
        {
            _loginView.Hide();
            _mainView.Show();
        }

        private void ViewModel_LoginFailed(Object? sender, EventArgs e)
        {
            MessageBox.Show("Bejelenkezés sikertelen!", "Cinema", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ViewModel_LogoutSucceeded(Object? sender, EventArgs e)
        {
            _mainView.Hide();
            _loginView.Show();
        }

        private void ViewModel_MessageApplication(Object? sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Cinema", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ViewModel_StartingMovieEdit(Object? sender, EventArgs e)
        {
            _movieEditorWindow = new MovieEditorWindow
            {
                DataContext = _mainViewModel
            };
            _movieEditorWindow.ShowDialog();
        }

        private void ViewModel_FinishingMovieEdit(Object? sender, EventArgs e)
        {
            if (_movieEditorWindow.IsActive)
            {
                _movieEditorWindow.Close();
            }
        }

        private async void ViewModel_StartingMovieImageChange(Object? sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "Images|*.jpg;*.jpeg;*.bmp;*.tif;*.gif;*.png;",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (dialog.ShowDialog(_movieEditorWindow).GetValueOrDefault(false))
            {
                _mainViewModel.SelectedMovie.Image = await File.ReadAllBytesAsync(dialog.FileName);
            }
        }

        private void ViewModel_StartingShowEdit(Object? sender, EventArgs e)
        {
            _showEditorWindow = new ShowEditorWindow
            {
                DataContext = _mainViewModel
            };
            _showEditorWindow.ShowDialog();
        }

        private void ViewModel_FinishingShowEdit(Object? sender, EventArgs e)
        {
            if (_showEditorWindow.IsActive)
            {
                _showEditorWindow.Close();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}
