using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    GestionArrosoir permet simplement de gérer l'arrosoir (Surbrillance et Eau qui coule)

    - Arrose plus et est moins précis
    De l'eau coule si on a l'arrosoir dans la main et que l'arrosoir est incliné

    Attribut de la classe :
    arrosoir : GameObject associé à l'arrosoir
    seuilAngle : Angle à partir duquel faire couler de l'eau du bout de l'arrosoir
    particulesEau : GameObjectEnfant de arrosoir qui est un système de particules
    n : Nombre de frame avant d'activer la surbrillance
    isGrabbed : Permet de dire si l'on a dans la main l'arrosoir, si c'est le cas on peut arroser sinon non
    surbrillance : Permet tout simplement la surbrillance 
*/

public class GestionArrosoir : MonoBehaviour
{
    private GameObject arrosoir;
    private const float seuilAngle = 45f;
    private GameObject particulesEau;
    private int n;
    private bool isGrabbed{get;set;}

    private Outline surbrillance;
    // Start is called before the first frame update
    void Start()
    {
        arrosoir = this.gameObject;
        particulesEau = arrosoir.transform.GetChild(0).gameObject;
        isGrabbed = false;
        n = 0;
        //outline_effect correspond à la surbrillance de l'arrosoir et Outline est le nom du script qui le gère.
        surbrillance = arrosoir.GetComponent<Outline>();
        surbrillance.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //toutes les 400 frames on active la surbrillance pour l'utilisateur
        //Debug.Log(n+ " Frames");
        if (!isGrabbed){
            n++;
            if(n % 400 == 0){
                n = 0;
                surbrillance.enabled = true;
            }
        //si il grab l'arrosoir on reset le timer de la surbrillance.
        }else{
            n = 0;
            surbrillance.enabled = false;
        }
            
        

        Quaternion rotation = arrosoir.transform.rotation;
        float angleZ = rotation.eulerAngles.z;
        float angleX = rotation.eulerAngles.x;


        if ((angleZ > seuilAngle && angleZ <360-seuilAngle) || (angleX > seuilAngle && angleX<360-seuilAngle))
        {
            // Activation du système de particules d'eau -> angle correct
            if (particulesEau != null && !particulesEau.activeSelf && isGrabbed) // Condition pour éviter de tout le temps activer
            {
                particulesEau.SetActive(true);
            }
            if (particulesEau != null && particulesEau.activeSelf && !isGrabbed) // Condition pour éviter de tout le temps désactiver
            {
                particulesEau.SetActive(false);
            }
        }
        else
        {
            // Désactivation du système de particules d'eau si angle pas correct
            if (particulesEau != null && particulesEau.activeSelf)
            {
                particulesEau.SetActive(false);
            }
        }
    }

    // Détecter que l'utilisateur a en main l'arrosoir
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag =="PlayerHand")
        {
            isGrabbed = true;
        }
    }

    // Détecter que l'utilisateur n'as plus en main l'arrosoir
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerHand")
        {
            isGrabbed = false;
        }
    }
}
