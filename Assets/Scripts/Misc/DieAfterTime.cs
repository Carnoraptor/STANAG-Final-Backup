using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAfterTime : MonoBehaviour
{
    [SerializeField] float timeUntilDeath;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DieSoon());
    }

    IEnumerator DieSoon()
    {
        yield return new WaitForSeconds(timeUntilDeath);
        Destroy(this.gameObject);
    }
}
