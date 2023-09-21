using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLighting : MonoBehaviour
{
    [SerializeField] float targetIntensity = 2f;
    [SerializeField] float minIntensity = 1.5f;
    [SerializeField] float maxIntensity = 2f;
    [SerializeField] float flickerSpeed = 0.01f;
    [SerializeField] float flickerRate = 0.5f;

    void Start()
    {
        RandomIntensity();
        InvokeRepeating("RandomIntensity", 0.3f, 0.3f);
    }

    void Update()
    {
        if (this.gameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity > targetIntensity)
        {
            this.gameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity -= flickerSpeed;
        }
        else if (this.gameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity < targetIntensity)
        {
            this.gameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity += flickerSpeed;
        }
        else
        {
            RandomIntensity();
        }
    }

    void RandomIntensity()
    {
        targetIntensity = Random.Range(minIntensity, maxIntensity);
    }
}
