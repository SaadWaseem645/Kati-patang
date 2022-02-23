using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] projectiles;

    [SerializeField]
    private float appearDistance = -5;

    [SerializeField]
    private float verticalOffset = 0f;

    [SerializeField]
    private float destructionDistance = 5f;

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "ProjectileSummoner":
                GameObject projectile = Instantiate(projectiles[Random.Range(0,7)], transform.position + new Vector3(0,verticalOffset,appearDistance), Quaternion.identity);
                SimpleForwardMotionScript script = projectile.GetComponent<SimpleForwardMotionScript>();
                script.setDestructionDistance(destructionDistance);
                break;
        }
    }
}
