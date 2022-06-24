using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ViveInput : MonoBehaviour
{
    
    public SteamVR_Action_Vector2 InfinadeckAction;
    public float Speed = 100.0f;
    public GameObject hacking;
    private bool hacking_used = false;
    private TreadmillAlgorithm hacking_algo;
    //public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        if (hacking.gameObject.activeInHierarchy)
        {
            hacking_used = true;
            hacking_algo = hacking.GetComponent<TreadmillAlgorithm>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Player player = Player.instance;
        //print(player.transform.rotation);
        GameObject PlayerCamera = GameObject.Find("VRCamera");
        //print((PlayerCamera.transform.rotation * Vector3.forward));
        //Console.WriteLine("Values x and y: {0} {1} ", (PlayerCamera.transform.rotation * Vector3.forward).x, (PlayerCamera.transform.rotation * Vector3.forward).y);
        if (SteamVR_Actions.default_Forward.GetState(SteamVR_Input_Sources.Any))
        {
            //print("up has been pressed");
            Vector3 direction = (PlayerCamera.transform.rotation * Vector3.forward);
            direction.y = 0;
            print(direction);
            player.trackingOriginTransform.position += direction * Speed * Time.deltaTime;
            //player.transform.position += direction * Speed * Time.deltaTime;
            //player.trackingOriginTransform.position += direction * Time.deltaTime * Speed;
            //Vector3 playerFeetOffset = player.trackingOriginTransform.position - player.feetPositionGuess;
            //player.trackingOriginTransform.position = teleportPosition + playerFeetOffset;
            //transform.parent.position += direction * Time.deltaTime * Speed;
        }
        if(hacking_used)
        {
            Vector3 direction = new Vector3(hacking_algo.ySpeed, 0f, -hacking_algo.xSpeed);
            player.trackingOriginTransform.position += direction * Speed * Time.deltaTime;
        }
        
        /*
        Vector2 InfinadeckDirection = InfinadeckAction.GetAxis(SteamVR_Input_Sources.Any);

        if(InfinadeckDirection != Vector2.zero)
        {
            print(InfinadeckDirection);
            Vector3 Direction = new Vector3(InfinadeckDirection.x, 0, InfinadeckDirection.y);
            transform.parent.position += Direction * Time.deltaTime * Speed;
        }
        */
        
    }
}
