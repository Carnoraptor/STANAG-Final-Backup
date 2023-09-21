using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Sprite door1;
    [SerializeField] Sprite door2;
    [SerializeField] Sprite door3;

    SpriteRenderer renderer;

    void Start()
    {
        renderer = this.GetComponent<SpriteRenderer>();
        renderer.sprite = door1;
    }

    public void Open()
    {
        StartCoroutine(OpenDoor());
    }

    IEnumerator OpenDoor()
    {
        renderer.sprite = door2;
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<BoxCollider2D>().enabled = false;
        renderer.sprite = door3;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Open();
        }
    }
}
