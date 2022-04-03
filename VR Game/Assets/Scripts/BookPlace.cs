using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BookPlace : MonoBehaviour
{
    public GameObject final_placement;
    public UnityEvent rightPlace;
    public UnityEvent wrongPlace;

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
        if (other.name == final_placement.name) {
            rightPlace.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == final_placement.name)
        {
            wrongPlace.Invoke();
        }
    }
}
