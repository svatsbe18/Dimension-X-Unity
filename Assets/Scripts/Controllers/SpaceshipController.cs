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

    [Header("Audio of the ship")]
    [Tooltip("Reference to the Audio Source of the spaceship")]
    [SerializeField] AudioSource spaceshipAudio;

    [SerializeField] AudioClip crashSound;

    /// <summary>
    /// Can the player move right?
    /// </summary>
    bool canMoveRight = true;

    /// <summary>
    /// Can the player move left?
    /// </summary>
    bool canMoveLeft = true;

    Vector3 initialPosition;

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

        initialPosition = transform.position;
    }

    void Update()
    {
        TouchInput();

        HardwareInput();

        ClampPosition();

        SpecialEffectInput();
    }

    void TouchInput()
    {
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");  //Getting input for the horizontal movement
        float z = transform.position.z;

        //Applying the horizontal movement
        if (horizontal > 0 && canMoveRight)
        {
            z = transform.position.z - horizontal * moveSpeed * Time.deltaTime;
        }
        else if (horizontal < 0 && canMoveLeft)
        {
            z = transform.position.z - horizontal * moveSpeed * Time.deltaTime;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }

    void HardwareInput()
    {
        float horizontal = Input.GetAxis("Horizontal");  //Getting input for the horizontal movement
        float z = transform.position.z;

        //Applying the horizontal movement
        if (horizontal > 0 && canMoveRight)
        {
            z = transform.position.z - horizontal * moveSpeed * Time.deltaTime;
        }
        else if (horizontal < 0 && canMoveLeft)
        {
            z = transform.position.z - horizontal * moveSpeed * Time.deltaTime;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, z);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.gm.Paused)
            {
                GameManager.gm.Resume();
            }
            else
            {
                GameManager.gm.Pause();
            }
        }
    }

    void SpecialEffectInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (GameManager.gm.SlowMotion)
            {
                GameManager.gm.DeactivateSlowMotionEffect();
            }
            else if (!GameManager.gm.PhaseThrough)
            {
                GameManager.gm.ActivateSlowMotionEffect();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (GameManager.gm.PhaseThrough)
            {
                GameManager.gm.DeactivatePhaseThroughEffect();
            }
            else if (!GameManager.gm.SlowMotion)
            {
                GameManager.gm.ActivatePhaseThroughEffect();
            }
        }
    }

    void ClampPosition()
    {
        //Clamping the horizontal movement
        if (transform.position.z <= -13)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -13);
        }
        if (transform.position.z >= 13)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 13);
        }
    }

    /// <summary>
    /// This function is called when you want to play again
    /// </summary>
    public void PlayAgain()
    {
        canMoveLeft = true;
        canMoveRight = true;
        moveSpeed = initialMoveSpeed;
        transform.position = initialPosition;
        spaceshipExhaust.SetActive(true);
    }

    void GameOver()
    {
        GameManager.gm.GameOver();
    }

    private void OnTriggerEnter(Collider other)
    {
        //To check wether we have collided with an obstacle
        if (other.gameObject.tag == "Obstacle")
        {
            if (!GameManager.gm.PhaseThrough)
            {
                this.enabled = false;
                GameManager.gm.IsGameOver = true;
                GameManager.gm.TrackMoveSpeed = 0;
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
