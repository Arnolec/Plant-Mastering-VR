using System.Collections;
using System.Collections.Generic;

/*
    DataMaitriseUser : Classe utile pour sauvegarder les statistiques de l'utilisateur dans un fichier lisible par un enseignant potentiel

    On doit ici transformer le dictionnaire en liste pour la sauvegarde car le dictionnaire n'est pas pris en compte par la librairie de sauvegarde

    Attributs de la classe:

    dataEntretien : Liste qui contient les statistiques individuelles de chaque plante
    pourcentageEntretienGlobal : Donne un indicateur sur l'entretien global des plantes 
    pourcentageEntretienEau : Donne un indicateur sur l'entretien global lié à l'eau des plantes
    pourcentageEntretienLumière : Donne un indicateur sur l'entretien global lié à la lumière des plantes
*/

[System.Serializable]
public class DataMaitriseUser 
{
    public List<DataEntretien> dataEntretien;

    public float pourcentageEntretienGlobal;
    public float pourcentageEntretienEau;
    public float pourcentageEntretienLumiere;

    public DataMaitriseUser(Dictionary<int,DataEntretien> dict, float global, float eau, float lumiere)
    {
        dataEntretien = new List<DataEntretien>(dict.Values);
        pourcentageEntretienGlobal = global;
        pourcentageEntretienEau = eau;
        pourcentageEntretienLumiere = lumiere;
    }
}