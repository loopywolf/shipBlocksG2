using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YourShipController : MonoBehaviour
{
    float targetRotation;
    //float currentSpeed = 0f;
    //float targetSpeed = 0f;
    //private Rigidbody2D rb2d;
    //public float moveSpeed;
    private Vector2 input;
    //public LayerMask wallsLayer;
    //public List<string> inventory = new List<string>(); //old inventory - to remove?
    //public int health = 100;
    //public GameObject powEffect;
    //public Enemy myEnemyMySelf;
    //private Vector3 destination;
    //private bool headedForDestination;
    //trying to make inventory
    //private Inventory inventory2;// = new Inventory();
    //[SerializeField] private UI_Inventory uiInventory;
    //tank movement
    public float ROTATE_SPEED = 1.0f;
    private float shipTurn;
    //private const float ANGLE_ADJUSTMENT = 90f;
    public float SlowDown;
    public float moveSpeed;
    Vector2 CurrentSpeed;

    //Crashjumper
    [SerializeField] float thrust = 5f;
    [SerializeField] float rotateSpeed = 5f;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] bool stabilizers = false;

    bool thrusting = false;
    float rotation = 0f;
    float angularDrag;
    float linearDrag;

    // Start is called before the first frame update
    void Start()
    {
        /* rb2d = this.GetComponent<Rigidbody2D>();
        targetRotation = rb2d.rotation; // this.transform.eulerAngles.z;
        CurrentSpeed = new Vector2(0f, 0f); */
        if (rb2d == null)
        {
            rb2d = GetComponent<Rigidbody2D>();
            angularDrag = rb2d.angularDrag;
            linearDrag = rb2d.drag;
        }
    }

    void Update()
    {
        PlayerInput();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void PlayerInput()
    {
        thrusting = Input.GetKey(KeyCode.W);
        if (Input.GetKey(KeyCode.A))
        {
            rotation = 1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotation = -1f;
        }
        else
        {
            rotation = 0f;
        }
        stabilizers = Input.GetKey(KeyCode.S);
    }

    void Movement()
    {
        if (thrusting)
        {
            rb2d.AddForce(this.transform.right * thrust); //was up
        }

        rb2d.AddTorque(rotation * rotateSpeed);
        if (stabilizers)
        {
            rb2d.angularDrag = 10f;
            rb2d.drag = 10f;
        }
        else
        {
            rb2d.angularDrag = angularDrag;
            rb2d.drag = linearDrag;
        }
    }

    // Update is called once per frame
    void FixedUpdate2()
    {
        float dx = Input.GetAxisRaw("Horizontal");
        float dy = Input.GetAxisRaw("Vertical");

        //Ship controls
        if (dx != 0)
        {
            //shipTransform.eulerAngles = new Vector3(
            //  shipTransform.eulerAngles.x,
            //shipTransform.eulerAngles.y,
            //shipTransform.eulerAngles.z + turnSpeed * dx);
            rb2d.MoveRotation(rb2d.rotation - ROTATE_SPEED * dx);
        }//A D rotate

        Vector3 Thrust = Vector3.zero;
        if (dy != 0)
        {
            Thrust = new Vector3(
                Mathf.Cos(rb2d.transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * dy,
                Mathf.Sin(rb2d.transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * dy,
                0f
                //CurrentSpeed.x + Mathf.Cos(rb2d.transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * dy * moveSpeed,
                //CurrentSpeed.y + Mathf.Cos(rb2d.transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * dy * moveSpeed
            );
            //CurrentSpeed = CurrentSpeed + Thrust;

            //rb2d.velocity = (transform.forward * dy) * 2;
        }//dy

        if (Thrust != Vector3.zero)
            rb2d.AddForce(Thrust * moveSpeed); //note this is applies in FixedUpdate

        //transform.Translate(CurrentSpeed * moveSpeed * dy * Time.deltaTime);
        /*Vector2 newPosition = new Vector2(
            rb2d.position.x + CurrentSpeed.x,
            rb2d.position.y + CurrentSpeed.y
        );
        rb2d.MovePosition(newPosition); */

        //trying to slow down by vector
        // float m = CurrentSpeed.magnitude;
        //CurrentSpeed = CurrentSpeed.normalized * (m / (1.0f + SlowDown));

    }//FIXEDUpdate
    void FixedUpdate3()
    {
        //The code for moving up/down and left/right
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rb2d.AddForce(input);
    }


}//class
