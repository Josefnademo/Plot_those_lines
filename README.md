# Plot Those Lines! – Crypto Edition

## 🎯 Objectif
Développer une application Windows Forms permettant d’afficher des graphiques
de séries temporelles sur des données de cryptomonnaies
(en utilisant l’API gratuite de [CoinGecko](https://www.coingecko.com/en/api)).  
Le projet intègre une planification sur 8 semaines, la rédaction d’un rapport détaillé
et le suivi de l’avancement via un journal de travail (JDT).

## ⚙️ Fonctionnalités
- Import des données JSON via API CoinGecko.
- Affichage interactif des prix sur un graphique (ScottPlot).
- Choix de la cryptomonnaie (Bitcoin, Ethereum, etc.).
- Sélection de la période d’analyse (1, 7, 30 ou 365 jours).
- Comparaison de plusieurs séries sur un même graphique.
- Gestion des erreurs réseau ou identifiant invalide.
- Méthodes d’extension et utilisation de LINQ pour le traitement des données.

## 🛠️ Technologies
- C# (.NET, Windows Forms)
- [ScottPlot](https://scottplot.net/) pour les graphiques
- `System.Text.Json` pour le parsing JSON
- LINQ pour les manipulations de données
- GitHub pour le versionning et la gestion du projet

## 📂 Structure du repo
- `doc/schema de fonctionnement d'application` – Schema de fonctionnement d'application
- `doc/Rapport` – Rapport PDF final
- `doc/JDT` – Journal de travail
- `doc/P_FUN-Specifications` – Cahier des charges
- `images/` –  des icons et images pour l'application
- `maquette/` –  Maquette `pdf` du projet
- `/src` – Code source du projet
- `.gitignore` – Fichiers ignorés
- [GitHub Project](https://github.com/users/Josefnademo/projects/5/views/1) – Suivi des User Stories

## 📑 User Stories
Toutes les User Stories sont documentées et suivies sur le [GitHub Project Board](https://github.com/users/Josefnademo/projects/5)

## 🧪 Tests

### [Tests unitaires](https://github.com/Josefnademo/Plot_those_lines/tree/main/src/PTL_Crypto/UnitTest_CryptoPrice)
- Vérification du parsing correct du JSON.
- Vérification de la conversion timestamp → DateTime.
- Vérification de la sélection de période (7/30/90 jours).
- Vérification de la manipulation des données avec LINQ (filtrage et transformation).
- Vérification de validité et d’ordre temporel des prix.

### Tests d’acceptation
- Lancer l’application, saisir "bitcoin" et afficher le graphique sur 7 jours.
- Comparer Bitcoin + Ethereum → vérifier que 2 courbes distinctes s’affichent correctement.
- Saisir un mauvais identifiant crypto → vérifier la gestion de l’erreur (MessageBox ou Label).

## 🚀 Exécution

1. Cloner le repo `git clone https://github.com/Josefnademo/Plot_those_lines.git`
2. Ouvrir le projet sous Visual Studio `src\PTL_Crypto\PTL_Crypto.sln`
3. Lancer l’exécutable

## 🔧 Développement & Tests
- Lancer les tests unitaires : `UnitTest_CryptoPrice (UnitTest1.cs)` (depuis le dossier de la solution)

## 📝 Documentation
- [Rapport PDF](https://github.com/Josefnademo/Plot_those_lines/blob/main/doc/Rapport.pdf) : Contenu détaillé du projet final(analyse, conception, mise en œuvre, tests et conclusion).
- [Rapport Markdown](https://github.com/Josefnademo/Plot_those_lines/blob/main/doc/Rapport.md) : version en ligne du rapport. 
- [Planification](https://github.com/Josefnademo/Plot_those_lines/blob/main/doc/Planification-initiale.md) : tâches sur 8 semaines.
- [Journal de travail (JDT)](https://github.com/Josefnademo/Plot_those_lines/blob/main/doc/Journal-de-Travail_NademoYosef.xlsx) : suivi de l’avancement, difficultés et solutions.
