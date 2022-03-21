using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
    public class Key : MonoBehaviour
    {
        public GameObject Lock;
        public UnityEvent UnlockDoor;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void AttachedToHand(Hand hand)
        {
            this.transform.rotation = hand.transform.rotation;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.name == Lock.name)
            {
                Rigidbody rb = other.transform.parent.GetComponentInParent<Rigidbody>();
                rb.isKinematic = false;
                Destroy(gameObject); 
                UnlockDoor.Invoke();

            }
        }

    }
}
