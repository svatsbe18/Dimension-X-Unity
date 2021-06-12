using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// For Managing the UI of the game
/// </summary>
public class UIManager : MonoBehaviour
{
    [Tooltip("The Text component that shows the current score of the player")]
    [SerializeField] TextMeshProUGUI scoreText;

    [Tooltip("The Text component that show the high score of the player")]
    [SerializeField] TextMeshProUGUI highScoreText;

    [Tooltip("The slider component to show the remaining power of the Slow Motion Effect")]
    [SerializeField] Slider slowMotionBar;

    [Tooltip("The slider component to show the remaining power of the Phase Through Effect")]
    [SerializeField] Slider phaseThroughBar;

    [SerializeField] GameObject bullet;

    [Tooltip("The Panel that drops off when the game is over")]
    [SerializeField] GameObject gameOverPanel;

    [SerializeField] GameObject pauseMenu;

    [SerializeField] Text gameOverScore;
    [SerializeField] Text gameOverHighScore;

    [SerializeField] GameObject optionsPanel;

    int highScore;
    int startTime;
    int score;

    Animator phaseThroughAnimator;
    Animator slowMotionAnimator;

    bool playedSlowMotion = false;
    bool playedPhaseThrough = false;

    void Start()
    {
        startTime = (int)Time.time;

        //Loading the initial data of the player
        PlayerData data = SaveSystem.LoadPlayer();

        if (data == null)
        {
            highScore = 0;
        }
        else
        {
            highScore = data.HighScore;
        }
        highScoreText.text = "High Score : " + highScore;

        scoreText.gameObject.SetActive(true);
        highScoreText.gameObject.SetActive(true);
        gameOverPanel.SetActive(false);
        optionsPanel.SetActive(false);
        pauseMenu.SetActive(false);

        bullet.SetActive(false);

        phaseThroughAnimator = phaseThroughBar.gameObject.GetComponent<Animator>();
        slowMotionAnimator = slowMotionBar.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.gm.IsGameOver)
        {
            return;
        }

        score = (int)Time.time - startTime - GameManager.gm.TimesPaused * 3;
        if(score<0)
        {
            score = 0;
        }
        scoreText.text = "Score : " + score;

        slowMotionBar.value = 1 - GameManager.gm.SlowMotionPower;
        phaseThroughBar.value = 1 - GameManager.gm.PhaseThroughPower;

        if (slowMotionBar.value == slowMotionBar.maxValue)
        {
            if (!playedSlowMotion)
            {
                slowMotionAnimator.Play("BarComplete");
            }
            playedSlowMotion = true;
        }
        else
        {
            playedSlowMotion = false;
        }

        if (phaseThroughBar.value == phaseThroughBar.maxValue)
        {
            if (!playedPhaseThrough)
            {
                phaseThroughAnimator.Play("BarComplete");
            }
            playedPhaseThrough = true;
        }
        else
        {
            playedPhaseThrough = false;
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Call when the game is over
    /// </summary>
    public void GameOver()
    {
        gameOverPanel.SetActive(true);

        gameOverScore.text = "Score : " + score;
        gameOverHighScore.text = "High Score : " + highScore;

        SaveScore();
    }

    /// <summary>
    /// Call to Save the score of the player after a session
    /// </summary>
    void SaveScore()
    {
        if (score > highScore)
        {
            gameOverHighScore.text = "High Score : " + score;
            SaveSystem.SavePlayer(new PlayerData(score));
        }
    }

    /// <summary>
    /// Handle when the Play Again button is clicked
    /// </summary>
    public void HandlePlayAgainButton()
    {
        AudioManager.instance.ButtonClicked();
        
        score = 0;
        startTime = (int)Time.time;

        PlayerData data = SaveSystem.LoadPlayer();
        highScore = data.HighScore;

        highScoreText.text = "High Score : " + highScore;
        gameOverPanel.SetActive(false);
        bullet.SetActive(false);

        GameManager.gm.PlayAgain();
    }

    public void HandleResumeButton()
    {
        Resume();
        GameManager.gm.Resume();
    }

    public void HandleSlowMotionEffect()
    {
        if (GameManager.gm.IsGameOver)
        {
            return;
        }
        if (GameManager.gm.SlowMotion)
        {
            GameManager.gm.DeactivateSlowMotionEffect();
        }
        else if (!GameManager.gm.PhaseThrough)
        {
            GameManager.gm.ActivateSlowMotionEffect();
        }
    }

    public void HandlePhaseThroughEffect()
    {
        if (GameManager.gm.IsGameOver)
        {
            return;
        }
        if (GameManager.gm.PhaseThrough)
        {
            GameManager.gm.DeactivatePhaseThroughEffect();
        }
        else if (!GameManager.gm.SlowMotion)
        {
            GameManager.gm.ActivatePhaseThroughEffect();
        }
    }

    public void BulletLoaded()
    {
        bullet.SetActive(true);
    }

    public void BulletFired()
    {
        bullet.SetActive(false);
    }

    public void HandleBulletButton()
    {
        GameManager.gm.FireBullet();
    }

    public void HandlePauseButton()
    {
        GameManager.gm.Pause();
    }

    public void HandleOptionsButton()
    {
        AudioManager.instance.ButtonClicked();
        optionsPanel.SetActive(true);
    }

    public void HandleBackButton()
    {
        AudioManager.instance.ButtonClicked();
        optionsPanel.SetActive(false);
    }

    /// <summary>
    /// Handle when the Exit button is clicked
    /// </summary>
    public void HandleExitButton()
    {
        AudioManager.instance.ButtonClicked();
        Invoke("Exit", 0.5f);
    }

    void Exit()
    {
        Application.Quit();
    }
}
