using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;

    public bool generateObstacles = true;

    public int minObstacles;
    public int maxObstacles;

    // Start is called before the first frame update
    void Start()
    {
        minObstacles = GameManager.gm.MinObstaclesPerTrack;
        maxObstacles = GameManager.gm.MaxObstaclesPerTrack;
        if (generateObstacles)
        {
            for(int i=0;i<Random.Range(minObstacles,maxObstacles); i++)
            {
                int x = Random.Range(-20, 20);
                int z = Random.Range(-12, 12);
                GameObject obstacle = Instantiate(obstaclePrefab);
                obstacle.transform.parent = this.transform;
                obstacle.transform.localPosition = new Vector3(x, 0, z);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
