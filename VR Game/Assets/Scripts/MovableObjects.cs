using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObjects : MonoBehaviour
{
    [Tooltip("Height at which the object is reset or deleted")]
    public float limitHeight = -10;

    [Tooltip("Position where the object will repsawn if destroyed")]
    public Vector3 RespawnPos;
    public Quaternion RespawnRot;
    public Vector3 RespawnScale { get; private set; }
    private void Awake()
    {
        var transform1 = transform;
        RespawnScale = transform1.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        print(transform.localPosition.y);
        if (transform.localPosition.y < limitHeight)
        {
            if (TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            
            var transform1 = transform;
            transform1.localPosition = RespawnPos;
            transform1.localRotation = RespawnRot;
            transform1.localScale = RespawnScale;

            
        }
        
    }
}
