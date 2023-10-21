using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol_StateMachine : Weapon_StateMachine
{
    private readonly string _stateIdle = "Pistol_Idle";
    private readonly string _stateFire = "Pistol_Fire";
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName(_stateIdle))
        {
            // code to execute when entering the Pistol_Idle state
            Debug.Log("Entering Pistol_Idle state");
        }
        else if (stateInfo.IsName(_stateFire))
        {
            // code to execute when entering the Pistol_Fire state
            Debug.Log("Entering Pistol_Fire state");
        }
        else
        {
            Debug.LogError("Unknown state: " + stateInfo.fullPathHash);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName(_stateIdle))
        {
            // code to execute when exiting the Pistol_Idle state
            Debug.Log("Exiting Pistol_Idle state");
        }
        else if (stateInfo.IsName(_stateFire))
        {
            // code to execute when exiting the Pistol_Fire state
            Debug.Log("Exiting Pistol_Fire state");
        }
        else
        {
            Debug.LogError("Unknown state: " + stateInfo.fullPathHash);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
