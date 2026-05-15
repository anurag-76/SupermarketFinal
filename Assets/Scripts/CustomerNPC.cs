using UnityEngine;
using UnityEngine.AI;

public class CustomerNPC : MonoBehaviour
{
    NavMeshAgent agent;
    float waitTimer = 0f;
    bool isWaiting = false;
    Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = 1.5f;
        GoToRandomPoint();
    }

    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
        animator.speed = agent.velocity.magnitude / 1.5f;

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
        // Allows a NPC to picks a random point.
        Vector3 randomDirection = Random.insideUnitSphere * 30f;
        randomDirection += transform.position;

        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, 30f, 1))
        {
            agent.SetDestination(hit.position);
        }
    }
}