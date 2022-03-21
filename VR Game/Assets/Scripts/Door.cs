using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private new Collider collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = this.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GrabbedHandle()
    {
        collider.isTrigger = true;
    }

    public void ReleasedHandle()
    {
        StartCoroutine(waiter());
        collider.isTrigger = false;
    }
    
    IEnumerator waiter()
    {
        yield return new WaitForSeconds(3);
    }
}
