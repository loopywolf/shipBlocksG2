using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YourShipController : MonoBehaviour
{
    float targetRotation;
    //float currentSpeed = 0f;
    //float targetSpeed = 0f;
    private Rigidbody2D rb2d;
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
    public float ROTATE_SPEED = 4.0f;
    private float shipTurn;
    //private const float ANGLE_ADJUSTMENT = 90f;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        targetRotation = rb2d.rotation; // this.transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetRotation < rb2d.rotation) shipTurn = -ROTATE_SPEED;
        else
        if (targetRotation > rb2d.rotation) shipTurn = ROTATE_SPEED;

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (input != Vector2.zero)
        {
            if (input.x != 0f)
            {
                //targetRotation = nearest(rb2d.rotation - input.x * 45f, 45f);
                targetRotation += input.x * ROTATE_SPEED;
                //Debug.Log("I. rb2drot=" + rb2d.rotation + " targetRotation=" + targetRotation);
                //targetRotation = modulo(targetRotation, 45f);
                //Debug.Log("II. rb2drot="+rb2d.rotation+" targetRotation=" + targetRotation);
            }//A D rotate
        }

        Debug.Log("targetRotation=" + targetRotation);
    }//Update

    void FixedUpdate()
    {
        //Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
        //rb2d.MoveRotation(targetRotation);//.rotation + landerTurn * ROTATE_SPEED * Time.fixedDeltaTime);
        //rb2d.MoveRotation(rb2d.rotation + landerTurn * ROTATE_SPEED * Time.fixedDeltaTime);
        /* if (rb2d.rotation != targetRotation)
        {
            float turn = targetRotation - rb2d.rotation;

            if (Math.Abs(turn) < ROTATE_SPEED)
                rb2d.rotation = targetRotation;
            else
            {
                rb2d.rotation = rb2d.rotation + 
            }
        }//need to rotate */
        //rb2d.MoveRotation(Mathf.LerpAngle(rb2d.rotation, targetRotation, ROTATE_SPEED * Time.deltaTime));
        //OK I want it to rotate in the direction of the turn but stop when it hits a 45 degree mark (snap) YES
        //looking ahead, this should be good! If you hold it down, he'll keep turning :fingerscrossed\

        float a = Mathf.Abs(targetRotation - rb2d.rotation);
        if (a < ROTATE_SPEED)
        {
            rb2d.MoveRotation(targetRotation);
            shipTurn = 0f;
        }//if

        //Ok, so I want it to turn in the direction of the turn input
        //rb2d.rotation += landerTurn;
        rb2d.MoveRotation(rb2d.rotation + shipTurn);
    }//F


}//class
