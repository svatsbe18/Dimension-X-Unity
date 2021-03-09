using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text highScoreText;

    [SerializeField] GameObject gameOverPanel;
    
    int highScore;
    int startScore;
    int score;

    // Start is called before the first frame update
    void Start()
    {
        startScore = (int)Time.time;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score : " + highScore;

        scoreText.gameObject.SetActive(true);
        highScoreText.gameObject.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        score = (int)Time.time - startScore;
        scoreText.text = "Score : " + score;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        SaveScore();
    }

    void SaveScore()
    {
        if(score>highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    public void HandlePlayAgainButton()
    {
        score = 0;
        startScore = (int)Time.time;

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score : " + highScore;
        gameOverPanel.SetActive(false);
        GameManager.gm.PlayAgain();
    }

    public void HandleExitButton()
    {
        SceneManager.LoadScene(0);
    }
}
