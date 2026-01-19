using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Attack Type")]
    [SerializeField] private bool canMeleeAttack = true;
    [SerializeField] private bool canRangedAttack = true;
    
    [Header("Attack Parameters")]
    [SerializeField] private float meleeAttackCooldown = 2f;
    [SerializeField] private float rangedAttackCooldown = 3f;
    [SerializeField] private int meleeDamage = 5;
    [SerializeField] private int rangedDamage = 3;
    [SerializeField] private int meleeRange = 2;
    [SerializeField] private int rangedRange = 10;
    
    [Header("Detection")]
    [SerializeField] private float detectionRadius = 15f;
    
    [Header("Ranged Attack")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;
    
    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxcollider;
    
    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip meleeAttackSound;
    [SerializeField] private AudioClip rangedAttackSound;
    
    [Header("Door to Activate on Death")]
    [SerializeField] private GameObject doorToActivate;
    
    private float meleeCooldownTimer = Mathf.Infinity;
    private float rangedCooldownTimer = Mathf.Infinity;

    //references
    private EnemyPatrol EnemyPatrol;
    private Animator anim;    
    private Health playerhealth;
    private Health bossHealth;
    private bool isDead = false;
    private Transform playerTransform;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        EnemyPatrol = GetComponentInParent<EnemyPatrol>();
        bossHealth = GetComponent<Health>();
        if (boxcollider == null)
            boxcollider = GetComponent<BoxCollider2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerTransform = playerObj.transform;
    }

    private void Update()
    {
        bool playerInDetectionRange = playerTransform != null &&
        Vector2.Distance(transform.position, playerTransform.position) <= detectionRadius;

        if (playerInDetectionRange)
            FlipTowardsPlayer();

        bool seesPlayer = playerInDetectionRange && PlayerInSight();

        if (EnemyPatrol != null)
            EnemyPatrol.SetPaused(playerInDetectionRange);

        if (!isDead && bossHealth != null && bossHealth.currentHealth <= 0)
        {
            BossDeath();
            return;
        }

        if (isDead)
            return;

        meleeCooldownTimer += Time.deltaTime;
        rangedCooldownTimer += Time.deltaTime;

        if (seesPlayer)
        {
            float distanceToPlayer = GetDistanceToPlayer();
            
            if (canMeleeAttack && playerhealth != null && playerhealth.currentHealth > 0 && distanceToPlayer <= meleeRange && meleeCooldownTimer >= meleeAttackCooldown)
            {
                PerformMeleeAttack();
            }
            else if (canRangedAttack && distanceToPlayer > meleeRange && distanceToPlayer <= rangedRange && rangedCooldownTimer >= rangedAttackCooldown)
            {
                PerformRangedAttack();
            }
        }
    }

    private void PerformMeleeAttack()
    {
        meleeCooldownTimer = 0;
        anim.SetTrigger("meleeAttack");
        if (SoundManager.instance != null && meleeAttackSound != null)
            SoundManager.instance.Playsound(meleeAttackSound);
    }

    private void PerformRangedAttack()
    {
        rangedCooldownTimer = 0;
        anim.SetTrigger("rangedAttack");
    }

    private bool PlayerInSight()
    {
        if (boxcollider == null)
            return false;

        if (playerTransform == null)
            return false;

        int maxRange = Mathf.Max(meleeRange, rangedRange);

        float directionSign = Mathf.Sign(playerTransform.position.x - transform.position.x);
        if (directionSign == 0)
            directionSign = 1;

        Vector2 center = boxcollider.bounds.center + Vector3.right * (maxRange * colliderDistance * directionSign);
        Vector2 size = new Vector2(boxcollider.bounds.size.x * maxRange, boxcollider.bounds.size.y);

        Collider2D hit = Physics2D.OverlapBox(center, size, 0f, playerLayer);
        if (hit != null)
            playerhealth = hit.GetComponent<Health>();

        return hit != null;
    }

    private float GetDistanceToPlayer()
    {
        if (playerhealth != null)
        {
            return Vector2.Distance(transform.position, playerhealth.transform.position);
        }
        return Mathf.Infinity;
    }

    private void FlipTowardsPlayer()
    {
        if (playerTransform == null)
            return;

        float dx = playerTransform.position.x - transform.position.x;
        if (dx == 0)
            return;

        if (EnemyPatrol != null)
        {
            EnemyPatrol.FaceDirection(dx);
            return;
        }

        //Fallback: flip this transform directly
        float absX = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(absX * Mathf.Sign(dx), transform.localScale.y, transform.localScale.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        if (boxcollider != null)
        {
            if (canMeleeAttack)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(
                    boxcollider.bounds.center + transform.right * meleeRange * transform.localScale.x * colliderDistance, 
                    new Vector3(boxcollider.bounds.size.x * meleeRange, boxcollider.bounds.size.y, boxcollider.bounds.size.z)
                );
            }
            
            if (canRangedAttack)
            {
                Gizmos.color = Color.purple;
                Gizmos.DrawWireCube(
                    boxcollider.bounds.center + transform.right * rangedRange * transform.localScale.x * colliderDistance, 
                    new Vector3(boxcollider.bounds.size.x * rangedRange, boxcollider.bounds.size.y, boxcollider.bounds.size.z)
                );
            }
        }
    }

    private void DamagePlayer()
    {
        if (PlayerInSight() && playerhealth != null)
        {
            float distanceToPlayer = GetDistanceToPlayer();
            if (distanceToPlayer <= meleeRange)
            {
                playerhealth.TakeDamage(meleeDamage);
            }
        }
    }

    private void RangedAttack()
    {
        if (fireballs != null && fireballs.Length > 0 && firepoint != null)
        {
            int fireballIndex = FindFireballs();
            if (fireballIndex < 0 || fireballIndex >= fireballs.Length || fireballs[fireballIndex] == null)
                return;

            if (SoundManager.instance != null && rangedAttackSound != null)
                SoundManager.instance.Playsound(rangedAttackSound);

            fireballs[fireballIndex].transform.position = firepoint.position;
            
            //Get the BossFireball component and activate it
            BossFireball fireball = fireballs[fireballIndex].GetComponent<BossFireball>();
            if (fireball != null)
            {
                //Set direction based on which way boss is facing
                float direction = Mathf.Sign(transform.localScale.x);
                fireball.ActivateFireball(direction);
            }
        }
    }

    private int FindFireballs()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
            {
                return i;
            }
        }
        return -1;
    }

    private void BossDeath()
    {
        Debug.Log("Boss has died.");
        isDead = true;
        
        if (EnemyPatrol != null)
        {
            EnemyPatrol.SetPaused(true);
            EnemyPatrol.enabled = false;
        }
        
        if (doorToActivate != null)
        {
            doorToActivate.SetActive(true);
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
}