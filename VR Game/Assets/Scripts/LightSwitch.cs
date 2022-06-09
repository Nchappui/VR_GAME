using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    private new Light light;
    private float init_int;
    // Start is called before the first frame update
    void Start()
    {
        light = this.GetComponentInChildren<Light>();
        init_int = light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Switch_Light()
    {
        if (light.intensity > 0.2f) light.intensity = 0.2f;
        else light.intensity = init_int;
    }
}
