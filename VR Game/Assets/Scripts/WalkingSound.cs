using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WalkingSound : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 init_pos;
    private Vector3 previous_pose;
    private bool is_playing = false;

    public Transform Player;
    public AudioSource sound;
    void Start()
    {
        init_pos = Player.transform.position;
        previous_pose = init_pos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs((previous_pose.x- Player.transform.position.x) + (previous_pose.z - Player.transform.position.z))>0.01 && !is_playing)
        {
            StartCoroutine(example());
            
        }
        if (previous_pose == Player.transform.position)
        {
            //sound.Stop();
        }
        previous_pose = Player.transform.position;
    }

    IEnumerator example()
    {
        is_playing = true;
        sound.Play();
        yield return new WaitWhile(() => sound.isPlaying);
        is_playing = false;
    }
}

