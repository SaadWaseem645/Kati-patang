using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
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
    private float railPush = 2f;

    [SerializeField]
    [Range(0f, 1f)]
    private float lerpPct = 0.3f;

    private PlayerScore playerScoreScript;

    private Vector2 currentPosition;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    private Animator animator;

    private bool stopTouch = false;
    private bool hasJumped = false;
    private bool hasCartJumped = false;
    private bool isFlying = false;
    private bool isDead = false;
    private bool hasWon = false;
    private bool stopCamera = false;
    private bool gameStarted = false;
    private bool winSequence = false;
    private float swipeRange = 200.0f;
    private float tapRange;

    private GameObject finalKite = null;
    [SerializeField]
    private float kiteScaleSize = 0.024f;

    private Rigidbody rb;

    [SerializeField]
    private Button button;
    [SerializeField]
    private Button nextButton;
    [SerializeField]
    private RawImage image;

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
        playerScoreScript = GetComponent<PlayerScore>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hasWon && !winSequence)
        {
            nextButton.gameObject.SetActive(true);
            winSequence = true;
        }

        if(playerScoreScript.getLives() <= 0 && !isDead)
        {
            isDead = true;
            stopTouch = true;
            animator.Play("BoyFall");
        }

        if (!isDead && !hasWon && gameStarted)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            checkTouch();
            Move();
            Swipe();
        }

        if (finalKite != null && !hasWon)
            reduceKiteSize();

    }

    private void FixedUpdate()
    {
       
        //Swipe();
    }

    public void startGame()
    {
        gameStarted = true;
        button.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
        animator.Play("BoyRunning");
    }

    private void LateUpdate()
    {
        if (transform.rotation.y != 0 && !hasWon)
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


            if (!stopTouch && !hasJumped && !isFlying && !isDead)
            {
                if(Distance.y < -swipeRange)
                {
                    //animator.Play("BoySlideDemo");
                    //Debug.Log("Down");
                    //transform.Translate(Vector2.up * -0.2f);
                    //stopTouch = true;
                    //hasJumped = true;
                }
                else if (Distance.y > swipeRange)
                {
                    animator.Play("BoyJump");
                    //Debug.Log("Up");
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

    private void reduceKiteSize()
    {
        Vector3 kiteScale = finalKite.transform.localScale;
        float scale = kiteScaleSize * Time.deltaTime;
        finalKite.transform.localScale = new Vector3(kiteScale.x - scale, kiteScale.y - scale, kiteScale.z - scale);
        if(finalKite.transform.localScale.x <= 184)
        {
            rb.useGravity = true;
            stopCamera = true;
            animator.Play("BoyJumpContinous");
            hasWon = true;
          
            transform.Translate(-Vector3.up * Time.deltaTime * speed);
            finalKite.transform.localPosition = new Vector3(0, 3.5f, -2);
            finalKite.transform.localEulerAngles = new Vector3(0, 0, finalKite.transform.localEulerAngles.z);
        }
    }

    private void checkTouch()
    {
        if (hasCartJumped)
            stopTouch = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isDead)
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
                    rb.velocity = Vector3.zero;

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
                case "FlyKite":
                    animator.Play("BoyFlying");
                    rb.velocity = Vector3.zero;
                    foreach (Transform child in other.transform)
                    {
                        Destroy(child.GetComponent<Rigidbody>());
                        child.parent = transform;
                        child.tag = "FlyKite";
                        isFlying = true;
                        child.localPosition = new Vector3(0, 6f, 0);
                        child.localEulerAngles = new Vector3(73, 0, child.localEulerAngles.z);
                        rb.useGravity = false;
                    }
                    StartCoroutine(startRun(2));
                    other.gameObject.SetActive(false);
                    break;

                case "BigKite":
                animator.Play("BoyFlying");
                foreach (Transform child in other.transform)
                {
                    Destroy(child.GetComponent<Rigidbody>());
                    child.parent = transform;
                    finalKite = child.gameObject;
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
            case "Projectile":
                animator.Play("BoyStumble");
                break;
            case "2x":
            case "3x":
            case "5x":
            case "7x":
            case "10x":
                    animator.Play("BoyDance");
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
                    stopTouch = true;
                    hasWon = true;
                    foreach (Transform child in transform)
                        if (child.CompareTag("BigKite"))
                            Destroy(child.gameObject);
                    break;

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(!isDead)
            switch (other.tag)
            {
                case "RoofEnd":
                    stopTouch = true;
                    if (transform.position.x < 0.2f && transform.position.x > -0.2f)
                        transform.position = new Vector3(0, transform.position.y, transform.position.z);
                    else if(transform.position.x > 0)
                        transform.Translate(-Vector3.right * Time.deltaTime * speed);
                    else if(transform.position.x < 0)
                        transform.Translate(Vector3.right * Time.deltaTime * speed);
                    break;
            }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!isDead)
            switch (other.tag)
            {
                case "RoofEnd":
                    stopTouch = false;
                    break;
            }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isDead)
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

            case "Rail":
                if (transform.position.x > 0)
                    transform.position = new Vector3(transform.position.x + railPush, transform.position.y, transform.position.z);
                else
                    transform.position = new Vector3(transform.position.x - railPush, transform.position.y, transform.position.z);
                break;
        }
    }

    public bool isStopCamera()
    {
        return stopCamera;
    }

    public bool isGameStarted()
    {
        return gameStarted;
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

    IEnumerator startRun(int secs)
    {
        yield return new WaitForSeconds(secs);
        rb.useGravity = true;
        isFlying = false;
        animator.Play("BoyRunning");
        foreach (Transform child in transform)
            if (child.CompareTag("FlyKite"))
                Destroy(child.gameObject);
    }

    IEnumerator startTouchSlide(int secs)
    {
        yield return new WaitForSeconds(secs);

        transform.Translate(Vector2.up * 0.5f);
        stopTouch = false;
    }
}
