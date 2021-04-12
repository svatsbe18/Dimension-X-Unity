using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Class for Moving the Tracks
/// </summary>
public class TrackMover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float newX = transform.position.x - GameManager.gm.trackMoveSpeed * Time.deltaTime;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        if(transform.position.x<=-50)
        {
            GameManager.gm.TrackGenerator(100, 0);
            Destroy(this.gameObject);
        }

        if(GameManager.gm.destroyTracks)
        {
            Destroy(this.gameObject);
        }
    }
}
