using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using TMPro;

public class BossGeneralAI : GeneralAI
{
    public BossAI bossAI;

    public GameObject bossUI;
    public GameObject bossUIInst;
    public Slider bossHealthSlider;
    public EventReference bossMusic;

    public int numPhases;
    public int phase;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.tag="Enemy"; 
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); //remember to have image as a child of the enemy object
        rb2d = GetComponent<Rigidbody2D>();
        playerObj = GameObject.FindWithTag("Player");
        playerScript = playerObj.GetComponent<Player>();

        //Main
        enemyName = bossAI._enemyName;
        enemyType = bossAI._enemyType;
        enemyID = bossAI._enemyID;
        //Stats
        enemyHP = bossAI._enemyHP;
        enemyArmor = bossAI._enemyArmor;
        enemyDamage = bossAI._enemyDamage;
        enemyArmorPierce = bossAI._enemyArmorPierce;
        enemySpeed = bossAI._enemySpeed;
        enemyAttackRate = bossAI._enemyAttackRate;
        enemyCurrentHealth = enemyHP;
        //Behavior
        doesContactDamage = bossAI._doesContactDamage;
        contactDamage = bossAI._contactDamage;
        contactAP = bossAI._contactAP;
        numPhases = bossAI._numPhases;
        //Graphics & Prefabs
        enemySprite = bossAI._enemySprite;
        enemyUtils.SetSpriteToReal(this);
        startingPosition = transform.position;
        bossMusic = bossAI._bossMusic;

        bossUIInst = Instantiate(bossUI, new Vector3(960f, 60f, 0f), Quaternion.identity);
        bossHealthSlider = GameObject.Find("BossHealthSlider").GetComponent<Slider>();
        bossHealthSlider.maxValue = enemyHP;

        GameObject.Find("BossTitle").GetComponent<TextMeshProUGUI>().text = bossAI._enemyName + ", " + bossAI._bossTitle;
        //GameObject.Find("BossNamePlate").GetComponent<TextMeshProUGUI>().text = bossAI._enemyName;

        phase = 1;
        

        ChangeBossHealthSlider();

        //Play boss music
        MusicManager.instance.ChangeMusic(bossMusic);
    }

    public override void TakeDamage(int damage, int armorPierce)
    {
        int armorLeft = enemyArmor - armorPierce;
            if (armorLeft < 0)
            {
                armorLeft = 0;
            }
            //Debug.Log(enemyName + " has " + armorLeft + " armor unpierced");
            int damagePassed = damage - armorLeft;
            if (damagePassed < 0)
            {
                damagePassed = 0;
            }
            //Debug.Log(enemyName + " takes " + damagePassed + " damage"); 
            enemyCurrentHealth -= damagePassed;
            if (numPhases == 2 && enemyCurrentHealth < (enemyHP / 2) && phase == 1)
            {
                ChangePhase(2);
            }
            if (numPhases == 3 && enemyCurrentHealth < (enemyHP / 3) * 2 && phase == 1)
            {
                ChangePhase(2);
            }
            if (numPhases == 3 && enemyCurrentHealth < (enemyHP / 3) && phase == 2)
            {
                ChangePhase(3);
            }
            if (enemyCurrentHealth <= 0)
            {
                enemyCurrentHealth = 0;
                Die();
            }

            ChangeBossHealthSlider();
    }

    //make this a delegate void
    public void ChangePhase(int newPhase)
    {

    }

    void ChangeBossHealthSlider()
    {   
        bossHealthSlider.value = enemyCurrentHealth;
    }

    void OnDestroy()
    {
        Destroy(bossUIInst);
    }
}
