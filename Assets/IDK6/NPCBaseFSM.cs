using UnityEngine;

public class NPCBaseFSM : StateMachineBehaviour
{
    public GameObject NPC;
    public GameObject player;
    public float speed = 2;
    public float rotSpeed = 1;
    public float preDistance = 3;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,int layerIndex)
    {
        NPC = animator.gameObject;
        player = NPC.GetComponent<tankAI>().GetPlayer();
    }
}