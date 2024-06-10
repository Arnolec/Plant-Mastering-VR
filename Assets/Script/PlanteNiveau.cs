using UnityEngine;
using System;

/*
    PlanteNiveau : Plante qui ne sera pas sauvegardée et qui est présente dans les niveau, 
    C'est un script hérité de Plante.cs, on peut désactiver son eau ou sa lumière et pour réussir le niveau sa condition doit être remplie
    
    Attributs de la classe :
    eauInactif : permet de désactiver l'eau d'une plante
    lumiereInactif : permet de désactiver la lumière d'une plante
    condition : La condition nécessaire pour réussir le niveau actuel de la plante
    idNiveau : Le niveauActuel de la plante

*/

public class PlanteNiveau : Plante
{
    public bool eauInactif;
    public bool lumiereInactif;
    public string condition;
    private int idNiveau;

    public PlanteNiveau()
    {
        eauInactif = true;
        lumiereInactif = true;
    }

    //  Initialiser correctement la plante
    public override void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Plante plante = this.transform.gameObject.GetComponent<Plante>();
        this.eauPlante = plante.getEauPlante();
        Destroy(plante);
        this.emojiPlante = new GestionEmoji();
        emojiPlante.instantierWarning(this);
        this.lumierePlante = new GestionLumiere(this);
        gameObjectParent = this.transform.gameObject;
        this.n = 0;
    }

    // Méthode bool qui retourne la santé de la plante concernant l'eau, si on l'ignore alors on active eauInactif et donc retourne true
    public override bool santeEau() 
    {
        return (eauInactif || eauPlante.Sante());
    }

    // Méthode bool qui retourne la santé de la plante concernant la lumière, si on l'ignore alors on active eauInactif et donc retourne true
    public override bool santeLumiere()
    {
        return (lumiereInactif || lumierePlante.Sante());
    }

    // Option plus clean : Remplacer condition par une liste de conditions possible
    // et donc parcourir la liste des conditions avec le switch pour s'assurer une a une que les conditions sont remplies (Pas eu le temps pour, mais ici on avait uniquement ces 4 conditions)
    public bool conditionRemplie()
    {
        switch (condition)
        {
            case "this.santeEau()":
                return this.santeEau();
            case "this.santeLumiere()":
                return this.santeLumiere();
            case "this.santeLumiere() && this.santeEau() && this.humidite == 0":
                return this.santeLumiere() && this.santeEau() && this.humidite == 0;
            case "this.santeLumiere() && this.santeEau()":
                return this.santeLumiere() && this.santeEau();
            default:
                return true;
        }
    }
}