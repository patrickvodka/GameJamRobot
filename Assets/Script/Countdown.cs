using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    private float elapsedTime;
    public float timeThreshold;
    public bool isTimerOn;
    public Text TimerText;

    

    void Update()
    {
        if (isTimerOn)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= timeThreshold)
            {
                Debug.Log("perdu");
                elapsedTime = 0;
            }
        
            isTimerOn = false;
        }
    }

    void time(float currentTime)
    {
        currentTime = elapsedTime;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        TimerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }



}
