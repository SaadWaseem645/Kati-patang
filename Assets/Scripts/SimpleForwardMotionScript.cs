using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleForwardMotionScript : MonoBehaviour
{

    [SerializeField]
    private float speed = 10;

    private float destructionDistance = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World);
        if (transform.position.z >= destructionDistance)
            Destroy(transform.gameObject);
    }

    public void setDestructionDistance(float destructionDistance)
    {
        this.destructionDistance = transform.position.z + destructionDistance;
    }
}
