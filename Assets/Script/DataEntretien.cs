using UnityEngine;
/*
    DataEntretien : Classe utile pour avoir les statistiques d'entretien d'une unique plante
    

    Attributs de la classe: 

    tempsVie: Indique depuis combien de temps la plante est présente sur la scène
    tempsManqueEau: Indique la quantité de temps où la plante a manqué d'eau
    tempsManqueLumière: Indique la quantité de temps où la plante a manqué de lumière
    tempsContinuManqueEau: Indique depuis combien de temps la plante est en manque continu d'eau
    tempsContinuManqueLumière: Indique depuis combien de temps la plante est en manque continu de lumière
    tempsContinuEntretienCorrect: Indique la quantité de temps depuis laquelle la plante est en continu bien entretenue si elle était censé faner
    bonEntretien: Utile pour détecter si la plante est censé faner ou non
    id: Utile pour différencier les plantes entre elles
*/

[System.Serializable]
public class DataEntretien
{
    public int tempsVie;
    public int tempsManqueEau;
    public int tempsManqueLumiere;
    public int tempsContinuManqueEau;
    public int tempsContinuManqueLumiere;
    public int tempsContinuEntretienCorrect;
    public bool bonEntretien;
    public int id;

    public DataEntretien()
    {
        tempsVie = 0;
        tempsManqueEau = 0;
        tempsManqueLumiere = 0;
        tempsContinuManqueEau = 0;
        tempsContinuManqueLumiere = 0;
        tempsContinuEntretienCorrect = 0;
    }

    public DataEntretien(int idPlante)
    {
        tempsVie = 0;
        tempsManqueEau = 0;
        tempsManqueLumiere = 0;
        tempsContinuManqueEau = 0;
        tempsContinuManqueLumiere = 0;
        tempsContinuEntretienCorrect = 0;    
        id = idPlante;    
    }

    public DataEntretien(DataEntretien entretien)
    {
        tempsVie = entretien.tempsVie;
        tempsManqueEau = entretien.tempsManqueEau;
        tempsManqueLumiere = entretien.tempsManqueLumiere;
        bonEntretien = entretien.bonEntretien;
        tempsContinuManqueEau = 0;
        tempsContinuManqueLumiere = 0;
        tempsContinuEntretienCorrect = 0;
    }

    public void addTempsVie() // Méthode pour ajouter du temps de vie à la plante
    {
        tempsVie++;
    }

    public void addTempsManqueEau() // Méthode pour ajouter du temps lorsque la plante n'est pas assez hydratée
    {
        tempsManqueEau++;
        tempsContinuManqueEau++;
    }

    public void addTempsManqueLumiere() // Méthode pour ajouter du temps lorsque la plante n'est pas assez ensoleillée
    {
        tempsManqueLumiere++;
        tempsContinuManqueLumiere++;
    }

    public float EntretienGlobal() // Méthode pour avoir un indicateur sur l'entretien global de la plante
    {
        return (EntretienEau()+EntretienLumiere())/2;
    }
    
    public float EntretienEau() // Méthode pour avoir un indicateur sur l'entretien en eau de la plante
    {
        return 1 - ((float)tempsManqueEau)/(tempsVie);
    }
    public float EntretienLumiere() // Méthode pour avoir un indicateur sur l'entretien en lumière de la plante
    {
        return 1 - ((float)tempsManqueLumiere)/(tempsVie);
    }


    // Si le temps cumulé de manque d'eau + temps cumulé lumière dépasse 60 alors le bon entretien est false 
    // Et on peut donc passer la plante comme étant dans l'état fanée
    public void EtatPlante() // Méthode pour avoir un indicateur sur l'état de la plante
    {
        if (tempsContinuManqueLumiere + tempsContinuManqueEau > 60) 
        {
            bonEntretien = false;
        }
        else
        {
            bonEntretien = true;
        }
    }
}