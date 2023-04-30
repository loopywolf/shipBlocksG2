using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public bool isMoving;
    private Vector2 input;
    public LayerMask wallsLayer;
    public List<string> inventory = new List<string>(); //old inventory - to remove?
    public int health = 100;
    public GameObject powEffect;
    public Enemy myEnemyMySelf;
    private Vector3 destination;
    private bool headedForDestination;
    //trying to make inventory
    //private Inventory inventory2;// = new Inventory();
    //[SerializeField] private UI_Inventory uiInventory;
    //tank movement
    private float targetRotation;
    private Rigidbody2D rb2d;
    public float ROTATE_SPEED = 4.0f;
    private float landerTurn;
    private const float ANGLE_ADJUSTMENT = 90f;

    /*float nfmod(float a, float b)
    {
        return a - b * Mathf.FloorToInt(a / b);
    }//F   */

    float remainder(float n, float d)
    {
        if (n < 0)
        {
            n = Mathf.Abs(n);
            return -(n % d);
        } else
        {
            return (n % d);
        }//F
    }//F

    float nearest(float n,float d)
    {//finds the nearest multiple of d, e.g. d=45, would be 45,90,135,etc. or negatives -45 -90 -135, etc.
        int f = Mathf.RoundToInt(n / d);
        return f * d;
    }//F

    // Start is called before the first frame update
    void Start()
    {
        powEffect = Camera.main.GetComponent<CameraController>().powEffect; //TODO not efficient, you do the same in Bullet
        myEnemyMySelf = transform.GetComponent<Enemy>();
        headedForDestination = false;
        //tank movement
        rb2d = this.GetComponent<Rigidbody2D>();
        targetRotation = rb2d.rotation; // this.transform.eulerAngles.z;
    }//Start

    // Update is called once per frame
    void Update()
    {
        //landerTurn = 0f;
        //tank movement - turn
        //landerTurn = Mathf.Sign(targetRotation - rb2d.rotation);
        //Vector3 m_EulerAngleVelocity = new Vector3(0, 0, rotateLander * ROTATE_SPEED);
        if (targetRotation < rb2d.rotation) landerTurn = -ROTATE_SPEED;
        else
        if (targetRotation > rb2d.rotation) landerTurn = ROTATE_SPEED;

        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //pathing
            if (headedForDestination && input == Vector2.zero)
            {
                Debug.Log("Pathing " + transform.position + " to " + destination);
                if (destination == transform.position)
                {
                    headedForDestination = false;   //stop moving
                } else {
                    if (destination.x < transform.position.x)
                        input.x = -1.0f;
                    else
                    if (destination.x > transform.position.x)
                        input.x = 1.0f;

                    if (destination.y < transform.position.y)
                        input.y = -1.0f;
                    else
                    if (destination.y > transform.position.y)
                        input.y = 1.0f;
                }//if destination = position
            }//if headedForDestination

            if (input != Vector2.zero)
            {
                //Debug.Log("x=" + input.x + " y=" + input.y);
                /* var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y; */
                if(input.x != 0f)
                {
                    //targetRotation = rb2d.rotation - input.x * 45f;
                    //landerTurn = input.x;
                    //Debug.Log("rb2d.rotation=" + rb2d.rotation + "input.x="+input.x);
                    //Debug.Log("set targetRotation to " + targetRotation);

                    //if -1 find next rotation of 45 (presumes you are at a 45)
                    targetRotation = nearest(rb2d.rotation - input.x * 45f,45f);
                    Debug.Log("I. rb2drot=" + rb2d.rotation + " targetRotation=" + targetRotation);
                    //targetRotation = modulo(targetRotation, 45f);
                    //Debug.Log("II. rb2drot="+rb2d.rotation+" targetRotation=" + targetRotation);
                }//A D rotate

                if(input.y != 0f)   //w or s was pressed
                {
                    var targetPos = transform.position;

                    //determine which direction by rotation
                    float x = Mathf.RoundToInt(Mathf.Cos((targetRotation + ANGLE_ADJUSTMENT) * Mathf.Deg2Rad));
                    float y = Mathf.RoundToInt(Mathf.Sin((targetRotation + ANGLE_ADJUSTMENT) * Mathf.Deg2Rad));
                    Debug.Log("targetRot="+targetRotation+" Cos=" + x + " sin=" + y);
                    if (x > 0) targetPos.x = targetPos.x + 1 * input.y;
                    else
                    if (x < 0) targetPos.x = targetPos.x - 1 * input.y;

                    if (y > 0) targetPos.y = targetPos.y + 1 * input.y;
                    else
                    if (y < 0) targetPos.y = targetPos.y - 1 * input.y;

                    Debug.Log("walking");
                    if ( isWalkable(targetPos))
                      StartCoroutine(Move(targetPos));

                }//if

            }

        }//if !isMoving

        /* if(headedForDestination)
        {
            //the player must move towards its destination
            //if (isWalkable(destination))
            StartCoroutine(Move(destination)); //note: this is too fast, but it works!
        }//if */

        /*Debug.Log("Rot= " + rb2d.rotation + " snap?" + (rb2d.rotation % 45f));
        //if the rotation hits a 45 degree, stop
        if (rb2d.rotation % 45f < 0.5f)
        {
            landerTurn = 0f;
        }*/

    }//Update

    IEnumerator Move(Vector3 targetPos) //is it OK this is not FixedUpdate
    {
        isMoving = true;

        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }//F.cr

    private bool isWalkable(Vector3 targetPos)
    {
        if( Physics2D.OverlapCircle(targetPos, 0.2f, wallsLayer) != null )
        {
            return false;
        }
        return true;
    }//F

    internal void addItem(string v)
    {
        inventory.Add(v);
        //throw new NotImplementedException();
    }//F

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log("hitme " + hitInfo.name);

        //instantiate POW! - how??

        Enemy e = hitInfo.GetComponent<Enemy>();
        if (e != null) //it is an enemy
        {
            Vector2 midwayPoint = new Vector2(
                (transform.position.x + e.transform.position.x)/2.0f,
                (transform.position.y + e.transform.position.y)/2.0f);
            Instantiate(powEffect, midwayPoint, Quaternion.identity);
            if(myEnemyMySelf!=null)
                myEnemyMySelf.TakeDamage(e.h2hDamage);  //enemy needs to become a more generic "mob" class
            Debug.Log("Hit! " + e.h2hDamage + " health=" + health);
        }//if
        //Destroy(gameObject); */
    }//OnTriggerEnter2D

    internal void SetDestination(Vector3 mousePos)
    {
        destination = new Vector3( Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), 0f);
        headedForDestination = true;
        //sets the player mob's destination
        Debug.Log(name + " headed for destination " + destination);
    }

    private void Awake()
    {
        //inventory2 = new Inventory();
        //uiInventory.SetInventory(inventory2);

    }//F

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

        float a = Math.Abs(targetRotation - rb2d.rotation);
        if (a < ROTATE_SPEED)
        {
            rb2d.MoveRotation(targetRotation);
            landerTurn = 0f;
        }//if

        //Ok, so I want it to turn in the direction of the turn input
        //rb2d.rotation += landerTurn;
        rb2d.MoveRotation(rb2d.rotation + landerTurn);
    }//F

}//Class
