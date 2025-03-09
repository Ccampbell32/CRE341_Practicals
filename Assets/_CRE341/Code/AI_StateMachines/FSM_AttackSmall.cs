using UnityEngine;

public class FSM_AttackSmall : StateMachineBehaviour

{
    // Called when the state is entered
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get a reference to the AI_FSM script
        AI_FSM aiFSM = animator.GetComponent<AI_FSM>();

        // Deal damage to the player
        if (aiFSM != null && aiFSM.player != null)
        {
            PlayerHealth playerHealth = aiFSM.player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(aiFSM.attackDamage);
            }
        }
    }
}


