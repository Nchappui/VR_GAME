using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Valve.VR.InteractionSystem
{
    public class NarratorTuto6 : MonoBehaviour
    {
        public UnityEvent tried;
        private CircularDrive cd;
        public bool keyNotFound = true;

        // Start is called before the first frame update
        void Start()
        {
            cd = this.GetComponent<CircularDrive>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnCollisionEnter(Collision collision)
        {
            print(collision.gameObject.layer);
            if (collision.gameObject.layer == 3 && keyNotFound)
            {
                tried.Invoke();
            }
        }
        public void FoundKey()
        {
            keyNotFound = false;
        }


    }
}