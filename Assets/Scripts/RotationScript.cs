using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    [SerializeReference]
    private float speed;
    [SerializeReference]
    private bool X = false;
    [SerializeReference]
    private bool Y = true;
    [SerializeReference]
    private bool Z = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(X ? speed : 0f, Y ? speed : 0f, Z ? speed : 0f, Space.Self);
    }
}
