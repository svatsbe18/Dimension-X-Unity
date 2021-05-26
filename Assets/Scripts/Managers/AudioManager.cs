using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Spaceship Audio")]
    [Tooltip("Reference to the Audio Source of the spaceship")]
    [SerializeField] AudioSource spaceshipAudio;

    [SerializeField] AudioClip spaceshipSound;
        
    [Header("Background Audio")]
    [SerializeField] AudioSource backgroundAudio;
    [SerializeField] AudioClip specialEffectSound;
    [SerializeField] AudioClip backgroundClip;
    [SerializeField] AudioClip gameOverClip;
    [SerializeField] AudioClip buttonClip;

    // Start is called before the first frame update
    void Start()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        spaceshipAudio.clip = spaceshipSound;
        spaceshipAudio.loop = true;
        spaceshipAudio.Play();

        backgroundAudio.clip = backgroundClip;
        backgroundAudio.loop = true;
        backgroundAudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAgain()
    {
        spaceshipAudio.clip = spaceshipSound;
        spaceshipAudio.loop = true;
        spaceshipAudio.Play();

        backgroundAudio.clip = backgroundClip;
        backgroundAudio.loop = true;
        backgroundAudio.Play();
    }

    public void GameOver()
    {
        spaceshipAudio.Stop();

        backgroundAudio.Stop();
        backgroundAudio.clip = gameOverClip;
        backgroundAudio.Play();
    }

    public void ButtonClicked()
    {
        backgroundAudio.PlayOneShot(buttonClip);
    }

    public void ActivateSpecialEffectSound()
    {
        backgroundAudio.Stop();
        spaceshipAudio.Stop();
        spaceshipAudio.clip = specialEffectSound;
        spaceshipAudio.spatialBlend = 0;
        spaceshipAudio.Play();
    }

    public void DeactivateSpecialEffectSound()
    {
        backgroundAudio.Play();
        spaceshipAudio.Stop();
        spaceshipAudio.clip = spaceshipSound;
        spaceshipAudio.spatialBlend = 1;
        spaceshipAudio.Play();
    }
}
