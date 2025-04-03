using UnityEngine;

public class Top_FSM_Patrol : StateMachineBehaviour
{
    // OnStateEnter is called 
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject NPC_00 = GameObject.Find("NPC_00");
        // get AIState_Patrol 
        GameObject AIState_Patrol = NPC_00.transform.Find("AIState_Patrol").gameObject;
        // access animator component 
        Animator AIState_Patrol_animator = AIState_Patrol.GetComponent<Animator>();
        // AIState_Patrol
        AIState_Patrol_animator.Rebind(); // This restarts the animator from the beginning
    }

    // OnStateUpdate is called on each Update frame 
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called 
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
