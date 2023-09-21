using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SurviveLoad : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
