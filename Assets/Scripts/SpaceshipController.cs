using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    [SerializeField] float initialMoveSpeed = 5f;
    [SerializeField] float moveSpeed;

    [SerializeField] GameObject spaceshipExhaust;

    bool canMoveRight = true;
    bool canMoveLeft = true;

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = initialMoveSpeed;
        spaceshipExhaust.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal= Input.GetAxis("Horizontal");
        float z = transform.position.z;
        if (Application.isMobilePlatform)
        {
            Vector3 pos = transform.position;
            pos.y = Vector3.Dot(Input.gyro.gravity, Vector3.up) * moveSpeed;
            transform.position = pos;
        }

        if (horizontal > 0 && canMoveRight)
        {
            z = transform.position.z - horizontal * moveSpeed * Time.deltaTime;
        }
        else if(horizontal < 0 && canMoveLeft)
        {
            z = transform.position.z - horizontal * moveSpeed * Time.deltaTime;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, z);

        if(transform.position.z<=-13)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -13);
        }
        if (transform.position.z >= 13)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 13);
        }
    }

    public void PlayAgain()
    {
        moveSpeed = initialMoveSpeed;
        transform.position = new Vector3();
        spaceshipExhaust.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            GameManager.gm.GameOver();
            spaceshipExhaust.SetActive(false);
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
