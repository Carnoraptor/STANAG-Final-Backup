using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using FMODUnity;
using FMOD.Studio;
using Utils;
using MoreMountains.Feedbacks;

public class Player : MonoBehaviour
{
    Rigidbody2D rb2d;

    float horizontal;
    float vertical;
    //float scaleX = 5;

    //Movement Variables
    public float moveSpeed = 12f;

    public float accelerationRate = 70;
    public float deaccelerationRate = 50;
    public bool decelBoostSent = false;

    public float currentSpeed = 0;
    public float initSpeed = 35f;
    
    public bool isMoving = false;
    Vector2 movementVector;
    Vector2 direction;

    bool isSlowed = false;

    //Input leniency variables
    public float timeSinceW;
    public float lastWPressed;
    public float timeSinceA;
    public float lastAPressed;
    public float timeSinceS;
    public float lastSPressed;
    public float timeSinceD;
    public float lastDPressed;

    public float leniencyTime;


    //DashVariables
    public bool isDashing = false;
    public bool canDash = true;
    public float dashSpeed = 30f;
    public float dashCooldown = 1f;
    public float dashTime = 0.2f;

    public float stamina = 150f;
    public float maxStamina = 150f;
    public float staminaRechargeRate = 0.5f;

    public MMFeedbacks dashFeedback;

    //Player Variables
    public Transform playerTransform;
    public Vector2 position;
    Animator playerAnimator;
    AudioSource audioSource;

    //Gun Variables
    GameObject gunObj;
    GunHandler gunHandler;

    //Backend Variables
    float scaleX = 1;
    float scaleY = 1f;

    //Health Variables
    public int playerMaxHealth = 100;
    public int playerCurrentHealth;
    public int playerArmor;
    public bool playerDead = false;
    public float IFrames = 1.4f;
    public bool invincible = false;

    //Sounds
    public EventReference playerFootstepsRef;
    [SerializeField] public EventInstance playerFootsteps;

    //UI
    public Slider healthSlider;
    public Slider staminaSlider;
    public GameObject deathScreen;
    public float skullFlashInterval = 0.7f;
    public float skullFlashTimes = 5f;
    public GameObject skullIcon;
    public Material fadeMat;
    public Material bgFadeMat;
    public Animation deathScreenAnim;

    private static Player playerInstance;


    void Awake()
    {
        DontDestroyOnLoad(this);

                 
        if (playerInstance == null) 
        {
            playerInstance = this;
        } 
        else 
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        gunObj = GameObject.FindWithTag("Gun");

        if(SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "LevelDesigner" && SceneManager.GetActiveScene().name != "RoomDesignScene")
        {
            gunHandler = gunObj.GetComponent<GunHandler>();
        }
        playerCurrentHealth = playerMaxHealth;
        Time.timeScale = 1f;

        ResetDeathAlpha();

        //delegate onsceneload
        SceneManager.sceneLoaded += OnSceneLoad;
        
        //audio shenanigans
        //playerFootsteps = AudioManager.instance.CreateInstance(playerFootstepsRef);

        staminaSlider = GameObject.Find("StaminaBar").GetComponent<Slider>();
    
        StartCoroutine(InvincibilityFrames(3f));
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 4f;
            isSlowed = true;
            scaleY = 0.5f;
            transform.localScale = new Vector3(transform.localScale.x, scaleY, transform.localScale.z);
            if (currentSpeed > 4)
            {
                currentSpeed = 4;
            }
        }
        else
        {
            moveSpeed = 12f;
            isSlowed = false;
            scaleY = 0.8f;
            transform.localScale = new Vector3(transform.localScale.x, scaleY, transform.localScale.z);
        }
        RegisterInputTimes();
        IsReceivingMovementInput();
        Move();
        if (Input.GetKeyDown(KeyCode.Space) && stamina > 50f)
        {
            Dash();
        }

        //change this to just getkeydown and getkeyup?

        position = transform.position;
    }

    void FixedUpdate()
    {
        if (stamina < maxStamina)
        {
            stamina += staminaRechargeRate;
        }
        staminaSlider.value = stamina;
    }

    void MovementCalculations()
    {
        if (rb2d.velocity.x != 0 || rb2d.velocity.y != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }


        if (IsReceivingMovementInput())
        {
            //if footsteps isnt playing play footsteps
            PLAYBACK_STATE playbackState;
            playerFootsteps.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerFootsteps.start();
            }
        }
        else
        {
            playerFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }


        /*if (isMoving == true && currentSpeed < moveSpeed && IsReceivingMovementInput() == true)
        {
            currentSpeed *= accelerationRate;
            decelBoostSent = false;
        }
        else if (isMoving == false && IsReceivingMovementInput() == true) 
        {
            currentSpeed = initSpeed;
        }
        else if (isMoving == true && IsReceivingMovementInput() == false && decelBoostSent == false)
        {
            currentSpeed = currentSpeed / deaccelerationRate;

            Vector2 decelDir = DetermineMostRecentInputPair();
            //Debug.Log(decelDir);
            //rb2d.AddRelativeForce(decelDir * currentSpeed);

            //rb2d.AddForce(rb2d.velocity * 500f); should be currentSpeed
            decelBoostSent = true;
        }*/



        //misc shit
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down

        direction = new Vector2(horizontal, vertical).normalized;
    }

    public bool IsReceivingMovementInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            //Debug.Log("Recieving Movement Input");
            return true;
        }
        else
        {
            //Debug.Log("Not Recieving Movement Input");
            return false;
        }
    }

    //Input leniency
    public void RegisterInputTimes()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            lastWPressed = Time.time;
            timeSinceW = 0;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            lastAPressed = Time.time;
            timeSinceA = 0;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            lastSPressed = Time.time;
            timeSinceS = 0;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            lastDPressed = Time.time;
            timeSinceD = 0;
        }
    }

    public void UpdateInputTimes()
    {
        if (!Input.GetKey(KeyCode.W))
        {
            timeSinceW = Time.time - lastWPressed;
        }
        if (!Input.GetKey(KeyCode.A))
        {
            timeSinceA = Time.time - lastAPressed;
        }
        if (!Input.GetKey(KeyCode.S))
        {
            timeSinceS = Time.time - lastSPressed;
        }
        if (!Input.GetKey(KeyCode.D))
        {
            timeSinceD = Time.time - lastDPressed;
        }
    }

    Vector2 DetermineMostRecentInputPair()
    {
        UpdateInputTimes();
        /*float[] times = new float[4];
        times[0] = timeSinceW;
        times[1] = timeSinceA;
        times[2] = timeSinceS;
        times[3] = timeSinceD;
        float[] sortedTimes = times.Sort(times);
        Debug.Log(times);*/

        List<float> times = new List<float>();
        times.Add(timeSinceW);
        times.Add(timeSinceA);
        times.Add(timeSinceS);
        times.Add(timeSinceD);
        times.Sort();

        //checking for input times
        if (times[0] + leniencyTime < times[1])
        {
            if (times[0] == timeSinceW){return new Vector2(0, 1);}
            if (times[0] == timeSinceA){return new Vector2(-1, 0);}
            if (times[0] == timeSinceS){return new Vector2(0, -1);}
            if (times[0] == timeSinceD){return new Vector2(1, 0);}
        }


        if ((times[0] == timeSinceW && times[1] == timeSinceA) || (times[0] == timeSinceA && times[1] == timeSinceW)) { return new Vector2(-1, 1); }
        if ((times[0] == timeSinceW && times[1] == timeSinceS) || (times[0] == timeSinceS && times[1] == timeSinceW))  { return new Vector2(0, 0); }
        if ((times[0] == timeSinceW && times[1] == timeSinceD) || (times[0] == timeSinceD && times[1] == timeSinceW)) { return new Vector2(1, 1); }
        if ((times[0] == timeSinceA && times[1] == timeSinceS) || (times[0] == timeSinceS && times[1] == timeSinceA)) { return new Vector2(-1, -1); }
        if ((times[0] == timeSinceA && times[1] == timeSinceD) || (times[0] == timeSinceD && times[1] == timeSinceA)) { return new Vector2(0, 0); }
        if ((times[0] == timeSinceS && times[1] == timeSinceD) || (times[0] == timeSinceD && times[1] == timeSinceS)) { return new Vector2(1, -1); }
        else return Vector2.zero;

        //float[] sortedTimes = times.OrderBy(time => time.Value);
    }

    void Move()
    {
        //MovementCalculations();
        //Flip(); WORK ON FLIPPING PLAYER AS IT CURRENTLY DISLIKES DOING THAT
        if (isDashing == false)
        {
            if (IsReceivingMovementInput())
            {
                //this method of movement is becoming an issue with deacceleration and making movement feel good. i need it to start feeling better, but i'm way too tired for that now. this is going on the trello
                //ill fix it later lmao
                movementVector = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
                rb2d.velocity = movementVector;
            }
        }
        MovementCalculations();
    }

    public void FlipLeft()
    {
        transform.localScale = new Vector3(scaleX, scaleY, transform.localScale.z);
    }

    public void FlipRight()
    {
        transform.localScale = new Vector3(scaleX * (-1), scaleY, transform.localScale.z);
    }



    public void Dash()
    {
        //Debug.Log("Dashing!");
        DetermineMostRecentInputPair();

        isDashing = true;

        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);
        Vector3 mouseScreenPosition = Input.mousePosition;

        Vector3 playerToMouseVector = (mouseScreenPosition - playerScreenPosition).normalized;
    
        rb2d.velocity = playerToMouseVector * dashSpeed;

        stamina -= 50f;

        StartCoroutine(DashPause());

        StartCoroutine(DashCooldown());
    }

    IEnumerator DashPause()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.07f);
        Time.timeScale = 1f;
    }

    IEnumerator DashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;  
        canDash = true;
    }



    //Health Functions
    public void TakeDamage(int damage, int armorPierce)
    {
        int armorLeft = playerArmor - armorPierce;
        if (armorLeft < 0)
        {
            armorLeft = 0;
        }
        int damagePassed = damage - armorLeft;
        if (damagePassed < 0)
        {
            damagePassed = 0;
        }

        if (invincible == false)
        {
            playerCurrentHealth -= damagePassed;
            StartCoroutine(InvincibilityFrames(IFrames));
            RefreshHealthUI();
        }
        Debug.Log("ow");

        //DEATH

        if (playerCurrentHealth <= 0 && playerDead == false)
        {
            playerDead = true;
            Time.timeScale = 0.4f;
            StartCoroutine(PlayerDeath());
        }
        
    }

    public IEnumerator PlayerDeath()
    {
        Debug.Log("deadass ngl");
        StartCoroutine(FlashSkull());
        yield return StartCoroutine(Utils.GenUtils.WaitForTrueSeconds(skullFlashInterval * skullFlashTimes * 2 * (1 + (1 - Time.timeScale))));
        deathScreen.SetActive(true);
        healthSlider.gameObject.SetActive(false);
        InvokeRepeating("FadeDeathBG", 0f, 0.001f);
        yield return StartCoroutine(Utils.GenUtils.WaitForTrueSeconds(0.5f));
        InvokeRepeating("FadeDeathScreen", 0f, 0.001f);
        //FadeDeathScreen();
        //Make the animation stop and set as exclusively the opened scroll
        //Make the background fade to gray when death
    }

    public IEnumerator FlashSkull()
    {
        skullIcon.SetActive(true);
        for (var i = 0; i < skullFlashTimes; i++)
        {
            skullIcon.SetActive(true);
            yield return StartCoroutine(Utils.GenUtils.WaitForTrueSeconds(skullFlashInterval * (1 + (1 - Time.timeScale))));
            skullIcon.SetActive(false);
            yield return StartCoroutine(Utils.GenUtils.WaitForTrueSeconds(skullFlashInterval * (1 + (1 - Time.timeScale))));
        }
    }

    void FadeDeathBG()
    {

        if (bgFadeMat.color.a < 2)
        {
            Color color = bgFadeMat.color ;
            color.a = bgFadeMat.color.a + 0.002f;
            bgFadeMat.color = color;
        }
        else
        {
            CancelInvoke("FadeDeathScreen");
        }
    }

    void FadeDeathScreen()
    {
        if (fadeMat.color.a < 2)
        {
            Color color = fadeMat.color ;
            color.a = fadeMat.color.a + 0.002f;
            fadeMat.color = color;
        }
        else
        {
            CancelInvoke("FadeDeathScreen");
        }
    }

    public IEnumerator InvincibilityFrames(float num)
    {
        invincible = true;
        StartCoroutine(FlashPlayer());
        yield return new WaitForSeconds(num);
        invincible = false;
    }

    public IEnumerator FlashPlayer()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds (0.2f);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds (0.2f);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds (0.2f);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds (0.2f);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds (0.2f);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds (0.2f);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds (0.2f);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }




    //Collisions
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.gameObject.tag == "Door" && GameState.currentRoomClear)
        {
            //Debug.Log("door open");
            col.transform.parent.parent.gameObject.GetComponent<Room>().OpenDoors();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.gameObject.tag == "Floor")
        {
            //Debug.Log("susy baka " + col.gameObject);
            GameState.currentRoom = col.gameObject.transform.parent.parent.gameObject.GetComponent<Room>();
            //Debug.Log("Current room is " + GameState.currentRoom);
        }
    }





    //UI
    public void RefreshHealthUI()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find("Health Bar").GetComponent<Slider>();   
        }
        healthSlider.value = playerCurrentHealth;
    }

    public void ResetDeathAlpha()
    {
        Color color = fadeMat.color ;
        color.a = 0;
        fadeMat.color = color;

        Color color2 = bgFadeMat.color ;
        color2.a = 0;
        bgFadeMat.color = color2;
    }






    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded");
        playerDead = false;
        playerCurrentHealth = playerMaxHealth;
        healthSlider = GameObject.Find("Health Bar").GetComponent<Slider>();
        deathScreen = GameObject.Find("DeathMenu");
        skullIcon = GameObject.Find("SkullIcon");
        ResetDeathAlpha();
        deathScreen.SetActive(false);
        skullIcon.SetActive(false);
    }
}
