using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Animator animator; // Reference to the Animator
    public string sceneToLoad; // Name of the scene to load

    private void Start()
    {
        if (animator == null)
        {
            Debug.LogError("Animator not assigned.");
            return;
        }

        // Add listener to the animation event
        animator.GetComponent<Animator>().GetBehaviour<AnimationStateBehaviour>().OnAnimationComplete += OnAnimationComplete;
    }

    private void OnDestroy()
    {
        if (animator != null)
        {
            animator.GetComponent<Animator>().GetBehaviour<AnimationStateBehaviour>().OnAnimationComplete -= OnAnimationComplete;
        }
    }

    private void OnAnimationComplete()
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
