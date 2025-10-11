# Rapport du projet – Plot Those Lines! (Crypto Edition)

## 1. Introduction

### 1.1 Objectifs pédagogiques

Ce projet est conçu pour me permettre de mettre en pratique les compétences acquises dans les modules de programmation.

J’ai appliqué plusieurs notions clés du cours :

- la programmation orientée objet avec C# ;
- l’utilisation de LINQ comme approche moderne de traitement des données ;
- la gestion d’une API externe et la désérialisation JSON ;
- la mise en œuvre de tests unitaires afin d’assurer la qualité du code ;
- la création d’une interface utilisateur graphique (WinForms) ;
- la gestion de projet avec GitHub (projets, commits, versioning) ;
- la production d’une documentation professionnelle (Rapport, JDT, README).

L’objectif pédagogique est donc double : d’un côté apprendre à développer une application concrète avec un client, et de l’autre acquérir des bonnes pratiques professionnelles en termes de planification, organisation et communication technique.

### 1.2 Objectifs produit

Le produit attendu est une application Windows Forms qui permet de visualiser des séries temporelles sous forme de graphiques.

- L’utilisateur pourra importer et afficher les données issues d’une API externe (CoinGecko).
- L’application doit être capable d’afficher plusieurs séries sur un même graphique (par ex. Bitcoin + Ethereum).
- L’utilisateur doit pouvoir choisir la période d’analyse (7, 30, 90 jours).
- Des fonctions supplémentaires permettront d’analyser les données de manière flexible (zoom, légendes, choix de couleurs).

Le produit n’est pas seulement une démonstration technique : il doit être utilisable et compréhensible par un utilisateur non technique qui souhaite visualiser facilement l’évolution des cryptomonnaies.

## 2. Domaine et sources de données

Le projet s’inscrit dans le secteur des cryptomonnaies, un domaine en évolution rapide et riche en données quantitatives.

J’ai choisi le domaine des cryptomonnaies pour plusieurs raisons :

- C’est un secteur en forte croissance et très médiatisé.
- Les données financières et temporelles y sont particulièrement adaptées à la représentation graphique.
- Il existe plusieurs sources de données fiables et gratuites.

### 2.1 Sources de données

- CoinGecko API (https://www.coingecko.com/en/api)
  CoinGecko est une plateforme reconnue dans le domaine des cryptos. Son API gratuite fournit des données historiques et en temps réel au format JSON. C’est cette source que j’utiliserai pour alimenter mon application.

- Investing.com – Live Charts (https://www.investing.com/charts/live-charts)
  En analysant cette plateforme, j’ai pu m’inspirer des fonctionnalités de visualisation qui pourraient être intéressantes à intégrer (choix d’un actif, intervalle de temps, graphiques multi-séries).

### 2.2 Justification du choix

J’ai préféré l’utilisation d’une API JSON plutôt que de fichiers CSV pour plusieurs raisons :

1. Les données sont toujours à jour.
1. L’API permet une intégration dynamique sans téléchargement manuel.
1. Le format JSON est mieux adapté à la sérialisation en objets C#.

## 3. Analyse et planification

### 3.1 User Stories

Voir [README.md](https://github.com/Josefnademo/Plot_those_lines/blob/main/README.md)
(section User Stories) et le [GitHub Project](https://github.com/users/Josefnademo/projects/5) associé.

Exemples:

- Importer des données JSON depuis l’API CoinGecko.
- Afficher un graphique temporel avec ScottPlot.
- Comparer plusieurs cryptomonnaies.
- Documentation (Rapport, JDT, README, Planification).

### 3.2 Planification

J’ai planifié le projet sur **8 semaines** pour un total de 24 à 32 périodes, avec une marge de sécurité.

Consulter le fichier séparé [Planification.md](https://github.com/Josefnademo/Plot_those_lines/blob/main/doc/Planification.md)

## 4. Réalisation

### 4.1 Choix techniques

- **Framework** : C# Windows Forms (facile à mettre en œuvre et compatible avec ScottPlot).
- **API externe** : CoinGecko (format JSON).
- **Parsing JSON** : System.Text.Json.
- **Manipulation de données** : LINQ (remplace les boucles for).
- **Graphiques** : ScottPlot (librairie simple et efficace).

### 4.2 Organisation du code

- Namespace principal : PTL_Crypto.

- Classes :

  - CryptoPrice (modèle de données).
  - ApiClient (récupération JSON).
  - PlotManager (gestion graphique).

- Méthodes d’extension :
  - Conversion timestamp → DateTime.
  - Conversion liste JSON → séries exploitables par ScottPlot.

## 5. Tests

### Tests unitaires

Les tests unitaires ont été conçus pour valider la fiabilité et la cohérence des différentes étapes de traitement des données dans l’application.  
Chaque test correspond à une partie spécifique du flux logique — de la récupération des données jusqu’à leur affichage sur le graphique.

---

- a. **Vérification du parsing du JSON depuis l’API CoinGecko**  
  **Objectif** : s’assurer que les données téléchargées (format JSON) sont correctement désérialisées en objets `CryptoPrice`.  
  **Méthode** : test de désérialisation simulée à partir d’un échantillon JSON.  
  **Résultat attendu** : chaque objet contient un symbole valide, un prix positif et une date correcte.  
  **Lien avec l’application** : cette étape correspond au chargement initial des données depuis l’API dans la logique principale.

---

- b. **Vérification de la conversion timestamp → DateTime**  
  **Objectif** : garantir que les timestamps Unix reçus de l’API sont correctement convertis en format `DateTime` lisible par C#.  
  **Méthode** : utilisation d’une méthode d’extension personnalisée (`FromUnixTimestamp`).  
  **Résultat attendu** : les dates sont en ordre chronologique croissant et au format UTC.  
  **Lien avec l’application** : cette conversion est essentielle pour l’affichage temporel correct des prix sur le graphique.

---

- c. **Vérification du filtrage des périodes avec LINQ**  
  **Objectif** : s’assurer que le filtrage des périodes (7, 30, 90 jours) renvoie les bons intervalles de données.  
  **Méthode** : application de requêtes LINQ sur les listes d’objets `CryptoPrice` avec filtrage par `DateTime`.  
  **Résultat attendu** : seules les entrées correspondant à la période sélectionnée sont conservées.  
  **Lien avec l’application** : cette étape correspond au choix de la période d’affichage du graphique par l’utilisateur.

---

- d. **Vérification de la transformation des données en séries ScottPlot**  
  **Objectif** : contrôler la correspondance entre les données brutes (JSON) et les séries prêtes à être tracées sur le graphique.  
  **Méthode** : simulation d’un graphique à partir de listes de valeurs pour les axes X (temps) et Y (prix).  
  **Résultat attendu** : les points affichés correspondent aux prix réels, dans le bon ordre temporel.  
  **Lien avec l’application** : cette étape valide la préparation des données avant leur affichage visuel via ScottPlot.

---

- e. **Test de validité et d’ordre temporel des prix**

  **Objectif** : vérifier que chaque objet `CryptoPrice` contient un symbole valide, un prix positif et que les dates sont dans le bon ordre chronologique.

  **Méthode** : création d’une liste de données simulées (`BTC` à différents instants) et utilisation des assertions XUnit pour contrôler :

  - que les symboles ne sont pas vides,
  - que les prix sont positifs,
  - que les timestamps ne sont pas dans le futur,
  - que les dates sont bien ordonnées.

  **Résultat attendu** : toutes les vérifications passent sans erreur, confirmant la cohérence et la validité des données.

  **Lien avec l’application** : ce test garantit que les données manipulées et affichées dans les graphiques sont logiquement correctes, évitant les anomalies comme des prix négatifs ou des dates inversées.

### Tests d’acceptation

Les tests d’acceptation permettent de vérifier que le comportement global de l’application correspond aux attentes de l’utilisateur.

| Cas de test | Description                                      | Résultat attendu                                             | Statut    |
| ----------- | ------------------------------------------------ | ------------------------------------------------------------ | --------- |
| 1           | Lancer l’application et rechercher “bitcoin”     | Le graphique Bitcoin s’affiche (30 jours par défaut)         | ✅ Réussi |
| 2           | Ajouter “Ethereum” et cocher les deux cryptos    | Les deux courbes apparaissent distinctement sur le graphique | ✅ Réussi |
| 3           | Décocher “Ethereum” dans la liste                | La courbe Ethereum disparaît, seule Bitcoin reste visible    | ✅ Réussi |
| 4           | Changer la période d’analyse (7 → 30 → 90 jours) | Le graphique se met à jour avec la nouvelle période          | ✅ Réussi |

## 6. Journal de travail (JDT)

Le Journal de travail est disponible dans [JDT.xlsx](https://github.com/Josefnademo/Plot_those_lines/blob/main/doc/Journal-de-Travail_NademoYosef.xlsx)

_Il contient les tâches réalisées à chaque séance, la durée et un commentaire (progrès, difficultés, corrections)._

## 7. Utilisation de l’IA

- Recherche d’API adaptées (CoinGecko, alternatives).
- Reformulation des User Stories.
- Assistance technique pour LINQ et Windows Forms(un des source d'information théorique).

Toutes les suggestions ont été vérifiées, adaptées et comprises avant intégration.

## 8. Conclusion

J’ai appris à :

- Travailler avec une API externe (CoinGecko) et traitement de données JSON.
- Manipuler des données JSON avec LINQ.
- Générer et afficher des séries temporelles avec ScottPlot.
- Implémentation de tests unitaires et tests d’acceptation réels.

Ce projet m’a permis de lier théorie et pratique, en réalisant un outil concret d’analyse visuelle des cryptomonnaies.

L’application Plot Those Lines! (Crypto Edition) constitue une solution complète, claire et évolutive pour visualiser les tendances des cryptomonnaies de manière intuitive et fiable.

## 9. Références

- CoinGecko API: https://www.coingecko.com/en/api
- Exemple d’interface: https://www.investing.com/charts/live-charts
- ScottPlot: https://scottplot.net
- Documentation Microsoft LINQ: https://learn.microsoft.com/dotnet/csharp/linq
- CoinMarketCap: https://coinmarketcap.com
