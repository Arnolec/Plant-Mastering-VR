using UnityEngine;
using UnityEngine.UI;

/*
    LightManager : Permet de simuler le soleil

    Attributs de la classe :
    LightSlider : Slider du menu pour dire jour nuit ou entre deux
    autoLight : Checkbox clickable pour mettre le cycle jour nuit en action
    dayLength : Durée d'un jour
    time; Temps écoulé depuis le début du jour
*/

public class LightManager : MonoBehaviour
{
    Light myLight;
    [SerializeField] private Toggle autoLight;
    [SerializeField] private Slider LightSlider;
    [SerializeField] private float dayLength = 60; // Durée d'un jour en secondes

    private float time; // Temps écoulé depuis le début du jour
    void Start()
    {
        myLight = GetComponent<Light>();
    }
    void Update()
    {
        if(autoLight.isOn){
            // Mise à jour du temps
            time = (time + Time.deltaTime) % dayLength;
            
            // Calcul de l'intensité lumineuse : 
            // - Sinus pour avoir une variation périodique entre -1 et 1
            // - time / dayLength : c'est la proportion du temps écoulé dans la journée
            // - x 0.5 + 0.5 pour remttre [0,1]
            // - 2*PI pour avoir en radians
            float intensity = Mathf.Sin(time / dayLength * 2 * Mathf.PI) * 0.5f + 0.5f;
            myLight.intensity = intensity; 
        }else
            myLight.intensity = LightSlider.value;

    }
    public void AutomatiqueLight(bool isOn) // Mettre en automatique le temps si cliqué sur la Checkbox
    {
        autoLight.isOn = isOn;
    }
}
