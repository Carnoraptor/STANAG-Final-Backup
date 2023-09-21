using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Category
{
    Wall,
    Floor,
    Door,
    Decor, 
    Hazard
}

[CreateAssetMenu (fileName = "Tile", menuName = "Tiles/Create New Tile")]
public class PlaceableTile : ScriptableObject
{
    [SerializeField] Category category;
    [SerializeField] TileBase tileBase;

    public Category Category{
        get{
            return category;
        }
    }

    public TileBase TileBase{
        get{
            return tileBase;
        }
    }

}
