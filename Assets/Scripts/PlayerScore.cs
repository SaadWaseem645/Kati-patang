using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField]
    private GameObject bigKite;

    private int lives = 3;
    private int score = 0;

    void Start()
    {
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
                Vector3 kiteScale = bigKite.transform.localScale;
                bigKite.transform.localScale = new Vector3(kiteScale.x + 0.1f, kiteScale.y + 0.1f, kiteScale.z + 0.1f);
                break;
        }
    }

}
