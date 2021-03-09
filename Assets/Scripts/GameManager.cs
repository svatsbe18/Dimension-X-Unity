using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    [SerializeField] GameObject trackPrefab;
    public float startTrackMoveSpeed = 20f;
    [HideInInspector] public float trackMoveSpeed;
    [SerializeField] float speedIncrementer = 2f;
    [SerializeField] float timeToIncreaseSpeed = 2f;
    float currentTime;

    GameObject player;
    SpaceshipController playerController;

    UIManager ui;

    [HideInInspector] public bool destroyTracks = false;

    // Start is called before the first frame update
    void Awake()
    { 
        if(gm==null)
        {
            gm = this;
        }
    }

    private void Start()
    {
        Time.timeScale = 1;

        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<SpaceshipController>();

        currentTime = timeToIncreaseSpeed;
        trackMoveSpeed = startTrackMoveSpeed;

        ui = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time>=currentTime)
        {
            currentTime += timeToIncreaseSpeed;
            trackMoveSpeed += speedIncrementer;

            playerController.MoveSpeed += speedIncrementer;
        }
    }

    void TrackCreator()
    {
        destroyTracks = false;
        TrackGenerator(0, 0, false);
        TrackGenerator(50, 0);
        TrackGenerator(100, 0);
    }

    public void TrackGenerator(int x,int y,bool generateObstacles = true)
    {
        GameObject track = Instantiate(trackPrefab, new Vector3(x, y), Quaternion.identity, this.transform);
        if(generateObstacles==false)
        {
            track.GetComponent<ObstacleGenerator>().generateObstacles = false;
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        ui.GameOver();
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        trackMoveSpeed = startTrackMoveSpeed;
        destroyTracks = true;
        Invoke("TrackCreator", 0.1f);
        //TrackCreator();
        playerController.PlayAgain();
    }
}
