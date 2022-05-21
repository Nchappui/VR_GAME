using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalculateWeight : MonoBehaviour
{
    private float weight=0;
    public TextMeshPro text;
    public float target_weight;
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
        if(other.gameObject.TryGetComponent(out Weight w))
        {
            weight += w.weight;
            text.text = weight.ToString();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Weight w))
        {
            weight -= w.weight;
            text.text = weight.ToString();
        }
    }

    public bool enough_weight()
    {
        if (target_weight <= weight)
        {
            return true;
        }
        return false;
    }
}
