using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using PRojet_NPF.Models;
using Microsoft.EntityFrameworkCore;

namespace PRojet_NPF.ViewModels
{
    public class Combat : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // Héros joueur
        private Models.Hero? _playerHero;
        public Models.Hero? PlayerHero
        {
            get => _playerHero;
            set { _playerHero = value; OnPropertyChanged(nameof(PlayerHero)); }
        }

        private int _playerHP;
        public int PlayerHP
        {
            get => _playerHP;
            set { _playerHP = value; OnPropertyChanged(nameof(PlayerHP)); OnPropertyChanged(nameof(PlayerHPPercent)); }
        }

        private int _playerMaxHP;
        public int PlayerMaxHP
        {
            get => _playerMaxHP;
            set { _playerMaxHP = value; OnPropertyChanged(nameof(PlayerMaxHP)); }
        }

        public double PlayerHPPercent => PlayerMaxHP > 0 ? (double)PlayerHP / PlayerMaxHP * 400 : 0;
        // Héros ennemi
        private Models.Hero? _enemyHero;
        public Models.Hero? EnemyHero
        {
            get => _enemyHero;
            set { _enemyHero = value; OnPropertyChanged(nameof(EnemyHero)); }
        }

        private int _enemyHP;
        public int EnemyHP
        {
            get => _enemyHP;
            set { _enemyHP = value; OnPropertyChanged(nameof(EnemyHP)); OnPropertyChanged(nameof(EnemyHPPercent)); }
        }

        private int _enemyMaxHP;
        public int EnemyMaxHP
        {
            get => _enemyMaxHP;
            set { _enemyMaxHP = value; OnPropertyChanged(nameof(EnemyMaxHP)); }
        }
        public double EnemyHPPercent => EnemyMaxHP > 0 ? (double)EnemyHP / EnemyMaxHP * 400 : 0;

        // Score et log
        private int _score;
        public int Score
        {
            get => _score;
            set { _score = value; OnPropertyChanged(nameof(Score)); }
        }

        private string _combatLog = "Choisissez un héros et lancez le combat !";
        public string CombatLog
        {
            get => _combatLog;
            set { _combatLog = value; OnPropertyChanged(nameof(CombatLog)); }
        }

        private bool _combatActive = false;
        public bool CombatActive
        {
            get => _combatActive;
            set { _combatActive = value; OnPropertyChanged(nameof(CombatActive)); }
        }

        public ObservableCollection<Models.Hero> Heroes { get; set; } = new();
        public ObservableCollection<Spell> PlayerSpells { get; set; } = new();

        public ICommand UseSpellCommand { get; }
        public ICommand NewCombatCommand { get; }

        public Combat()
        {
            UseSpellCommand = new RelayCommand(ExecuteSpell);
            NewCombatCommand = new RelayCommand(_ => StartNewCombat());
            LoadHeroes();
        }
        public void SetPlayerHero(Models.Hero hero)
        {
            SelectedPlayerHero = hero;
        }
        private void LoadHeroes()
        {
            using var db = new AppDbContext();
            var heroes = db.Heroes.Include(h => h.Spells).ToList();
            Heroes.Clear();
            foreach (var h in heroes)
                Heroes.Add(h);
        }

        private Models.Hero? _selectedPlayerHero;
        public Models.Hero? SelectedPlayerHero
        {
            get => _selectedPlayerHero;
            set
            {
                _selectedPlayerHero = value;
                OnPropertyChanged(nameof(SelectedPlayerHero));
                if (value != null) StartCombat(value);
            }
        }

        private void StartCombat(Models.Hero playerHero)
        {
            // Setup joueur
            PlayerHero = playerHero;
            PlayerMaxHP = playerHero.Health;
            PlayerHP = playerHero.Health;

            PlayerSpells.Clear();
            foreach (var s in playerHero.Spells)
                PlayerSpells.Add(s);

            // Setup ennemi (+10% HP, +5% dégâts)
            var random = new Random();
            var enemies = Heroes.Where(h => h.ID != playerHero.ID).ToList();
            var baseEnemy = enemies[random.Next(enemies.Count)];

            EnemyHero = new Models.Hero
            {
                ID = baseEnemy.ID,
                Name = $"{baseEnemy.Name} (Élite)",
                Health = (int)(baseEnemy.Health * 1.1),
                Spells = baseEnemy.Spells.Select(s => new Spell
                {
                    ID = s.ID,
                    Name = s.Name,
                    Damage = (int)(s.Damage * 1.05),
                    Description = s.Description
                }).ToList()
            };

            EnemyMaxHP = EnemyHero.Health;
            EnemyHP = EnemyHero.Health;

            CombatActive = true;
            CombatLog = $"Combat commencé ! {PlayerHero.Name} VS {EnemyHero.Name}";
        }

        private void ExecuteSpell(object? parameter)
        {
            if (!CombatActive || parameter is not Spell spell) return;

            // Joueur attaque
            EnemyHP = Math.Max(0, EnemyHP - spell.Damage);
            CombatLog = $"Vous utilisez {spell.Name} → {spell.Damage} dégâts sur {EnemyHero!.Name} !";

            if (EnemyHP <= 0)
            {
                Score += 100;
                CombatActive = false;
                string msg = "Bien jouer !";
                MessageBox.Show($"{msg}\n\n+100 points ! Score : {Score}", "Victoire !");
                return;
            }

            // Ennemi contre-attaque
            var random = new Random();
            var enemySpells = EnemyHero.Spells.ToList();
            if (enemySpells.Count > 0)
            {
                var enemySpell = enemySpells[random.Next(enemySpells.Count)];
                PlayerHP = Math.Max(0, PlayerHP - enemySpell.Damage);
                CombatLog += $"\n{EnemyHero.Name} utilise {enemySpell.Name} → {enemySpell.Damage} dégâts !";
            }

            if (PlayerHP <= 0)
            {
                
                CombatActive = false;
                int scoreFinal = Score;
                Score = 0;
                string msg = "GAME OVER ";
                MessageBox.Show($"{msg}\n\nScore final : {scoreFinal}", "Défaite...");
                
            }
        }

        private void StartNewCombat()
        {
            if (SelectedPlayerHero != null)
                StartCombat(SelectedPlayerHero);
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}