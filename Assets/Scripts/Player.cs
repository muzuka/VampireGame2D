using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public float maxHealth;
    public float maxGarlic;
    public float attackRange;
	public float movementSpeed;
	public float stopDistance;
	public float jumpSpeed = 1;
    public float lightDamage;
    public float holyDamage;
    public Image healthBar;
    public Image garlicBar;
    public Transform crypt;

    Rigidbody2D body;
    Attackable health;

	GameObject target;
	Vector2 destination;
	Vector3 jumpEquation;
	bool falling;
    bool jumping;

    public float jumpForce;
    public float jumpTime;
    float jumpTimeCounter;
    bool stoppedJumping;

    public ParticleSystem smokeEffect;
    
    bool grounded;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public float groundCheckRadius;

    bool leftPressed = false;
    bool rightPressed = false;

    bool inLight = false;

    int garlicAmount = 0;

    float horizontal = 0f;
    float vertical = 0f;

	bool debug;

	// Use this for initialization
	void Start ()
    {
        body = GetComponent<Rigidbody2D>();
        health = GetComponent<Attackable>();
        health.setHealth(maxHealth);

        reset();
	}

    private void OnEnable()
    {
        EventManager.StartListening("Reset", reset);
        EventManager.StartListening("EnteredLight", enteredLight);
        EventManager.StartListening("LeftLight", leftLight);
    }

    private void OnDisable()
    {
        EventManager.StopListening("Reset", reset);
        EventManager.StopListening("EnteredLight", enteredLight);
        EventManager.StopListening("LeftLight", leftLight);
    }

    // Update is called once per frame
    void Update ()
    {

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
		falling = GetComponent<Rigidbody2D>().velocity.y < 0;
        healthBar.fillAmount = health.health / health.maxHealth;
        garlicBar.fillAmount = garlicAmount / maxGarlic;

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if(horizontal != 0)
            GetComponent<SpriteRenderer>().flipX = horizontal < 0f;

        GetComponent<Rigidbody2D>().velocity = new Vector2(horizontal * movementSpeed * (1 - garlicBar.fillAmount), GetComponent<Rigidbody2D>().velocity.y);

        if (!GameController.paused)
        {
            if (Input.GetButtonDown("Fire1"))
            {

                if (canEnterCrypt())
                {
                    EventManager.TriggerEvent("WinGame");
                }
                else
                {
                    GameObject target = getAttackTarget();

                    if (target != null)
                    {
                        drinkBlood(target);
                    }
                }
            }

            if (Input.GetButtonDown("Jump"))
            {
                jumpDown();
            }
            else if (Input.GetButtonUp("Jump"))
            {
                jumpUp();
            }

            if (inLight)
            {
                health.attacked(Time.deltaTime * lightDamage);
                if (!smokeEffect.isPlaying)
                {
                    smokeEffect.Play();
                }
            }
            else
            {
                smokeEffect.Stop();
            }
        }
	}

	void FixedUpdate ()
	{
		// fixes collider fall through
		if (falling)
			Time.fixedDeltaTime = 0.02f;
		else
			Time.fixedDeltaTime = 0.01f;

        if (jumping && !stoppedJumping)
        {
            if (jumpTimeCounter > 0)
            {
                body.velocity = new Vector2(body.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
        }
	}

    public void jumpDown ()
    {
        jumping = true;
        if (grounded)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }
    }

    public void jumpUp ()
    {
        jumping = false;
        jumpTimeCounter = 0;
        stoppedJumping = true;
    } 

    void drinkBlood (GameObject target)
    {
        garlicAmount += target.GetComponent<HasBlood>().garlicContent;

        if (target.GetComponent<HasBlood>().holy)
        {
            health.attacked(holyDamage);
            garlicAmount = 0;
        }
        else
        {
            health.repair(target.GetComponent<HasBlood>().healFactor);
        }

        Destroy(target);
    }

    bool canEnterCrypt ()
    {
        bool canEnter = false;

        if (Mathf.Abs(crypt.position.x - transform.position.x) < attackRange)
        {
            canEnter = true;
        }

        return canEnter;
    }

    GameObject getAttackTarget ()
    {
        HasBlood[] citizens = FindObjectsOfType<HasBlood>();
        float minDist = float.MaxValue;
        GameObject closestCitizen = null;


        for (int i = 0; i < citizens.Length; i++)
        {
            float distance = Vector2.Distance(citizens[i].transform.position, transform.position);

            if (GetComponent<SpriteRenderer>().flipX)
            {
                if (citizens[i].transform.position.x < transform.position.x && distance <= attackRange)
                {
                    if (distance < minDist)
                    {
                        minDist = distance;
                        closestCitizen = citizens[i].gameObject;
                    }
                }
            }
            else
            {
                if (citizens[i].transform.position.x > transform.position.x && distance <= attackRange)
                {
                    if (distance < minDist)
                    {
                        minDist = distance;
                        closestCitizen = citizens[i].gameObject;
                    }
                }
            }
        }

        return closestCitizen;
    }

    void enteredLight ()
    {
        inLight = true;
    }

    void leftLight ()
    {
        inLight = false;
    }

    private float getDistanceToPosition (Vector2 pos)
	{
		return Vector2.Distance(transform.position, pos);
	}

    private void reset ()
    {
		stoppedJumping = true;
        jumpTimeCounter = jumpTime;
		body.velocity = new Vector2(0.0f, 0.0f);
    }
}
