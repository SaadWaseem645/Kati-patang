using UnityEngine;

public class PlayerScore : MonoBehaviour
{

    [SerializeField]
    private GameObject[] reactionsBad;
    [SerializeField]
    private GameObject[] reactionsGood;

    [SerializeField]
    private GameObject bigKite;

    [SerializeField]
    private float kiteScaleSize = 0.1f;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip pointClip;
    [SerializeField]
    private AudioClip obstacleClip;
    [SerializeField]
    private AudioClip projectileClip;

    private int lives = 3;
    private int score = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    { 
        
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Obstacle":
                audioSource.PlayOneShot(obstacleClip);
                GameObject bad = Instantiate(reactionsBad[Random.Range(0, 4)], transform.position + new Vector3(0, 2f, 0), Quaternion.identity);
                bad.transform.parent = transform;
                lives--;
                break;
            case "PickupKite":
                audioSource.PlayOneShot(pointClip);
                score++;
                GameObject good = Instantiate(reactionsGood[0], transform.position + new Vector3(0, 2f, 0), Quaternion.identity);
                good.transform.parent = transform;
                Vector3 kiteScale = bigKite.transform.localScale;
                bigKite.transform.localScale = new Vector3(kiteScale.x + kiteScaleSize, kiteScale.y + kiteScaleSize, kiteScale.z + kiteScaleSize);
                break;
            case "Projectile":
                audioSource.PlayOneShot(projectileClip);
                GameObject bad1 = Instantiate(reactionsBad[Random.Range(0, 4)], transform.position + new Vector3(0, 2f, 0), Quaternion.identity);
                bad1.transform.parent = transform;
                lives--;
                break;
        }
    }

    public int getLives()
    {
        return lives;
    }

}
