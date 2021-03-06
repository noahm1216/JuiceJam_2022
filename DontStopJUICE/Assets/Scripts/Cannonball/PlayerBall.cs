using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    public bool justMovement;
    public Rigidbody rbBall;
    public float speedMove;
    public Transform tranCam;

    public Transform currentCannon;
    public float speedRotate;
    public bool inCannon;
    public float powerCannon;

    public GameObject camView, camScore;

    // Update is called once per frame
    void Update()
    {
        if (justMovement)
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
        }
        //if the cannonball one
        else
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
                    currentCannon.transform.Rotate(speedRotate * Time.deltaTime, 0, 0);
                }
                if (Input.GetKey("a"))
                {
                    currentCannon.transform.Rotate(-speedRotate * Time.deltaTime, 0, 0);
                }

                //cannon shoot
                if (Input.GetKey("space"))
                {
                    print("launch");
                    transform.position = currentCannon.GetChild(0).position;
                    rbBall.isKinematic = false;
                    rbBall.AddForce(currentCannon.forward * powerCannon);
                    //rbBall.AddRelativeForce(currentCannon.forward * powerCannon);
                    inCannon = false;
                    currentCannon.parent.tag = "Cannon";
                    currentCannon = null;
                }
            }
        }

    }//end update


    private void OnCollisionEnter(Collision col)
    {
        if (!justMovement)
        {



            if (col.transform.tag == "Cannon")
            {
                currentCannon = col.transform.GetChild(0);
                rbBall.isKinematic = true;
                transform.position = currentCannon.position;
                inCannon = true;
                col.transform.tag = "Untagged";
            }
        }
    }//end of collision enter

    //enter
    private void OnTriggerEnter(Collider trig)
    {
        if (!justMovement)
        {


            if (trig.tag == "ChangeCamera")
            {
                camView.SetActive(false);
                camScore.SetActive(true);
            }
        }
    }//trigger enter

    //exit
    private void OnTriggerExit(Collider trig)
    {
        if (!justMovement)
        {


            if (trig.tag == "ChangeCamera")
            {
                camView.SetActive(true);
                camScore.SetActive(false);
            }
        }
    }//trigger exit


}//end of playerball script
