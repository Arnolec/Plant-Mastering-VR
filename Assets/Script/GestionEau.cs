using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
    Script GestionEau : Sous script rattaché au script Plante

    L'objectif de GestionEau est de gérer en interne le besoin d'eau de la plante

    Attributs :

    Si l'on a besoin d'informations générique de la plante pour la gestion de l'eau alors on utilisera infosPlante, pas utilisé actuellement
    - infosPlante : Le script plante qui est rattaché à celui ci, utile pour l'initialisation et pour avoir des informations sur la santé de la plante

    - seuilInf : entier définissant la quantité d'eau minimale pour que la plante soit en bonne santé
    - seuilSup : entier définissant la quantité d'eau maximale pour que la plante soit en bonne santé
    - seuilInf : entier définissant la quantité d'eau maximale atteignable pour la plante
    - consommation : float définissant la consommation d'eau de la plante
    - quantiteActuelleEau : float indiquant la quantité d'eau de la plante

*/

[System.Serializable]
public class GestionEau
{

    [SerializeField] private int seuilInf;
    [SerializeField] private int seuilSup;
    [SerializeField] private int seuilMax;
    [SerializeField] private int consommation; // Consommation d'eau par la plante -> 1 Consommation faible, 5 Consommation moyenne, 10 Consommation Importante (Perd de 1 à 10 d'eau par seconde)
    [SerializeField] private float quantiteActuelleEau;
    private float humidite;
    public GestionEau(int inf, int sup, int conso, int quantiteEau, float humi) // Constructeur pour gérer le chargement de données, probablement pas utlisé
    {
        seuilInf = inf;
        seuilSup = sup;
        consommation = conso;
        quantiteActuelleEau = quantiteEau;
        seuilMax = (int) 1.5*seuilSup;
        setHumidite(humi);
    }

    public GestionEau(int inf, int sup, int conso, float humi) // Constructeur pour gérer le chargement de données, probablement pas utlisé
    {
        seuilInf = inf;
        seuilSup = sup;
        consommation = conso;
        seuilMax = (int) 1.5*seuilSup;
        setHumidite(humi);
    }

    public GestionEau() // Valeur par défaut si rien n'existe
    {
        seuilInf = 150;
        seuilSup = 500;
        quantiteActuelleEau = 0;
        consommation = 3;
        seuilMax = (int) (1.5*seuilSup);
        setHumidite(0.0f);
    }


    // Méthode appelée à l'intérieur de Plante pour augmenter sa quantité d'eau 
    public void receptionEau()
    {
        if(quantiteActuelleEau < seuilMax)
        {
            quantiteActuelleEau +=1;
        }
    }


    // Méthode appelée à l'intérieur de Plante pour gérer la consommation d'eau 
    public void consommationEau()
    {
        double logHumi = 0.0f;
        if (humidite <= 0.1f)
        {
            logHumi = -1;
        }
        else
        {
            logHumi = Math.Log(humidite,10);
        }
        switch(consommation)
        {
            case 1:
                quantiteActuelleEau -= (float) -2*humidite+1; // Formule plus adaptée pour une consommation de 1
                break;
            case 2:
                quantiteActuelleEau -= (float) (-2*1.25*humidite+2); // Formule plus adaptée pour une consommation de 2
                break;
            default:
                quantiteActuelleEau -= (float) (-(logHumi-1)*consommation*0.75-1); // Formule adaptée pour le reste
                break;

        }
        if (quantiteActuelleEau < 0)
        {
            quantiteActuelleEau = 0;
        }
    }

    // Méthode appelée dans la plante qui retourne sa quantité d'eau
    public float getEau()
    {
        return quantiteActuelleEau;

    }

    // Méthode utilisée par GestionEmoji pour l'affichage du réservoir
    public float getPourcentage()
    {
        return ((float)quantiteActuelleEau/(float)seuilMax);
    }

    // Méthode retournant la santé de la plante par rapport à l'eau
    public bool Sante()
    {
        return (quantiteActuelleEau > seuilInf && quantiteActuelleEau < seuilSup); 
    }

    // Méthode utilisé pour définir au début, lorsque l'on charge les plante la quantité d'eau
    public void setEau(float Eau)
    {
        if(Eau > seuilMax)
        {
            quantiteActuelleEau = seuilMax;
        }
        else if(Eau < 0)
        {
            quantiteActuelleEau =0;
        }
        else
        {
            quantiteActuelleEau = Eau;
        }
    }


    // Méthode utilisé pour définir l'humidité
    public void setHumidite(float qte)
    {
        if(qte >= 0 && qte <= 1)
        {
            humidite = qte;
        }
    }
}
