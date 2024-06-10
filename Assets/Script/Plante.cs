using UnityEngine;
/*
    Principe du script Plante:

    Tout les modèles de plantes 3D sont déjà dans notre scène et possèdent le script Plante.
    Les plantes sont préexistantes, le script (équivalent à un objet) y est rattaché, on décide d'afficher ou de ne pas afficher la plante avec l'attribut active

    Au lancement on charge d'un fichier un DataPlante qui viendra recharger le script à l'identique des données au moment ou l'on avait sauvegardé pour la dernière fois,
    A la sauvegarde, on sauvegarde toutes les données importantes à l'intérieur d'un DataPlante (Méthode LoadDataFromSerialization et GetDataForSerialization)


    Chaque besoin ou gestion de la plante est subdivisé en sous classes.
    Sous classes existantes :

    -   GestionEau : permet de gérer en interne le besoin de l'eau de la plante
    -   GestionLumière : permet de gérer en interne le besoin de lumière de la plante
    -   GestionEmoji : permet la gestion en interne des différents emojis/warnings indiquant l'état de la plante

    Attributs de la classe Plante :
    -   prefab : Précise le prefab (espèce) de la plante, (utile pour recréer la plante) 
    -   id : Id unique qui permet de différencier les plantes
    -   fane : Précise si notre plante est fanée ou non
    -   gameObjectParent : Référence le gameObject(modèle) Plante rattaché à l'objet
    -   n : Attribut pour ralentir le nombre d'actualisations du script plante
    -   humidite : Donne la valeur d'humidité de la plante, pas vraiment utile car retransmis aussi via gestionEau (Lié à environnement)
    -   temperature : Donne la temperature de la plante (Lié à l'environnement), n'a pas d'influence actuellement sur la plante
    -   meshRenderer : A priori n'a pas d'utilité mais a été ajouté pour essayer de faire changer l'aspect de la plante 
*/

public class Plante : MonoBehaviour {
    public string prefab;
    public int id;
    public bool fane;
    public GameObject gameObjectParent{get; protected set;}
    protected int n;
    protected float humidite;
    public int temperature;
    protected MeshRenderer meshRenderer; // Référence au MeshRenderer de la plante
    [SerializeField] protected GestionEau eauPlante; // Système de gestion de l'eau
    public GestionLumiere lumierePlante{get; protected set;} // Système de gestion de la lumière
    protected GestionEmoji emojiPlante; // Système de gestion des Emojis

    // Méthode appelée au lancement de la scène
    public virtual void Start() {
        meshRenderer = GetComponent<MeshRenderer>();
        emojiPlante = new GestionEmoji();
        emojiPlante.instantierWarning(this);
        if(eauPlante == null) // Si la plante n'a pas été chargée au démarrage()
        {
            eauPlante = new GestionEau();
        }
        gameObjectParent = this.transform.gameObject;
        lumierePlante = new GestionLumiere(this);
        n=0;
    }


    // Méthode appelée à chaque frame
    void Update()
    {
        if(n % 100 == 0) // ralentissement de la plante pour pas avoir des performances trop coûteuses
        {
            emojiPlante.miseAJourEmojis();
            eauPlante.consommationEau();
            n = 1;
        }
        n++;
    }

    // Méthode qui récupère les particules rentrant en collision avec notre Collider Plante
    public void OnParticleCollision(GameObject other)
    {
        if(other.tag == "EauParticle") // Si le système de particule qui rentre en collision est EauParticle alors ajouter de l'eau
        {
            eauPlante.receptionEau();
        }
    }

    public float getEau() // Retourne la quantité d'eau actuelle
    {
        return eauPlante.getEau();
    }
    public virtual bool santeEau() // Méthode bool qui retourne la santé de la plante concernant l'eau
    {
        return eauPlante.Sante();
    }

    public virtual bool santeLumiere() // Retourne la santé de la plante concernant la lumière
    {
        return lumierePlante.Sante();
    }

    public float getPourcentageEau() // Méthode foat qui retourne le pourcentage d'eau pour l'affichage du réservoir d'eau
    {
        return eauPlante.getPourcentage();
    }

    public bool santeGlobale() // Méthode bool qui retourne la santé de la plante, seule l'eau est implémentée ici
    {
        return (santeEau() && santeLumiere());
    }

    //planteData à été chargé depuis le DataManager
    public void LoadDataFromSerialization(DataPlante planteData) // Charger la plante depuis un fichier
    {
        if(planteData == null)
        {
            Debug.LogWarning("Aucune donnée de plante à charger.");
            return;
        }else{
            eauPlante.setEau(planteData.eauPlante);
            prefab = planteData.prefab;
            lumierePlante = planteData.lumierePlante;
            id = planteData.id;
        }
    }
    public DataPlante GetDataForSerialization() // Stocker les données à sauvegarder de cette plante dans un DataPlante
    {
        // On crée les données de la plante
        return new DataPlante(this);
    }

    public GestionEau getEauPlante() // Utile pour copier coller le script pour des planteNiveaux
    {
        return eauPlante;
    }
    
    protected void setQuantiteEau(int qte) // Pour charger l'eau au démarrage sans que l'on mette la composante gestionEau en public
    {
        eauPlante.setEau(qte);
    }

    public void setHumidite(float qte) // Pour charger l'humidité au passage d'une zone sans que l'on mette la composante gestionEau en public
    {
        eauPlante.setHumidite(qte);
        humidite = qte;
    }
}