using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
    GestionNiveau gère le lancement d'un nouveau niveau lorsque l'on appuie sur le bouton valider et gère aussi le tableau d'information
*/

public class GestionNiveau : MonoBehaviour
{
    private FabriqueNiveau fabriqueNiveau;

    // Textes pour le tableau d'information
    [SerializeField] private TextMeshPro niveauActuelText;
    [SerializeField] private TextMeshPro plantesASoccuper;
    [SerializeField] private TextMeshPro temperatureActuelle;
    [SerializeField] private TextMeshPro humiditeActuelle;
    [SerializeField] private TextMeshPro remarque;
    
    // Liste du niveau
    [SerializeField] private TMP_Dropdown ChoixNiveau;

    // Objets à téléporter dans la zone des niveaux ou à remettre à sa place
    [SerializeField] private GameObject XROrigin;
    [SerializeField] private GameObject PanneauInformations;
    [SerializeField] private GameObject Serre;
    [SerializeField] private GameObject Maison;
    [SerializeField] private GameObject Parasol;
    [SerializeField] private GameObject Arrosoir;
    [SerializeField] private GameObject Brumisateur;
    [SerializeField] private GameObject Valide;
    [SerializeField] private GameObject NonValide;

    // Les 4 différents environnements
    private GestionEnvironnement EnvZonePrincipale;
    private GestionEnvironnement EnvZoneTuto;
    private GestionEnvironnement EnvZoneSerre;
    private GestionEnvironnement EnvZoneMaison;


    private bool nivValide; // Donne si le niveau est validé ou non
    private int nivCourant; // Donne le niveauCourant
    private int frameUpdate; // Evite de trop actualiser

    // Permet de sauvegarder les états des plantes
    private struct ObjectState 
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public ObjectState(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
    ObjectState originalMaison;
    ObjectState originalSerre;
    ObjectState originalPanneauInformations;
    ObjectState originalXROrigin;
    ObjectState originalParasol;
    ObjectState originalArrosoir;
    ObjectState originalBrumisateur;
    

    private void Start()
    {
        // Sauvegarde des positions et rotations des objets pour les remettre à leur place lors du retour zone principale
        originalMaison = new ObjectState(Maison.transform.position, Maison.transform.rotation);
        originalSerre = new ObjectState(Serre.transform.position, Serre.transform.rotation);
        originalPanneauInformations = new ObjectState(PanneauInformations.transform.position, PanneauInformations.transform.rotation);
        originalXROrigin = new ObjectState(XROrigin.transform.position, XROrigin.transform.rotation);
        originalParasol = new ObjectState(Parasol.transform.position, Parasol.transform.rotation);
        originalArrosoir = new ObjectState(Arrosoir.transform.position, Arrosoir.transform.rotation);
        originalBrumisateur = new ObjectState(Brumisateur.transform.position, Brumisateur.transform.rotation);


        EnvZoneMaison = Maison.GetComponent<GestionEnvironnement>();
        EnvZoneSerre = Serre.GetComponent<GestionEnvironnement>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Environnement"))
        {
            switch (obj.name)
            {
                case "Environement Zone Tuto":
                    EnvZoneTuto = obj.GetComponent<GestionEnvironnement>();
                    break;
                case "Environement Zone Principale":
                    EnvZonePrincipale = obj.GetComponent<GestionEnvironnement>();
                    break;
            }
        }
        fabriqueNiveau = GetComponent<FabriqueNiveau>();
        // Des que le bouton est cliqué, on appelle la fonction AllerAuNiveauSelectionne
        // Fonction déjà appelée depuis l'inspecteur! (Laisser en commentaire) 
        //BoutonNiveau.onClick.AddListener(AllerAuNiveauSelectionne); 
        AfficherInformationsNiveau();
        frameUpdate = 0;
    }

    private void Update()
    {
        if (frameUpdate == 100)
        {
            if (fabriqueNiveau.NiveauReussi())
            {
                nivValide = true;
            }
            AfficherNiveauReussi();

            // Si dans un niveau autre que le sandbox, on s'assure que le panneau d'information soit toujours face à la caméra.           
            if(fabriqueNiveau.NiveauCourant != 0)
            {
               // On fait en sorte que le panneau d'information soit toujours face à la caméra (le joueur)
                Vector3 directionToCamera = XROrigin.transform.position - PanneauInformations.transform.position;
                Quaternion rotationToCamera = Quaternion.LookRotation(directionToCamera);
                // Attention, il ne faut modifier que l'axe Y!!!
                Vector3 eulerAngles = PanneauInformations.transform.eulerAngles;
                eulerAngles.y = rotationToCamera.eulerAngles.y;
                PanneauInformations.transform.eulerAngles = eulerAngles; 
            }
            

            frameUpdate = 0;
        }
        frameUpdate++;
    }

    // Zone 0 : espace ouvert / jardin. Il sera toujours présent dans tout les cas
    // Zone 1 : Serre
    // Zone 2 : Espace fermé (Maison pour l'instant)
    private void AfficherInformationsNiveau() // Affiche les information liés au niveau actuel
    {
        if(fabriqueNiveau.NiveauCourant == 0)
        {
            niveauActuelText.text = "Menu principal/SandBox";
            plantesASoccuper.text = "Toutes les plantes sont disponibles. Toutes les zones sont disponibles.";
            temperatureActuelle.text = "Température actuelle :" + EnvZonePrincipale.temperature + "°C";
            humiditeActuelle.text = "Humidité actuelle :" + EnvZonePrincipale.humidity + "%";
            remarque.text = "Amusez-vous bien !";
            return;
        }else{
            niveauActuelText.text = "Niveau " + fabriqueNiveau.GetNiveaux()[fabriqueNiveau.NiveauCourant].nomNiveau;
            plantesASoccuper.text = "Jardin: " + fabriqueNiveau.GetNiveaux()[fabriqueNiveau.NiveauCourant].nbPlantes[0] + " Serre:" + fabriqueNiveau.GetNiveaux()[fabriqueNiveau.NiveauCourant].nbPlantes[1] + " Maison: " + fabriqueNiveau.GetNiveaux()[fabriqueNiveau.NiveauCourant].nbPlantes[2];
            temperatureActuelle.text = "Température actuelle :" + EnvZoneTuto.temperature + "°C";
            humiditeActuelle.text = "Humidité actuelle :" + EnvZoneTuto.humidity + "%";
            remarque.text = "Remarque : " + fabriqueNiveau.GetNiveaux()[fabriqueNiveau.NiveauCourant].nomNiveau;
        }
    }

    // Téléporte tous les objets au bon endroit et lance le niveau sélectionné par l'utilisateur
    public void AllerAuNiveauSelectionneDropdown(){
        int nivAvant = nivCourant;
        nivCourant = ChoixNiveau.value;
        nivValide = false;
        DataManager dt = Object.FindObjectOfType<DataManager>();
        // Cas de base
        if(nivCourant == 0 && nivAvant != 0) //On retourne dans la zone classique, chargement du bac à sable
        {
            fabriqueNiveau.SupprimerNiveau();
            // On remet les objets à leur place
            Serre.transform.position = originalSerre.Position;
            Serre.transform.rotation = originalSerre.Rotation;
            Maison.transform.position = originalMaison.Position;
            Maison.transform.rotation = originalMaison.Rotation;
            PanneauInformations.transform.position = originalPanneauInformations.Position;
            PanneauInformations.transform.rotation = originalPanneauInformations.Rotation;
            XROrigin.transform.position = originalXROrigin.Position;
            XROrigin.transform.rotation = originalXROrigin.Rotation;
            Parasol.transform.position = originalParasol.Position;
            Parasol.transform.rotation = originalParasol.Rotation;
            Arrosoir.transform.position = originalArrosoir.Position;
            Arrosoir.transform.rotation = originalArrosoir.Rotation;
            Brumisateur.transform.position = originalBrumisateur.Position;
            Brumisateur.transform.rotation = originalBrumisateur.Rotation;
            AfficherInformationsNiveau();
            Serre.SetActive(true);
            Maison.SetActive(true);
            dt.reactiverPlantes();
            return;
        }
        else if(nivCourant !=0 && nivAvant != nivCourant) // Niveau a changé et c'est pas le bac à sable
        {
            if(nivAvant == 0)
            {
                dt.desactiverPlantes();
            }
            Vector3 mainPosition = new Vector3(-27.6599998f,0.4672995f,-62.0400009f);
            fabriqueNiveau.SupprimerNiveau();
            fabriqueNiveau.GenerationNiveau(nivCourant);
            AfficherInformationsNiveau();

            // On déplace les objets dans la zone tuto. Il faudra vérifier les angles.
            XROrigin.transform.position = mainPosition;
            PanneauInformations.transform.position = new Vector3(-28.8299999f,0.430000007f,-63.4199982f);
            PanneauInformations.transform.rotation = new Quaternion(2.15728022e-08f,0.160372823f,3.50505891e-09f,0.987056494f);
            Parasol.transform.position = new Vector3(-27.4699993f,0.425330162f,-69.2099991f);
            Arrosoir.transform.position = new Vector3(-26.4699993f,0.425330162f,-69.2099991f);
            Brumisateur.transform.position = new Vector3(-25.4699993f,0.7f,-69.2099991f);
            
            int[] nbPlantesParZones = fabriqueNiveau.GetNiveaux()[nivCourant].nbPlantes;
            Debug.Log(nbPlantesParZones[0] + " " + nbPlantesParZones[1] + " " + nbPlantesParZones[2]);
            Maison.SetActive(true);
            Maison.transform.position = new Vector3(-25.9699993f,2.28999996f,-72.1800003f);
            Maison.transform.rotation = new Quaternion(0f,-0.330038786f,0f,-0.943967402f);
            if (nbPlantesParZones[1] != 0) // La serre doit être activée sinon pas affichée
            {
                Serre.SetActive(true);
                Serre.transform.position = new Vector3(-21.8600006f,0.569999993f,-69.6999969f);
                Serre.transform.rotation = new Quaternion(0.318938255f,0.619588673f,0.637682378f,-0.328252137f);
            }
            else
            {
                Serre.SetActive(false);
            }
        }
    }


    // Affiche la croix ou le tick en fonction de si le niveau est réussi ou non
    private void AfficherNiveauReussi()
    {
        if (nivCourant == 0)
        {
            Valide.SetActive(false);
            NonValide.SetActive(false);
        }
        else if (nivValide)
        {
            Valide.SetActive(true);
            NonValide.SetActive(false);
        }
        else
        {
            Valide.SetActive(false);
            NonValide.SetActive(true);
        }
    }
}