using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using PRojet_NPF.Models;

namespace PRojet_NPF.ViewModels
{
    public class Hero : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand OpenSettingsCommand { get; } =
        new RelayCommand(_ => new PRojet_NPF.Views.SettingsWindow().ShowDialog());
        public Combat CombatContext { get; } = new Combat();

        public ObservableCollection<Models.Hero> Heroes { get; set; } = new();

        private Models.Hero? _selectedHero;
        public Models.Hero? SelectedHero
        {
            get => _selectedHero;
            set
            {
                _selectedHero = value;
                OnPropertyChanged(nameof(SelectedHero));
                UpdateFilteredSpells();
            }
        }

        private Models.Hero? _filterHero;
        public Models.Hero? FilterHero
        {
            get => _filterHero;
            set
            {
                _filterHero = value;
                OnPropertyChanged(nameof(FilterHero));
                UpdateFilteredSpells();
            }
        }

        private Spell? _selectedSpell;
        public Spell? SelectedSpell
        {
            get => _selectedSpell;
            set { _selectedSpell = value; OnPropertyChanged(nameof(SelectedSpell)); }
        }

        public ObservableCollection<Spell> FilteredSpells { get; set; } = new();

        public ICommand SelectHeroCommand { get; }

        public Hero()
        {
            SelectHeroCommand = new RelayCommand(_ =>
            {
                if (SelectedHero != null)
                {
                    CombatContext.SetPlayerHero(SelectedHero);
                    MessageBox.Show($"{SelectedHero.Name} sélectionné pour le combat !", "Héros choisi");
                }
            });

            LoadData();
        }

        private void LoadData()
        {
            using var db = new AppDbContext();
            var heroes = db.Heroes
                .Include(h => h.Spells)
                .ToList();

            Heroes.Clear();
            foreach (var h in heroes)
                Heroes.Add(h);
        }

        private void UpdateFilteredSpells()
        {
            FilteredSpells.Clear();
            var source = FilterHero ?? SelectedHero;
            if (source != null)
                foreach (var s in source.Spells)
                    FilteredSpells.Add(s);
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}