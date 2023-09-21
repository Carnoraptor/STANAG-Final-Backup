using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnLoad : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    void SetToDestroy()
    {
        //DestroyOnLoad(this.gameObject);
    }
}
