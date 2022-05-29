using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Valve.VR.InteractionSystem
{
    public class BookPlace : MonoBehaviour
    {
        public GameObject final_placement;
        public UnityEvent rightPlace;
        public UnityEvent wrongPlace;

        public Hand left;
        public Hand right;

        private bool placed = false;
        private Interactable interactable;

        // Start is called before the first frame update
        void Start()
        {
            interactable = this.GetComponent<Interactable>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name.Contains("Book_placement") && !placed)
            {
                //print("Found placement");
                left.DetachObject(this.gameObject);
                right.DetachObject(this.gameObject);
                this.transform.rotation = Quaternion.Euler(0, - 90, 0);
                this.transform.position=other.transform.position;
                placed = true;
            }

            if (other.name == final_placement.name)
            {
                rightPlace.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.name.Contains("Book_placement"))
            {
                placed = false;
            }

            if (other.name == final_placement.name)
            {
                wrongPlace.Invoke();
            }
        }
    }
}