using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSTracker : MonoBehaviour
{
    TextMeshProUGUI fpsCounter;
    void Start()
    {
        fpsCounter = this.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        fpsCounter.text = (1 / Time.unscaledDeltaTime).ToString() + " FPS";
    }
}
