using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using TMPro;

/*
    FabriqueNiveau est un script qui est utilisé avec GestionNiveau, il permet de créer un niveau avec des plantesNiveau si on lui fournit en paramètre un niveau

    Idée d'amélioration -> Rajouter de niveaux supplémentaire possible pour l'utilisateur à la main à partir d'un JSON en faisant marcher loadNiveauFromJson

    Attributs de la classe:
    niveaux : Liste des niveaux 
    ListPrefabs : Liste des prefabs des plantes existants
    positionsSUpport : Liste des positions possibles des supports et donc des plantes générées pour les différents environnements
    NiveauCourant : niveau actuel
    ChoixNiveau : Liste des Niveaux
    ModeleSupport : GameObject à copier pour avoir un support
*/

// TO DO :
// - Pouvoir modifier les valeurs de température et d'humidité des environnements
public class FabriqueNiveau : MonoBehaviour
{
    private List<List<GameObject>> SupportsParEnvironnements;
    private Niveau[] niveaux;
    private List<string> ListPrefabs;
    // Liste des positions possibles pour les supports
    private Dictionary<int, List<Vector3>> positionsSupports = new Dictionary<int, List<Vector3>>
    {
        // Jardin : 0
        // Serre : 1
        // Maison : 2
        { 0, new List<Vector3> { new Vector3(-27.9300003f,0.430000007f,-69.3000031f), new Vector3(-29.7399998f,0.430000007f,-69.3000031f), new Vector3(-31.6800003f,0.430000007f,-69.3000031f), new Vector3(-31.6800003f,0.430000007f,-70.25f), new Vector3(-28.8299999f,0.430000007f,-70.25f), new Vector3(-26.1599998f,0.430000007f,-70.25f), new Vector3(-28.2099991f,0.430000007f,-71.9700012f), new Vector3(-29.1420002f,0.430000007f,-68.6959991f)} },
        { 1, new List<Vector3> { new Vector3(-21.8110008f,0.569999993f,-71.5390015f), new Vector3(-20.0919991f,0.569999993f,-70.4280014f), new Vector3(-22.7709999f,0.569999993f,-70.0660019f), new Vector3(-23.6499996f,0.569999993f,-68.6699982f), new Vector3(-21.0909996f,0.569999993f,-68.6699982f) } },
        { 2, new List<Vector3> { new Vector3(-23.4750004f,1.02900004f,-59.7490005f),new Vector3(-21.8400002f,1.02900004f,-56.8100014f),new Vector3(-22.8500004f,1.02900004f,-58.2000008f), new Vector3(-20.6200008f,1.02900004f,-56.862999f), new Vector3(-18.8400002f,1.02900004f,-57.8100014f), new Vector3(-21.9899998f,1.02900004f,-60.4399986f), new Vector3(-19.3899994f,1.02900004f,-59.5900002f) } }
    };
    public int NiveauCourant{ get; private set; }

    [SerializeField] private TMP_Dropdown ChoixNiveau;
    public Niveau[] GetNiveaux()
    {
        return niveaux;
    }
    public GameObject ModeleSupport;

    public class Niveau
    {
        /*
        Attributs d'un niveau:
        numero : Numéro du niveau
        rangePlanteMin : Intervalle min des prefabs de plantes qui peuvent être ajoutées 
        rangePlanteMax : Intervalle max des prefabs de plantes qui peuvent être ajoutées 
        nbPlantes : nombres de plantes par environnement
        lumiere : Précise si le niveau souhaite ou non que la lumiere soit ignorée sur les plantes
        eau : Précise si le niveau souhaite ou non que l'eau soit ignorée sur les plantes
        qteEau : Quantité d'eau initiale des plantes au début du niveau
        nomNiveau : nom du niveau pour l'utilisateur
        conditionNiveau : Condition sur chaque plante pour que le niveau soit réussi, non correctement fonctionnel -> revoir dans planteNiveau
        */
        public int numero;
        public int rangePlanteMin;
        public int rangePlanteMax;
        public int[] nbPlantes;
        public bool lumiere;
        public bool eau;
        public float qteEau;
        public string nomNiveau;
        public string conditionNiveau;

        public Niveau(int num, int rangeMin, int rangeMax, int[] nbPlantesZones, bool lum, bool e, float qte, string id, string cond)
        {
            numero = num;
            rangePlanteMax = rangeMax;
            rangePlanteMin = rangeMin;
            nbPlantes = nbPlantesZones;
            lumiere = lum;
            eau = e;
            qteEau = qte;
            nomNiveau = id;
            conditionNiveau = cond;
        }
        public Niveau loadNiveauFromJson(int numero)
        {
            string path = Application.dataPath + "/Niveaux/Niveau" + numero + ".json";
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<Niveau>(json);
        }
        // Pas testé mais peut peut-être fonctionner
        // Exemple de fichier JSON pour le niveau 5
        //{
        //     "numero": 5,
        //     "rangePlanteMin": 0,
        //     "rangePlanteMax": 3,
        //     "nbPlantes": {0,3,0},
        //     "lumiere": false,
        //     "eau": false,
        //     "qteEau": 0.1f,
        //     "nomNiveau": "Plusieurs types de plantes",
        //     "conditionNiveau": "this.santeLumiere() && this.santeEau()"
        //}

    }
    void Start()
    {
        SupportsParEnvironnements = new List<List<GameObject>>();
        ChargerSupports();
        ConfigurationNiveaux();
        AjouterNiveauxOption();
        DataManager dt = Object.FindObjectOfType<DataManager>();
        ListPrefabs = new List<string>();
        ListPrefabs = dt.ListPrefabs;
        NiveauCourant = 0;
    }

    private void ConfigurationNiveaux() // Configurer les niveaux de bases actuels
    {
        niveaux = new Niveau[11];
        niveaux[0] = new Niveau(0, 0, 7, new int[] {0,0,0}, false, false, 0.1f, "Sandbox", "true");
 
        niveaux[1] = new Niveau(1, 0, 0, new int[] {1,0,0}, true, false, 0.1f, "Tuto", "this.santeEau()");
        niveaux[2] = new Niveau(2, 0, 0, new int[] {3,0,0}, true, false, 0.1f, "Manque eau", "this.santeEau()");
        niveaux[3] = new Niveau(3, 0, 0, new int[] {3,0,0}, false, true, 0.9f, "Manque lumiere", "this.santeLumiere()");
        niveaux[4] = new Niveau(4, 0, 0, new int[] {0,3,0}, false, false, 0.9f, "Mauvais environnement", "this.santeLumiere() && this.santeEau() && this.Humidite == 0"); // Cactus dans serre
        niveaux[5] = new Niveau(5, 1, 1, new int[] {0,3,0}, false, false, 0.1f, "Changement Plante", "this.santeLumiere() && this.santeEau()");
        niveaux[6] = new Niveau(6, 0, 3, new int[] {0,3,0}, false, false, 0.1f, "Plusieurs types de plantes", "this.santeLumiere() && this.santeEau()");
        niveaux[7] = new Niveau(7, 0, 7, new int[] {0,3,0}, false, false, 0.1f, "Tous types de plantes", "this.santeLumiere() && this.santeEau()");
        niveaux[8] = new Niveau(8, 0, 7, new int[] {0,0,3}, false, false, 0.1f, "Dernier environnement", "this.santeLumiere() && this.santeEau()");
        niveaux[9] = new Niveau(9, 0, 7, new int[] {3,0,2}, false, false, 0.1f, "Plus de plantes !", "this.santeLumiere() && this.santeEau()");
        niveaux[10] = new Niveau(10, 0, 7, new int[] {3,2,2}, false, false, 0.1f, "Toujours plus de plantes !", "this.santeLumiere() && this.santeEau()");
    }
    public void ChargerSupports() // Pas utilisé il me semble mais permet de charger la liste des supports
    {
        GameObject[] Environnements = GameObject.FindGameObjectsWithTag("Environnement");
        GameObject[] ListeSupports = GameObject.FindGameObjectsWithTag("Support");
        bool[] SupportChecked = new bool[ListeSupports.Length];
        int i;
        foreach (GameObject env in Environnements)
        {
            i = 0;
            List<GameObject> SupportEnv = new List<GameObject>();
            foreach (GameObject Support in ListeSupports)
            {
                if (!SupportChecked[i] && env.GetComponent<Collider>().bounds.Contains(Support.transform.position))
                {
                    SupportEnv.Add(env);
                    SupportChecked[i] = true;
                }
                i++;
            }
            SupportsParEnvironnements.Add(SupportEnv);
        }
    }

    //Génère un niveau
    public void GenerationNiveau(int numNiveau){
        // Vérifie si le numéro du niveau est dans la plage valide (0-9)
        if (numNiveau > 10 || numNiveau < 0)
        {
            Debug.Log("Niveau non compris");
            return;
        }
        // Récupère le niveau correspondant au numéro
        Niveau nv = niveaux[numNiveau];
        NiveauCourant = numNiveau;
        // Vérifie si la plage de plantes est dans la plage valide (0-7)
        // Correspond au 6 types de plantes disponibles: Cactus PB / Cactus / Calathea / Monstera / Plante Consommatrice / Plante Lambda / Plante Sans Nom
        if (nv.rangePlanteMin < 0 || nv.rangePlanteMax > 7)
        {
            Debug.Log("Erreur de range de plante");
            return;
        }
        List<Vector3> positionsUtilisees = new List<Vector3>();
        // Boucle sur les trois environnements
        for (int i = 0; i < 3; i++)
        {
            int j = 0;
            // Tant que le nombre de plantes générées est inférieur au nombre de plantes requis pour la zone
            while (j < nv.nbPlantes[i])
            {
                j++;
                // Génère le type de plante aléatoirement
                int rdmPlante = Random.Range(nv.rangePlanteMin, nv.rangePlanteMax);
                GameObject modelePlante = (GameObject)Resources.Load("Prefabs/" + ListPrefabs[rdmPlante]);
                
                // Définit la position de la plante
                List<Vector3> positionsPossible = new List<Vector3>(positionsSupports[i]); // Crée une copie de la liste pour pouvoir la modifier
                Vector3 positionSupport;

                // Continue à choisir une position aléatoire jusqu'à en trouver une qui n'a pas encore été utilisée
                do
                {
                    int randomIndex = Random.Range(0, positionsPossible.Count);
                    positionSupport = positionsPossible[randomIndex];
                    positionsPossible.RemoveAt(randomIndex); // Supprime la position de la liste pour qu'elle ne soit pas réutilisée
                } while (positionsUtilisees.Contains(positionSupport));

                positionsUtilisees.Add(positionSupport); // Ajoute la position à la liste des positions utilisées
                
                GameObject support = Instantiate(ModeleSupport,positionSupport,Quaternion.identity);
                support.tag = "SupportNiveau"; // Change le tag pour facilement le supprimer
                Vector3 positionPlante = new Vector3(positionSupport.x, positionSupport.y+0.5f, positionSupport.z);
                // Instancie la plante à la position définie
                GameObject gameObjectPlante = Instantiate(modelePlante, positionPlante, Quaternion.identity);
                // Change le tag de la plante pour qu'elle ne soit pas sauvegardée
                gameObjectPlante.tag = "PlanteNiveau";
                // Ajoute le script PlanteNiveau à la plante
                PlanteNiveau ptNiv = gameObjectPlante.AddComponent<PlanteNiveau>();
                ptNiv.eauInactif = nv.eau;
                ptNiv.lumiereInactif = nv.lumiere;
                ptNiv.condition = nv.conditionNiveau;
            }
        }
    }

    public void SupprimerNiveau() // Supprime le niveau actuel et repasse à 0 le niveauCourant
    {
        NiveauCourant = 0;
        GameObject[] planteTableau = GameObject.FindGameObjectsWithTag("PlanteNiveau");
        foreach (GameObject plante in planteTableau)
        {
            Destroy(plante);
        }
        GameObject[] supports = GameObject.FindGameObjectsWithTag("SupportNiveau");
        foreach (GameObject support in supports)
        {
            Destroy(support);
        }
    }

    private void AjouterNiveauxOption() // Ajoute la liste des niveaux possible au début de l'app
    {
        foreach (Niveau nv in niveaux)
        {
            ChoixNiveau.options.Add(new TMP_Dropdown.OptionData("Niveau " + nv.numero + " : " + nv.nomNiveau));
        }
        ChoixNiveau.RefreshShownValue();
    }

    public bool NiveauReussi() // Indique si le niveau est réussi ou non
    {
        PlanteNiveau[] planteTableau = GameObject.FindObjectsOfType<PlanteNiveau>();
        foreach (PlanteNiveau plante in planteTableau)
        {
            if (!plante.conditionRemplie())
            {
                return false;
            }
        }
        return true;
    }
}
