using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaitForXSeconds : MonoBehaviour
{
    public UnityEvent waiting_done;
    public float seconds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void start_waiting()
    {
        print("started");
        print(seconds);
        StartCoroutine(enumerator());
    }
    IEnumerator enumerator()
    {
        print("in it"); 
        yield return new WaitForSeconds(5);
        print("done");
        waiting_done.Invoke();
    }
}
