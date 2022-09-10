using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody rb;

    public float Gravity = 200;
    Vector3 GravityDirection = Vector3.down;
    Transform GravityTransform;


    float Horizontal;
    float Vertical;

    float Xvel;
    float Yvel;
    float Zvel;

    bool OnSlope = false;

    public float Speed;
    float FinalSpeed;
    public float JumpForce;

    bool Jumped;

    public GameObject GroundPoint;
    public LayerMask ExcludePlayer;

    bool FixedGrounded = false;

    private void OnTriggerEnter(Collider other)
    {

        //Debug.Log(other.name);
        
    }

    void Start()
    {
        FinalSpeed = Speed;
        GravityTransform = transform;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Horizontal = Input.GetAxis("Horizontal") * 0.85f;
        Vertical = Input.GetAxis("Vertical");
        
        if (Input.GetButtonDown("Jump"))
        {
            if (!Jumped)
            {
                Jumped = true;
            }
        }

    }

    private void FixedUpdate()
    {


        Vector3 Movement;

        FixedGrounded = isGrounded();

        
        
        if (FixedGrounded && Jumped)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(JumpForce * transform.up, ForceMode.Impulse);
            Jumped = false;
            FixedGrounded = false;
        }
        else if(!FixedGrounded && Jumped)
        {
            Jumped = false;
        }


        if (!FixedGrounded)
        {
            FinalSpeed = Speed / 6f;
        }
        else
        {
            FinalSpeed = Speed;
        }



        Movement = Vector3.ClampMagnitude(((transform.forward * Vertical) + (transform.right * Horizontal)), 1f);
        Vector3 FinalMove = Vector3.ProjectOnPlane(Movement, GravityDirection).normalized * Movement.magnitude * FinalSpeed;

        rb.AddForce(FinalMove, ForceMode.Force);


        rb.AddForce(GravityDirection * Gravity, ForceMode.Acceleration);

        Vector3 AdjustedVelocity;

        if (FixedGrounded)
        {
            AdjustedVelocity = GravityTransform.InverseTransformDirection(rb.velocity);
            AdjustedVelocity = new Vector3(AdjustedVelocity.x * 0.965f, AdjustedVelocity.y, AdjustedVelocity.z * 0.965f);
            AdjustedVelocity = GravityTransform.TransformDirection(AdjustedVelocity);
            
            rb.velocity = AdjustedVelocity;

        }
        else
        {
            AdjustedVelocity = rb.velocity;
            AdjustedVelocity = new Vector3(AdjustedVelocity.x * 0.9905f, AdjustedVelocity.y, AdjustedVelocity.z * 0.9905f);

            rb.velocity = AdjustedVelocity;

        }

        
        
    }

    bool timerstarted;

    bool isGrounded()
    {
        RaycastHit hit;

        if (Physics.SphereCast(GroundPoint.transform.position, 0.4875f, -transform.up, out hit, 0.055f, ExcludePlayer, QueryTriggerInteraction.Ignore))
        {

            if(hit.normal != Vector3.up && hit.normal != Vector3.right)
            {
                //we can expand on this later if need be but this should be fine for now!
                OnSlope = true;
                GravityDirection = -hit.normal;
                GravityTransform = hit.transform;
            }
            else
            {
                OnSlope = false;
                GravityDirection = -Vector3.up;
                GravityTransform = transform;
            }

            return true;

        }

        //temporary data debugging!
        if (!timerstarted)
        {
            StartCoroutine(TimeJump());
        }

        OnSlope = false;
        GravityDirection = -Vector3.up;
        GravityTransform = transform;
        return false;

    }


    IEnumerator TimeJump()
    {
        timerstarted = true;
        float TimeInAir = 0;

        while (!isGrounded())
        {
            TimeInAir += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        //Debug.Log("We spent " + TimeInAir + " in the air!");
        timerstarted = false;

    }


}
