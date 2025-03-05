using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Handle player death (e.g., show game over screen, restart level)
        Debug.Log("Player has died!");
        // Optionally, disable player controls or destroy the player object
        gameObject.SetActive(false);
    }
}

