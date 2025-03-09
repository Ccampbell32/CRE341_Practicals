using UnityEngine;

public class Top_FSM_Attack : StateMachineBehaviour
{
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // reference to the AI_FSM script
        AI_FSM aiFSM = animator.GetComponent<AI_FSM>();

        // Start the attack animation 
      
        animator.SetTrigger("Attack");

        // Optionally, you can set a flag to indicate that the attack is in progress
        aiFSM.isAttacking = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        AI_FSM aiFSM = animator.GetComponent<AI_FSM>();

        // Checking if the attack animation is finished
        if (stateInfo.normalizedTime >= 1f)
        {
            // Reset the attack flag
            aiFSM.isAttacking = false;

            
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        AI_FSM aiFSM = animator.GetComponent<AI_FSM>();

        // Reset the attack flag
        aiFSM.isAttacking = false;
    }
}
