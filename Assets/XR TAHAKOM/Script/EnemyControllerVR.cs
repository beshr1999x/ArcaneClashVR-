using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerVR : MonoBehaviour
{
    public EnemyType enemyType;
    public int scoreValue; 

    private int health; 
    public int maxHealth; 
    public int damageToPlayer; // Damage strength it causes to player
    public float originalSpeed; 
    public Color normalHealthbarColor;
    public Color frozenHealthbarColor;
    private float currentSpeed; // Speed at which it moves currently
    [SerializeField] FloatingHealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();

        // Set the default value of Healthbar colors
        ColorUtility.TryParseHtmlString("#4ECF4D", out normalHealthbarColor);
        ColorUtility.TryParseHtmlString("#00F2FF", out frozenHealthbarColor);
    }

    void Start()
    {
        currentSpeed = originalSpeed;
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        healthBar.ChangeHealthBarColor(normalHealthbarColor);

        // Assign the enemyType if it's not already set
        if (enemyType == EnemyType.None)
        {
            enemyType = DetermineEnemyType();
        }

        if (PlayerController.instance != null)
        {
            PlayerController.instance.OnPlayerDeath += HandlePlayerDeath;
        }
    }

    void Update()
    {
        // Move enemy in VR environment
        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);
        
        
            if (Input.GetKeyDown(KeyCode.H)) // Press H to reduce health
            {
                UpdateHealth(-10); // Reduces health by 10
            }
            if (Input.GetKeyDown(KeyCode.J)) // Press J to increase health
            {
                UpdateHealth(10); // Increases health by 10
            }
        
    }

    private void DestroyEnemy()
    {
        // Notify the EnemySpawner script about the enemy's destruction
        EnemySpawnerVR spawner = FindObjectOfType<EnemySpawnerVR>();

        if (spawner != null)
        {
            spawner.HandleEnemyDestruction(this);
        }

        // Return the enemy to the object pool
        string enemyTypeString = enemyType.ToString();
        ObjectpoolVR.Instance.ReturnObjectToPool(enemyTypeString, gameObject);

        // Return enemy to the pool instead of destroying it
        //Destroy(gameObject);
    }

    private EnemyType DetermineEnemyType()
    {
        // Determine the enemy type based on the game object's name or other criteria
        if (gameObject.name.Contains("Enemy_Elemental"))
        {
            return EnemyType.Enemy_Elemental;
        }
        else if (gameObject.name.Contains("Enemy_Arcane_Fiend"))
        {
            return EnemyType.Enemy_Arcane_Fiend;
        }
        else if (gameObject.name.Contains("Enemy_Shadow_Caster"))
        {
            return EnemyType.Enemy_Shadow_Caster;
        }
        // Default to a specific enemy type if no condition is met
        return EnemyType.Enemy_Elemental;
    }

    public void UpdateHealth(int damage)
    {
        // Reduce health
        health -= damage;

        // Update health bar
        healthBar.UpdateHealthBar(health, maxHealth);

        // Destroy if no health remaining
        if (health <= 0)
        {
            DestroyEnemy();
        }
    }

    public void SlowDown(float slowingImpact, float slowingDuration)
    {
        // Set new reduced speed -- make sure it's not less than 0.1
        currentSpeed = Mathf.Max(0.1f, currentSpeed - slowingImpact);

        // Change health bar color
        healthBar.ChangeHealthBarColor(frozenHealthbarColor);

        // Return speed and health bar color back to normal after a while
        StartCoroutine(ReturnSpeedToNormal(slowingImpact, slowingDuration));
    }

    private IEnumerator ReturnSpeedToNormal(float slowingImpact, float slowingDuration)
    {
        yield return new WaitForSeconds(slowingDuration);

        // Add back the speed that was decreased
        // This is implemented because multiple shots will add up 
        // Shouldn't happen, but ensure that the original speed is not exceeded
        currentSpeed = Mathf.Min(originalSpeed, currentSpeed + slowingImpact);

        // Change health bar color to normal
        healthBar.ChangeHealthBarColor(normalHealthbarColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boundary")
        {
            // Prevent endless enemies from remaining in game
            DestroyEnemy();

            // Update player HP once enemy reaches end of track
            PlayerController.instance.UpdateHealth(-damageToPlayer);
        }
    }

    private void OnDestroy()
    {
        if (PlayerController.instance != null)
        {
            PlayerController.instance.OnPlayerDeath -= HandlePlayerDeath;
        }
    }

    private void HandlePlayerDeath()
    {
        // Stop moving or disable the enemy
        currentSpeed = 0f;
        enabled = false;
    }
}
