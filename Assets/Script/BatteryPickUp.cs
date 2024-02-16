using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickUp : MonoBehaviour
{
    // Cette fonction est appelée lorsque l'objet entre en collision avec un autre collider
    private void OnTriggerEnter(Collider other)
    {
        // Vérifier si le joueur est celui qui a entré en collision avec l'objet
        if (other.CompareTag("Player"))
        {
            // Faire disparaître l'objet
            gameObject.SetActive(false); // Ou Destroy(gameObject) si vous voulez détruire l'objet complètement


        }
    }
}