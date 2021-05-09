using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// For Managing the UI of the game
/// </summary>
public class UIManager : MonoBehaviour
{
    [Tooltip("The Text component that shows the current score of the player")]
    [SerializeField] Text scoreText;

    [Tooltip("The Text component that show the high score of the player")]
    [SerializeField] Text highScoreText;

    [Tooltip("The slider component to show the remaining power of the Slow Motion Effect")]
    [SerializeField] Slider slowMotionBar;

    [Tooltip("The slider component to show the remaining power of the Phase Through Effect")]
    [SerializeField] Slider phaseThroughBar;

    [Tooltip("The Panel that drops off when the game is over")]
    [SerializeField] GameObject gameOverPanel;

    [SerializeField] GameObject pauseMenu;

    [SerializeField] Text gameOverScore;
    [SerializeField] Text gameOverHighScore;

    [SerializeField] GameObject optionsPanel;

    [SerializeField] AudioSource source;

    [SerializeField] AudioClip backgroundClip;
    [SerializeField] AudioClip gameOverClip;
    [SerializeField] AudioClip buttonClip;

    int highScore;
    int startScore;
    int score;

    Animator phaseThroughAnimator;
    Animator slowMotionAnimator;

    bool playedSlowMotion = false;
    bool playedPhaseThrough = false;

    void Start()
    {
        startScore = (int)Time.time;

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

        phaseThroughAnimator = phaseThroughBar.gameObject.GetComponent<Animator>();
        slowMotionAnimator = slowMotionBar.gameObject.GetComponent<Animator>();

        source.clip = backgroundClip;
        source.loop = true;
        source.Play();
    }

    void Update()
    {
        if (GameManager.gm.gameOver)
        {
            return;
        }
        score = (int)Time.time - startScore;
        scoreText.text = "Score : " + score;

        slowMotionBar.value = 1 - GameManager.gm.slowMotionPower;
        phaseThroughBar.value = 1 - GameManager.gm.phaseThroughPower;

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

        source.Stop();
        source.clip = gameOverClip;
        source.Play();

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
        source.PlayOneShot(buttonClip);

        score = 0;
        startScore = (int)Time.time;

        PlayerData data = SaveSystem.LoadPlayer();
        highScore = data.HighScore;

        highScoreText.text = "High Score : " + highScore;
        gameOverPanel.SetActive(false);

        source.clip = backgroundClip;
        source.loop = true;
        source.Play();

        GameManager.gm.PlayAgain();
    }

    public void HandleResumeButton()
    {
        Resume();
        GameManager.gm.Resume();
    }

    public void HandleSlowMotionEffect()
    {
        if (GameManager.gm.gameOver)
        {
            return;
        }
        if (GameManager.gm.slowMotion)
        {
            GameManager.gm.DeactivateSlowMotionEffect();
        }
        else if (!GameManager.gm.phaseThrough)
        {
            GameManager.gm.ActivateSlowMotionEffect();
        }
    }

    public void HandlePhaseThroughEffect()
    {
        if (GameManager.gm.gameOver)
        {
            return;
        }
        if (GameManager.gm.phaseThrough)
        {
            GameManager.gm.DeactivatePhaseThroughEffect();
        }
        else if (!GameManager.gm.slowMotion)
        {
            GameManager.gm.ActivatePhaseThroughEffect();
        }
    }

    public void HandlePauseButton()
    {
        GameManager.gm.Pause();
    }

    public void HandleOptionsButton()
    {
        source.PlayOneShot(buttonClip);
        optionsPanel.SetActive(true);
    }

    public void HandleBackButton()
    {
        source.PlayOneShot(buttonClip);
        optionsPanel.SetActive(false);
    }

    /// <summary>
    /// Handle when the Exit button is clicked
    /// </summary>
    public void HandleExitButton()
    {
        source.PlayOneShot(buttonClip);
        Invoke("Exit", buttonClip.length);
    }

    void Exit()
    {
        Application.Quit();
    }
}
