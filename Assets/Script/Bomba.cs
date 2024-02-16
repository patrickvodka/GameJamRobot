using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomba : MonoBehaviour
{
    public GameObject exp;
    public float expForce=1000;
    public float radius=5;
   
    public void Explode()
    {
        //Instantiate(exp, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log(nearbyObject);
                rb.AddExplosionForce(expForce, transform.position, radius);
            }
        }
        Destroy(gameObject);
    }
    
}
