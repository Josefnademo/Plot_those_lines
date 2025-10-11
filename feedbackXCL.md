# Feedback

## 14.9

- User stories:
  - Problèmes de formulation, voir feedback général
  - Vous avez fait une maquette, mais vos US n'y font pas référence
- Git: Relisez `git.pdf` du project handbook: vos commits ne sont pas bien nommés
- Journal: très bien!
- Global: OK

## 23.9

- Votre message Teams dit "presque chaque issue contient les TA, et bien-sûr des US". Ce n'est pas correct:
  - Une issue qui a le label "User Story" **EST** une user story
  - Une User Story **CONTIENT**:
    1. Une description "en tant que ..."
    2. Des tests d'acceptance (Contexte, Action, Résultat) vérifiables
    3. Une ou plusieurs maquette qui permet de comprendre le contexte et/ou l'action et/ou le résultat
- User stories, en général
  - Mettez les tests dans la partie description de l'issue, pas dans un commentaire. Et insérez la (ou les) maquette(s) à la suite des TA. Cela rend la story plus lisible
  - Mettez un label "User Story" sur vos US, car vous avez des issues dans votre repo qui ne sont pas du tout des US ("prototype API" par exemple)
  - Vous ne faites toujours pas référence à vos maquettes. Pire: vous avez un test qui dit "Dans l’interface, je peux sélectionner plusieurs cryptomonnaies.", mais il n'y a rien dans votre maquette qui propose un mode de sélection
  - Je n'arrive toujours pas à comprendre le fonctionnement de votre application sur la base de ce que vous me présentez comme analyse
- Pour les détails: regardez les commentaires dans les stories elle-même

## 26.9

- Mettez la maquette dans la description de l'[issue](https://github.com/Josefnademo/Plot_those_lines/issues/19)
- Concentrez-vous sur cette story, qui n'a pas besoin de l'API puisqu'elle se base sur les données stockées
- Formulez une vraie user story qui concerne l'API. Selon mon interprétation de ce que vous avez écrit, cela pourrait être quelquechose du genre: quand on démarre l'app, on a un message qu'on va chercher des données récentes sur CoinGecko. Quand on les a récupérées, un message annonce la disponibilté de nouvelles données (ou signale une erreur)
- Votre gitignore est un peu juste. Il n'exclut pas les fichiers temporaires de contexte de visual studio (contenu du dossier `.vs`). Procédez à un [refresh](https://sigalambigha.home.blog/2020/03/11/how-to-refresh-gitignore/) svp

## 9.10

- Le point concernant le `.gitignore` ci-dessus doit toujours être corrigé
- jdt: très bien
- Mettez une maquette dans la US "Bouton d'importation"
- Le rapport doit être étoffé, mais il est plutôt bien parti

## 10.10 (80%)

- Rythme (avancement du code): OK
- Qualité (normes, commentaires): KO
  - il vous faut renommer les contrôles utilisés. On ne peut pas bien comprendre votre code avec des identificateurs tels que `button1`,`button2`,...
  - il reste des chemins d'accès vers les fichiers qui sont trop dépendants de votre poste de travail (Form1:137)
- Connaissances professionnelles (LinQ): OK
- Processus de travail (jdt, git, git project, release):
  - Release KO, Elle doit être accompagnée du journal de travail et du rapport (voir project handbook)
  - il y a un problème avec vos issues: le repos contient plein d'issues qui sont sans rapports avec le projet
  - de plus, il manque toujours les labels "user story" que j'ai demandé il y a quelques semaines.
  - veiller à bien prendre connaissance du message que j'ai déposé dans le canal P_FUN et que nous avons vu en classe hier
- Expression (user stories, rapport): KO. Je ne retrouve aucune user Story. Vous avez une bonne base de rapport, mais il doit encore être étoffé.
- Ecologie (.gitignore): OK
- Attitude groupe: vous devez faire mieux. Vous étiez absent hier, vous m'avez rien annoncé dans le canal Teams.
- Attitude personnelle: OK
