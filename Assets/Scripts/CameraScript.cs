using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    [SerializeField]
    private GameObject player;

    private Vector3 offsetPosition;

    [SerializeReference]
    private float cameraPositionMultiplier = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        offsetPosition = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)
            transform.position = (new Vector3(player.transform.position.x * cameraPositionMultiplier, player.transform.position.y * 1f, player.transform.position.z * 1f)) + offsetPosition;
        //transform.position = player.transform.position + offsetPosition;
    }
}
