using UnityEngine;

public class PlayerScore : MonoBehaviour
{

    private int lives = 3;

    void Start()
    {
        
    }

    void Update()
    {

        Debug.Log("Lives: " + lives);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Obstacle":
                lives--;
                break;
        }
    }

}
