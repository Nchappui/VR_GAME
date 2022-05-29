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
        public UnityEvent PlaySound;
        private Rigidbody rb;
        public Vector3 LockOrientation;
        private bool isInLock = false;
        private bool willBeDestroyed=false;
        // Start is called before the first frame update
        void Start()
        {
            rb = this.GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isInLock)
            {
                LockOrientation.x += (float)2;
                this.transform.rotation = Quaternion.Euler(LockOrientation);
                if (!willBeDestroyed)
                {
                    willBeDestroyed = true;
                    StartCoroutine(DestroyKey());
                }
            }
        }
        
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.name == Lock.name)
            {
                rb.isKinematic = true;
                this.transform.position = other.transform.position;
                this.transform.rotation = Quaternion.Euler(LockOrientation);
                isInLock = true;


            }
        }
        IEnumerator DestroyKey()
        {
            PlaySound.Invoke();
            yield return new WaitForSeconds(2);
            Destroy(this.gameObject);
            UnlockDoor.Invoke();
        }

    }
}
