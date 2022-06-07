using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLighting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.layer == 6)
        {
            RenderSettings.ambientIntensity = 0.8f;
        }
    }
}
