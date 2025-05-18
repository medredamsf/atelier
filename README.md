# AtelierCleanApp API

## Introduction

AtelierCleanApp est une application API RESTful développée avec .NET 8, conçue pour gérer des données de joueurs de tennis. Le projet sert d'exemple pratique pour l'implémentation des principes de la **Clean Architecture**, en mettant l'accent sur une structure de code découplée, testable, maintenable et évolutive.

L'objectif principal est de démontrer comment organiser une application en couches distinctes (Domaine, Application, Infrastructure, Présentation) avec des dépendances claires allant vers le centre (Domaine). Cela permet une meilleure séparation des préoccupations et facilite l'évolution et le test des différentes parties de l'application de manière isolée. L'API expose des endpoints pour des opérations CRUD de base sur les joueurs, comme la récupération d'un joueur par son ID et l'obtention d'une liste de tous les joueurs triés par leur rang.

## Getting Started

Cette section vous guidera pour obtenir une copie du projet et la faire fonctionner sur votre machine locale à des fins de développement et de test.

### Installation process

1.  **Cloner le dépôt** :
    ```bash
    git clone <url_du_depot>
    cd AtelierCleanApp
    ```
    Si vous avez les fichiers localement, assurez-vous d'être dans le répertoire racine de la solution (`AtelierCleanApp/`).

2.  **Restaurer les dépendances NuGet**:
    Ouvrez un terminal à la racine de la solution et exécutez :
    ```bash
    dotnet restore
    ```

### Software dependencies

* **[.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)** : Assurez-vous d'avoir la version 8 du SDK .NET installée.
* **SQL Server**: Une instance de SQL Server est nécessaire pour la persistance des données. La chaîne de connexion devra être configurée.
* **Outils Entity Framework Core**: Pour gérer les migrations de base de données. Si vous ne les avez pas, installez-les globalement :
    ```bash
    dotnet tool install --global dotnet-ef
    ```
    (N'oubliez pas de redémarrer votre terminal après l'installation globale.)


### API references

Une fois l'application démarrée en mode Développement, la documentation de l'API et l'interface de test Swagger (OpenAPI) sont généralement disponibles à l'adresse suivante :
* `/swagger` (par exemple, `https://localhost:PORT/swagger`)

Les endpoints principaux incluent :
* `GET /api/players`: Récupère tous les joueurs, triés par rang.
* `GET /api/players/{id}`: Récupère un joueur spécifique par son ID.

### Configuration de la Base de Données

1.  **Configurer la Chaîne de Connexion**:
    * Ouvrez le fichier `src/AtelierCleanApp.Api/appsettings.json`.
    * Modifiez la section `ConnectionStrings` pour pointer vers votre instance SQL Server :
        ```json
        {
          "ConnectionStrings": {
            "DefaultConnection": "Server=your_server_name;Database=AtelierCleanAppDb;User ID=your_user;Password=your_password;TrustServerCertificate=True;"
          },
          // ...
        }
        ```

## Build and Test

### Build

Pour compiler la solution entière :
1.  Ouvrez un terminal à la racine de la solution (`AtelierCleanApp/`).
2.  Exécutez la commande :
    ```bash
    dotnet build
    ```
    Cela compilera tous les projets de la solution.

### Test

Pour exécuter tous les tests unitaires de la solution :
1.  Ouvrez un terminal à la racine de la solution (`AtelierCleanApp/`).
2.  Exécutez la commande :
    ```bash
    dotnet test
    ```
    Cela découvrira et exécutera les tests dans tous les projets de test (`*.Tests.csproj`). Les résultats des tests seront affichés dans le terminal.

### Exécuter l'API

Pour démarrer l'application API :
1.  Naviguez vers le répertoire du projet API :
    ```bash
    cd src/AtelierCleanApp.Api
    ```
2.  Exécutez la commande :
    ```bash
    dotnet run
    ```
    L'API sera accessible via les URLs affichées dans la console (généralement `https://localhost:PORT` et `http://localhost:PORT`).

