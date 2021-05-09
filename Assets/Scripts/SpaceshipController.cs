using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
/// Script for controlling the Player (Spaceship)
/// </summary>
public class SpaceshipController : MonoBehaviour
{
    [Tooltip("Set the initial move speed of the spaceship")]
    [SerializeField] float initialMoveSpeed = 5f;
    float moveSpeed;

    [Tooltip("Reference to the exhaust of the spaceship")]
    [SerializeField] GameObject spaceshipExhaust;

    [Tooltip("Reference to the Audio Source of the spaceship")]
    [SerializeField] AudioSource spaceshipAudio;

    [SerializeField] AudioSource backgroundAudio;

    [SerializeField] AudioClip spaceshipSound;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip specialEffectSound;

    /// <summary>
    /// Can the player move right?
    /// </summary>
    bool canMoveRight = true;

    /// <summary>
    /// Can the player move left?
    /// </summary>
    bool canMoveLeft = true;

    bool pause = false;

    /// <summary>
    /// For getting and setting the move speed of the player (spaceship)
    /// </summary>
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    void Start()
    {
        moveSpeed = initialMoveSpeed;
        spaceshipExhaust.SetActive(true);

        if(spaceshipAudio==null)
        {
            spaceshipAudio = GetComponentInChildren<AudioSource>();
        }
        spaceshipAudio.clip = spaceshipSound;
        spaceshipAudio.loop = true;
        spaceshipAudio.Play();
    }

    void Update()
    {
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");  //Getting input for the horizontal movement
        float z = transform.position.z;
       
        //Applying the horizontal movement
        if (horizontal > 0 && canMoveRight)
        {
            z = transform.position.z - horizontal * moveSpeed * Time.deltaTime;
        }
        else if(horizontal < 0 && canMoveLeft)
        {
            z = transform.position.z - horizontal * moveSpeed * Time.deltaTime;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, z);

        //Clamping the horizontal movement
        if(transform.position.z <= -13)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -13);
        }
        if (transform.position.z >= 13)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 13);
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            if(GameManager.gm.slowMotion)
            {
                GameManager.gm.DeactivateSlowMotionEffect();
            }
            else if(!GameManager.gm.phaseThrough)
            {
                GameManager.gm.ActivateSlowMotionEffect();
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if(GameManager.gm.phaseThrough)
            {
                GameManager.gm.DeactivatePhaseThroughEffect();
            }
            else if(!GameManager.gm.slowMotion)
            {
                GameManager.gm.ActivatePhaseThroughEffect();
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(pause)
            {
                pause = false;
                GameManager.gm.Resume();
            }
            else
            {
                pause = true;
                GameManager.gm.Pause();
            }
        }
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

    /// <summary>
    /// This function is called when you want to play again
    /// </summary>
    public void PlayAgain()
    {
        canMoveLeft = true;
        canMoveRight = true;
        moveSpeed = initialMoveSpeed;
        transform.position = new Vector3();
        spaceshipExhaust.SetActive(true);
        spaceshipAudio.clip = spaceshipSound;
        spaceshipAudio.loop = true;
        spaceshipAudio.Play();
    }

    void GameOver()
    {
        GameManager.gm.GameOver();
        spaceshipAudio.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        //To check wether we have collided with an obstacle
        if (other.gameObject.tag == "Obstacle")
        {
            if (!GameManager.gm.phaseThrough)
            {
                this.enabled = false;
                GameManager.gm.gameOver = true;
                GameManager.gm.trackMoveSpeed = 0;
                spaceshipExhaust.SetActive(false);
                spaceshipAudio.Stop();
                spaceshipAudio.PlayOneShot(crashSound);
                Invoke("GameOver", crashSound.length);
            }
        }

        if (other.gameObject.tag == "LeftBorder")
        {
            Debug.Log("LeftBorder");
            canMoveLeft = false;
        }

        if (other.gameObject.tag == "RightBorder")
        {
            Debug.Log("RightBorder");
            canMoveRight = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "LeftBorder")
        {
            Debug.Log("LeftBorder");
            canMoveLeft = true;
        }
        if (other.gameObject.tag == "RightBorder")
        {
            Debug.Log("RightBorder");
            canMoveRight = true;
        }
    }
}
