using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlankStick : MonoBehaviour
{
    private bool has_been_added = false;
    public GameObject right_plank;
    private int touch_count = 0;
    private PlankPuzzle pz;
 
    // Start is called before the first frame update
    void Start()
    {
        pz = right_plank.GetComponent<PlankPuzzle>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == right_plank.name)
        {
            touch_count++;
            if (touch_count == 2)
            {
                pz.wellPlacedPlanks++;
                has_been_added = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == right_plank.name)
        {
            touch_count--;
            if (has_been_added)
            {
                pz.wellPlacedPlanks--;
                has_been_added = false;
            }

        }
    }
}
