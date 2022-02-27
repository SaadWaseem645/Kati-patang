using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallKiteScript : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private PlayerScript playerScript;

    [SerializeField]
    private float fastSpeed;
    [SerializeField]
    private float playerSpeed;
    private float speed;
    [SerializeField]
    private float playerDistance;
    [SerializeField]
    private float floatDownPosition;

    [SerializeField]
    private float rotationSpeed;

    private bool stopped = false;
    private string motionDirection = "forward";

    void Start()
    {
        playerScript = player.GetComponent<PlayerScript>();
        speed = fastSpeed;
        InvokeRepeating("rotateKite", 2.0f, 1.0f/60.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position.z - player.transform.position.z) >= playerDistance)
            speed = playerSpeed;
        else
            speed = fastSpeed;

        if (!stopped && playerScript.isGameStarted())
        {
            if (motionDirection.Equals("forward"))
                transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World);
            else if (motionDirection.Equals("up"))
                transform.Translate(Vector3.up * Time.deltaTime * speed, Space.World);
        }

        if (stopped && transform.position.y > player.transform.position.y + floatDownPosition)
            transform.Translate(-Vector3.up * Time.deltaTime * playerSpeed, Space.World);
    }

    void rotateKite()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag){
            case "KiteLander":
                speed = 0;
                stopped = true;
                break;
            case "KiteLanderForward":
                motionDirection = "forward";
                break;
            case "KiteLanderUp":
                motionDirection = "up";
                break;
        }
    }

}
