using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalP : MonoBehaviour, IPlatforms
{
    public bool selected = false;

    private Rigidbody2D m_Rigidbody2D;
    private float verticalMove = 0f;
    public float moveSpeed = 50f;
    public float runAcceleration = 35f;
    public float runDecceleration = 100f;
    public float runMaxSpeed = 2.5f;
    [HideInInspector] private float runAccelAmount;
	[HideInInspector] private float runDeccelAmount;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            m_Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            //player move this object if it is selected
            verticalMove = Input.GetAxisRaw("Vertical") * moveSpeed;
        }else{
            verticalMove = 0;
            m_Rigidbody2D.velocity = new Vector3(0,0,0);
            m_Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void FixedUpdate()
    {
        Move(verticalMove);
    }

    public void onClickEvent()
    {
        selected = true;
    }

    public void Deselect()
    {
        selected = false;
    }

    public void Move(float speed)
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
}
