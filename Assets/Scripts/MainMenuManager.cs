using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Slider loadingSlider;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip buttonClip;

    void OnEnable()
    {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        loadingPanel.SetActive(false);
    }

    public void HandlePlayButton()
    {
        source.PlayOneShot(buttonClip);
        mainPanel.SetActive(false);
        loadingPanel.SetActive(true);
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(1);
        while(!loadOperation.isDone)
        {
            loadingSlider.value = loadOperation.progress / 0.9f;

            yield return null;
        }
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
