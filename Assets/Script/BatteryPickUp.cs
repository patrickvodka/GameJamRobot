using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickUp : MonoBehaviour
{
    // Cette fonction est appel�e lorsque l'objet entre en collision avec un autre collider
    private void OnTriggerEnter(Collider other)
    {
        // V�rifier si le joueur est celui qui a entr� en collision avec l'objet
        if (other.CompareTag("Player"))
        {
            // Faire dispara�tre l'objet
            gameObject.SetActive(false); // Ou Destroy(gameObject) si vous voulez d�truire l'objet compl�tement


        }
    }
}