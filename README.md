

# Plant Mastering VR

![Page de couverture du projet](https://www.dropbox.com/scl/fi/1mlbqp77h0597680f94u4/Vue-Projet.png?rlkey=ijxdgyuw02e7a45wm8u1jd27u&st=pma87qji&raw=1)
# Introduction

Cette application a été réalisée dans le cadre d'un projet de groupe en 3ème année à l'INSA Rennes. L'objectif du projet était d'innover dans la manière d'enseigner l'entretien de plantes à l'aide de la réalité virtuelle (VR).

L'application offre plusieurs scénarios de formation et simule de manière réaliste les besoins d'une plante.

Le projet vise donc à faire acquérir aux utilisateurs des compétences pratiques et théoriques en matière de jardinage et d'entretien de plantes dans un environnement interactif et engageant. Que vous soyez un jardinier amateur ou un professionnel cherchant à perfectionner vos compétences, cette formation en VR offre une expérience d'apprentissage unique et enrichissante.

# Installation du projet
**Prérequis** : 
 - Un casque VR en mode développeur 
 - Unity
 - Une application de gestion du casque style Meta Quest Link
 - Steam VR

Pour installer le projet il suffit ensuite de cloner le projet sur votre ordinateur, de lancer Unity en ouvrant le projet et d'avoir le casque prêt à l'utilisation par l'ordinateur puis vous pourrez lancer le projet avec le bouton Start de Unity.

![Vue du projet sur Unity](https://www.dropbox.com/scl/fi/sn3wye1lll6wus9vzn72z/Demarrage.png?rlkey=4cpq2sqi79yz93j4tjftckift&st=1uvvpd68&raw=1)

# Fonctionnalités réalisées

Cette section détaille les fonctionnalités présentes dans notre projet, divisées en différentes catégories :

## Fonctionnalités liées à la simulation d'une plante

-   **Le besoin en eau:** Toutes les plantes ont un besoin en eau, ce
    besoin diffère d'une espèce de plante à une autre, c'est pourquoi
    nous avons ajouté plusieurs profils type de consommation d'eau et de
    réception d'eau.

-   **L'effet de l'humidité :** L'utilisateur a la possibilité de
    modifier l'humidité de son environnement, mais cela a un impact
    direct sur la plante. En effet, un environnement peu humide et sec
    entraînera un besoin fréquent en eau pour la plante puisqu'elle
    desséchera rapidement. Inversement, un environnement très humide
    produira l'effet contraire en submergeant la plante en eau. Si
    aucune action n'est prise par l'utilisateur, il y a un risque que la
    plante pourrisse.

-   **Sensibilité à la lumière et à la température :** De la même
    manière, une exposition excessive aux rayons du soleil et une
    température trop élevée entraîneront la fanaison de la plante. Pour
    éviter cela, des aides pédagogiques apparaîtront pour indiquer à
    l'apprenant que la plante a un besoin non rempli. Ainsi, ce dernier
    pourra l'abriter pour la protéger du soleil, si aucune action n'est
    prise, la plante risque de faner.

-   **Modélisation des états de la plante :** Grâce au logiciel Blender,
    nous avons pu modéliser graphiquement différents états pour une
    plante. Cela permet de rendre l'interaction plus réaliste pour
    l'utilisateur, notamment lorsque la plante fane ou pourrit.

       Les deux états de la plante :
    <div style="display: flex; justify-content: space-between;"> <img src="https://www.dropbox.com/scl/fi/jn0ttjpt59kbmk9ett3dt/Cactus-fan.png?rlkey=8eroxat715fvdu81e28qd3598&st=t4e7ht36&raw=1" alt="Image de cactus fané"> <img src="https://www.dropbox.com/scl/fi/1n3bk0dyojuk48x3qxqb7/Cactus.png?rlkey=qbbqqrzjlw2nfm8ketm04ebky&st=x6pmqbbh&raw=1" alt="Image de cactus non fané" style="width:14.5%;"> </div>
    
## Fonctionnalités liées aux interactions

-   **Interaction avec plusieurs outils :** L'utilisateur aura à sa
    disposition un arrosoir ou un brumisateur afin de fournir à la
    plante ses besoins en eau. Le brumisateur a un débit d'eau de sortie
    inférieur à celui de l'arrosoir, permettant ainsi de contrôler la
    quantité d'eau délivrée, notamment pour des plantes sensibles à
    l'humidité (comme les cactus).

       <div style="display: flex; justify-content: space-between;"> <img src="https://www.dropbox.com/scl/fi/25z242lu0u6skj06d1fgo/Arrosoir.png?rlkey=0vvlqspni2soo0d6as4jjsagp&st=g634bccz&raw=1" alt="Image de l'arrosoir" style="width:25%;"> <img src="https://www.dropbox.com/scl/fi/msft2yx3x9in5jayc5uuu/Brumisateur.png?rlkey=p44s26ep8h5p5piqqcxpepqzc&st=fzr0xqjh&raw=1" alt="Image du brumisateur" style="width:24%;"> </div>
         Images de l'arrosoir et du brumisateur

    
-   **Se mouvoir dans l'espace :** L'utilisateur a la possibilité de se
    déplacer librement dans l'environnement à l'aide de la
    téléportation.

-   **Gestion des environnements :** L'apprenant à la possibilité de
    gérer son environnement en modifiant la température ou l'humidité
    ressenties par les plantes dans une zone précise et cela grâce à un
    bouton rotatif.
    
    Menu de la serre : 
    
    ![Image du menu de la serre](https://www.dropbox.com/scl/fi/4cze63l3pwiqr2g99taz4/Serre.png?rlkey=mdij6c7lr9yy6luxownnp7gzl&st=w2ayrk9s&raw=1)
-   **Terrain de jeu:** L'utilisateur dispose de plusieurs
    environnements pour s'occuper de ses plantes : une maison, un jardin
    ou même une serre. Chacun de ces endroits dispose de
    caractéristiques différentes (température, humidité, etc.)
    compatibles avec certaines espèces de plantes. Un cycle jour/nuit
    est implémenté pour se rapprocher au maximum de la réalité.

## Fonctionnalités pédagogiques

La pédagogie est au coeur même de notre projet. Ainsi, plusieurs
fonctionnalités pédagogiques ont été introduites afin d'aider et de
guider l'utilisateur en cas de difficulté :

- **Ajout d'indicateurs visuels:** Des indicateurs visuels sont placés
au-dessus de la plante pour indiquer à l'utilisateur l'état de santé
globale de la plante et ses besoins actuels en eau ou autres. En
fonction de la difficulté choisie, certains indicateurs pourront
apparaître ou disparaître.

  Emojis apparaissant à côté de la plantes : 
  
  ![Image d'une plante et de ses emojis](https://www.dropbox.com/scl/fi/w46zrsgww6rht57z3zzqb/CropPlanteEmoji.png?rlkey=bn76va6ss5ep7m90y6bcxvdmy&st=7367gxmy&raw=1)

- **Guide d'informations :** Fournit à l'utilisateur des informations
cruciales sur les plantes pour bien les entretenir, telles que leurs
besoins en eau, la température idéale et l'humidité optimale. Fournit
également des tutoriels à suivre pour utiliser au mieux l'application.

  ![Guide d'information sur une plante](https://www.dropbox.com/scl/fi/ftfkrzh56tjhanimvnysn/Guide.png?rlkey=tzmcowhkivac3d2vuz8am3k5z&st=c9xx1re6&raw=1)

- **Choix des plantes à ajouter :** Pour permettre à l'apprenant d'évoluer
à son rythme, ce dernier aura le choix de gérer le nombre de plantes
qu'il souhaite entretenir et leur espèce, cela se fait grâce à un menu
dédié.

 Menus dédiés à la gestion des plantes :
 ![Menus de gestion des plantes](https://www.dropbox.com/scl/fi/239b2ltt8bn86qkuo0hmz/Menus.png?rlkey=g5igyxg1yygq51erxydwri2jd&st=7lidlw71&raw=1)

- **Sauvegarde et affichage du score de l'utilisateur :** Permet à
l'utilisateur d'avoir une idée précise et concrète sur ses capacités à
gérer et à entretenir des plantes grâce à un score[^1] calculé sur ses
interactions dans l'environnement.

- **Ajout d'un système de niveau:** Un système de niveaux a été mis en
place pour permettre à l'apprenant de progresser graduellement. Il
comprend des niveaux pré-scénarisés de difficulté croissante, allant du
niveau 1 (considéré comme facile, peu de connaissances en jardinage
demandées avec plusieurs aides affichées pour faciliter la validation
par l'utilisateur) au niveau 10 (exigeant l'application de toutes les
connaissances acquises lors des niveaux précédents, sans aides, et
impliquant l'utilisation de plusieurs outils et équipements, comme une
serre). Il existe également le niveau initial : `bac à sable` permettant
à l'apprenant de configurer librement l'environnement selon ses
préférences.

- **Panneau récapitulatif :** Un panneau récapitulatif comportant
plusieurs informations sur le niveau en cours de l'utilisateur est
disponible pour permettre à ce dernier de bien se situer.

  ![Panneau récapitulatif](https://www.dropbox.com/scl/fi/d4enl61je9pn0d2vobayw/Panneau-Recapitulatif.png?rlkey=9sgst3vncf4m24qzvnep10pma&st=yth3hq93&raw=1)

## Fonctionnalités diverses

-   **Sauvegarde :** Les données des plantes: valeurs actuelles des
    besoins, statistiques et positions sont sauvegardées au format JSON
    lorsque l'utilisateur quitte la zone initiale, que ce soit parce
    qu'il a quitté l'application ou qu'il effectue un niveau guidé.
    Ainsi il est possible pour conserver les performances de
    l'application de supprimer nos plantes pour les recréer lorsque le
    niveau initial sera rechargé soit par le lancement de l'application
    soit par le panneau récapitulatif.

-   **Statistiques :** Des statistiques d'entretien des plantes sont
    établies. Elles ont pour but premier de détecter si l'utilisateur
    est en difficulté et donc dans ce cas de lui fournir de l'aide,
    elles permettent également à quelqu'un d'extérieur de pouvoir
    évaluer le niveau de maitrise de l'utilisateur.

# Contributeurs
Ce projet a été réalisé par Arno Lécrivain en collaboration avec Mohamed Amine Lahmamsi, Tom Lafay, Galip Utku Akay et Michèle Christine Mbeutoum-Nya. sous l'encadrement de Mathieu Risy.
# Licence

Ce projet est sous une licence [MIT](https://www.mit.edu/~amini/LICENSE.md "Licence MIT").

Template pour citer le travail :
MIT (c) 2024 LECRIVAIN Arno

[^1]: Proportionnel au temps passé où la plante est correctement
    entretenue
