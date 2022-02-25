using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpwardSquiggleScript : MonoBehaviour
{
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private float rotationSpeed = 10;
    [SerializeField]
    private float rotationLimit = 15;
    private bool reverse = false;
    [SerializeField]
    private float destroyLimit = 5f;

    private Vector3 startingLocation;

    // Start is called before the first frame update
    void Start()
    {
        startingLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed, Space.World);
        if (transform.localRotation.z <= -rotationLimit)
            reverse = false;
        if (transform.localRotation.z >= rotationLimit)
            reverse = true;

        if (reverse)
            transform.Rotate(0f,0f, -rotationSpeed * Time.deltaTime);
        else
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        if(transform.position.y - startingLocation.y > destroyLimit)
            Destroy(transform.gameObject);
    }
}
