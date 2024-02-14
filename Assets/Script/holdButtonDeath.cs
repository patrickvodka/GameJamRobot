using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class holdButtonDeath : MonoBehaviour
{
    public float holdTimer=0;
    public float holduration = 1f;
    private bool isHoling = false;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        if (isHoling)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer>=holduration)
            {
                
            }
            
        }
    }
}
