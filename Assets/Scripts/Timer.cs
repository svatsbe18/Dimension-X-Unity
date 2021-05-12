using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float time = 3f;

    Text text;

    float stopTime;

    float currentTrackMoveSpeed;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        int remainingTime = (int)(stopTime - Time.time);
        text.text = remainingTime.ToString();
        
        if(Time.time>=stopTime)
        {
            gameObject.SetActive(false);
            GameManager.gm.trackMoveSpeed = currentTrackMoveSpeed;
            GameManager.gm.paused = false;
        }
    }

    public void Initiate()
    {
        gameObject.SetActive(true);
        if (GameManager.gm.trackMoveSpeed != 0)
        {
            currentTrackMoveSpeed = GameManager.gm.trackMoveSpeed;
            GameManager.gm.trackMoveSpeed = 0;
        }
        GameManager.gm.paused = true;
        stopTime = Time.time + time;
        Debug.Log("Timer initiated");
    }
}
