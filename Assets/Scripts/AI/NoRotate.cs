using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoRotate : MonoBehaviour
{
    [SerializeField]bool canRotate = false;
    [SerializeField]bool canFlip = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canRotate != true)
        {
            this.gameObject.transform.rotation = Quaternion.identity;
        }

        if (canFlip != false)
        {
            if (GameObject.FindWithTag("Player").transform.position.x > this.gameObject.transform.position.x)
            {
                FlipLeft();
            }
            else{
                FlipRight();
            }
        }
    }

    public void FlipLeft()
    {
        transform.localScale = new Vector3(1, 1, transform.localScale.z);
    }

    public void FlipRight()
    {
        transform.localScale = new Vector3(1 * (-1), 1, transform.localScale.z);
    }
}
