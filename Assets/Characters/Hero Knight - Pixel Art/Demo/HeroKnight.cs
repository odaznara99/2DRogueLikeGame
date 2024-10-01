using UnityEngine;
using System.Collections;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] bool       m_noBlood = false;    
    [SerializeField] GameObject m_slideDust;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;   
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;

    //Attack Variables
    public Transform            attackPoint;
    public float                attackRange = 2f; // Player's attack range
    public int                  attackDamage = 20;
    public float                attackCooldown = 1f; // Time between attacks
    private float               lastComboAttackTime = 0.0f; //Time between attacks of combo
    private float               lastAttackTime; // Time after full combo

    //Blocking
    public bool isBlocking = false;
    public bool isParry = false;

    //Dead
    public bool playerIsDead = false; //Track Player Dead State

    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        attackPoint    = transform.Find("AttackPoint").GetComponent<Transform>();
        lastAttackTime = Time.time - attackCooldown;

        // Ignore Collision Between Enemy and Player
        Physics2D.IgnoreLayerCollision(3, 6);
    }

    // Update is called once per frame
    void Update ()
    {
        
        // Increase timer that controls attack combo
        lastComboAttackTime += Time.deltaTime;

        // Increase timer that checks roll duration
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if(m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
            
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        // Move
        if (!m_rolling )
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        //Death
        if (Input.GetKeyDown("e") && !m_rolling)
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }
            
        //Hurt
        else if (Input.GetKeyDown("q") && !m_rolling)
            m_animator.SetTrigger("Hurt");

        //Attack
        else if(Input.GetMouseButtonDown(0) && lastComboAttackTime > 0.25f && !m_rolling)
        {
            Attack();
        }

        // Block
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            StartCoroutine(Parry());
            m_animator.SetTrigger("Block");
            isBlocking = true;
            m_animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool("IdleBlock", false);

        // Roll
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        }
            

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded && !m_rolling)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }
    }

    void Attack()
    {
        // Reference to the AttackPoint if not assigned
        if (attackPoint == null)
        {
            attackPoint = transform.Find("AttackPoint").GetComponent<Transform>();
        }

        // Check if enough time has passed since the last combo
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (lastComboAttackTime > 0.25f && !m_rolling)
            {
                // Find all nearby enemies within the attack range
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

                foreach (Collider2D enemy in hitEnemies)
                {
                    if (enemy.CompareTag("Enemy"))
                    {
                        // Apply damage to the enemy
                        StartCoroutine(enemy.GetComponent<Bandit>().TakeDamage(attackDamage));
                    }
                }

                m_currentAttack++;

                // Call one of three attack animations "Attack1", "Attack2", "Attack3"
                m_animator.SetTrigger("Attack" + m_currentAttack);

                // Reset timer
                lastComboAttackTime = 0.0f;

                // If the combo is complete (after the third attack), apply the cooldown
                if (m_currentAttack >= 3)
                {
                    // Loop back to one for next combo
                    m_currentAttack = 0;

                    // Set cooldown after the full combo
                    lastAttackTime = Time.time;
                    Debug.Log("Combo completed, entering cooldown.");
                }
            }
        }

        // Reset combo if too much time has passed between attacks
        if (lastComboAttackTime > 2.0f)
        {
            m_currentAttack = 0;
            Debug.Log("Combo reset due to delay.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the attack range when the player is selected in the editor
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    IEnumerator Parry() {
        isParry = true;
        yield return new WaitForSeconds(1f);
        isParry = false;
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    public bool NoBlood() { 
    return m_noBlood;
    }

    public bool SetPlayerDead() {
        playerIsDead = true;
        return playerIsDead;
    }
}
