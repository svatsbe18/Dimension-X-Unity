using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float time = 3f;

    TextMeshProUGUI text;

    float stopTime;

    float currentTrackMoveSpeed;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        int remainingTime = (int)(stopTime - Time.time);
        text.text = remainingTime.ToString();
        
        if(Time.time>=stopTime)
        {
            gameObject.SetActive(false);
            GameManager.gm.TrackMoveSpeed = currentTrackMoveSpeed;
            GameManager.gm.Paused = false;
        }
    }

    public void Initiate()
    {
        gameObject.SetActive(true);
        if (GameManager.gm.TrackMoveSpeed != 0)
        {
            currentTrackMoveSpeed = GameManager.gm.TrackMoveSpeed;
            GameManager.gm.TrackMoveSpeed = 0;
        }
        GameManager.gm.Paused = true;
        stopTime = Time.time + time;
        Debug.Log("Timer initiated");
    }
}
