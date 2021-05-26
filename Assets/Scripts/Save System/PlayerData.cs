using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for storing the PlayerData at runtime
/// </summary>
[System.Serializable]
public class PlayerData
{
    int highScore;

    /// <summary>
    /// Constructor of the class
    /// </summary>
    /// <param name="score">The score of the player</param>
    public PlayerData(int score)
    {
        highScore = score;
    }

    /// <summary>
    /// To set and get the high score of the player
    /// </summary>
    public int HighScore
    {
        get { return highScore; }
        set { highScore = value; }
    }
}
