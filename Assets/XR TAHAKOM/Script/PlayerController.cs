using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public event Action OnPlayerDeath;
    public static PlayerController instance;

    private int health;
    public int maxHealth;
    //public float movementSpeed = 2;
    [SerializeField] FloatingHealthBar healthBar;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
    }

  

    public void UpdateHealth(int healthDifference)
    {
        // Reduce or increase health
        health += healthDifference;

        // Update health bar
        healthBar.UpdateHealthBar(health, maxHealth);

        // End game if health is 0
        if (health <= 0)
        {
            // Player has died
            OnPlayerDeath?.Invoke();
            // Disable or destroy the player GameObject
            gameObject.SetActive(false);
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("damage");
        }
    }
}
