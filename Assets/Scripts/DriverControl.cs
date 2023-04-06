using UnityEngine;

public class DriverControl : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.01f;
    [SerializeField] private float steerSpeed = 0.015f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PointerScript ps;
    [SerializeField] private DeliveryController dc;
    [SerializeField] private GameStatus gs;
    private readonly float driftAmount = 0.95f;
    private readonly float maxSpeed = 8;

    private float accInput;
    private float rotationAngle;
    private float steerInput;
    private bool tookPackage;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ps = FindObjectOfType<PointerScript>();
        dc = FindObjectOfType<DeliveryController>();
        gs = FindObjectOfType<GameStatus>();
        rotationAngle = transform.localRotation.eulerAngles.z;
    }

    // Update is called once per frame
    private void Update()
    {
        setInputVector();
    }

    private void FixedUpdate()
    {
        if (gs.isThereTime())
        {
            acc();
            killSideVelocity();
            steer();

        }

    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.tag == "crash")
            gs.punish(50);
        else if (coll.collider.tag == "tree") gs.punish(20);
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "package")
        {
            if (tookPackage == false)
            {
                tookPackage = true;
                coll.gameObject.SetActive(false);
                dc.SetHaveLocation(false);
            }
        }

        else if (coll.tag == "delivery")
        {
            if (tookPackage)
            {
                tookPackage = false;
                coll.gameObject.SetActive(false);
                gs.addScore(100);
                gs.resetTimer();
                dc.SetHaveLocation(false);

            }
        }

    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag == "road" && accInput != 0) gs.addScore();
    }

    private void acc()
    {
        //if gas button is not pressed add drag to stop the car
        if (accInput == 0)
            rb.drag = 2.0f;
        else rb.drag = 0.2f;



        //max speed check
        var currentSpeed = Vector2.Dot(transform.up, rb.velocity);
        if (currentSpeed > maxSpeed && accInput > 0)
            return;

        //create a force for car
        //transform.up means forward in 2d
        Vector2 forceVector = transform.up * accInput * moveSpeed;
        //apply that force
        rb.AddForce(forceVector);
    }

    private void steer()
    {
        //limit the cars ability to turn when moving slowly
        var minSpeedAllow = rb.velocity.magnitude / 4;

        //Clamp01 limits the value between 0 or 1 like regular clamp
        minSpeedAllow = Mathf.Clamp01(minSpeedAllow);

        //update the rotation angle based on the input
        rotationAngle += steerInput * steerSpeed * minSpeedAllow;
        //apply steering
        rb.MoveRotation(rotationAngle);
    }

    private void killSideVelocity()
    {
        //find forward velocity by multiplying dot product of velocity and transform.up
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        //find right velocity by multiplying dot product of velocity and transform.right
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);
        rb.velocity = forwardVelocity + rightVelocity * driftAmount;
    }

    private void setInputVector()
    {
        accInput = Input.GetAxis("Vertical");
        if (accInput != 0)
            steerInput = Input.GetAxis("Horizontal");
    }

    public float GetLateralVelocity()
    {
        //how fast the car is moving sideways
        return Vector2.Dot(transform.right, rb.velocity);
    }

    public float GetForwardVelocity()
    {
        //how fast the car is moving forward
        return Vector2.Dot(transform.up, rb.velocity);
    }

    public bool isBreaking()
    {
        var sideV = GetLateralVelocity();
        var forwardV = GetForwardVelocity();

        if (accInput < 0 && forwardV > 0) return true;
        if (Mathf.Abs(sideV) > 3) return true;
        return false;
    }


    public float GetAccInput()
    {
        return accInput;
    }

    public bool ifTookPackage()
    {
        return tookPackage;
    }
}
