using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CabinetSound : MonoBehaviour
{

    // Start is called before the first frame update
    private Quaternion init_rot;
    private Quaternion previous_rot;
    private bool is_playing = false;

    public AudioSource open;
    //public AudioSource close;
    void Start()
    {
        init_rot = this.transform.rotation;
        previous_rot = init_rot;
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(previous_rot.y - this.transform.rotation.y)>0.05 && !is_playing)
        {
            StartCoroutine(example());
            previous_rot = this.transform.rotation;
        }
        else if (previous_rot == this.transform.rotation && is_playing)
        {
            open.Stop();
        }
        /*
        if (previous_rot != this.transform.rotation && this.transform.rotation == init_rot)
        {
            close.Play();
        }
        */

    }

    IEnumerator example()
    {
        is_playing = true;
        open.Play();
        yield return new WaitWhile(() => open.isPlaying);
        is_playing = false;
    }
}
