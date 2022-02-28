using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    private TMPro.TextMeshProUGUI text;
    [SerializeField]
    private TMPro.TextMeshProUGUI liveText;
    [SerializeField]
    private GameObject kiteParticle;

    private int lives = 3;
    private int score = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        text.text = score.ToString();
        liveText.text = lives.ToString();
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
                kiteParticle.GetComponent<ParticleSystem>().Play();
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
            case "2x":
                score *= 2;
                break;
            case "3x":
                score *= 3;
                break;
            case "5x":
                score *= 5;
                break;
            case "7x":
                score *= 7;
                break;
            case "10x":
                score *= 10;
                break;
        }
    }

    public int getLives()
    {
        return lives;
    }


}
