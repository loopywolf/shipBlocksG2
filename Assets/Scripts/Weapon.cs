using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject gatherPrefab;
    public const float FIRING_DISTANCE = 1.10f; //will 
    public const float GATHER_DISTANCE = 2.0f;
    private GameObject player;

    private void Start()
    {
        player = Camera.main.GetComponent<CameraController>().currentPlayer;
    }//Start

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) //&& false) //was temporarily disabled to try raycast clicking
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2d, Vector2.zero);
            bool gathered = false;
            if (hit.collider != null)
            {
                MyTile mt = hit.collider.gameObject.GetComponent<MyTile>();
                if (mt != null )
                {
                    //did we click a tile? if so, gather that tile
                    Debug.Log("LMB clicked tile " + hit.collider.gameObject.name + " at " + mousePos2d);
                    //if the tile is close by, gather it
                    float d = Vector2.Distance(mousePos2d, player.transform.position);
                    if(d <= GATHER_DISTANCE)
                    {
                        mt.gather();
                        gathered = true;
                    }
                }//if
            }
            if( !gathered) { 
                Shoot(bulletPrefab);    //can remove this parameter
            }//if hit not null
        }//if

    }//Update

    void Shoot(GameObject bullet)
    {
        //Make firepoint based on player object
        //GameObject p = GameObject.Find("Player"); //need better way - step 2
        //GameObject p = Camera.main.GetComponent<CameraController>().currentPlayer;//moved
        //if (p == null) Debug.Log("player not found.");
        Vector2 newFirePoint = player.transform.position;
        //shooting logic
        Vector2 cursorInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = cursorInWorldPos - new Vector2(newFirePoint.x, newFirePoint.y);
        Vector2 directionNormalized = direction.normalized;
        //firepoint part 2
        newFirePoint = new Vector2( newFirePoint.x + (directionNormalized.x * FIRING_DISTANCE),
                                    newFirePoint.y + (directionNormalized.y * FIRING_DISTANCE));
        //Debug.Log("direction = (" + direction.x + "," + direction.y);
        //float aimAngle = Vector2.SignedAngle(firePoint.position, cursorInWorldPos);
        //Debug.Log("aimAngle=" + aimAngle);
        //Quaternion aimQ = Quaternion.LookRotation(direction, Vector3.up);
        //Quaternion aimQ = Quaternion.LookRotation(cursorInWorldPos - new Vector2( firePoint.position.x, firePoint.position.y ),Vector3.up);

        //3rd attempt - success
        float ang = AngleBetweenVector2(firePoint.position, cursorInWorldPos);
        Quaternion aimQ = Quaternion.AngleAxis(ang, Vector3.forward);

        //GameObject go = Instantiate(bulletPrefab, firePoint.position, aimQ);// Quaternion.AngleAxis(aimAngle,Vector3.forward)); 
        GameObject go = Instantiate(bullet, newFirePoint, aimQ);// Quaternion.AngleAxis(aimAngle,Vector3.forward)); 
        // a tip said don't use - Quaternion.Euler(direction.x,direction.y,0f)); // firePoint.rotation);
        Bullet bpf = go.GetComponent<Bullet>();
        bpf.aimNormal = directionNormalized;
    }//F

    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }

}//class
