using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    private new Light light;
    private float init_range;
    // Start is called before the first frame update
    void Start()
    {
        light = this.GetComponentInChildren<Light>();
        init_range = light.range;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Switch_Light()
    {
        if (light.range > 0) light.range = 0;
        else light.range = init_range;
    }
}
