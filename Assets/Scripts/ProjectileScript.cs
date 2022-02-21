using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] projectiles;

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "ProjectileSummoner":
                break;
        }
    }
}
