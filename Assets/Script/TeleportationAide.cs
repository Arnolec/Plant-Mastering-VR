using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

/*
    TeleportationAide permet d'aller devant le guide et de revenir à la position originelle grâce au bouton X de la manette gauche

    Attribut de la classe :
    player : Le joueur (utile pour le téléporter)
    boutonAideReference : Le bouton qu'on utilise pour se téléporter 
    teleportAnchor : L'endroit ou on se téléporte
    AideDifficulte : Aide fournie potentiellement si l'utilisateur est en difficulté
    tp : Pour savoir ou téléporter le joueur, 1 sur le guide et 0 pour revenir à la position sauvegardée
*/

public class TeleportationAide : MonoBehaviour 
{
    public InputActionReference boutonAideReference = null;
    public GameObject teleportAnchor; // Assurez-vous de lier l'ancre de téléportation dans l'inspecteur
    public GameObject player;
    public GameObject AideDifficulte;
    private Vector3 positionSauvegardee;

    private int tp;
    private void Start()
    {
        tp = 0;
    }

    // Effectué au réveil du script (ici le début normalement)
    private void Awake()
    {
        boutonAideReference.action.started += TeleportPlayer;
    }

    // Fonction pour téléporter le joueur
    private void TeleportPlayer(InputAction.CallbackContext context)
    {
        AideDifficulte.SetActive(false);
        if (tp == 0) // Pour se téléporter sur l'aide
        {
            positionSauvegardee = player.transform.position;
            player.transform.position = teleportAnchor.transform.position;
            tp = 1;
        }
        else // Pour revenir à l'endroit d'avant aide
        {
            player.transform.position = positionSauvegardee;
            tp = 0;
        }
    }
}