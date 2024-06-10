using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Dynamic;
using TMPro;
using System.Text.RegularExpressions;

/*
    DataManager : Classe utilisée pour la sauvegarde de plantes ainsi que la gestion des plantes (Ajout, Suppression)
    Vous pouvez retrouver ce qui a été sauvegardé dans votre AppData dans le LocalLow/DefaultCompany

    Attributs de la classe :

    TouteNosPlantes : Utilisé au moment de la sauvegarde des plante, stocke les données des plantes dans une liste
    PlantesList : Utilisé pour sauvegarder les plantes (On peut sauvegarder uniquement les types primitifs ou les objets, pouvant contenant des listes)
    entretienPlantes : Utilisé pour sauvegarder les statistiques de l'utilisateur
    ListPrefabs : Liste des prefabs nécessaire pour savoir si le prefab existe, nécessite l'ajout à la main dans le start ou le fichier dans prefab du Appdata
    ObjetSauvegardePrefabs : Permet simplement de sauvegarder la liste des prefab
    DropdownAjout : Dropdown de la liste d'ajout des plantes (Donne les options)
    Dropdown Suppression : Dropdown de la liste de suppression des plantes (Permet la suppression d'une des plantes existantes)
    newID : Indique à quel id doit être associé une plante qu'on ajouterait
    Stats : GameObject liée au statistiques pour les récupérer aisément et interagir avec

    Evolution possible : 
    L'attribut ListPrefabs n'a pas vraiment d'utilité à être sauvegardé, peut être proposer un ajout d'espèce par l'utilisateur 
    pour ajouter des espèce à sa guise par l'utilisateur
*/

public class DataManager : MonoBehaviour{
    private List<DataPlante> TouteNosPlantes = new List<DataPlante>();
    private DataPlanteList PlantesList;
    private DataMaitriseUser entretienPlantes;
    public List<string> ListPrefabs = new List<string>();
    private DataPrefab ObjetSauvegardePrefabs;

    [SerializeField] private TMP_Dropdown DropdownAjout;
    [SerializeField] private TMP_Dropdown DropdownSuppression;
    private int newID;
    [SerializeField] private GameObject Stats;



    //Effectué au démarrage
    public void Start()
    {
        ChargerDonnees();
    }

    //Effectué quand l'application se ferme
    public void OnApplicationQuit()
    {
        FabriqueNiveau fb = Object.FindObjectOfType<FabriqueNiveau>();
        if(fb.NiveauCourant == 0) // Si l'on est dans le niveau 0, la sandbox, donc les plantes sont chargées alors sauvegarder
        {
            SauvegarderDonnees();
        }
    }

    //Sauvegarde des différentes données dans les fichiers AppData/LocalLow/DefaultCompany
    public void SauvegarderDonnees()
    {
        SauvegarderPlantes(); // Préparer la liste avec les données des plantes
        SauvegarderStatistiques(); // Préparer les données des statistiques
        PlantesList = new DataPlanteList(TouteNosPlantes, newID); // Stocker les données des plantes dans un objet prêt à la sauvegarde
        ObjetSauvegardePrefabs = new DataPrefab(ListPrefabs); // Sauvegarder la liste des prefabs
        // Sauvegarder ici aussi les autres données s'il y en a d'autres...
        // JsonUtility.ToJson() permet de convertir un objet en JSON
        string dataPlantesJson = JsonUtility.ToJson(PlantesList, true);
        // On efface le fichier existant pour le remplacer
        File.Delete(Application.persistentDataPath + "/plantes.json");
        // On écrit le JSON dans un fichier
        //Debug.Log(Application.persistentDataPath + "/plantes.json");
        File.WriteAllText(Application.persistentDataPath + "/plantes.json", dataPlantesJson);

        string dataPrefabsJson = JsonUtility.ToJson(ObjetSauvegardePrefabs, true);
        File.Delete(Application.dataPath + "/prefabs.json");
        File.WriteAllText(Application.dataPath + "/prefabs.json", dataPrefabsJson);

        string dataEntretienJson = JsonUtility.ToJson(entretienPlantes, true);
        File.Delete(Application.persistentDataPath + "/statistiquesEntretien.json");
        File.WriteAllText(Application.persistentDataPath + "/statistiquesEntretien.json", dataEntretienJson);
    }

    public void SauvegarderPlantes()
    {
        // On sauvegarde les données de chaque plante individuellement
        // Il faut que la classe Plante ait une méthode GetDataForSerialization() qui retourne un objet de type DataPlante
        TouteNosPlantes = new List<DataPlante>();
        GameObject[] GameObjectPlantes = GameObject.FindGameObjectsWithTag("Plante");
        List<Plante> nosPlantes = new List<Plante>();
        // Possibilité de réduire en effaçant la première boucle for avec nosPlantes = GameObject.FindObjectsOfType<Plante>();
        foreach(GameObject go in GameObjectPlantes) // Récupérer les plantes
        {
                nosPlantes.Add(go.GetComponent<Plante>());
        }
        foreach (Plante plante in nosPlantes)
        {
            DataPlante planteData = plante.GetDataForSerialization();
            TouteNosPlantes.Add(planteData);
        }
    }

    // Réactiver les statistiques des plantes lorsque l'on revient dans le niveau bac à sable et venir recharger les plantes et les statistiques
    public void reactiverPlantes()
    {
        Stats.SetActive(true);
        ChargerDonnees();
    }

    // Désactiver les statistiques des plantes lorsque l'on quitte le niveau bac à sable et supprimer les plantes et les statistiques pour gagner de la performance
    public void desactiverPlantes()
    {
        SauvegarderDonnees();
        Stats.GetComponent<MaitriseUser>().removeStats();
        Stats.SetActive(false);
        GameObject[] GameObjectPlantes = GameObject.FindGameObjectsWithTag("Plante");
        foreach(GameObject go in GameObjectPlantes)
        {
            // On doit d'abord détruire le script puis ensuite le gameObject associé
            Plante Script = go.GetComponent<Plante>();
            Destroy(Script); 
            Destroy(go);
        }
        supprimerToutesOptions();
    }

    // Prépare les statistiques à être sauvegardées
    public void SauvegarderStatistiques()
    {
        MaitriseUser Statistiques = Object.FindObjectOfType<MaitriseUser>();
        entretienPlantes = Statistiques.GetDataForSerialization();
    }

    //Recharger les prefabs les plantes et les statistiques
    public void ChargerDonnees()
    {
        string path = Application.dataPath;
        string pathPrefabs = path + "/prefabs.json";
        if (File.Exists(pathPrefabs)){
            string dataPrefabsJson = File.ReadAllText(pathPrefabs);
            ObjetSauvegardePrefabs = JsonUtility.FromJson<DataPrefab>(dataPrefabsJson);
        }
        else{
            Debug.LogWarning("Aucune prefabs à charger.");
        }
        ChargerPrefabs();

        string pathPlantes = path + "/plantes.json";
        if (File.Exists(pathPlantes)){
            string dataPlantesJson = File.ReadAllText(pathPlantes);
            PlantesList = JsonUtility.FromJson<DataPlanteList>(dataPlantesJson);
            TouteNosPlantes = PlantesList.TouteNosPlantes;
            newID = PlantesList.newID;
        }
        else{
            Debug.LogWarning("Aucune donnée de plante à charger.");
        }
        ChargerPlantes();
        // Charger les autres données
        string pathStats = path + "/statistiquesEntretien.json";
        if (File.Exists(pathStats)){
            string dataEntretienJson = File.ReadAllText(pathStats);
            DataMaitriseUser entretienPlantes = JsonUtility.FromJson<DataMaitriseUser>(dataEntretienJson);
            MaitriseUser Statistiques = Object.FindObjectOfType<MaitriseUser>();
            Statistiques.LoadDataFromSerialization(entretienPlantes);
        }
        else{
            Debug.LogWarning("Aucune statistique de plante à charger.");
        }
    }

    //Recharger nos plantes dans la scène
    public void ChargerPlantes()
    {
        if (TouteNosPlantes.Count != 0)
        {
            foreach (DataPlante planteDonnees in TouteNosPlantes) // Pour chaque donnée de plante sauvegardée
            {
                    string prefab = planteDonnees.prefab;
                    if(ListPrefabs.Contains(prefab)) // Si le prefab est présent dans notre liste existante
                    {
                        GameObject modelePlante = (GameObject) Resources.Load("Prefabs/"+prefab);
                        if (modelePlante != null) 
                        {
                            // Création d'un clone du prefab (espèce) et ajout du script en lui injectant les données sauvegardées
                            GameObject gameObjectPlante = Instantiate(modelePlante,planteDonnees.position, Quaternion.identity);
                            Plante scriptPlante = gameObjectPlante.GetComponent<Plante>();
                            AddNewOption(prefab,planteDonnees.id);
                            scriptPlante.LoadDataFromSerialization(planteDonnees);
                            // Le prefab est retrouvé, ensuite un GameObject est créé à partir du prefab
                            // Puis on modifie le script plante avec les données de son DataPlante associé grâce au LoadDataFromSerialization
                        }
                    }
                    else{ 
                        GameObject modelePlante = (GameObject) Resources.Load("Prefabs/Plante Lambda"); // Plante par défaut remplace le prefab si le prefab demandé n'est pas trouvé
                        GameObject gameObjectPlante = Instantiate(modelePlante, planteDonnees.position, Quaternion.identity);
                        Plante scriptPlante = gameObjectPlante.GetComponent<Plante>();
                        AddNewOption("Plante Lambda",planteDonnees.id);
                        scriptPlante.LoadDataFromSerialization(planteDonnees);
                        Debug.LogWarning("Plante Lambda  Chargée.");
                    }
            }
        }
        else
        {
            Debug.LogWarning("Aucune plante à charger.");
        }
    }

    // Pour ajouter un prefab, très peu utile
    public void AjouterPrefab(string nom)
    {
        if(!ListPrefabs.Contains(nom))
        {
            ListPrefabs.Add(nom);
        }
    }

    // Permet de recharger la liste des prefabs sauvegardée
    public void ChargerPrefabs()
    {
        ListPrefabs = ObjetSauvegardePrefabs.ListPrefabs;
    }

    // Ajoute une plante grâce au menu de gestion des plantes
    public void AjouterPlante()
    {
        int nb = DropdownAjout.value; // On reprend la valeur de la liste donc la plante à ajouter
        GameObject modelePlante = (GameObject) Resources.Load("Prefabs/"+ListPrefabs[nb]); // On charge le prefab à cloner
        GameObject gameObjectPlante = Instantiate(modelePlante,new Vector3(-2.9f,1.1f,6.45f), Quaternion.identity); // On le clone
        Plante scriptPlante = gameObjectPlante.GetComponent<Plante>();
        scriptPlante.id = newID;
        AddNewOption(ListPrefabs[nb],newID);
        MaitriseUser Statistiques = Object.FindObjectOfType<MaitriseUser>();
        Statistiques.ajouterStatsPlantes(newID, scriptPlante);
        newID++;
    }

    // Suppression d'une plante grâce au menu de gestion des plantes
    public void SupprimerPlante()
    {
        int index = DropdownSuppression.value;
        string idString = DropdownSuppression.options[index].text;
        int id = int.Parse(Regex.Replace(idString, "[^0-9]", "")); // On récupère le string prefab : id donc on met une expression régulière pour avoir uniquement id
        GameObject[] PlantesGO = GameObject.FindGameObjectsWithTag("Plante");
        foreach(GameObject plante in PlantesGO) // Parcourt les plantes
        {
            if (plante.GetComponent<Plante>().id == id) // Cherche si correspond à l'id de la plante à supprimer
            {
                Destroy(plante);
            }
        }
        RemoveOption();
        MaitriseUser Statistiques = Object.FindObjectOfType<MaitriseUser>();
        Statistiques.SupprimerStatsPlantes(id);
    }

    // On a ajouté une plante (Au début ou pendant que l'app tourne) et donc on rajoute dans la liste de suppression la plante ajoutée
    private void AddNewOption(string prefab ,int id)
    {
        DropdownSuppression.options.Add(new TMP_Dropdown.OptionData(prefab + " :"+id));
        DropdownSuppression.RefreshShownValue();
    }

    // On a supprimé une plante donc on veut la supprimer de la liste des plantes existantes
    private void RemoveOption()
    {
        DropdownSuppression.options.RemoveAt(DropdownSuppression.value);
        DropdownSuppression.value = 0;
        DropdownSuppression.RefreshShownValue();
    }

    // Supprime toutes les options de la liste de suppression des plantes (Lorsque l'on sort du bac à sable)
    private void supprimerToutesOptions()
    {
        DropdownSuppression.ClearOptions();
    }
}