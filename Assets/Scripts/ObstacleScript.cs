using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [SerializeField] float minScale = 2f;
    [SerializeField] float maxScale = 7f;
    [SerializeField] float rotationOffset = 15f;

    [SerializeField] GameObject destructEffect;

    Transform myT;
    Vector3 randomRotation;
    // Start is called before the first frame update
    void Awake()
    {
        myT = transform;
    }

    // Update is called once per frame
    void Start()
    {
        //hi
        Vector3 scale;
        scale.x = Random.Range(minScale, maxScale);
        scale.y = Random.Range(minScale, maxScale);
        scale.z = Random.Range(minScale, maxScale);
        myT.localScale = scale;
        randomRotation.x = Random.Range(-rotationOffset, rotationOffset);
        randomRotation.y= Random.Range(-rotationOffset, rotationOffset);
        randomRotation.z= Random.Range(-rotationOffset, rotationOffset);

    }
     void Update()
    {
        myT.Rotate(randomRotation * Time.deltaTime);
    }

    public void Destruct()
    {
        Instantiate(destructEffect, transform.position, Quaternion.identity, transform.parent);

        Destroy(gameObject);
    }
}
