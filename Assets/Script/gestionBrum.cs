using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    gestionBrum permet simplement de gérer l'arrosoir (Surbrillance et Eau qui coule)

    - Arrose moins et est plus précis

    De l'eau coule si on a l'arrosoir dans la main et que l'on appuie sur la gachette de derrière 

    Attribut de la classe :
    brum : GameObject associé au brum
    particulesEau : GameObjectEnfant de brum qui est un système de particules
    isGrabbed : Permet de dire si l'on a dans la main l'arrosoir, si c'est le cas on peut arroser sinon non
    isButtonPressed : Permet de dire si l'on a appuyé sur la gachette de derrière
*/

public class gestionBrum : MonoBehaviour
{
    private GameObject brum;
    private GameObject particulesEau;
    private bool isGrabbed;
    private bool isButtonPressed;

    void Start()
    {
        brum = this.gameObject;
        particulesEau = brum.transform.GetChild(0).gameObject;
        isGrabbed = false;
        isButtonPressed = false;
    }

    void Update()
    {
        // Si le brumisateur est tenu et que le bouton est pressé, activer les particules d'eau
        if (isGrabbed)
        {
            if (particulesEau != null && !particulesEau.activeSelf && isButtonPressed) // Condition pour éviter de tout le temps activer
            {
                particulesEau.SetActive(true);
            }
            if (particulesEau != null && particulesEau.activeSelf && !isButtonPressed) // Condition pour éviter de tout le temps désactiver
            {
                particulesEau.SetActive(false);
            }
        }
        else
        {
            if (particulesEau != null && particulesEau.activeSelf)
            {
                particulesEau.SetActive(false);
            }
        }
    }

    // Détecter que l'utilisateur a en main le brumisateur
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerHand")
        {
            isGrabbed = true;
        }
    }

    // Détecter que l'utilisateur n'a plus en main le brumisateur
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerHand")
        {
            isGrabbed = false;
        }
    }

    // Permet de dire que l'on a détecté que le bouton est appuyé (à partir du gameObject sur Unity) 
    public void Appuyer()
    {
        isButtonPressed = true;
    }

    // Permet de dire que l'on a détecté que le bouton n'est plus appuyé (à partir du gameObject sur Unity) 
    public void Desappuyer()
    {
        isButtonPressed = false;
    }
}
