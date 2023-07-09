using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupedP : MonoBehaviour, IPlatforms
{
    public bool selected = false;
    public bool freezed = false;
    //choose base on the types of the group platform
    [Header("Platform Properties")]
    public bool horizontal = false;
    public bool vertical = false;
    public bool jump = false;
    public bool rotate = false;
    [Tooltip("Drag the rotate platform of this group here, it will become the pivot point")]
    public GameObject rotatePlatform = null;
    private Vector3 pivot;

    private Rigidbody2D m_Rigidbody2D;
    private float horizontalMove = 0f;
    private float verticalMove = 0f;
    [Header("Platform Speed Setting")]
    public float moveSpeed = 50f;
    public float runAcceleration = 35f;
    public float runDecceleration = 100f;
    public float runMaxSpeed = 2.5f;
    [HideInInspector]
    private float runAccelAmount;
    [HideInInspector]
    private float runDeccelAmount;

    private List<Transform> childrenList = new List<Transform>();
    private List<Transform> childrenListOutline = new List<Transform>();
    private float timer = 0f;
    [Tooltip("the duration of jump platform triggers")]
    public float activeDuration = 10f;

    private bool rotating = false;
    [Tooltip("the duration of rotation")]
    public float totalTime = 2f;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        GetAllChildren();
        if (jump)
        {
            GetAllJumpPlatformsInChildren();
            animator = childrenList[0].gameObject.GetComponent<Animator>();
        }
            
        // print(childrenList[0]);
        
            
    }

    // Update is called once per frame
    void Update()
    {
        if(rotate)
            pivot = rotatePlatform.transform.position;
        if (selected && !freezed)
        {
            activateOutlines();
            spritePress();
            m_Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            //player move this object if it is selected
            if (vertical)
                verticalMove = Input.GetAxisRaw("Vertical") * moveSpeed;
            if (horizontal)
                horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
            if (rotate)
            {
                
                if (Input.GetKeyDown("q") && !rotating)
                {
                    //rotate 90 degree anticlockwisely
                    StartCoroutine(Rotate(this.transform, pivot, Vector3.forward, 90, totalTime));
                    //transform.RotateAround(pivot, Vector3.forward, 90);
                }
                if (Input.GetKeyDown("e") && !rotating)
                {
                    //rotate 90 degree clockwisely
                    StartCoroutine(Rotate(this.transform, pivot, Vector3.forward, -90, totalTime));
                }
            }
            if (jump)
            {
                if (Input.GetKeyDown("space"))
                {
                    animator.SetBool("Extend", true);
                    timer = activeDuration;
                    activateJumpPlatforms();
                }
            }
        }
        else if(selected != true)
        {
            disactivateOutlines();
            spriteRelease();
            verticalMove = 0;
            horizontalMove = 0;
            m_Rigidbody2D.velocity = new Vector3(0, 0, 0);
            m_Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }else if(selected && freezed){
            if (jump)
            {
                if (Input.GetKeyDown("space"))
                {
                    animator.SetBool("Extend", true);
                    timer = activeDuration;
                    activateJumpPlatforms();
                }
            }
            disactivateOutlines();
            spriteRelease();
            verticalMove = 0;
            horizontalMove = 0;
            m_Rigidbody2D.velocity = new Vector3(0, 0, 0);
            m_Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }
        timer = timer - 0.1f;
        if(jump){
           if (timer < 0) {
            animator.SetBool("Extend", false);
            disactivateJumpPlatforms();
            } 
        }
        
        if(freezed){
            spriteFreeze();
        }else{
            spriteUnFreeze();
        }
    }

    private IEnumerator Rotate(Transform Transform, Vector3 targetTransform, Vector3 rotateAxis, float degrees, float totalTime)
    {
        if (rotating)
            yield return null;
        rotating = true;
 
        Quaternion startRotation = Transform.rotation;
        Vector3 startPosition = Transform.position;
        // Get end position;
        transform.RotateAround(targetTransform, rotateAxis, degrees);
        Quaternion endRotation = Transform.rotation;
        Vector3 endPosition = Transform.position;
        Transform.rotation = startRotation;
        Transform.position = startPosition;
 
        float rate = degrees / totalTime;
        //Start Rotate
        for (float i = 0.0f; Mathf.Abs(i) < Mathf.Abs(degrees); i += Time.deltaTime * rate)
        {
            Transform.RotateAround(targetTransform, rotateAxis, Time.deltaTime * rate);
            yield return null;
        }
        Transform.rotation = endRotation;
        Transform.position = endPosition;
        rotating = false;
    }
    

    void FixedUpdate()
    {
        MoveV(verticalMove);
        MoveH(horizontalMove);
    }

    public void onClickEvent()
    {
        selected = true;
    }

    public void Deselect()
    {
        selected = false;
    }

    public void MoveV(float speed)
    {
        float targetSpeed = speed;
        float accelRate = 0f;
        //amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - m_Rigidbody2D.velocity.y;

        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        m_Rigidbody2D.AddForce(movement * Vector2.up, ForceMode2D.Force);
    }

    public void MoveH(float speed)
    {
        float targetSpeed = speed;
        float accelRate = 0f;
        //amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - m_Rigidbody2D.velocity.x;

        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        m_Rigidbody2D.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    public void activateJumpPlatforms()
    {
        if (childrenList != null)
        {
            foreach (Transform tempTransform in childrenList)
            {
                tempTransform.GetChild(2).gameObject.SetActive(true);
            }
        }
    }

    public void disactivateJumpPlatforms()
    {
        if (childrenList != null)
        {
            foreach (Transform tempTransform in childrenList)
            {
                //Debug.Log(tempTransform.GetChild(0).gameObject.name);
                tempTransform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }

    void GetAllJumpPlatformsInChildren()
    {
        foreach (Transform child in transform)
            if (child.CompareTag("Jump"))
            {
                childrenList.Add(child);
            }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            freezed = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            freezed = false;
        }
    }

    public void activateOutlines()
    {
        if (childrenListOutline != null)
        {
            foreach (Transform tempTransform in childrenListOutline)
            {
                tempTransform.GetChild(0).gameObject.SetActive(true);
                tempTransform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    public void disactivateOutlines()
    {
        if (childrenListOutline != null)
        {
            foreach (Transform tempTransform in childrenListOutline)
            {
                tempTransform.GetChild(0).gameObject.SetActive(false);
                tempTransform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    void GetAllChildren()
    {
        foreach (Transform child in transform)
            childrenListOutline.Add(child);
    }

    public void spritePress()
    {
        if (childrenListOutline != null)
        {
            foreach (Transform tempTransform in childrenListOutline)
            {
                tempTransform.gameObject.GetComponent<SpriteChange>().Pressed();
            }
        }
    }

    public void spriteRelease()
    {
        if (childrenListOutline != null)
        {
            foreach (Transform tempTransform in childrenListOutline)
            {
                tempTransform.gameObject.GetComponent<SpriteChange>().Released();
            }
        }
    }

    public void spriteFreeze()
    {
        if (childrenListOutline != null)
        {
            foreach (Transform tempTransform in childrenListOutline)
            {
                tempTransform.gameObject.GetComponent<SpriteChange>().Freeze();
            }
        }
    }

    public void spriteUnFreeze()
    {
        if (childrenListOutline != null)
        {
            foreach (Transform tempTransform in childrenListOutline)
            {
                tempTransform.gameObject.GetComponent<SpriteChange>().UnFreeze();
            }
        }
    }
}
