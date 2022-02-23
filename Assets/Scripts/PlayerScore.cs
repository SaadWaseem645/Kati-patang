using UnityEngine;
using UnityEngine.UI;
public class PlayerScore : MonoBehaviour
{
    [SerializeField]
    private GameObject bigKite;


    [SerializeField]
    private Text scoreTxt, KiteTxt, FlyTxt;
    private SmallKiteScript kiteScript;

    [SerializeField]
    private float kiteScaleSize = 0.1f;


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

                scoreTxt.text = score.ToString();

                Vector3 kiteScale = bigKite.transform.localScale;
                bigKite.transform.localScale = new Vector3(kiteScale.x + kiteScaleSize, kiteScale.y + kiteScaleSize, kiteScale.z + kiteScaleSize);
                break;
            case "Projectile":
                lives--;

                break;
        }
    }

    public int getLives()
    {
        return lives;
    }

}
