using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

namespace Valve.VR.InteractionSystem.Sample
{
    public class HintButton : MonoBehaviour
    {
        public HoverButton hoverButton;

        public GameObject prefab;
        
        private int hintNumber;

        private void Start()
        {
            hoverButton.onButtonDown.AddListener(OnButtonDown);
            hintNumber = 1;
        }

        private void OnButtonDown(Hand hand)
        {
            StartCoroutine(GetHint());
        }

        private IEnumerator GetHint()
        {
            GameObject hint = GameObject.Instantiate<GameObject>(prefab);
            hint.transform.position = this.transform.position;
            TextMeshPro hintText = hint.GetComponentInChildren<TextMeshPro>();
            hintText.text = "hint no " + hintNumber.ToString();
            hintNumber += 1;


            Rigidbody rigidbody = hint.GetComponent<Rigidbody>();
            if (rigidbody != null)
                rigidbody.isKinematic = true;


            Vector3 initialScale = Vector3.one * 0.01f;
            //Vector3 targetScale = Vector3.one * (1 + (Random.value * 0.25f));
            Vector3 targetScale = prefab.transform.localScale;

            float startTime = Time.time;
            float overTime = 0.5f;
            float endTime = startTime + overTime;

            while (Time.time < endTime)
            {
                hint.transform.localScale = Vector3.Slerp(initialScale, targetScale, (Time.time - startTime) / overTime);
                yield return null;
            }


            if (rigidbody != null)
                rigidbody.isKinematic = false;
        }
    }
}