using UnityEngine;

// Serializable pour que la classe soit sauvegardable en données JSON ! 
// DataPlante est une classe qui permet de sauvegarder les données importantes d'une plante
// Plante est utilisée, et s'appuie sur DataPlante pour sauvegarder ses données.
[System.Serializable]
public class DataPlante {

    public Vector3 position;
    public string prefab;
    public float eauPlante;
    public GestionLumiere lumierePlante;
    public int id;
    public DataPlante(Plante plante)
    {
        position = plante.gameObjectParent.transform.position;
        prefab = plante.prefab;
        eauPlante = plante.getEau();
        lumierePlante = plante.lumierePlante;
        id = plante.id;
    }
}