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
        private bool notSolved = true;

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
            if (collision.gameObject.layer == 3 && notSolved)
            {
                tried.Invoke();
            }
        }
        public void FoundKey()
        {
            notSolved = false;
        }


    }
}