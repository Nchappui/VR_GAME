using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayBirdSound : MonoBehaviour
{
    public UnityEvent reached;

    public bool detect_second = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.layer == 6)
        {
            if (!detect_second)
            {
                reached.Invoke();
                print("reached");
            }
            else
            {
                detect_second = false;
            }
        }
    }
}
