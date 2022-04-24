using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObjects : MonoBehaviour
{
    [Tooltip("Height at which the object is reset or deleted")]
    public float limitHeight = -10;

    [Tooltip("Position where the object will repsawn if destroyed")]
    public Vector3 RespawnPos;
    public Quaternion RespawnRot { get; private set; }
    public Vector3 RespawnScale { get; private set; }
    private void Awake()
    {
        var transform1 = transform;
        RespawnPos = transform1.position;
        RespawnRot = transform1.rotation;
        RespawnScale = transform1.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < limitHeight)
        {
            var transform1 = transform;
            transform1.position = RespawnPos;
            transform1.rotation = RespawnRot;
            transform1.localScale = RespawnScale;

            if (TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
