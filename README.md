# Plot Those Lines! – Crypto Edition

## 🎯 Objectif
Développer une application Windows Forms permettant d’afficher des graphiques 
de séries temporelles sur des données de cryptomonnaies 
(en utilisant l’API gratuite de [CoinGecko](https://www.coingecko.com/en/api)).

## ⚙️ Fonctionnalités
- Import des données JSON via API CoinGecko.
- Affichage des prix sur un graphique interactif (ScottPlot).
- Choix de la cryptomonnaie (Bitcoin, Ethereum, etc.).
- Choix de la période (7 jours, 30 jours, 90 jours).
- Comparaison de plusieurs séries sur un même graphique.
- Gestion des erreurs réseau / API.

## 🛠️ Technologies
- C# (.NET, Windows Forms)
- [ScottPlot](https://scottplot.net/) pour les graphiques
- `System.Text.Json` pour le parsing JSON
- LINQ pour les manipulations de données
- GitHub pour le versionning et la gestion du projet

## 📂 Structure du repo
- `doc/Rapport` – Rapport PDF final
- `doc/JDT` – Journal de travail
- `doc/CDC` – Cahier des charges
- `/src` – Code source du projet
- `.gitignore` – Fichiers ignorés
- GitHub Project – Suivi des User Stories

## 📑 User Stories
Voir le tableau de projet GitHub: [Project Board](./https://github.com/users/Josefnademo/projects/5)

## 🧪 Tests
- Tests unitaires pour le parsing JSON.
- Tests unitaires pour la manipulation des données avec LINQ.

## 📝 Documentation
Le rapport final contient :
- Objectifs et description du domaine (cryptomonnaies).
- Planification (User Stories).
- Rapport de tests.
- Journal de travail.
- Usage de l’IA.
- Conclusion.

## 🚀 Exécution
1. Cloner le repo
2. Ouvrir le projet sous Visual Studio
3. Lancer l’exécutable
