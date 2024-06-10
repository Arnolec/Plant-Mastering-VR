using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    DataPlanteList permet de sauvegarder la liste des données des plantes, est utilisé car JsonUtility ne permet pas de sauvegarder directement une liste
*/
[System.Serializable]
public class DataPlanteList
{
    public List<DataPlante> TouteNosPlantes = new List<DataPlante>();
    public int newID;

    public DataPlanteList(List<DataPlante> plantes, int id)
    {
        TouteNosPlantes = plantes;
        newID = id;
    }

}
