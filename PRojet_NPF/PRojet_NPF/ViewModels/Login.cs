using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using PRojet_NPF.Models;

namespace PRojet_NPF.ViewModels
{
    public class Login : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
   
        private string _username = "";
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        private string _errorMessage = "";
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
        }

        public ICommand LoginCommand { get; }
        public ICommand OpenSettingsCommand { get; }

        public Login()
        {
            LoginCommand = new RelayCommand(ExecuteLogin);
            OpenSettingsCommand = new RelayCommand(_ => new PRojet_NPF.Views.SettingsWindow().ShowDialog());
        }

        private void ExecuteLogin(object? parameter)
        {
            string password = parameter as string ?? "";
            string hash = HashPassword(password);

            using var db = new AppDbContext();
            var user = db.Logins.FirstOrDefault(l =>
                l.Username == Username && l.PasswordHash == hash);

            if (user != null)
            {
                var mainApp = new PRojet_NPF.Views.MainAppWindow();
                mainApp.Show();
                System.Windows.Application.Current.MainWindow.Close();
            }
            else
            {
                ErrorMessage = "Identifiants incorrects.";
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}