using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public GunHandler gunHandler;

    public Sprite nullSprite;

    void Awake () 
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

	QualitySettings.vSyncCount = 0;  // VSync must be disabled
	Application.targetFrameRate = 60;
    DontDestroyOnLoad(this);

    gunHandler = GameObject.FindWithTag("Gun").GetComponent<GunHandler>();

    InvokeRepeating("CheckBulletNum", 0f, 1f);
    }

    
    public void Restart()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player>().skullIcon.SetActive(true);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        player = GameObject.FindGameObjectWithTag("Player");
        Vector2 pos = new Vector2(0, 0);
        player.transform.position = pos;
        Time.timeScale = 1;
        //player.GetComponent<Player>().OnSceneLoad(SceneManager.GetActiveScene());
    }

    public const int MAX_NUM_BULLETS = 15000;

    public void CheckBulletNum()
    {
        GameObject[] allBullets = GameObject.FindGameObjectsWithTag("EnemyProjectile");
        
        if (allBullets.Length > MAX_NUM_BULLETS + 1000)
        {
            ClearAllBullets(allBullets);
        }
        else if (allBullets.Length > MAX_NUM_BULLETS)
        {
            CleanBullets(allBullets);
        }
    }

    public void CleanBullets(GameObject[] allBullets)
    {
        Debug.Log("Cleaning " + allBullets.Length + " bullets!");
        for (int i = 0; i < allBullets.Length; i++)
        {
            Destroy(allBullets[allBullets.Length - 1]);
            if (allBullets.Length < MAX_NUM_BULLETS)
            {
                return;
            }
        }
    }

    public void ClearAllBullets(GameObject[] allBullets)
    {
        Debug.Log("Clearing " + allBullets.Length + " bullets!");
        foreach (GameObject bullet in allBullets)
        {
            Destroy(bullet);
        }
    }
}
