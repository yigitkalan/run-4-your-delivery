using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverControl : MonoBehaviour
{
    private bool tookPackage = false;

    [SerializeField] private float moveSpeed = 0.01f;
    [SerializeField] private float steerSpeed = 0.01f;
    private float driftAmount = 0.95f;
    private float maxSpeed = 10;

    private float accInput;
    private float steerInput;
    private float rotationAngle;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PointerScript ps;
    [SerializeField] private DeliveryController dc;
    [SerializeField] private GameStatus gs;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ps = FindObjectOfType<PointerScript>();
        dc = FindObjectOfType<DeliveryController>();
        gs = FindObjectOfType<GameStatus>();
        rotationAngle = transform.localRotation.eulerAngles.z;

    }

    // Update is called once per frame
    void Update(){
        setInputVector();
    }
    void FixedUpdate()
    {
        if(gs.isThereTime()) {
            acc();
            killSideVelocity();
            steer();
        }
    }

    void acc(){
        //if gas button is not pressed add drag to stop the car
        if(accInput == 0)
            rb.drag = 2.0f;
        else rb.drag = 0f;

        //max speed check
        float currentSpeed = Vector2.Dot(transform.up , rb.velocity);
        if(currentSpeed > maxSpeed && accInput > 0)
            return;

        //create a force for car
        //transform.up means forward in 2d
        Vector2 forceVector = transform.up * accInput * moveSpeed;
        //apply that force
        rb.AddForce(forceVector);
    }

    void steer(){
        //limit the cars ability to turn when moving slowly
        float minSpeedAllow = rb.velocity.magnitude / 2;

        //Clamp01 limits the value between 0 or 1 like regular clamp
        minSpeedAllow = Mathf.Clamp01(minSpeedAllow);

        //update the rotation angle based on the input
        rotationAngle += steerInput * steerSpeed * minSpeedAllow;
        //apply steering
        rb.MoveRotation(rotationAngle);
    }

    void killSideVelocity(){
        //find forward velocity by multiplying dot product of velocity and transform.up
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        //find right velocity by multiplying dot product of velocity and transform.right
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);

        rb.velocity = forwardVelocity + rightVelocity * driftAmount;
    }

    void setInputVector(){
        accInput = Input.GetAxis("Vertical");
        if(accInput != 0)
            steerInput = Input.GetAxis("Horizontal");
    }




    void OnTriggerEnter2D(Collider2D coll){
        if(coll.tag == "package"){
            if(tookPackage == false){
                tookPackage = true;
                coll.gameObject.SetActive(false);
                dc.SetHaveLocation(false);
            }
        }

        else if(coll.tag == "delivery") {
            if(tookPackage == true) {
                tookPackage = false;
                coll.gameObject.SetActive(false);
                gs.addScore(100);
                gs.resetTimer();
                dc.SetHaveLocation(false);
            }
        }
    }

    void OnTriggerStay2D(Collider2D coll) {
        if(coll.tag == "road") {
            gs.addScore();
        }
    }

    void OnCollisionEnter2D(Collision2D coll){
        if(coll.collider.tag == "crash")
            gs.punish(50);
        else if(coll.collider.tag == "tree" ) {
            gs.punish(20);
        }
    }

    public bool ifTookPackage(){
        return tookPackage;
    }
}

