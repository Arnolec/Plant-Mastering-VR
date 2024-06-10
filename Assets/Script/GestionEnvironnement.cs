using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.XRContent.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

/*
    GestionEnvironnement permet de simuler le fonctionnement d'un environnement

    Attributs de la classe :
    humidity : Humidité de l'environnement
    temperature : Température de l'environnement
    triggerCollider : Utilité ? Peut être pour définir la zone de l'environnement
    potentiometreHumidity: Bouton de l'environnement pour changer la valeur d'humidité
    temperatureText : affichage sur le menu de gestion de l'environnement de la température
    humidityText : affichage sur le menu de gestion de l'environnement de l'humidité
    planteInCollider : Listes de toutes les plantes qui appartiennent à l'environnement
*/

public class GestionEnvironnement : MonoBehaviour
{
    public float humidity;
    public int temperature;
    private Collider triggerCollider;
    [SerializeField] private GameObject potentiometreHumidity;

    public TextMeshProUGUI temperatureText;
    public TextMeshProUGUI humidityText;
    private List<Plante> planteInCollider = new List<Plante>();

    // Quand le jeu démarre, la méthode OnTriggerEnter est appelé et initialise avec les premières
    // plantes qui sont déjà dans le collider. Ensuite on update au fur et à mesure les plantes qui
    // entrent et sortent du collider.
    private void Start()
    {
        triggerCollider = GetComponent<Collider>();
        if (temperatureText !=null)
        {
            updateTempEnvironment(temperature);
        }

    }
    public void updateFromKnob(float valeur) // Mettre à jour la valeur d'humidité à partir du bouton de gestion d'environnement
    {
        updateHumidityEnvironment(valeur);
    }
    public void updateEverythingPlante(Plante plante) // Mettre à jour les valeurs d'environnement d'une plante
    {
        updateHumidityPlant(plante);
        updateTempPlant(plante);
    }
    public void updateHumidityEnvironment(float valeur) // Mise à jour de l'humidité de l'environnement
    {
        humidity = valeur;
        humidityText.text = "Humidité: " + (humidity*100).ToString()+ "%";
    }
    public void updateTempEnvironment(int valeur) // Mise à jour de la température de l'environnement
    {
        temperature = valeur;
        temperatureText.text = "Température: " + temperature.ToString() + "°C";
    }

    public void updateHumidityPlant(Plante plante) // Mise à jour de l'humidité de la plante
    {
        plante.setHumidite(humidity);
    }
    private void updateTempPlant(Plante plante) // Mise à jour de la température de la plante
    {
        plante.temperature = temperature;
    }
    private void OnTriggerEnter(Collider other) // Ajoute la plante entrante dans l'environnement
    {
        if (other.gameObject.GetComponent<Plante>() != null)
        {
            Plante plante = other.gameObject.GetComponent<Plante>();
            if (!planteInCollider.Contains(plante))
            {
                planteInCollider.Add(plante);
                updateEverythingPlante(plante);
            }
            else
            {
                Debug.LogError("Erreur gestion environnement, plante déjà présente dans la liste lors de l'entrée");
            }
        }
    }
    private void OnTriggerExit(Collider other) // Enleve la plante de l'environnement si elle n'y appartient plus
    {
        if (other.gameObject.GetComponent<Plante>() != null)
        {
            Plante plante = other.gameObject.GetComponent<Plante>();
            if (planteInCollider.Contains(plante))
            {
                planteInCollider.Remove(plante);
            }
            else
            {
                Debug.LogError("Erreur gestion environnement, plante non présente dans la liste lors de la sortie");
            }
        }
    }
}