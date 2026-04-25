# Hero Arena - WPF

## Description
Application de combat tour par tour entre héros, développée en C#.

## Prérequis
- Visual Studio 2022
- .NET 8
- SQL Server Express
- SQL Server Management Studio (SSMS)

## Installation

### 1. Base de données
Ouvrir SSMS et exécuter le script SQL suivant :

```sql
CREATE DATABASE ExerciceHero;
GO
USE ExerciceHero;
GO

CREATE TABLE Login (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL
);

CREATE TABLE Player (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL,
    LoginID INT,
    FOREIGN KEY (LoginID) REFERENCES Login(ID)
);

CREATE TABLE Hero (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL,
    Health INT NOT NULL,
    ImageURL NVARCHAR(255) NULL
);

CREATE TABLE Spell (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL,
    Damage INT NOT NULL,
    Description NVARCHAR(MAX)
);

CREATE TABLE PlayerHero (
    PlayerID INT NOT NULL,
    HeroID INT NOT NULL,
    PRIMARY KEY (PlayerID, HeroID),
    FOREIGN KEY (PlayerID) REFERENCES Player(ID),
    FOREIGN KEY (HeroID) REFERENCES Hero(ID)
);

CREATE TABLE HeroSpell (
    HeroID INT NOT NULL,
    SpellID INT NOT NULL,
    PRIMARY KEY (HeroID, SpellID),
    FOREIGN KEY (HeroID) REFERENCES Hero(ID),
    FOREIGN KEY (SpellID) REFERENCES Spell(ID)
);
```

### 2. Initialiser les données
- Lancer l'application
- Cliquer sur **⚙**
- Vérifier la chaîne de connexion
- Cliquer sur **Initialiser les données**

### 3. Connexion par défaut
- **Username :** admin
- **Password :** admin

## Packages NuGet
- Microsoft.EntityFrameworkCore 8.0.x
- Microsoft.EntityFrameworkCore.SqlServer 8.0.x
- Microsoft.EntityFrameworkCore.Tools 8.0.x

## Structure du projet
/Models        → Modèles de données + DbContext
/ViewModels    → ViewModels
/Views         → Fenêtres XAML
/Services      → Services
/Resources     → Styles et ressources XAML

## Fonctionnalités
- Login avec hash SHA256
- Gestion des héros et spells
- Combat tour par tour
- Barres de vie dynamiques
- Score cumulatif
- Héros tombes a 0 HP fin du combat perte du score
- Paramètres de connexion BDD configurables

## Notes techniques
- Chaque héros possède 4 spells
- L'ennemi généré a +10% HP et +5% dégâts