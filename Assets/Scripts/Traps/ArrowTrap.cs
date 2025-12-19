using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] arrows;
    private float cooldownTimer;
    [Header ("SFX")]
    [SerializeField] private AudioClip arrowSound;
    private void Attack()
    {
        cooldownTimer = 0;

        if (SoundManager.instance != null)
            SoundManager.instance.Playsound(arrowSound);
        arrows[Findarrows()].transform.position = firepoint.position;
        arrows[Findarrows()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }
    private int Findarrows()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown)
        {
            Attack();
        }
    }
}
