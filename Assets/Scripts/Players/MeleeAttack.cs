using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private int damage = 4;
    [SerializeField] private float range = 1;
    
    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    
    [Header("Enemy Layer")]
    [SerializeField] private LayerMask enemyLayer;
    
    [Header("Audio")]
    [SerializeField] private AudioClip meleeSound;
    
    [Header("Cooldown")]
    [SerializeField] private float attackCooldown;
    
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;
        
        if (Input.GetMouseButton(1) && cooldownTimer > attackCooldown)
        {
            meleeAttack();
        }
    }

    private void meleeAttack()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.Playsound(meleeSound);
        }
        anim.SetTrigger("meleeAtk");
        cooldownTimer = 0;
    }

    private void DamageEnemy()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0,
            Vector2.left,
            0,
            enemyLayer
        );

        if (hit.collider != null)
        {
            Health enemyHealth = hit.transform.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (boxCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(
                boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
                new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
            );
        }
    }
}
