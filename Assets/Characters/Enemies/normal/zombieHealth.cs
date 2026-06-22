using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public int maxHealth = 60;
    public int currentHealth;

    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
        else
        {
            animator.SetTrigger("hurt");
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
}