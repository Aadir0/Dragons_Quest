using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    [Header("Enemy")]
    [SerializeField] private Transform enemy;
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;
    [Header("Idle Behaviour")]
    [SerializeField] private float IdleDuration;
    private float idleTimer;
    [Header("Enemy Animations")]
    [SerializeField] private Animator anim;

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    public void OnDisable()
    {
        anim.SetBool("moving", false);
    }
    private void Update()
    {
        if (!movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
            {
                MoveInDirection(-1);
            }
            else
            {
                ChangeInDirection();
            }
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                ChangeInDirection();
            }
        }
    }
    private void ChangeInDirection()
    {
        anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > IdleDuration)
        {
            movingLeft = !movingLeft;
        }
    }
    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("moving", true);
        //MAke enemy face direction
       enemy.localScale = new Vector3(Mathf.Sign(initScale.x * _direction), initScale.y, initScale.z);

        //Make enemy move in direction
        enemy.position = new Vector3(enemy.position.x + _direction * speed, enemy.position.y, enemy.position.z);
    }
}