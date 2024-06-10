using UnityEngine;
using UnityEngine.UI;

/*
    Script GestionEmoji : Sous script rattaché au script Plante

    L'objectif de GestionEmoji est de gérer l'affichage ou non des indicateurs en fonction de la difficulté et de la santé de la plante

    Attributs :

    - infosPlante : Le script plante qui est rattaché à celui ci, utile pour l'initialisation et pour avoir des informations sur la santé de la plante
    - Reservoir : GameObject complet du réservoir d'eau
    - warningEau : GameObject du warning (cercle rouge) eau
    - SREmojiGlobal : Gestion de l'affichage du warning Eau

    - emojiGlobal : GameObject de l'emoji global de santé
    - warningLumiere : GameObject utilisé pour dire si la plante ne va pas bien en lumière
    - AffichageGlobalBien : Affichage bonne santé pour emojiGlobal
    - AffichageGlobalMauvais : Affichage mauvaise santé pour emojiGlobal
    - SREmojiGlobal : Gestion de l'affichage de l'emoji Global 
    - SRWarningEau : Gestion de l'affichage du warning d'eau
    - SRWarningLumiere : Gestion de l'affichage du warning de lumière

    - SliderDifficulte : Slider de difficulté (unique dans la scène), l'affichage a besoin de sa valeur de difficulté
    - difficulteFrameAvant : Sauvegarde la difficulté d'avant, permet d'actualiser les indicateurs si la difficulté a changé entre temps

    - changementEmojiGlobal/changementWarningEau/changementWarningLumiere -> utiles dans le cas où difficulté =1 , permet de les actualiser seulement à un changement de santé de la plante
*/

public class GestionEmoji
{
    private Plante infosPlante;
    private GameObject Reservoir;
    private GameObject warningEau;
    private GameObject emojiGlobal;
    private GameObject warningLumiere;
    private Slider SliderDifficulte;
    private Sprite affichageGlobalBien;
    private Sprite affichageGlobalMauvais;
    private SpriteRenderer SRWarningLumiere;
    private SpriteRenderer SREmojiGlobal;
    private SpriteRenderer SRWarningEau;
    private int difficulteFrameAvant;
    private int changementEmojiGlobal; 
    private int changementWarningEau;
    private int changementWarningLumiere;


    /*
        Todo -> Positionner à la création de la plante les Emojis au bon endroits
    */


    // Méthode initialisant au lancement du programme tous les indicateurs nécessaire au bon fonctionnement de ce script
    public void instantierWarning(Plante notrePlante) // A la création d'une plante on crée les emojis qui vont avec
    {
        difficulteFrameAvant = 4;
        changementEmojiGlobal = -1;
        changementWarningEau = -1;
        changementWarningLumiere = -1;

        // On réutilise la méthode GetDataForSerialization qui renvoie un DataPlante à partir de la plante.
        Reservoir = notrePlante.gameObject.transform.GetChild(0).gameObject;
        emojiGlobal = notrePlante.gameObject.transform.GetChild(1).gameObject;
        warningEau = notrePlante.gameObject.transform.GetChild(2).gameObject;
        warningLumiere = notrePlante.gameObject.transform.GetChild(3).gameObject;
        

        infosPlante = notrePlante;
        SliderDifficulte = GameObject.FindWithTag("SliderDiff").GetComponent<Slider>();

        SRWarningEau = warningEau.gameObject.GetComponent<SpriteRenderer>();
        SRWarningLumiere = warningLumiere.gameObject.GetComponent<SpriteRenderer>();
        SREmojiGlobal = emojiGlobal.gameObject.GetComponent<SpriteRenderer>();

        // Resources.Load<Sprite> est une méthode utilisée dans Unity pour charger un objet Sprite 
        // à partir du dossier Resources.
        affichageGlobalBien = Resources.Load<Sprite>("emojiGlobalBien");
        affichageGlobalMauvais = Resources.Load<Sprite>("emojiGlobalMauvais");
        SRWarningEau.sprite = Resources.Load<Sprite>("manqueEau");
        SRWarningLumiere.sprite = Resources.Load<Sprite>("lumiereWarning");
    }


    
    // Méthode mettant à jour tous les warnings/emojis de la plante, manque le cas pour la lumiere avec afficherWarningLumiere
    public void miseAJourEmojis() 
    {
        if(SliderDifficulte.value != difficulteFrameAvant) // Utile pour simplement mettre à jour si la difficulté n'a pas évolué
        {
            difficulteFrameAvant = (int) SliderDifficulte.value;
            switch(SliderDifficulte.value)
            {
                case 1: 
                    afficherReservoir(true);
                    afficherWarningEau(true);
                    afficherWarningLumiere(true);
                    afficherEmojiGlobal(true);
                    miseAJourReservoirEau(); // Si la difficulté a changé entre deux appels, le réservoir doit être affiché et la quantité d'eau être mise à jour
                    break;
                case 2:
                    afficherReservoir(true);
                    afficherWarningEau(false);
                    afficherWarningLumiere(false);
                    afficherEmojiGlobal(false);
                    miseAJourReservoirEau();  // Si la difficulté a changé entre deux appels, le réservoir doit être affiché et la quantité d'eau être mise à jour
                    break;
                case 3:
                    afficherReservoir(false);
                    afficherWarningEau(false);
                    afficherWarningLumiere(false);
                    afficherEmojiGlobal(false);
                    break;
                default:
                    Debug.LogError("Valeur de Slider non prise en compte");
                    break;
            }
        }
        else
        {
            if(SliderDifficulte.value < 3) // Si la difficulté n'a pas changé entre deux miseAJourEmojis on remet juste à jour la quantité d'eau si affichée
            {
                miseAJourReservoirEau();
            }
            if(SliderDifficulte.value == 1) // Si la difficulté n'a pas changé entre deux miseAJourEmojis on remet à jour les warnings et emojis
            {
                majEmojiGlobal();
                majWarningEau();
                majWarningLumiere();
            }
        }
    }
    private void afficherWarningLumiere(bool affichage) // Méthode permettant de gérer l'affichage du warning manque ou surplus de lumière
    {
        if (affichage == false)
        {
            SRWarningLumiere.enabled = false;
        }
        else
        {
            majWarningLumiere();
        }
    }
    private void majWarningLumiere() // Méthode qui permet l'affichage du warning de lumière
    {
        if(changementWarningLumiere != 1 && infosPlante.santeLumiere())
        {
            SRWarningLumiere.enabled = false;
            changementWarningLumiere = 1;
        }
        else if (changementWarningLumiere != 0 && !infosPlante.santeLumiere())
        {
            SRWarningLumiere.enabled = true;
            changementWarningLumiere = 0;
        }
    }
    private void afficherReservoir(bool affichage) // Méthode qui gère l'affichage du réservoir d'eau
    {
        Reservoir.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = affichage;
        Reservoir.gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().enabled = affichage;
        Reservoir.gameObject.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().enabled = affichage;
        Reservoir.gameObject.transform.GetChild(3).gameObject.GetComponent<MeshRenderer>().enabled = affichage;
    }

    private void miseAJourReservoirEau() // Méthode qui permet la mise à jour du réservoir d'eau, est appelée simplement si le réservoir est censé être visible
    {
            Reservoir.gameObject.transform.GetChild(3).gameObject.transform.localScale = new Vector3(0.04f,0.002f+0.05f*infosPlante.getPourcentageEau(),0.04f);
            Reservoir.gameObject.transform.GetChild(3).transform.localPosition = new Vector3(0.00f,-0.05f+0.05f*infosPlante.getPourcentageEau(),0.00f);
    }

    private void afficherEmojiGlobal(bool affichage) // Méthode permettant de gérer l'affichage de l'Emoji Global
    {

        if (affichage == false)
        {
            SREmojiGlobal.enabled = false;
        }
        else
        {
            SREmojiGlobal.enabled = true;
            majEmojiGlobal();
        }
    }

    private void majEmojiGlobal() // Méthode permettant l'affichage de l'emoji de santé global
    {
        if(changementEmojiGlobal != 1 && infosPlante.santeGlobale())
        {
            SREmojiGlobal.sprite = affichageGlobalBien;
            changementEmojiGlobal = 1;
        }
        else if (changementEmojiGlobal != 0 && !infosPlante.santeGlobale())
        {
            SREmojiGlobal.sprite = affichageGlobalMauvais;
            changementEmojiGlobal = 0;
        }
    }

    private void afficherWarningEau(bool affichage) // Méthode permettant de gérer l'affichage du warning manque ou surplus d'eau
    {
        if (affichage == false)
        {
            SRWarningEau.enabled = false;
        }
        else
        {
            majWarningEau();
        }
    }

    private void majWarningEau()  // Mise à jour du warning d'eau
    {
            if(changementWarningEau != 1 && infosPlante.santeEau())
            {
                SRWarningEau.enabled = false;
                changementWarningEau = 1;
            }
            else if (changementWarningEau != 0 && !infosPlante.santeEau())
            {
                SRWarningEau.enabled = true;
                changementWarningEau = 0;
            }
    }
}