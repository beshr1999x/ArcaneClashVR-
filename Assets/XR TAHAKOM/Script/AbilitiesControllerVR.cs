/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;
using UnityEngine.UI;

public class AbilitiesControllerVR : MonoBehaviour
{
    public GameObject attack_1;
    public GameObject attack_2;
    public GameObject attack_3;
    public float rateOfFire1 = 0.5f; 
    public float rateOfFire2 = 0.5f; 
    public float rateOfFire3 = 0.5f; 
    public float speedspwan = 10f; 

    private int activeAttackIndex = 0;
    private bool[] canFire = new bool[3];
    private Dictionary<int, Coroutine> fireCoroutines = new Dictionary<int, Coroutine>();
    private AttackUI attackUI;
    private Transform centerEyeAnchor;

    private void Start()
    {
        attackUI = FindObjectOfType<AttackUI>();
        if (attackUI != null)
        {
            attackUI.SetActiveAttack(activeAttackIndex);
        }

        for (int i = 0; i < canFire.Length; i++)
        {
            canFire[i] = true;
        }

        // Create object pools for attack projectiles
        if (ObjectpoolVR.Instance != null)
        {
            ObjectpoolVR.Instance.CreatePool("Attack1", attack_1, 10);
            ObjectpoolVR.Instance.CreatePool("Attack2", attack_2, 10);
            ObjectpoolVR.Instance.CreatePool("Attack3", attack_3, 10);
        }
        else
        {
            Debug.LogError("ObjectpoolVR.Instance is null.");
        }

        // Find the CenterEyeAnchor transform
        centerEyeAnchor = Camera.main != null ? Camera.main.transform : null;
        if (centerEyeAnchor == null)
        {
            Debug.LogError("CenterEyeAnchor not found.");
        }
    }

    private void Update()
    {
        // Change active attack using VR controller buttons
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            ChangeActiveAttack(0);
        }
        else if (OVRInput.GetDown(OVRInput.Button.One))
        {
            ChangeActiveAttack(1);
        }
        else if (OVRInput.GetDown(OVRInput.Button.One))
        {
            ChangeActiveAttack(2);
        }

        // Fire the active attack using VR controller trigger
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && canFire[activeAttackIndex])
        {
            FireAttack();
        }

        if (attackUI != null)
        {
            attackUI.UpdateCooldowns(canFire);
        }
    }

    public int GetActiveAttackIndex()
    {
        return activeAttackIndex;
    }

    private void ChangeActiveAttack(int index)
    {
        activeAttackIndex = index;
        if (attackUI != null)
        {
            attackUI.SetActiveAttack(activeAttackIndex);
        }

        Debug.Log("Changed active attack to: " + (activeAttackIndex + 1));
    }
    private void FireAttack()
    {
        if (ObjectpoolVR.Instance == null)
        {
            Debug.LogError("ObjectpoolVR.Instance is null.");
            return;
        }

        string attackTag = $"Attack{activeAttackIndex + 1}";
        GameObject attackPrefab;
        float rateOfFire;
        switch (activeAttackIndex)
        {
            case 0:
                attackPrefab = attack_1;
                rateOfFire = rateOfFire1;
                break;
            case 1:
                attackPrefab = attack_2;
                rateOfFire = rateOfFire2;
                break;
            case 2:
                attackPrefab = attack_3;
                rateOfFire = rateOfFire3;
                break;
            default:
                return;
        }

        // Use the position and rotation of the RightHandAnchor
        Transform rightHandAnchor = GameObject.Find("RightHandAnchor")?.transform;
        if (rightHandAnchor == null)
        {
            Debug.LogError("RightHandAnchor not found.");
            return;
        }

        Vector3 spawnPosition = rightHandAnchor.position;
        Quaternion spawnRotation = rightHandAnchor.rotation;
        Vector3 forwardDirection = rightHandAnchor.forward;

        // Debugging logs
        Debug.Log($"Spawn Position: {spawnPosition}, Spawn Rotation: {spawnRotation.eulerAngles}, Forward Direction: {forwardDirection}");

        // Get the attack projectile from the object pool
        GameObject attackProjectile = ObjectpoolVR.Instance.GetObjectFromPool(attackTag);
        if (attackProjectile != null)
        {
            attackProjectile.transform.position = spawnPosition;
            attackProjectile.transform.rotation = spawnRotation;
            Rigidbody rb = attackProjectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = forwardDirection * speedspwan;
                Debug.Log($"Projectile Velocity: {rb.velocity}");
            }
        }
        else
        {
            attackProjectile = Instantiate(attackPrefab, spawnPosition, spawnRotation);
            if (attackProjectile != null)
            {
                Rigidbody rb = attackProjectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = forwardDirection * speedspwan;
                    Debug.Log($"Projectile Velocity: {rb.velocity}");
                }
                else
                {
                    Debug.LogError("Rigidbody component not found on instantiated projectile.");
                }
            }
            else
            {
                Debug.LogError("Failed to instantiate attack projectile.");
            }
        }

        Debug.Log("Fired attack " + (activeAttackIndex + 1) + " at position: " + spawnPosition + " with rotation: " + spawnRotation.eulerAngles);

        canFire[activeAttackIndex] = false;

        if (fireCoroutines.ContainsKey(activeAttackIndex))
        {
            StopCoroutine(fireCoroutines[activeAttackIndex]);
        }

        fireCoroutines[activeAttackIndex] = StartCoroutine(ResetFire(activeAttackIndex, rateOfFire));
    }
    private IEnumerator ResetFire(int attackIndex, float rateOfFire)
    {
        yield return new WaitForSeconds(rateOfFire);
        canFire[attackIndex] = true;
        fireCoroutines.Remove(attackIndex);
    }
}
*/
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;
using UnityEngine.UI;

public class AbilitiesControllerVR : MonoBehaviour
{
    public GameObject attack_1;
    public GameObject attack_2;
    public GameObject attack_3;
    public float rateOfFire1 = 0.5f; // Rate of fire for attack_1 in seconds
    public float rateOfFire2 = 0.5f; // Rate of fire for attack_2 in seconds
    public float rateOfFire3 = 0.5f; // Rate of fire for attack_3 in seconds
    public float speedspwan = 10f; // Speed of the projectiles

    private int activeAttackIndex = 0;
    private bool[] canFire = new bool[3];
    private Dictionary<int, Coroutine> fireCoroutines = new Dictionary<int, Coroutine>();
    private AttackUI attackUI;
    private Transform centerEyeAnchor;
    private Transform rightHandAnchor;

    private void Start()
    {
        attackUI = FindObjectOfType<AttackUI>();
        if (attackUI != null)
        {
            attackUI.SetActiveAttack(activeAttackIndex);
        }

        for (int i = 0; i < canFire.Length; i++)
        {
            canFire[i] = true;
        }

        // Create object pools for attack projectiles
        if (ObjectpoolVR.Instance != null)
        {
            ObjectpoolVR.Instance.CreatePool("Attack1", attack_1, 10);
            ObjectpoolVR.Instance.CreatePool("Attack2", attack_2, 10);
            ObjectpoolVR.Instance.CreatePool("Attack3", attack_3, 10);
        }

        // Find the CenterEyeAnchor transform
        centerEyeAnchor = Camera.main.transform;
        if (centerEyeAnchor == null)
        {
            Debug.LogError("CenterEyeAnchor not found.");
        }

        // Find the right hand anchor transform
        rightHandAnchor = GameObject.Find("RightHandAnchor").transform;
        if (rightHandAnchor == null)
        {
            Debug.LogError("RightHandAnchor not found.");
        }
    }

    private void Update()
    {
        // Change active attack using VR controller button (One)
        if (OVRInput.GetDown(OVRInput.Button.One)) // "A" button
        {
            ChangeActiveAttack((activeAttackIndex + 1) % 3); // Cycle through attacks
        }

        // Fire the active attack using VR controller trigger
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && canFire[activeAttackIndex])
        {
            FireAttack();
        }

        if (attackUI != null)
        {
            attackUI.UpdateCooldowns(canFire);
        }
    }

    public int GetActiveAttackIndex()
    {
        return activeAttackIndex;
    }

    private void ChangeActiveAttack(int index)
    {
        activeAttackIndex = index;
        if (attackUI != null)
        {
            attackUI.SetActiveAttack(activeAttackIndex);
        }

        Debug.Log("Changed active attack to: " + (activeAttackIndex + 1));
    }

    private void FireAttack()
    {
        if (ObjectpoolVR.Instance == null)
        {
            Debug.LogError("ObjectpoolVR.Instance is null.");
            return;
        }

        string attackTag = $"Attack{activeAttackIndex + 1}";
        GameObject attackPrefab;
        float rateOfFire;
        switch (activeAttackIndex)
        {
            case 0:
                attackPrefab = attack_1;
                rateOfFire = rateOfFire1;
                break;
            case 1:
                attackPrefab = attack_2;
                rateOfFire = rateOfFire2;
                break;
            case 2:
                attackPrefab = attack_3;
                rateOfFire = rateOfFire3;
                break;
            default:
                return;
        }

        // Use the position and rotation of the right hand anchor
        Vector3 spawnPosition = rightHandAnchor != null ? rightHandAnchor.position : Vector3.zero;
        Quaternion spawnRotation = rightHandAnchor != null ? rightHandAnchor.rotation : Quaternion.identity;
        Vector3 forwardDirection = rightHandAnchor != null ? rightHandAnchor.forward : Vector3.forward;

        // Debugging logs
        Debug.Log($"Spawn Position: {spawnPosition}, Spawn Rotation: {spawnRotation.eulerAngles}, Forward Direction: {forwardDirection}");

        // Get the attack projectile from the object pool
        GameObject attackProjectile = ObjectpoolVR.Instance.GetObjectFromPool(attackTag);
        if (attackProjectile != null)
        {
            attackProjectile.transform.position = spawnPosition;
            attackProjectile.transform.rotation = spawnRotation;
            Rigidbody rb = attackProjectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = forwardDirection * speedspwan;
                Debug.Log($"Projectile Velocity: {rb.velocity}");
            }
        }
        else
        {
            attackProjectile = Instantiate(attackPrefab, spawnPosition, spawnRotation);
            if (attackProjectile != null)
            {
                Rigidbody rb = attackProjectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = forwardDirection * speedspwan;
                    Debug.Log($"Projectile Velocity: {rb.velocity}");
                }
                else
                {
                    Debug.LogError("Rigidbody component not found on instantiated projectile.");
                }
            }
            else
            {
                Debug.LogError("Failed to instantiate attack projectile.");
            }
        }

        Debug.Log("Fired attack " + (activeAttackIndex + 1) + " at position: " + spawnPosition + " with rotation: " + spawnRotation.eulerAngles);

        canFire[activeAttackIndex] = false;

        if (fireCoroutines.ContainsKey(activeAttackIndex))
        {
            StopCoroutine(fireCoroutines[activeAttackIndex]);
        }

        fireCoroutines[activeAttackIndex] = StartCoroutine(ResetFire(activeAttackIndex, rateOfFire));
    }

    private IEnumerator ResetFire(int attackIndex, float rateOfFire)
    {
        yield return new WaitForSeconds(rateOfFire);
        canFire[attackIndex] = true;
        fireCoroutines.Remove(attackIndex);
    }
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;
using UnityEngine.UI;

public class AbilitiesControllerVR : MonoBehaviour
{
    public GameObject attack_1;
    public GameObject attack_2;
    public GameObject attack_3;
    public float rateOfFire1 = 0.5f; // Rate of fire for attack_1 in seconds
    public float rateOfFire2 = 0.5f; // Rate of fire for attack_2 in seconds
    public float rateOfFire3 = 0.5f; // Rate of fire for attack_3 in seconds
    public float speedspwan = 10f; // Speed of the projectiles

    public GameObject attack_1_UI; // GameObject for attack 1 UI
    public GameObject attack_2_UI; // GameObject for attack 2 UI
    public GameObject attack_3_UI; // GameObject for attack 3 UI

    private int activeAttackIndex = 0;
    private bool[] canFire = new bool[3];
    private Dictionary<int, Coroutine> fireCoroutines = new Dictionary<int, Coroutine>();
    private AttackUI attackUI;
    private Transform centerEyeAnchor;
    private Transform rightHandAnchor;

    private void Start()
    {
        attackUI = FindObjectOfType<AttackUI>();
        if (attackUI != null)
        {
            attackUI.SetActiveAttack(activeAttackIndex);
        }

        for (int i = 0; i < canFire.Length; i++)
        {
            canFire[i] = true;
        }

        // Create object pools for attack projectiles
        if (ObjectpoolVR.Instance != null)
        {
            ObjectpoolVR.Instance.CreatePool("Attack1", attack_1, 10);
            ObjectpoolVR.Instance.CreatePool("Attack2", attack_2, 10);
            ObjectpoolVR.Instance.CreatePool("Attack3", attack_3, 10);
        }

        // Find the CenterEyeAnchor transform
        centerEyeAnchor = Camera.main.transform;
        if (centerEyeAnchor == null)
        {
            Debug.LogError("CenterEyeAnchor not found.");
        }

        // Find the right hand anchor transform
        rightHandAnchor = GameObject.Find("RightHandAnchor").transform;
        if (rightHandAnchor == null)
        {
            Debug.LogError("RightHandAnchor not found.");
        }

        // Initialize the active attack UI
        UpdateActiveAttackUI();
    }

    private void Update()
    {
        // Change active attack using VR controller button (One)
        if (OVRInput.GetDown(OVRInput.Button.One)) // "A" button
        {
            ChangeActiveAttack((activeAttackIndex + 1) % 3); // Cycle through attacks
        }

        // Fire the active attack using VR controller trigger
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && canFire[activeAttackIndex])
        {
            FireAttack();
        }

        if (attackUI != null)
        {
            attackUI.UpdateCooldowns(canFire);
        }
    }

    public int GetActiveAttackIndex()
    {
        return activeAttackIndex;
    }

    private void ChangeActiveAttack(int index)
    {
        activeAttackIndex = index;
        if (attackUI != null)
        {
            attackUI.SetActiveAttack(activeAttackIndex);
        }

        Debug.Log("Changed active attack to: " + (activeAttackIndex + 1));
        UpdateActiveAttackUI();
    }

    private void UpdateActiveAttackUI()
    {
        // Deactivate all attack UIs
        attack_1_UI.SetActive(false);
        attack_2_UI.SetActive(false);
        attack_3_UI.SetActive(false);

        // Activate the current attack UI
        switch (activeAttackIndex)
        {
            case 0:
                attack_1_UI.SetActive(true);
                break;
            case 1:
                attack_2_UI.SetActive(true);
                break;
            case 2:
                attack_3_UI.SetActive(true);
                break;
        }
    }

    private void FireAttack()
    {
        if (ObjectpoolVR.Instance == null)
        {
            Debug.LogError("ObjectpoolVR.Instance is null.");
            return;
        }

        string attackTag = $"Attack{activeAttackIndex + 1}";
        GameObject attackPrefab;
        float rateOfFire;
        switch (activeAttackIndex)
        {
            case 0:
                attackPrefab = attack_1;
                rateOfFire = rateOfFire1;
                break;
            case 1:
                attackPrefab = attack_2;
                rateOfFire = rateOfFire2;
                break;
            case 2:
                attackPrefab = attack_3;
                rateOfFire = rateOfFire3;
                break;
            default:
                return;
        }

        // Use the position and rotation of the right hand anchor
        Vector3 spawnPosition = rightHandAnchor != null ? rightHandAnchor.position : Vector3.zero;
        Quaternion spawnRotation = rightHandAnchor != null ? rightHandAnchor.rotation : Quaternion.identity;
        Vector3 forwardDirection = rightHandAnchor != null ? rightHandAnchor.forward : Vector3.forward;

        // Debugging logs
        Debug.Log($"Spawn Position: {spawnPosition}, Spawn Rotation: {spawnRotation.eulerAngles}, Forward Direction: {forwardDirection}");

        // Get the attack projectile from the object pool
        GameObject attackProjectile = ObjectpoolVR.Instance.GetObjectFromPool(attackTag);
        if (attackProjectile != null)
        {
            attackProjectile.transform.position = spawnPosition;
            attackProjectile.transform.rotation = spawnRotation;
            Rigidbody rb = attackProjectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = forwardDirection * speedspwan;
                Debug.Log($"Projectile Velocity: {rb.velocity}");
            }
        }
        else
        {
            attackProjectile = Instantiate(attackPrefab, spawnPosition, spawnRotation);
            if (attackProjectile != null)
            {
                Rigidbody rb = attackProjectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = forwardDirection * speedspwan;
                    Debug.Log($"Projectile Velocity: {rb.velocity}");
                }
                else
                {
                    Debug.LogError("Rigidbody component not found on instantiated projectile.");
                }
            }
            else
            {
                Debug.LogError("Failed to instantiate attack projectile.");
            }
        }

        Debug.Log("Fired attack " + (activeAttackIndex + 1) + " at position: " + spawnPosition + " with rotation: " + spawnRotation.eulerAngles);

        canFire[activeAttackIndex] = false;

        if (fireCoroutines.ContainsKey(activeAttackIndex))
        {
            StopCoroutine(fireCoroutines[activeAttackIndex]);
        }

        fireCoroutines[activeAttackIndex] = StartCoroutine(ResetFire(activeAttackIndex, rateOfFire));
    }

    private IEnumerator ResetFire(int attackIndex, float rateOfFire)
    {
        yield return new WaitForSeconds(rateOfFire);
        canFire[attackIndex] = true;
        fireCoroutines.Remove(attackIndex);
    }
}
