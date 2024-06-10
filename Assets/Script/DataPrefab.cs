using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    DataPrefab permet simplement la sauvegarde de la liste des prefabs dans le Appdata dans le fichier prefabs.json
*/

[System.Serializable]
public class DataPrefab
{
    public List<string> ListPrefabs = new List<string>();

    public DataPrefab(List<string> prefabs)
    {
        ListPrefabs = prefabs;
    }
}
