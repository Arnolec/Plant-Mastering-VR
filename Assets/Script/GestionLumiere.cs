using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    GestionLumiere permet de simuler le comportement d'une plante avec la lumière

    Ca a été mal implémenté, c'est de l'instantané, il faudrait sans doute faire la somme des 20 dernières valeurs reçues pour mieux le gérer
    et réécrire dans le tableau en fonction du temps n : [n%20] pour réécrire dans la bonne valeur sans déplacer

    System.Serializable bizarre, pas utile je pense

    Attribut de la classe : 
    myLight : Lumière de la plante
    pourcentageBloquageSoleil : Bloquer un peu la lumière reçu si le rayon reçu par la plante passe par un bloquant
    minLightLevel : Quantité de lumière min sinon manque de lumière
    maxLightLevel : Quantité de lumière max sinon trop de lumière
    planteTransform : Modèle de la plante utilisé pour calculer la lumière reçue
*/
[System.Serializable]
public class GestionLumiere
{

    private Light myLight;
    private float pourcentageBloquageSoleil = 0.5f;
    [SerializeField] private float minLightLevel;
    [SerializeField] private float maxLightLevel;
    private Transform planteTransform; 

    public GestionLumiere(Plante plante)
    {
        // Si les valeurs ne sont pas précisé dans l'inspecteur, 
        // on les initialise à des valeurs par défaut.
        if(minLightLevel == 0)
        {
            minLightLevel = 0.5f;
        }
        if(maxLightLevel == 0)
        {
            maxLightLevel = 1.5f;
        }

        myLight = GameObject.Find("Lumiere").GetComponent<Light>();
        
        // Utile pour récupérer la position de la plante.
        planteTransform = plante.transform;
    }

    public float getLightLevel() // Récupérer l'intensité lumineuse reçue par la plante
    {
        if (myLight != null && planteTransform != null)
        {

            RaycastHit hit;// RaycastHit pour détecter les collisions avec les objets entre la plante et la lumière.

            // Vecteur qui pointe vers la lumière.
            Vector3 directionToLight = (myLight.transform.position - planteTransform.position).normalized;

            // Distance entre la plante et la lumière.
            float distanceToLight = Vector3.Distance(planteTransform.position, myLight.transform.position);

            float lightIntensity = myLight.intensity; // Intensité de la lumière.

            // On vérifie si le rayon de la lumière entre en collision avec un objet qui bloque la lumière.
            if (Physics.Raycast(planteTransform.position, directionToLight, out hit, distanceToLight))
            {
                // Si l'objet qui bloque la lumière est un objet qui bloque la lumière.
                if (hit.collider.gameObject.CompareTag("BloqueLumiere"))
                {
                    
                    // Alors on réduit l'intensité de la lumière.
                    lightIntensity *= pourcentageBloquageSoleil;
                    
                }
            }          
            return lightIntensity;
        }
        Debug.LogError("Erreur : la lumière ou la plante n'a pas été trouvée pour la plante :" + planteTransform.gameObject.name + " !");
        return 0;
    }

    public bool Sante()
    {
        return (getLightLevel() > minLightLevel && getLightLevel() < maxLightLevel);
    }
}
