using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //[SerializeField] private Color baseColor, offsetColor;
    //[SerializeField] private SpriteRenderer spriteRenderer;
    public int ID;
    public bool isRoom;
    public bool isConnected = false;
    public bool isMidTile = false;

    public GameObject rightTile;
    public Tile rightScriptTile;
    public GameObject leftTile;
    public Tile leftScriptTile;
    public GameObject upTile;
    public Tile upScriptTile;
    public GameObject downTile;
    public Tile downScriptTile;

    //public bool hasDownWay, hasUpWay, hasLeftWay, hasRightWay;
    public int wayConfig;

    //SpriteRenderer tileRenderer;
    //[SerializeField] Color isRoomColor;

    void Start()
    {
        //tileRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(bool isOffset)
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.color = isOffset ? offsetColor : baseColor;
    }
}
