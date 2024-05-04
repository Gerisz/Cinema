using Cinema.Admin.Model;
using Cinema.Admin.View;
using Cinema.Admin.ViewModel;
using System.Configuration;
using System.Data;
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

        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _service = new CinemaAPIService(new Uri(ConfigurationManager.AppSettings["baseAddress"]));

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
            //_mainViewModel.StartingItemEdit += ViewModel_StartingItemEdit;
            //_mainViewModel.FinishingItemEdit += ViewModel_FinishingItemEdit;
            //_mainViewModel.StartingImageChange += ViewModel_StartingImageChange;

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

        /*private void ViewModel_StartingItemEdit(object sender, EventArgs e)
        {
            _editorView = new ItemEditorWindow
            {
                DataContext = _mainViewModel
            };
            _editorView.ShowDialog();
        }

        private void ViewModel_FinishingItemEdit(object sender, EventArgs e)
        {
            if (_editorView.IsActive)
            {
                _editorView.Close();
            }
        }

        private async void ViewModel_StartingImageChange(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "Images|*.jpg;*.jpeg;*.bmp;*.tif;*.gif;*.png;",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (dialog.ShowDialog(_editorView).GetValueOrDefault(false))
            {
                _mainViewModel.SelectedItem.Image = await File.ReadAllBytesAsync(dialog.FileName);
            }
        }*/
    }

}
