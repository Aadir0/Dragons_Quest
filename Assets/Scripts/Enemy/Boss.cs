using NUnit.Framework;
using UnityEditor;
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

    private void Awake()
    {
        anim = GetComponent<Animator>();
        EnemyPatrol = GetComponentInParent<EnemyPatrol>();
        bossHealth = GetComponent<Health>();
    }

    private void Update()
    {
        if (!isDead && bossHealth != null && bossHealth.currentHealth <= 0)
        {
            BossDeath();
            return;
        }

        if (isDead)
            return;

        meleeCooldownTimer += Time.deltaTime;
        rangedCooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            FlipTowardsPlayer();
            
            float distanceToPlayer = GetDistanceToPlayer();
            
            if (canMeleeAttack && distanceToPlayer <= meleeRange && meleeCooldownTimer >= meleeAttackCooldown && playerhealth != null && playerhealth.currentHealth > 0)
            {
                PerformMeleeAttack();
            }
            else if (canRangedAttack && distanceToPlayer > meleeRange && distanceToPlayer <= rangedRange && rangedCooldownTimer >= rangedAttackCooldown)
            {
                PerformRangedAttack();
            }
        }

        if (EnemyPatrol != null)
        {
            EnemyPatrol.enabled = !PlayerInSight();
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
        int maxRange = Mathf.Max(meleeRange, rangedRange);
        RaycastHit2D hit = Physics2D.BoxCast(
            boxcollider.bounds.center + transform.right * maxRange * transform.localScale.x * colliderDistance, 
            new Vector3(boxcollider.bounds.size.x * maxRange, boxcollider.bounds.size.y, boxcollider.bounds.size.z), 
            0, 
            Vector2.left, 
            0.2f, 
            playerLayer
        );

        if (hit.collider != null)
        {
            playerhealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;
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
        if (playerhealth != null)
        {
            float playerDirection = playerhealth.transform.position.x - transform.position.x;
            
            bool facingRight = transform.localScale.x > 0;
            bool playerIsRight = playerDirection > 0;
            
            if (facingRight != playerIsRight)
            {
                Vector3 newScale = transform.localScale;
                newScale.x = -1.5f; // This preserves the magnitude, just flips the sign
                transform.localScale = newScale;
            }
        }
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
        if (SoundManager.instance != null && rangedAttackSound != null)
            SoundManager.instance.Playsound(rangedAttackSound);
        
        if (fireballs != null && fireballs.Length > 0 && firepoint != null)
        {
            int fireballIndex = FindFireballs();
            fireballs[fireballIndex].transform.position = firepoint.position;
            
            // Get the BossFireball component and activate it
            BossFireball fireball = fireballs[fireballIndex].GetComponent<BossFireball>();
            if (fireball != null)
            {
                // Set direction based on which way boss is facing
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
        return 0;
    }

    private void BossDeath()
    {
        isDead = true;
        
        if (EnemyPatrol != null)
        {
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