using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ViveInput : MonoBehaviour
{
    
    public SteamVR_Action_Vector2 touchpadAction;
    // Start is called before the first frame update
    void Start()
    {
        
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
            Vector3 direction = (PlayerCamera.transform.rotation * Vector3.forward);
            direction.y = 0;
            transform.parent.position += direction * Time.deltaTime;
        }
        Vector2 touchpadValue = touchpadAction.GetAxis(SteamVR_Input_Sources.Any);

        if(touchpadValue != Vector2.zero)
        {
            //print(touchpadValue);
        }
        
    }
}
