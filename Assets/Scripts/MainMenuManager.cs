using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip buttonClip;

    public void HandlePlayButton()
    {
        source.PlayOneShot(buttonClip);
        Invoke("Play", buttonClip.length);
    }

    void Play()
    {
        SceneManager.LoadScene(1);
    }

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
