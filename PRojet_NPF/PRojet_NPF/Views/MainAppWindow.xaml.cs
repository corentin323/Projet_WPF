using PRojet_NPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PRojet_NPF.Views
{
    public partial class MainAppWindow : Window
    {
        public MainAppWindow()
        {
            InitializeComponent();
            DataContext = new Hero();
        }
    }
}