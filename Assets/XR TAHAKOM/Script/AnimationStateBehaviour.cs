using UnityEngine;

public class AnimationStateBehaviour : StateMachineBehaviour
{
    public event System.Action OnAnimationComplete;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnAnimationComplete?.Invoke();
    }
}
