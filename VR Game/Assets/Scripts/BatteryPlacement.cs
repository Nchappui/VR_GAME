using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BatteryPlacement : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Placement;
    public UnityEvent working_projector;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == Placement.name)
        {
            Destroy(this.gameObject);
            foreach (MeshRenderer renderer in other.GetComponentsInChildren<MeshRenderer>())
            {
                renderer.enabled = true;
            }
            working_projector.Invoke();

        }
    }
}
