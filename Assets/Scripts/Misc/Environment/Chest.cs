using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject droppedGun;
    Vector2 gunDropPos;
    //public ItemPools itemPools;

    Gun gunToSpawn;

    void Start()
    {
        //itemPools = Resources.Load<ItemPools>("Data/ItemPools");
        //gunToSpawn = itemPools.allGuns[1];
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            OpenChest(ItemPools.allGuns[Random.Range(1, 8)]);
        }
    }

    void OpenChest(Gun gunToGenerate)
    {
        gunDropPos.Set(this.transform.position.x, this.transform.position.y - 2);
        GameObject gunDroppedObj = Instantiate(droppedGun, gunDropPos, Quaternion.identity);
        DroppedGun dG = gunDroppedObj.GetComponent<DroppedGun>();
        Debug.Log("Gun Contains " + gunToGenerate._gunName);
        dG.GenerateIdentity(gunToGenerate);
    }
}
