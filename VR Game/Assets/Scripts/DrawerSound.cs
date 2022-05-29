using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerSound : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 init_pos;
    private Vector3 previous_pose;
    private bool is_playing = false;

    public AudioSource open;
    public AudioSource close;
    void Start()
    {
        init_pos = this.transform.position;
        previous_pose = init_pos;
    }

    // Update is called once per frame
    void Update()
    {
        if (previous_pose != this.transform.position && !is_playing)
        {
            StartCoroutine(example());
            previous_pose = this.transform.position;
        }
        else if (previous_pose == this.transform.position && is_playing)
        {
            open.Stop();
        }
        if (previous_pose != this.transform.position && this.transform.position == init_pos)
        {
            close.Play();
        }

    }

    IEnumerator example()
    {
        is_playing = true;
        open.Play();
        yield return new WaitWhile(() => open.isPlaying);
        is_playing = false;
    }
}
