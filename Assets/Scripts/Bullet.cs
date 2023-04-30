using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb2d;
    public int damage = 40;
    GameObject powEffect; 
    public Vector2 aimNormal;
    //public float travelAngle;

    // Start is called before the first frame update
    void Start()
    {
        rb2d.velocity = aimNormal * speed;
        powEffect = Camera.main.GetComponent<CameraController>().powEffect; //has to work
    }//start

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log("hit " + hitInfo.name);
        //first, bullet goes POW!
        Instantiate(powEffect, transform.position, Quaternion.identity);
        //the thing that is hit needs to react - I think the health collider is reacting to the bullet
        Enemy e = hitInfo.GetComponent<Enemy>();
        if(e!=null)
        {
            e.TakeDamage(damage);
        }//if
        Destroy(gameObject);
    }//OnTriggerEnter2D

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }//OnBecameInvisible

}//class
