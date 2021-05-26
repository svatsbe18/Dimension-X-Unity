using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] Vector3 positionOffset;
    [SerializeField] Vector3 rotationOffset;

    [SerializeField] float smoothPositionTime = 5;
    [SerializeField] float smoothRotationTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LateUpdate()
    {
        Vector3 position = target.position + positionOffset;
        Vector3 eulerAngles = target.eulerAngles + rotationOffset;

        transform.position = Vector3.Lerp(transform.position, position, smoothPositionTime * Time.deltaTime);
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, eulerAngles, smoothRotationTime * Time.deltaTime);
    }
}
