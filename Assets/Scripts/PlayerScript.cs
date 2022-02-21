using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class PlayerScript : MonoBehaviour
{

    [SerializeField]
    private float speed;
    [SerializeField]
    private float horizontalSpeed;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float cartJumpHeight;
    [SerializeField]
    private float cartJumpHeightHigh;

    [SerializeField]
    [Range(0f, 1f)]
    private float lerpPct = 0.3f;

    private Vector2 currentPosition;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    private Animator animator;

    private bool stopTouch = false;
    private bool hasJumped = false;
    private bool hasCartJumped = false;
    private bool isFlying = false;
    private float swipeRange = 200.0f;
    private float tapRange;

    private Rigidbody rb;

    private int position = 0;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();
        Application.targetFrameRate = 60;

    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        checkTouch();
        Move();
        Swipe();
    }

    private void FixedUpdate()
    {
       
        //Swipe();
    }

    private void LateUpdate()
    {
        if (transform.rotation.y != 0)
        {

            float rotationOffset = 0;
            float rotationValue = transform.rotation.y;

            if (rotationValue > 0)
            {
                rotationOffset = -1 * rotationSpeed;
            }
            else if (rotationValue < 0)
            {
                rotationOffset = 1 * rotationSpeed;
            }

            if (rotationValue < rotationSpeed && rotationValue > -1 * rotationSpeed)
            {
                rotationOffset = -1 * rotationValue;

            }

            transform.Rotate(0, rotationOffset * 30f, 0, Space.Self);
        }
    }

    private void Move()
    {
        
        if (Touch.activeTouches.Count > 0 && Touch.activeTouches[0].phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            startTouchPosition = Touch.activeTouches[0].screenPosition;
        }

        if (Touch.activeTouches.Count > 0 && (Touch.activeTouches[0].phase == UnityEngine.InputSystem.TouchPhase.Moved) && !stopTouch)
        {

            currentPosition = Touch.activeTouches[0].screenPosition;
            Vector2 distance = currentPosition - startTouchPosition;

            //if (distance.x < 0)
//{
                //float veloctyY = rb.velocity.y;
               //Vector3 velocityComponent = Vector3.Lerp(rb.velocity, (Touch.activeTouches[0].delta.x * transform.right * horizontalSpeed), lerpPct);
                //.velocity
                //rb.velocity = Vector3.Lerp(rb.velocity, Touch.activeTouches[0].delta.x * transform.right * horizontalSpeed, lerpPct);
                //transform.Translate(-Vector3.right * Time.deltaTime * horizontalSpeed);
            //}
            
            if (distance.x > 0 || distance.x < 0)
            {
                float velocityY = rb.velocity.y;
                //rb.velocity = Vector3.Lerp(rb.velocity, (Touch.activeTouches[0].delta.x * transform.right * horizontalSpeed), lerpPct);
                Vector3 velocityComponent = Vector3.Lerp(rb.velocity, (Touch.activeTouches[0].delta.x * transform.right * horizontalSpeed), lerpPct);
                rb.velocity = new Vector3(velocityComponent.x, rb.velocity.y);
            }

            if (Touch.activeTouches[0].delta.x > 0 && transform.rotation.y < 0.1)
                transform.Rotate(0, 1, 0, Space.Self);
            else if (Touch.activeTouches[0].delta.x < 0 && transform.rotation.y > -0.1)
                transform.Rotate(0, -1, 0, Space.Self);
        }

        if (Touch.activeTouches.Count > 0 && (Touch.activeTouches[0].phase == UnityEngine.InputSystem.TouchPhase.Stationary || Touch.activeTouches[0].phase == UnityEngine.InputSystem.TouchPhase.Ended))
        {
            rb.velocity = new Vector3(0, rb.velocity.y);
        }


    }

    public void Swipe()
    {
        if(Touch.activeTouches.Count > 0 && Touch.activeTouches[0].phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            startTouchPosition = Touch.activeTouches[0].screenPosition;
        }

        if(Touch.activeTouches.Count > 0 && Touch.activeTouches[0].phase == UnityEngine.InputSystem.TouchPhase.Moved)
        {
            currentPosition = Touch.activeTouches[0].screenPosition;
            Vector2 Distance = currentPosition - startTouchPosition;


            if (!stopTouch && !hasJumped && !isFlying)
            {
                if(Distance.y < -swipeRange)
                {
                    //animator.Play("BoySlideDemo");
                    Debug.Log("Down");
                    //transform.Translate(Vector2.up * -0.2f);
                    //stopTouch = true;
                    //hasJumped = true;
                }
                else if (Distance.y > swipeRange)
                {
                    animator.Play("BoyJump");
                    Debug.Log("Up");
                    stopTouch = true;
                    hasJumped = true;
                    rb.velocity = Vector3.up * jumpHeight;
                    StartCoroutine(startTouch(1));
                }
            }

        }

        if(Touch.activeTouches.Count > 0 && Touch.activeTouches[0].phase == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            hasJumped = false;
        }
    }

    private void checkTouch()
    {
        if (hasCartJumped)
            stopTouch = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "PickupKite":
                other.gameObject.SetActive(false);
                break;

            case "Cart":
                if (!hasCartJumped)
                {
                    hasCartJumped = true;
                    animator.Play("BoyJumpContinous");

                    bool jumpHigh = false;

                    foreach(Transform child in transform)
                        if (child.CompareTag("SmallKite"))
                            jumpHigh = true;

                    if (jumpHigh)
                        rb.AddForce(Vector3.up * cartJumpHeightHigh, ForceMode.Impulse);
                    else
                        rb.AddForce(Vector3.up * cartJumpHeight, ForceMode.Impulse);

                    speed = speed * 1.2f;
                    stopTouch = true;
                    other.transform.Rotate(new Vector3(0, 0, 20), Space.Self);
                }
                break;
            case "SmallKite":
                foreach(Transform child in other.transform)
                {
                    Destroy(child.GetComponent<Rigidbody>());
                    child.parent = transform;
                    child.tag = "SmallKite";
                    child.localPosition = new Vector3(0, 4.7f, -1.5f);
                    child.localScale = new Vector3(120, 120, 120);
                    child.localEulerAngles = new Vector3(child.localEulerAngles.x, 0, child.localEulerAngles.z);

                }

                other.gameObject.SetActive(false);
                break;
            case "BigKite":
                animator.Play("BoyFlying");
                foreach (Transform child in other.transform)
                {
                    Destroy(child.GetComponent<Rigidbody>());
                    child.parent = transform;
                    child.tag = "BigKite";
                    isFlying = true;
                    child.localPosition = new Vector3(0, 6f, 0);
                    //child.localScale = new Vector3(120, 120, 120);
                    child.localEulerAngles = new Vector3(73, 0, child.localEulerAngles.z);
                    rb.useGravity = false;

                }

                other.gameObject.SetActive(false);
                break;

            case "Obstacle":
                animator.Play("BoyStumble");
                stopTouch = true;
                StartCoroutine(startTouch(1));
                break;
            case "ObstacleLeft":
                animator.Play("BoyShoulderHitLeft");
                stopTouch = true;
                StartCoroutine(startTouch(1));
                break;
            case "ObstacleRight":
                animator.Play("BoyShoulderHitRight");
                stopTouch = true;
                StartCoroutine(startTouch(1));
                break;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "RoofPath":
                if (hasCartJumped)
                {
                    animator.Play("BoyRunning");
                    hasCartJumped = false;
                    stopTouch = false;
                    speed = speed / 1.2f;
                    foreach (Transform child in transform)
                        if (child.CompareTag("SmallKite"))
                            Destroy(child.gameObject);
                }
                break;
        }
    }

    IEnumerator resetSpeed(int secs)
    {
        yield return new WaitForSeconds(secs);
        

    }

    IEnumerator startTouch(int secs)
    {
        yield return new WaitForSeconds(secs);
        stopTouch = false;
    }

    IEnumerator startTouchSlide(int secs)
    {
        yield return new WaitForSeconds(secs);

        transform.Translate(Vector2.up * 0.5f);
        stopTouch = false;
    }
}
