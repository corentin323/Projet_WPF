using PRojet_NPF.ViewModels;
using System.Windows;

namespace PRojet_NPF.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = new Settings();
        }
    }
}