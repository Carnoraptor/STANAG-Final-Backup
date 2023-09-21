using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{
    //THE ISSUE IS THAT IT DIVIDES

    [SerializeField] private int width, height;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform cam;
    [SerializeField] int maxRooms = 18;
    [SerializeField] int minRooms = 12;
    [SerializeField] int tileDim = 1;
    int currentRooms = 0;
    int currentID = 1;

    //[SerializeField] private Color midRoomColor;
    private List<Tile> tileList = new List<Tile>();
    private List<Tile> roomList = new List<Tile>();
    private GameObject[] allTiles;

    public Tile midTile;

    //Room Prefab Lists
    [SerializeField] private GameObject[] LWayRooms;
    [SerializeField] private GameObject[] RWayRooms;
    [SerializeField] private GameObject[] UWayRooms;
    [SerializeField] private GameObject[] DWayRooms;
    [SerializeField] private GameObject[] LUWayRooms;
    [SerializeField] private GameObject[] LDWayRooms;
    [SerializeField] private GameObject[] RLWayRooms;
    [SerializeField] private GameObject[] RUWayRooms;
    [SerializeField] private GameObject[] RDWayRooms;
    [SerializeField] private GameObject[] UDWayRooms;
    [SerializeField] private GameObject[] RLUWayRooms;
    [SerializeField] private GameObject[] RUDWayRooms;
    [SerializeField] private GameObject[] LUDWayRooms;
    [SerializeField] private GameObject[] RLDWayRooms;
    [SerializeField] private GameObject[] RLUDWayRooms;

    public bool hasFinalLayout = false;
    GameObject player;
    //private SpriteRenderer midTileRend;

    void Start()
    {
        //Finding of objects
        player = GameObject.FindGameObjectWithTag("Player");

        //Initialization of arrays
        if (RWayRooms == null) { RWayRooms = new GameObject [1]; }
        if (LWayRooms == null) { LWayRooms = new GameObject [1]; }
        if (UWayRooms == null) { UWayRooms = new GameObject [1]; }
        if (DWayRooms == null) { DWayRooms = new GameObject [1]; }
        if (LUWayRooms == null) { LUWayRooms = new GameObject [1]; }
        if (LDWayRooms == null) { LDWayRooms = new GameObject [1]; }
        if (RLWayRooms == null) { RLWayRooms = new GameObject [1]; }
        if (RUWayRooms == null) { RUWayRooms = new GameObject [1]; }
        if (RDWayRooms == null) { RDWayRooms = new GameObject [1]; }
        if (UDWayRooms == null) { UDWayRooms = new GameObject [1]; }
        if (RLUWayRooms == null) { RLUWayRooms = new GameObject [1]; }
        if (RUDWayRooms == null) { RUDWayRooms = new GameObject [1]; }
        if (LUDWayRooms == null) { LUDWayRooms = new GameObject [1]; }
        if (RLDWayRooms == null) { RLDWayRooms = new GameObject [1]; }
        if (RLUDWayRooms == null) { RLUDWayRooms = new GameObject [1]; }

        if (allTiles == null) { allTiles = GameObject.FindGameObjectsWithTag("Tile"); }

        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = (-1 * (width / 2)) + (tileDim / 2); x < (width / 2) + (tileDim / 2); x += tileDim)
        {
            for (int y = (-1 * (height / 2)) + (tileDim / 2); y < (height / 2) + (tileDim / 2); y += tileDim)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity);
                //spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.isRoom = false;

                //var isOffset = (x % 60 == 0 && y % 60 != 0) || (x % 60 != 0 && y % 60 == 0);
                //spawnedTile.Init(isOffset);

                spawnedTile.ID = currentID;
                currentID += 1;
                spawnedTile.name = $"Tile {spawnedTile.ID}";
                tileList.Add(spawnedTile);
            }
        }

        cam.transform.position = new Vector3((float)width/2 -0.5f, (float)height / 2 - 0.5f,-10);
        GenerateBroadLayout();
    }

    public void GenerateBroadLayout()
    {
        float middleTileX = Mathf.Round(width / 2);
        float middleTileY = Mathf.Round(height / 2);

        GameObject middleTile = GameObject.Find("Tile " + (currentID / 2));
        midTile = middleTile.GetComponent(typeof(Tile)) as Tile;
        midTile.isRoom = true;
        //SpriteRenderer midTileRend = middleTile.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        //midTileRend.color = midRoomColor;
        
        //loop over every tile and check if it has a tile adjacent. loop that like 3 times or so ig.

        foreach (Tile t in tileList)
        {
            //var leftTile;
            //var rightTile;
            //var upTile;
            //var downTile;

            if(t.ID < (currentID - 11))
            {
                t.rightTile = GameObject.Find("Tile " + (t.ID + 11));
                t.rightScriptTile = t.rightTile.GetComponent(typeof(Tile)) as Tile;
            }
            else
            {
                t.rightTile = GameObject.Find("UndefinedTile");
                t.rightScriptTile = t.rightTile.GetComponent(typeof(Tile)) as Tile;
            }

            if(t.ID > 11)
            {
                t.leftTile = GameObject.Find("Tile " + (t.ID - 11));
                t.leftScriptTile = t.leftTile.GetComponent(typeof(Tile)) as Tile;
            }
            else
            {
                t.leftTile = GameObject.Find("UndefinedTile");
                t.leftScriptTile = t.leftTile.GetComponent(typeof(Tile)) as Tile;
            }
            
            //ISSUE HERE


            if(t.ID != tileList.Count){
            if(t.ID % Mathf.Sqrt(tileList.Count) != 1 || t.ID % Mathf.Sqrt(tileList.Count) != 2 || t.ID % Mathf.Sqrt(tileList.Count) != 3 || t.ID % Mathf.Sqrt(tileList.Count) != 4 || t.ID % Mathf.Sqrt(tileList.Count) != 5 || t.ID % Mathf.Sqrt(tileList.Count) != 6 || t.ID % Mathf.Sqrt(tileList.Count) != 7 || t.ID % Mathf.Sqrt(tileList.Count) != 8 || t.ID % Mathf.Sqrt(tileList.Count) != 9 ||t.ID % Mathf.Sqrt(tileList.Count) != 10 || t.ID % Mathf.Sqrt(tileList.Count) != 11)
            {
                t.upTile = GameObject.Find("Tile " + (t.ID + 1));
                t.upScriptTile = t.upTile.GetComponent(typeof(Tile)) as Tile;
            }
            else
            {
                t.upTile = GameObject.Find("UndefinedTile");
                t.upScriptTile = t.upTile.GetComponent(typeof(Tile)) as Tile;
            }
            }
            else
            {
                t.upTile = GameObject.Find("UndefinedTile");
                t.upScriptTile = t.upTile.GetComponent(typeof(Tile)) as Tile;
            }

            if(t.ID != 1 && t.ID != 12 && t.ID != 23 && t.ID != 34 && t.ID != 45 && t.ID != 56 && t.ID != 67 && t.ID != 78 && t.ID != 89 && t.ID != 100 && t.ID != 111 && t.ID != 121)
            {
                t.downTile = GameObject.Find("Tile " + (t.ID - 1));
                t.downScriptTile = t.downTile.GetComponent(typeof(Tile)) as Tile;
            }
            else
            {
                t.downTile = GameObject.Find("UndefinedTile");
                t.downScriptTile = t.downTile.GetComponent(typeof(Tile)) as Tile;
            }
        }

        for(int i=0; i<5; i++)
        {
            foreach (Tile t in tileList)
            {
                if (t.rightScriptTile.isRoom == true)
                {
                    int RNG = Random.Range(1, 4);
                    if (RNG == 1)
                    {
                        t.isRoom = true;
                    }
                }
                if (t.leftScriptTile.isRoom == true)
                {
                    int RNG = Random.Range(1, 4);//cumm
                    if (RNG == 1)//cumm
                    {//cumm
                        t.isRoom = true;
                    }
                }
                if (t.upScriptTile.isRoom == true)
                {
                    int RNG = Random.Range(1, 4);
                    if (RNG == 1)
                    {
                        t.isRoom = true;
                    }
                }
                if (t.downScriptTile.isRoom == true)
                {
                    int RNG = Random.Range(1, 4);
                    if (RNG == 1)
                    {
                        t.isRoom = true;
                    }
                }
            }
        }

        for(int i=0; i<5; i++)
        {
            foreach (Tile t in tileList)
            {
                if (t.downScriptTile.isRoom == true && t.upScriptTile.isRoom == true && t.leftScriptTile.isRoom == true && t.rightScriptTile.isRoom == true)
                {
                    int RNG = Random.Range(1, 3);
                    if(RNG > 1)
                    {
                        t.isRoom = false;
                        //Debug.Log("Room killed itself");
                    }
                }

                if (t.downScriptTile.isRoom == false && t.upScriptTile.isRoom == false && t.leftScriptTile.isRoom == false && t.rightScriptTile.isRoom == false)
                {
                    t.isRoom = false;
                    //Debug.Log("Room killed itself");
                }
            }
        }

        midTile.isRoom = true;
        midTile.isConnected = true;

        for(int i=0; i<5; i++)
        {
            foreach (Tile t in tileList)
            {
                if (t.rightScriptTile.isConnected == true && t.rightScriptTile.isRoom == true)
                {
                    t.isConnected = true;
                }
                else if (t.leftScriptTile.isConnected == true && t.leftScriptTile.isRoom)
                {
                    t.isConnected = true;
                }
                else if (t.upScriptTile.isConnected == true && t.upScriptTile.isRoom == true)
                {
                    t.isConnected = true;
                }
                else if (t.downScriptTile.isConnected == true && t.downScriptTile.isRoom == true)
                {
                    t.isConnected = true;
                }
                else if (t.transform.position.x == middleTile.transform.position.x && t.transform.position.y == middleTile.transform.position.y)
                {

                }
                else
                {
                    t.isConnected = false;
                    //Debug.Log("Nonconnector found! " + t.name);
                }
            }
        }
        
        foreach (Tile t in tileList)
        {
            if (t.isConnected == false)
            {
                t.isRoom = false;
                Destroy(t);
                //break;
            }

            if (t.isRoom)
            {
                currentRooms += 1;
            }
        }

        if (currentRooms > maxRooms)
        {
            //Debug.Log("Too many rooms!");
            KillAllTiles();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (currentRooms < minRooms)
        {
            //Debug.Log("Not enough rooms!");
            KillAllTiles();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            //Debug.Log("We've got enough of these mfs");
            hasFinalLayout = true;
            GenerateRooms();
        }

        //Debug.Log(currentRooms);
    }

    void GenerateRooms()
    {
        //player.transform.position = midTile.transform.position;

        foreach (Tile t in tileList)
        {
            if (t.isRoom == false)
            {
                Destroy(t);
            }
        }

        foreach (Tile t in tileList)
        {
            if (t.rightScriptTile.isRoom == true && t.isRoom == true)
                {
                    t.wayConfig += 1000;
                }
            if (t.leftScriptTile.isRoom == true && t.isRoom == true)
                {
                    t.wayConfig += 100;
                }
            if (t.upScriptTile.isRoom == true && t.isRoom == true)
                {
                    t.wayConfig += 10;
                }
            if (t.downScriptTile.isRoom == true && t.isRoom == true)
                {
                    t.wayConfig += 1;
                }

            //GameObject newTestObj = Instantiate(LWayRooms[0], t.transform.position, Quaternion.identity);

             //Universal Order: Right, Left, Up, Down -
            GameObject gennedRoom = null;

            switch (t.wayConfig)
            {
                //   RLUD
                case 1000:
                //1D R
                GameObject R = Instantiate(RWayRooms[0], t.transform.position, Quaternion.identity);
                gennedRoom = R;
                break;
                case 0100:
                GameObject L = Instantiate(LWayRooms[0], t.transform.position, Quaternion.identity);
                gennedRoom = L;
                //1D L
                break;
                case 0010:
                GameObject U = Instantiate(UWayRooms[0], t.transform.position, Quaternion.identity);
                gennedRoom = U;
                //1D U
                break;
                case 0001:
                //1D D
                GameObject D = Instantiate(DWayRooms[0], t.transform.position, Quaternion.identity);
                gennedRoom = D;
                break;  
                case 1100:
                //2D RL
                GameObject RL = Instantiate(RLWayRooms[0], t.transform.position, Quaternion.identity);
                gennedRoom = RL;
                break;
                case 1010:
                //2D RU
                GameObject RU = Instantiate(RUWayRooms[0], t.transform.position, Quaternion.identity);
                gennedRoom = RU;
                break;
                case 1001:
                //2D RD
                GameObject RD = Instantiate(RDWayRooms[0], t.transform.position, Quaternion.identity);
                gennedRoom = RD;
                break;
                case 0110:
                //2D LU
                GameObject LU = Instantiate(LUWayRooms[0], t.transform.position, Quaternion.identity);
                gennedRoom = LU;
                break;
                case 0101:
                //2D LD
                GameObject LD = Instantiate(LDWayRooms[0], t.transform.position, Quaternion.identity);
                gennedRoom = LD;
                break;
                case 0011:
                //2D UD
                GameObject UD = Instantiate(UDWayRooms[0], t.transform.position, Quaternion.identity);
                gennedRoom = UD;
                break;
                case 1110:
                //3D RLU
                GameObject RLU = Instantiate(RLUWayRooms[0], t.transform.position, Quaternion.identity);
                gennedRoom = RLU;
                break;
                case 1101:
                //3D RLD
                GameObject RLD = Instantiate(RLDWayRooms[0], t.transform.position, Quaternion.identity);
                gennedRoom = RLD;
                break;
                case 1011:
                //3D RUD
                GameObject RUD = Instantiate(RUDWayRooms[0], t.transform.position, Quaternion.identity);
                gennedRoom = RUD;
                break;
                case 0111:
                //3D LUD
                GameObject LUD = Instantiate(LUDWayRooms[0], t.transform.position, Quaternion.identity);
                gennedRoom = LUD;
                break;
                case 1111:  
                //4D RLUD
                GameObject RLUD = Instantiate(RLUDWayRooms[0], t.transform.position, Quaternion.identity);
                gennedRoom = RLUD;
                break;
            }

            if (t == midTile)
            {
                gennedRoom.GetComponent<Room>().isStartingRoom = true;
                GameState.currentRoom = gennedRoom.GetComponent<Room>();
            }
        }

        KillAllTiles();

        FinishGen();
    }

    void KillAllTiles()
    {
        allTiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject t in allTiles)
        {
            Destroy(t);
        }
    }

    void FinishGen()
    {
        GameObject astar = GameObject.FindWithTag("Astar");

        astar.GetComponent<AstarPath>().Scan();
    }
}