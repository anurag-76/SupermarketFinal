using UnityEngine;
using UnityEngine.AI;

public class CustomerNPC : MonoBehaviour
{
    NavMeshAgent agent;
    float waitTimer = 0f;
    bool isWaiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToRandomPoint();
    }

    void Update()
    {
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                GoToRandomPoint();
            }
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            isWaiting = true;
            waitTimer = Random.Range(2f, 5f);
        }
    }

    void GoToRandomPoint()
    {
        // Picks a random point within 15 units around the NPC
        Vector3 randomDirection = Random.insideUnitSphere * 8f;
        randomDirection += transform.position;

        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, 15f, 1))
        {
            agent.SetDestination(hit.position);
        }
    }
}