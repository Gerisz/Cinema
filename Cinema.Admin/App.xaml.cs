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
    public partial class App : Application
    {
        private CinemaAPIService _service;
        private MainViewModel _mainViewModel;
        private LoginViewModel _loginViewModel;
        private MainWindow _mainView;
        private LoginWindow _loginView;
        private MovieEditorWindow _movieEditorWindow;

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

            _mainView = new MainWindow
            {
                DataContext = _mainViewModel
            };

            // Alkalmazás leállítása a főablak bezárásakor
            // (alapértelmezetten az összes ablak bezárásakor történik, de a login ablakot csak elrejteni fogjuk)
            MainWindow = _mainView;
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            _loginView.Closed += LoginView_Closed; // bejelentkezési ablak bezárásakor is leállítás

            _loginView.Show();
        }

        private void LoginView_Closed(object sender, EventArgs e)
        {
            Shutdown();
        }

        private void ViewModel_LoginSucceeded(object sender, EventArgs e)
        {
            _loginView.Hide();
            _mainView.Show();
        }

        private void ViewModel_LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("Login unsuccessful!", "TodoList", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ViewModel_LogoutSucceeded(object sender, EventArgs e)
        {
            _mainView.Hide();
            _loginView.Show();
        }

        private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "TodoList", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ViewModel_StartingMovieEdit(object sender, EventArgs e)
        {
            _movieEditorWindow = new MovieEditorWindow
            {
                DataContext = _mainViewModel
            };
            _movieEditorWindow.ShowDialog();
        }

        private void ViewModel_FinishingMovieEdit(object sender, EventArgs e)
        {
            if (_movieEditorWindow.IsActive)
            {
                _movieEditorWindow.Close();
            }
        }

        private async void ViewModel_StartingMovieImageChange(object sender, EventArgs e)
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
    }

}
