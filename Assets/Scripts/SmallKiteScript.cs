using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallKiteScript : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

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

    void Start()
    {
        speed = fastSpeed;
        InvokeRepeating("rotateKite", 2.0f, 1.0f/60.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position.z - player.transform.position.z) >= playerDistance && !stopped)
            speed = playerSpeed;
        transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World);

        if(stopped && transform.position.y > floatDownPosition)
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
        }
    }

}
