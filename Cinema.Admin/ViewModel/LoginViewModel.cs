using Cinema.Admin.Model;
using System.Net.Http;
using System.Windows.Controls;

namespace Cinema.Admin.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly CinemaAPIService _model;
        private Boolean _isLoading;

        public DelegateCommand LoginCommand { get; private set; }

        public String UserName { get; set; }

        public Boolean IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler LoginSucceeded = null!;
        public event EventHandler LoginFailed = null!;

        public LoginViewModel(CinemaAPIService model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _model = model;
            UserName = String.Empty;
            IsLoading = false;

            LoginCommand = new DelegateCommand(_ => !IsLoading,
                    param => Login(param as PasswordBox ?? new PasswordBox()));
        }

        private async void Login(PasswordBox passwordBox)
        {
            if (passwordBox == null)
                return;

            try
            {
                IsLoading = true;
                bool result = await _model.LoginAsync(UserName, passwordBox.Password);
                IsLoading = false;

                if (result)
                    OnLoginSuccess();
                else
                    OnLoginFailed();
            }
            catch (HttpRequestException ex)
            {
                OnMessageApplication($"Server error occurred: ({ex.Message})");
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Unexpected error occurred: ({ex.Message})");
            }
        }

        private void OnLoginSuccess()
        {
            LoginSucceeded?.Invoke(this, EventArgs.Empty);
        }

        private void OnLoginFailed()
        {
            LoginFailed?.Invoke(this, EventArgs.Empty);
        }
    }
}
