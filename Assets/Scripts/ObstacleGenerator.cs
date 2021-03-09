using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;

    public bool generateObstacles = true;
    [SerializeField] int minimumObstacles = 10;
    [SerializeField] int maximumObstacles = 20;

    // Start is called before the first frame update
    void Start()
    {
        if(minimumObstacles>maximumObstacles)
        {
            Debug.LogError("Minimum Obstacles variable should be less than maximum");
        }
        if(generateObstacles)
        {
            for(int i=0;i<Random.Range(minimumObstacles,maximumObstacles); i++)
            {
                int x = Random.Range(-20, 20);
                int z = Random.Range(-12, 12);
                GameObject obstacle = Instantiate(obstaclePrefab);//, new Vector3(x, 0, z), Quaternion.identity, this.transform);
                obstacle.transform.parent = this.transform;
                obstacle.transform.localPosition = new Vector3(x, 0, z);
            }
        }
        //Instantiate(obstaclePrefab, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
