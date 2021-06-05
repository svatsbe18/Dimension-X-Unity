using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasers : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("Off", 0.5f);
    }

    void Off()
    {
        gameObject.SetActive(false);
    }
}
