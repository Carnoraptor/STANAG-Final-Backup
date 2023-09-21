using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresentTileManager : MonoBehaviour
{
    public List<Sprite> floor1Walls = new List<Sprite>();

    public Sprite currentSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisplayNewTiles(List<Sprite> sprites)
    {
        for (int i = 0; i < sprites.Count; i++)
        {
            GameObject spriteSlot = GameObject.Find("SpriteSlot" + i);
            spriteSlot.GetComponent<Image>().sprite = sprites[i];
        }
    }

    void SelectNewTile(int tileIndex)
    {
        currentSprite = floor1Walls[tileIndex];
    }
}
