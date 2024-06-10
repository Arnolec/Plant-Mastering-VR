using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    MaitriseUser : Classe utilisée pour établir les statistiques de l'utilisateur et lui proposer de l'aide s'il est en difficulté

    Attribut de la classe :
    difficulteFacile / Moyenne / Difficile : Ordre de grandeur de la difficulté pas forcément très utile
    nosPlantes : Listes des plantes surlesquelles on établit les statistiques
    dataEntretienMap : Dictionnaire avec l'id des plantes en clé et les statistiques liées à la plante en valeur
    pourcentageEntretienGlobal : Permet de dire si l'utilisateur est à l'aise avec l'entretien global des plantes
    pourcentageEntretienEau : Permet de dire si l'utilisateur est à l'aise avec l'entretien en eau des plantes
    pourcentageEntretienLumière : Permet de dire si l'utilisateur est à l'aise avec l'entretien en lumière des plantes
    frameAction : une unité de temps des statistiques, un peu plus agréable à regarder par la suite les valeur d'entretien des plantes
    tempsDifficulteUtilisateur : Indicateur pour savoir si afficher aide pour aller au guide
    aideActive : Dit si l'aide est active ou non
    aideDifficulte : GameObject d'aide pour indiquer d'aller au guide avec X
    head : Tête du joueur, utilisé pour laisser devant sa tête l'aide s'il galère
*/
public class MaitriseUser : MonoBehaviour
{
    private const float difficulteFacile = 0.3f;
    private const float difficulteMoyenne = 0.5f;
    private const float difficulteDifficile = 0.75f;
    [SerializeField]private List<Plante> nosPlantes;
    [SerializeField]public Dictionary<int,DataEntretien> dataEntretienMap;
    private float pourcentageEntretienGlobal;
    private float pourcentageEntretienEau;
    private float pourcentageEntretienLumiere;
    private int frameAction;
    private int tempsDifficulteUtilisateur;
    private bool aideActive;
    public GameObject aideDifficulte;
    public Transform head;
    public MaitriseUser()
    {
        dataEntretienMap = new Dictionary<int, DataEntretien>();
    }


    public void Start(){
        frameAction = 0;
        tempsDifficulteUtilisateur = 35;
        GameObject[] GameObjectPlantes = GameObject.FindGameObjectsWithTag("Plante");
        nosPlantes = new List<Plante>();
        foreach (GameObject go in GameObjectPlantes)
        {
            nosPlantes.Add(go.GetComponent<Plante>());
        }
        foreach(Plante p in nosPlantes)
        {
            if (!dataEntretienMap.ContainsKey(p.id))
            {
                dataEntretienMap.Add(p.id, new DataEntretien(p.id));
            }

        }
    }

    public void Update()
    {
        if(frameAction == 100)
        {
            foreach(Plante plante in nosPlantes)
            {
                miseAJourEntretien(plante);
            }
            frameAction = 0;
            utilisateurDifficulte();
        }
        if (aideActive)
        {
            majPopupAide();
        }
        frameAction++;
    }

    public void miseAJourEntretien(Plante p) // Méthode appelée dans Plante pour mettre à jour les stats d'entretien
    {
        int idPlante = p.id;
        DataEntretien planteEntretien = dataEntretienMap[idPlante];
        planteEntretien.addTempsVie();
        if (!p.santeEau())
        {
            planteEntretien.addTempsManqueEau();
        }
        else
        {
            planteEntretien.tempsContinuManqueEau = 0;
        }

        if (!p.santeLumiere())
        {
            planteEntretien.addTempsManqueLumiere();
        }
        else
        {
            planteEntretien.tempsContinuManqueLumiere = 0;
        }
        planteEntretien.EtatPlante();
        if(planteEntretien.bonEntretien == false) // Si la plante n'est pas bien entretenue il faut 15 seconde de bon entretien cumulé de tous ses facteurs pour la repasser dans un bon état
        {
            if(p.santeEau() && p.santeLumiere())
            {
                planteEntretien.tempsContinuEntretienCorrect +=1;
            }
            else
            {
                planteEntretien.tempsContinuEntretienCorrect = 0;
            }
            if (planteEntretien.tempsContinuEntretienCorrect >= 15)
            {
                planteEntretien.bonEntretien = true;
                planteEntretien.tempsContinuEntretienCorrect = 0;
            }
            p.fane = true;
        }
        else
        {
            p.fane = false;
        }
    }

    private void utilisateurDifficulte() // Détection de si l'utilisateur est en difficulté ou non et affichage de l'aide 
    {
        SetGlobalDatas();
        if(pourcentageEntretienGlobal < difficulteMoyenne || pourcentageEntretienEau < difficulteMoyenne || pourcentageEntretienLumiere < difficulteMoyenne)
        {
            if (tempsDifficulteUtilisateur >= 45 || aideDifficulte.activeSelf)
            {
                tempsDifficulteUtilisateur = 0;
                aideDifficulte.SetActive(true);
                aideActive = true;
            }
            else
            {
                aideDifficulte.SetActive(false);
                aideActive = false;
            }
        }
        tempsDifficulteUtilisateur++;
    }

    private void majPopupAide() // Mettre à jour l'aide devant l'utilisateur si elle est active, fonctionne pas très bien depuis un merge
    {
        aideDifficulte.transform.position = head.position + new Vector3(head.forward.x,1, head.forward.z).normalized * 2;
        aideDifficulte.transform.LookAt(new Vector3(head.position.x,aideDifficulte.transform.position.y,head.position.z));
        aideDifficulte.transform.forward *=-1;
    }

    public void LoadDataFromSerialization(DataMaitriseUser dtUser) // Charger les données depuis un fichier
    {
        foreach(DataEntretien dt in dtUser.dataEntretien)
        {
            dataEntretienMap.Add(dt.id, dt);
        }
    }

    private void SetGlobalDatas() // Calcul des statistiques globales de l'utilisateur
    {
        float sumGlobal = 0f;
        float sumEau = 0f;
        float sumLumiere = 0f;
        int i = 0; 
        foreach(DataEntretien dt in dataEntretienMap.Values)
        {
            sumGlobal+= dt.EntretienGlobal();
            sumEau += dt.EntretienEau();
            sumLumiere += dt.EntretienLumiere();
            i++;
        }
        pourcentageEntretienGlobal = sumGlobal/i;
        pourcentageEntretienEau = sumEau/i;
        pourcentageEntretienLumiere = sumLumiere/i;
    }

    public DataMaitriseUser GetDataForSerialization() // Préparer les données à Sauvegarder
    {
        SetGlobalDatas();
        return new DataMaitriseUser(dataEntretienMap,pourcentageEntretienGlobal,pourcentageEntretienEau,pourcentageEntretienLumiere);
    }

    public void ajouterStatsPlantes(int id, Plante plante) // Ajouter les statistiques d'une plante
    {
        nosPlantes.Add(plante); // Potentiel oubli rajouté, à enlever si donne erreur
        this.dataEntretienMap.Add(id, new DataEntretien(id));
    }

    public void SupprimerStatsPlantes(int id) // Supprimer les statistiques d'une plante
    {
        this.dataEntretienMap.Remove(id);
        foreach(Plante plante in nosPlantes)
        {
            if(plante.id == id)
            {
                nosPlantes.Remove(plante);
            }
        }
    }

    public void removeStats() // Supprimer les statistiques lorsque l'on quitte le bac à sable pour pas consommer de la mémoire ni du CPU
    {
        nosPlantes = new List<Plante>();
        dataEntretienMap = new Dictionary<int, DataEntretien>();
    }
}
