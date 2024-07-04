using UnityEngine;
using UnityEngine.UI;

public class AttackUI : MonoBehaviour
{
    public Image[] attackCircles;
    public Color activeColor = Color.green;
    public Color inactiveColor = Color.white;
    public Color cooldownColor = Color.red;
    public Transform vrCamera; // Reference to the VR camera

    private void Start()
    {
        // If not set in the inspector, try to find the VR camera
        if (vrCamera == null)
        {
            vrCamera = Camera.main.transform;
        }
    }

    private void Update()
    {
        // Make the UI face the VR camera
        if (vrCamera != null)
        {
            Vector3 direction = (transform.position - vrCamera.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void SetActiveAttack(int index)
    {
        for (int i = 0; i < attackCircles.Length; i++)
        {
            attackCircles[i].color = (i == index) ? activeColor : inactiveColor;
        }
    }

    public void UpdateCooldowns(bool[] canFire)
    {
        AbilitiesControllerVR abilitiesController = FindObjectOfType<AbilitiesControllerVR>();
        if (abilitiesController == null) return;

        for (int i = 0; i < attackCircles.Length; i++)
        {
            if (!canFire[i])
            {
                attackCircles[i].color = cooldownColor;
            }
            else if (i == abilitiesController.GetActiveAttackIndex())
            {
                attackCircles[i].color = activeColor;
            }
            else
            {
                attackCircles[i].color = inactiveColor;
            }
        }
    }
}
