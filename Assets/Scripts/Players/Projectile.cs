using UnityEngine;
using UnityEngine.SceneManagement;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifetime;
    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        //If projectile already hit something then to restrict movement
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);
        lifetime += Time.deltaTime;

        //To set projectile lifetime so that it does not travle infinitly
        if (lifetime > 5)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;

        //Disable the projectile hitbox so it does not collide again
        boxCollider.enabled = false;
        anim.SetTrigger("explode");

        if (collision.CompareTag("Enemy"))
        {
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                collision.GetComponent<Health>().TakeDamage(2);
            }
            else
            {
                collision.GetComponent<Health>().TakeDamage(1);
            }
        }
    }

    //Called by Playerattack to shoot the fireball
    public void SetDirection(float _direction)
    {
        //Reset lifetime because projectile is reused from pool
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        //Flip direction of projectile based of travel direction
        float localScaleX = transform.localScale.x;

        //If projectile is facing wrong direction then fliping it
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        //Applying flipped scale
        transform.localScale = new Vector2(localScaleX, transform.localScale.y);
    }

    //called by explosion animation using animation events
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}