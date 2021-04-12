using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] Vector3 positionOffset;
    [SerializeField] Vector3 rotationOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = target.position + positionOffset;
        Vector3 eulerAngles = target.eulerAngles + rotationOffset;

        transform.position = position;
        transform.eulerAngles = eulerAngles;
    }
}
