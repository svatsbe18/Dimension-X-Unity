using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    int highScore;

    public PlayerData(int score)
    {
        highScore = score;
    }

    public int HighScore
    {
        get { return highScore; }
        set { highScore = value; }
    }
}
