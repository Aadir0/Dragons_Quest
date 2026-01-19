using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private int range;
    [Header ("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxcollider;
    [Header ("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    [Header ("Audio Clip")]
    [SerializeField] private AudioClip attackSound;

    //references
    private EnemyPatrol EnemyPatrol;
    private Animator anim;    
    private Health playerhealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        EnemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        //Attack only when player in sight
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown && playerhealth != null && playerhealth.currentHealth > 0)
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
                if (SoundManager.instance != null)
                    SoundManager.instance.Playsound(attackSound);
            }
        }

        if (EnemyPatrol != null)
        {
            EnemyPatrol.enabled = !PlayerInSight();
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxcollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(boxcollider.bounds.size.x * range, boxcollider.bounds.size.y, boxcollider.bounds.size.z), 0, Vector2.left, 0.2f, playerLayer);

        if (hit.collider != null)
        {
            playerhealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxcollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(boxcollider.bounds.size.x * range, boxcollider.bounds.size.y, boxcollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        //If player is still in range
        if (PlayerInSight())
        {
            playerhealth.TakeDamage(damage);
        }
    }
}
