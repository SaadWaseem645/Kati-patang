using UnityEngine;

public class LeftRightRepeatedMotion : MonoBehaviour
{

    [SerializeField]
    private float maxDistance = 5;
    [SerializeField]
    private float speed = 10;

    private float initialPosition;
    private float left;
    private float right;
    [SerializeField]
    private bool isLeft = false;
    [SerializeField]
    private bool upDown = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!upDown)
        {
            initialPosition = transform.position.x;
            right = (maxDistance / 2);
            left = -right;

            right += initialPosition;
            left += initialPosition;
        }
        else
        {
            initialPosition = transform.position.y;
            right = (maxDistance / 2);
            left = -right;

            right += initialPosition;
            left += initialPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isLeft)
            transform.Translate((upDown ? Vector3.down : Vector3.left) * Time.deltaTime * speed, Space.World);
        else transform.Translate((upDown ? Vector3.up : Vector3.right) * Time.deltaTime * speed, Space.World);

        if (!upDown)
        {
            
            if (transform.position.x >= right)
                isLeft = true;
            else if (transform.position.x <= left)
                isLeft = false;
        }
        else
        {
            if (transform.position.y >= right)
                isLeft = true;
            else if (transform.position.y <= left)
                isLeft = false;
        }
    }
}
