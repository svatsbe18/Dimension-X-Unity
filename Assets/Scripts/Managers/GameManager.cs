using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Script for Managing the Game
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// A public static reference to the Game Manager Script
    /// </summary>
    public static GameManager gm;

    [Tooltip("Storing the Prefab of track to spawn at runtime")]
    [SerializeField] GameObject trackPrefab;

    [Tooltip("Provide a Volume Component to add Visual Effects to the Camera")]
    [SerializeField] Volume volume;
    VolumeProfile volumeProfile;

    Vignette vignette;
    WhiteBalance whiteBalance;

    [Header("Slow Motion Effect")]

    /// <summary>
    /// The Vignette Color for the Slow Motion Effect
    /// </summary>
    [Tooltip("Provide a Color of the Vignette for the Slow Motion Effect")]
    [SerializeField] Color slowMotionColor;

    /// <summary>
    /// Shows the Slow Motion Power that has been used
    /// </summary>
    float slowMotionPower;

    bool increaseSlowMotion = false;

    [Tooltip("For How long should the Slow Motion Effect remain Active")]
    [SerializeField] float slowMotionEffectTime = 5f;

    [Tooltip("How long to regenerate the Slow Motion Effect")]
    [SerializeField] float slowMotionRegenerationTime = 20f;

    [Header("Phase Through Effect")]

    /// <summary>
    /// The Vignette Color for the Phase Through Effect
    /// </summary>
    [Tooltip("Provide a Color of the Vignette for the Slow Motion Effect")]
    [SerializeField] Color phaseThroughColor;

    /// <summary>
    /// Shows the Phase Through Power that has been used
    /// </summary>
    float phaseThroughPower;

    bool increasePhaseThrough = false;

    [Tooltip("For How long should the Phase Through Effect remain Active")]
    [SerializeField] float phaseThroughEffectTime = 5f;

    [Tooltip("How long to regenerate the Phase Through Effect")]
    [SerializeField] float phaseThroughRegenerationTime = 20f;

    [Header("Shooting")]

    [Tooltip("Time to reload the gun after shooting")]
    [SerializeField] float reloadTime = 10;

    [Tooltip("The range of the gun fire")]
    [SerializeField] float shootRange = 100;

    [Header("Track Movement Speed")]

    /// <summary>
    /// The Starting Movement Speed of the track
    /// </summary>
    [Tooltip("What should be the Movement Speed of the track")]
    [SerializeField] float startTrackMoveSpeed = 20f;

    /// <summary>
    /// The Current Movement Speed og the Track
    /// </summary>
    float trackMoveSpeed;

    [Tooltip("How much Speed should be incremented after some time")]
    [SerializeField] float speedIncrementer = 2f;

    [Tooltip("After How much time should the speed be incremented")]
    [SerializeField] float timeToIncreaseSpeed = 2f;
    float currentTime;

    [SerializeField] Timer pauseTimer;

    [Header("For the Obstacles")]

    [SerializeField] int minObstaclesPerTrack = 5;
    [SerializeField] int maxObstaclesPerTrack = 10;

    int minObstacles;
    int maxObstacles;

    [SerializeField] int obstacleIncreaser = 2;

    SpaceshipController playerController;

    UIManager ui;

    AudioManager audioManager;

    /// <summary>
    /// Should we destroy the tracks?
    /// </summary>
    bool destroyTracks = false;

    /// <summary>
    /// Is the Phase Through Effect Active?
    /// </summary>
    bool phaseThrough = false;

    /// <summary>
    /// Is the Slow Motion Effect Active?
    /// </summary>
    bool slowMotion = false;

    /// <summary>
    /// Is the game Over?
    /// </summary>
    bool gameOver = false;

    /// <summary>
    /// Is the game paused?
    /// </summary>
    bool paused = false;

    /// <summary>
    /// How many times has the game been paused
    /// </summary>
    int timesPaused = 0;

    public float SlowMotionPower
    {
        get { return slowMotionPower; }
    }

    public float PhaseThroughPower
    {
        get { return phaseThroughPower; }
    }

    public float ReloadTime
    {
        get { return reloadTime; }
    }

    public float ShootRange
    {
        get { return shootRange; }
    }

    public float TrackMoveSpeed
    {
        get { return trackMoveSpeed; }
        set { trackMoveSpeed = value; }
    }

    public int MinObstaclesPerTrack
    {
        get { return minObstacles; }
    }

    public int MaxObstaclesPerTrack
    {
        get { return maxObstacles; }
    }

    public bool DestroyTracks
    {
        get { return destroyTracks; }
    }

    public bool PhaseThrough
    {
        get { return phaseThrough; }
    }

    public bool SlowMotion
    {
        get { return slowMotion; }
    }

    public bool IsGameOver
    {
        get { return gameOver; }
        set { gameOver = value; }
    }

    public bool Paused
    {
        get { return paused; }
        set { paused = value; }
    }

    public int TimesPaused
    {
        get { return timesPaused; }
    }

    void Awake()
    { 
        if(gm==null)
        {
            gm = this;
        }

        if (playerController == null)
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<SpaceshipController>();
        }

        ui = FindObjectOfType<UIManager>();

        audioManager = FindObjectOfType<AudioManager>();

        minObstacles = minObstaclesPerTrack;
        maxObstacles = maxObstaclesPerTrack;
    }

    void Start()
    {
        currentTime = timeToIncreaseSpeed;
        trackMoveSpeed = startTrackMoveSpeed;

        volumeProfile = volume.profile;

        bool result = volumeProfile.TryGet<Vignette>(out vignette);
        if(!result)
        {
            vignette = volumeProfile.Add<Vignette>();
            Debug.LogWarning("Vignette Component was not added, Added a default one");
        }

        result = volumeProfile.TryGet<WhiteBalance>(out whiteBalance);
        if(!result)
        {
            whiteBalance = volumeProfile.Add<WhiteBalance>();
            Debug.LogWarning("WhiteBalance Component was not added, Added a default one");
        }

        slowMotionPower = 1;
        phaseThroughPower = 1;

        increasePhaseThrough = false;
        increaseSlowMotion = false;

        whiteBalance.active = false;
        vignette.active = false;

        timesPaused = 0;
        pauseTimer.Initiate();
        timesPaused++;

        TrackCreator();
    }

    void Update()
    {
        if(gameOver)
        {
            return;
        }

        if(Time.time>=currentTime)
        {
            currentTime += timeToIncreaseSpeed;

            if (!slowMotion && !paused)
            {
                trackMoveSpeed += speedIncrementer;

                playerController.MoveSpeed += speedIncrementer;
            }
        }

        if(increaseSlowMotion)
        {
            slowMotionPower += Time.deltaTime / slowMotionEffectTime;
            if(vignette.intensity.max<=slowMotionPower)
            {
                slowMotionPower = vignette.intensity.max;
                DeactivateSlowMotionEffect();
                audioManager.DeactivateSpecialEffectSound();
            }
                    
            vignette.intensity.value = slowMotionPower;
        }
        else
        {
            slowMotionPower -= Time.deltaTime / slowMotionRegenerationTime;
            if(slowMotionPower<=vignette.intensity.min)
            {
                slowMotionPower = vignette.intensity.min;
            }
                      
            vignette.intensity.value = slowMotionPower;
        }

        if(increasePhaseThrough)
        {
            phaseThroughPower += Time.deltaTime / phaseThroughEffectTime;
            if(vignette.intensity.max<=phaseThroughPower)
            {
                phaseThroughPower = vignette.intensity.max;
                DeactivatePhaseThroughEffect();
                audioManager.DeactivateSpecialEffectSound();
            }

            vignette.intensity.value = phaseThroughPower;
        }
        else
        {
            phaseThroughPower -= Time.deltaTime / phaseThroughRegenerationTime;
            if(phaseThroughPower<=vignette.intensity.min)
            {
                phaseThroughPower = vignette.intensity.min;
            }

            vignette.intensity.value = phaseThroughPower;
        }
    }

    public void BulletLoaded()
    {
        ui.BulletLoaded();
    }

    public void BulletFired()
    {
        ui.BulletFired();
    }

    public void FireBullet()
    {
        playerController.Fire();
    }

    /// <summary>
    /// For Creating the entire track when the game is instantiated
    /// </summary>
    void TrackCreator()
    {
        destroyTracks = false;
        TrackGenerator(0, 0, false);
        TrackGenerator(50, 0);
        TrackGenerator(100, 0);
    }

    /// <summary>
    /// For generating a particular instance of the track
    /// </summary>
    /// <param name="x">x coordinate of the inital location</param>
    /// <param name="y">y coordinate of the initial location</param>
    /// <param name="generateObstacles">Whether obstacles need to be generated in the instance or not</param>
    public void TrackGenerator(int x,int y,bool generateObstacles = true)
    {
        GameObject track = Instantiate(trackPrefab, new Vector3(x, y), Quaternion.identity, this.transform);
        if(generateObstacles==false)
        {
            track.GetComponent<ObstacleGenerator>().generateObstacles = false;
        }
    }

    public void TrackDestroyed()
    {
        minObstacles = minObstacles + obstacleIncreaser;
        maxObstacles = maxObstacles + obstacleIncreaser;
    }

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0;
        ui.Pause();
    }

    public void Resume()
    {
        paused = false;
        Time.timeScale = 1;
        pauseTimer.Initiate();
        ui.Resume();
        timesPaused++;
    }

    /// <summary>
    /// Called when the Game is Over
    /// </summary>
    public void GameOver()
    {
        gameOver = true;

        trackMoveSpeed = 0;
        destroyTracks = true;
        ui.GameOver();
        playerController.enabled = false;

        phaseThrough = false;
        slowMotion = false;

        audioManager.GameOver();

        DeactivateSlowMotionEffect();

        DeactivatePhaseThroughEffect();
    }

    /// <summary>
    /// Called when the player wants to play again
    /// </summary>
    public void PlayAgain()
    {
        gameOver = false;

        minObstacles = minObstaclesPerTrack;
        maxObstacles = maxObstaclesPerTrack;

        Time.timeScale = 1;
        
        trackMoveSpeed = startTrackMoveSpeed;

        slowMotionPower = 1;
        phaseThroughPower = 1;

        playerController.enabled = true;

        increasePhaseThrough = false;
        increaseSlowMotion = false;

        if (whiteBalance != null)
        {
            whiteBalance.active = false;
        }
        if (vignette != null)
        {
            vignette.active = false;
        }

        timesPaused = 0;
        pauseTimer.Initiate();
        timesPaused++;
        
        TrackCreator();
        playerController.PlayAgain();
        audioManager.PlayAgain();
    }

    /// <summary>
    /// Call this Function when you want to Activate the Slow Motion Effect
    /// </summary>
    public void ActivateSlowMotionEffect()
    {
        audioManager.ActivateSpecialEffectSound();

        trackMoveSpeed /= 2;

        whiteBalance.temperature.value = whiteBalance.temperature.max;
        whiteBalance.active = true;

        vignette.color.value = slowMotionColor;

        vignette.intensity.value = slowMotionPower;

        vignette.active = true;
        increaseSlowMotion = true;
        slowMotion = true;
    }

    /// <summary>
    /// Call this Function when you want to Deactivate the Slow Motion Effect
    /// </summary>
    public void DeactivateSlowMotionEffect()
    {
        audioManager.DeactivateSpecialEffectSound();

        trackMoveSpeed *= 2;

        whiteBalance.temperature.value = 0;
        whiteBalance.active = false;

        vignette.active = false;
        increaseSlowMotion = false;
        slowMotion = false;
    }

    /// <summary>
    /// Call this Function to Activate the Phase Through Effect
    /// </summary>
    public void ActivatePhaseThroughEffect()
    {
        audioManager.ActivateSpecialEffectSound();

        whiteBalance.tint.value = whiteBalance.tint.max;
        whiteBalance.active = true;

        vignette.color.value = phaseThroughColor;

        vignette.intensity.value = phaseThroughPower;

        vignette.active = true;
        increasePhaseThrough = true;
        phaseThrough = true;
    }

    /// <summary>
    /// Call this Function to Deactivate the Phase Through Effect
    /// </summary>
    public void DeactivatePhaseThroughEffect()
    {
        audioManager.DeactivateSpecialEffectSound();

        whiteBalance.tint.value = 0;
        whiteBalance.active = false;

        vignette.active = false;
        increasePhaseThrough = false;
        phaseThrough = false;
    }
}
