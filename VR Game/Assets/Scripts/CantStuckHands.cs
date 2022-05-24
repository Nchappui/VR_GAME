using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Valve.VR.InteractionSystem{

public class CantStuckHands : MonoBehaviour
{
    private new Collider collider;
    private bool closed = true;
    private float init_rot;
    // Start is called before the first frame update
    void Start()
    {
        collider = this.GetComponent<Collider>();
        init_rot = this.transform.parent.rotation.y * 100;
    }

    // Update is called once per frame
    void Update()
    {
            //print(this.transform.parent.rotation.y);
            //print("test");
            //print(transform.GetComponent<Rigidbody>().mass);
        if (closed & Math.Abs(this.transform.parent.rotation.y) * 100 > init_rot + 5 & collider.isTrigger==false)
        {
            closed = false;
            HandCanPassThrough();
        }

        if (!closed & Math.Abs(this.transform.parent.rotation.y) * 100 <= init_rot + 5)
        {
            closed = true;
            HandCanPassThrough();
        }
    }


    public void HandCanPassThrough()
    {
        collider.isTrigger = true;
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(10);
        collider.isTrigger = false;
    }
}
}