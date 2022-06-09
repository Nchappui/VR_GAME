using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class updateUserCube : MonoBehaviour
{
    private Rigidbody u_rb;
    public InfinadeckConnection conn;
    // Start is called before the first frame update

    public GameObject user;
    public CharacterController userController;


    void Start()
    {
        u_rb = user.GetComponent<Rigidbody>();
        conn = GameObject.FindGameObjectWithTag("GameController").GetComponent<InfinadeckConnection>();
        user = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        GetPosition();
        GetVelocity();

        //velocity updates example 
        //SetVelocity(0.01f, 0.02f);
        SetVelocity(conn.getFilteredSpeeds()[0], conn.getFilteredSpeeds()[1]);

        //position updates example
        //SetPosition(0f, 3f);

        GetPosition();
        GetVelocity();
    }

    void GetPosition()
    {
        //Debug.Log("X coordinate: " + user.transform.position.x + ", Z coordinate: " + user.transform.position.z);
    }

    void GetVelocity()
    {
        //Debug.Log("X speed: " + u_rb.velocity.x + ", Z speed: " + u_rb.velocity.z);
    }

    void SetPosition(float xpos, float zpos)
    {
        //position precision threshold prevents the object jitters from infinite pos updates
        if ((Mathf.Abs(xpos-userController.transform.position.x)>=1f)||(Mathf.Abs(zpos - userController.transform.position.z) >= 1f)) 
        {
            userController.Move(new Vector3(xpos - userController.transform.position.x, 0, zpos - userController.transform.position.z));
        }
    }

    void SetVelocity(float xspeed, float zspeed) //choose params < 1 for better visibility
    {
        userController.Move(new Vector3(-1 * xspeed, u_rb.velocity.y, zspeed)/100);
        //userController.Move(Vector3.right * xspeed + Vector3.up * u_rb.velocity.y + Vector3.forward * zspeed);
        //bugfix to stop the cube spinning (set rotation constraints on XYZ) was found here: https://forum.unity.com/threads/how-do-i-stop-cube-from-rotating-while-moving.595033/
    }
}
