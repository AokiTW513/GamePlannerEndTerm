using UnityEngine;

public class Patrol : NPCBaseFSM
{
    GameObject[] wayPoints;
    int currentWP;

    void Awake()
    {
        wayPoints = GameObject.FindGameObjectsWithTag("wayPoint");
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        currentWP = 0;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,int layerIndex)
    {
        if (wayPoints.Length == 0)
            return;

        if (Vector3.Distance(wayPoints[currentWP].transform.position,NPC.transform.position) < preDistance)
        {
            currentWP = currentWP + 1;
            if (currentWP == wayPoints.Length)
                currentWP = 0;
            //currentWP = (currentWP + 1) % wayPoints.Length;
        }

        var direction = wayPoints[currentWP].transform.position - NPC.transform.position;
        NPC.transform.rotation = Quaternion.Slerp(NPC.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
        NPC.transform.Translate(0, 0, speed * Time.deltaTime);
        NPC.transform.GetChild(0).GetComponent<Light>().enabled = false;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,int layerIndex)
    {

    }
}