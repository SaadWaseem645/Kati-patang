using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField]
    private GameObject bigKite;

    private SmallKiteScript kiteScript;

    private int lives = 3;
    private int score = 0;

    void Start()
    {
        kiteScript = bigKite.GetComponent<SmallKiteScript>();
    }

    void Update()
    { 
        Debug.Log("Lives: " + lives);
        Debug.Log("Pickups: " + score);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Obstacle":
                lives--;
                break;
            case "PickupKite":
                score++;
                break;
        }
    }

}
