using UnityEngine;
using System.Collections.Generic;

public class AI_FSM : MonoBehaviour
{
    // public variables
    public GameObject player;
    
    // Private Variables
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private bool playerVisible;
    [SerializeField] private float chaseDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private float chaseHysterisis;
    [SerializeField] private float attackHysterisis;
    // AI animators
    Animator fsm_anim; // top level animator for the AI FSM
    [SerializeField] private Animator AIState_Patrol; // child animator for the AI FSM
    [SerializeField] private Animator AIState_Chase; // child animator for the AI FSM
    [SerializeField] private Animator AIState_Attack; // child animator for the AI FSM

    // Animator Params
    private static Dictionary<int, string> animStateHashToName = new Dictionary<int, string>();
    private const string FSM_PlayerSpotted = "PlayerSpotted";
    private const string FSM_Tired = "AI_Tired";
    private const string FSM_AttackInRange = "PlayerInAttackRange";

    //Attack variables
    public float attackDamage { get; private set; } = 20f;

    //Speed increase
    [SerializeField] private float npcSpeed = 2f; // Initial speed
    private float speedIncreaseTimer = 0f; // Timer for speed increase
    private UnityEngine.AI.NavMeshAgent agent;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // set initial player distance to infinity
        distanceToPlayer = Mathf.Infinity;
        fsm_anim = GetComponent<Animator>();
        
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = npcSpeed; // Set initial speed
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fsm_anim.GetCurrentAnimatorStateInfo(0).IsName("Patrol")) 
        {
           // Debug.Log("Patrol State");
            AIState_Patrol.enabled = true; 
            AIState_Chase.enabled = false;
            AIState_Attack.enabled = false;
        }
        else if (fsm_anim.GetCurrentAnimatorStateInfo(0).IsName("Chase")) 
        {
           //Debug.Log("Chase State");
            AIState_Patrol.enabled = false;
            AIState_Chase.enabled = true; 
            AIState_Attack.enabled = false;
        }
        else if (fsm_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) 
        {
            //Debug.Log("Attack State");
            AIState_Patrol.enabled = false;
            AIState_Chase.enabled = false;
            AIState_Attack.enabled = true;
        }
    }

    void FixedUpdate()
    {
        TopLevelFSMProcessing(); // process the top level FSM - every 1/60th of a second is fast enough
       
        speedIncreaseTimer += Time.deltaTime;
        if (speedIncreaseTimer >= 10f)
        {
            npcSpeed += 1f; // Increase speed by 1
            speedIncreaseTimer = 0f; // Reset the timer
        }

        TopLevelFSMProcessing();
        if (agent != null)
        {
            agent.speed = npcSpeed; // Update the speed based on the current `npcSpeed`
        }
    }

    float CheckPlayerDistance()
    {
        // calculate the distance to the player
        distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        return distanceToPlayer;
    }

    void TopLevelFSMProcessing()
    {
        //Debug.Log("Player Visible = " + playerVisible);
        if (playerVisible)
        {
            float distance = CheckPlayerDistance();
            // note that hysterisis is used to prevent the FSM from flickering between states
            if ( (CheckPlayerDistance() < chaseDistance - chaseHysterisis)) // if the player is within the chase distance, chase the player
            {
                fsm_anim.SetBool(FSM_PlayerSpotted, true); // if the player is within the chase distance, chase the player 
                if (CheckPlayerDistance() < attackDistance - attackHysterisis) fsm_anim.SetBool(FSM_AttackInRange, true); // if the player is within the attack distance, attack the player
                else if (CheckPlayerDistance() < attackDistance + attackHysterisis) fsm_anim.SetBool(FSM_AttackInRange, false);
            }
            else if (CheckPlayerDistance() > chaseDistance + chaseHysterisis) // if the player is outside the chase distance, stop chasing the player
            {
                fsm_anim.SetBool(FSM_PlayerSpotted, false);
                fsm_anim.SetBool(FSM_AttackInRange, false);
            }  
        }
        else
        {
            fsm_anim.SetBool(FSM_PlayerSpotted, false);
            fsm_anim.SetBool(FSM_AttackInRange, false);
        }
    }
  


    void OnDrawGizmos()
    {
        // draw the chase distance sphere
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
        // draw the attack distance sphere
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
