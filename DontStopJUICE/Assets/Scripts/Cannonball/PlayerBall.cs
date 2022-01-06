using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    public Rigidbody rbBall;
    public float speedMove;
    public Transform tranCam;

    public Transform currentCannon;
    public float speedRotate;
    public bool inCannon;
    public float powerCannon;

    // Update is called once per frame
    void Update()
    {

        if (!inCannon)
        {
            //movement inputs
            if (Input.GetKey("w"))
            {
                rbBall.AddForce(tranCam.forward * speedMove * Time.deltaTime);
            }
            if (Input.GetKey("s"))
            {
                rbBall.AddForce(tranCam.forward * -speedMove * Time.deltaTime);
            }
            if (Input.GetKey("d"))
            {
                rbBall.AddForce(tranCam.right * speedMove * Time.deltaTime);
            }
            if (Input.GetKey("a"))
            {
                rbBall.AddForce(tranCam.right * -speedMove * Time.deltaTime);
            }
        }//end not in cannon
        else
        //else in a cannon
        {
            //cannon rotations            
            if (Input.GetKey("d"))
            {
                currentCannon.transform.Rotate(0, -speedRotate * Time.deltaTime, 0);
            }
            if (Input.GetKey("a"))
            {
                currentCannon.transform.Rotate(0, speedRotate * Time.deltaTime, 0);
            }

            //cannon shoot
            if (Input.GetKey("space"))
            {
                print("launch");
                transform.Translate(currentCannon.forward * 25 * Time.deltaTime, Space.Self);
                rbBall.isKinematic = false;
                rbBall.AddForce(currentCannon.up * powerCannon);
                inCannon = false;
            }
        }

    }//end update


    private void OnCollisionEnter(Collision col)
    {

        if(col.transform.tag == "Cannon")
        {            
            currentCannon = col.transform.GetChild(0) ;
            rbBall.isKinematic = true;
            transform.position = currentCannon.position;
            inCannon = true;
            col.transform.tag = "Untagged";
        }
        
    }//end of collision enter


}//end of playerball script
