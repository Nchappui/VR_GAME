using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OpenHouseDoor : MonoBehaviour
{
    private Vector3 init_pos;
    private Vector3 open_pos;
    private Quaternion init_rot;
    private Quaternion open_rot;
    public Transform open_pos_tranform;
    private bool is_openning = false;
    public float speed = 1.0F;
    private float startTime;
    private float distance;

    private bool left_good_code;
    private bool right_good_code;

    public UnityEvent isOpenning;

    // Start is called before the first frame update
    void Start()
    {
        init_pos = this.transform.position;
        open_pos = open_pos_tranform.position;
        init_rot = this.transform.rotation;
        open_rot = open_pos_tranform.rotation;
        distance = Vector3.Distance(init_pos, open_pos);
    }

    // Update is called once per frame
    void Update()
    {
        if (is_openning)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / distance;
            this.transform.position = Vector3.Lerp(init_pos, open_pos, fractionOfJourney);
            this.transform.rotation = Quaternion.Lerp(init_rot, open_rot, fractionOfJourney);
            if (this.transform.position == open_pos)
            {
                is_openning = false;
            }
        }
    }

    public void change_left_true()
    {
        left_good_code = true;
        if (right_good_code)
        {
            startTime = Time.time;
            is_openning = true;
        }


    }

    public void change_right_true()
    {
        right_good_code = true;
        if (left_good_code)
        {
            startTime = Time.time;
            is_openning = true;
        }
    }

    public void change_left_wrong()
    {
        left_good_code = false;
    }
    public void change_right_wrong()
    {
        right_good_code = false;
    }

    public void open_gate()
    {
        if (!is_openning)
        {
            isOpenning.Invoke();
            startTime = Time.time;
            is_openning = true;
        }
    }
}
