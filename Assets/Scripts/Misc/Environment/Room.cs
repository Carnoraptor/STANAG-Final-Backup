using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Room : MonoBehaviour
{
    public bool isCleared = false;
    public int maxEnemies;
    public int currentEnemiesRemaining;
    GameObject doorMap;
    public bool isStartingRoom = false;
    public GameObject droppedGun;

    bool rewardGiven = false;

    void Start()
    {
        droppedGun = Resources.Load("DroppedGun") as GameObject;

        GetDoorMap();
        if (currentEnemiesRemaining < 1)
        {
            RoomCleared(false, 0f);
        }
        if (isStartingRoom)
        {
            GameState.currentRoom = this;
        }

        CountEnemies();
    }

    public void EnemyWithinKilled()
    {
        Debug.Log("Damn, you killed an enemy in " + GameState.currentRoom);
        currentEnemiesRemaining -= 1;
        if (currentEnemiesRemaining < 1 && maxEnemies > 0)
        {
            RoomCleared(true, 0f);
        }
        else
        {
            RoomCleared(false, 0f);
        }
    }

    public void RoomCleared(bool giveReward, float magnitude)
    {
        Debug.Log("good job u cleared " + GameState.currentRoom.gameObject);
        isCleared = true;
        if (giveReward/* && rewardGiven == false*/)
        {
            GameObject gunDroppedObj = Instantiate(droppedGun, this.gameObject.transform.position, Quaternion.identity);
            gunDroppedObj.GetComponent<DroppedGun>().GenerateIdentity(ItemPools.allGuns[Random.Range(0, ItemPools.allGuns.Length - 1)]);
            rewardGiven = true;
        }
        //code to drop things. money, health, etc
    }

    void GetDoorMap()
    {
        for(int i = 0; i <= this.gameObject.transform.childCount - 1; i++) // need to add -1?
        {
            if (Utils.GenUtils.HasComponent<Grid>(this.gameObject.transform.GetChild(i).gameObject))
            {
                GameObject grid = this.gameObject.transform.GetChild(0).gameObject;
                for(int j = 0; j < 5; j++) //Edit this per the amount of children on the Grid object
                {
                    //Debug.Log(grid.transform.GetChild(j).gameObject.name);
                    if (grid.transform.GetChild(j).tag == "Door")
                    {
                        doorMap = grid.transform.GetChild(j).gameObject;
                        //Debug.Log(doorMap.gameObject.name);
                    }
                }
            }
        }
        //Debug.Log(doorMap.name);
    }

    public void OnRoomEnter()
    {
        GameState.currentRoom = this;
        if (isCleared == true)
        {
            GameState.currentRoomClear = true;
        }
        else
        {
            GameState.currentRoomClear = false;        
        }
    }

    public void OpenDoors()
    {
        doorMap.SetActive(false);
    }

    void CountEnemies()
    {
        for(int i = 0; i <= this.gameObject.transform.childCount - 1; i++) // need to add -1?
        {
            if (this.gameObject.transform.GetChild(i).gameObject.name == "Enemies")
            {
                maxEnemies = this.gameObject.transform.GetChild(i).gameObject.transform.childCount;
            }
        }
        
        currentEnemiesRemaining = maxEnemies;
    }
}
 