using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCollision : MonoBehaviour
{
    public Transform head;
    public Transform feet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(head.position.x, feet.position.y, head.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.transform.name);
        print(collision.gameObject.layer);
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.transform.name);
        print(other.gameObject.layer);
    }
}
