using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using PRojet_NPF.Models;

namespace PRojet_NPF.ViewModels
{
    public class Settings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _connectionString =
            "Server=localhost\\SQLEXPRESS;Database=ExerciceHero;Trusted_Connection=True;TrustServerCertificate=True;";
        public string ConnectionString
        {
            get => _connectionString;
            set { _connectionString = value; OnPropertyChanged(nameof(ConnectionString)); }
        }

        private string _statusMessage = "";
        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); }
        }

        public ICommand SaveCommand { get; }
        public ICommand InitDataCommand { get; }

        public Settings()
        {
            SaveCommand = new RelayCommand(_ => SaveConnection());
            InitDataCommand = new RelayCommand(_ => InitData());
        }

        private void SaveConnection()
        {
            AppDbContext.SetConnectionString(ConnectionString);
            StatusMessage = "✔ Connexion sauvegardée !";
        }

        private void InitData()
        {
            try
            {
                using var db = new AppDbContext();

                if (!db.Logins.Any())
                {
                    db.Logins.Add(new Models.Login
                    {
                        Username = "admin",
                        PasswordHash = "47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU="
                    });
                    db.SaveChanges();
                }

                StatusMessage = "✔ Données initialisées avec succès !";
            }
            catch (Exception ex)
            {
                StatusMessage = $"❌ Erreur : {ex.Message}";
            }
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}