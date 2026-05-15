using UnityEngine;
using System.Collections;

public class ClerkNPC : MonoBehaviour
{
    Animator animator;

    float timer = 0f;
    float nextActionTime = 0f;
    bool isInSequence = false;

    string[] standingActions = {
        "Wave", "Bored", "ArmStretching",
        "NeckStretching", "Idle", "SadIdle", "StandToSit"
    };

    void Start()
    {
        animator = GetComponent<Animator>();

        animator.CrossFade("Wave", 0.25f);

        nextActionTime = Random.Range(3f, 6f);
    }

    void Update()
    {
        if (isInSequence) return;

        timer += Time.deltaTime;

        if (timer >= nextActionTime)
        {
            string chosen = standingActions[Random.Range(0, standingActions.Length)];

            if (chosen == "StandToSit")
            {
                StartCoroutine(SittingSequence());
            }
            else
            {
                PlayAction(chosen);
            }

            timer = 0f;
            nextActionTime = Random.Range(4f, 8f);
        }
    }

    void PlayAction(string stateName)
    {
        animator.CrossFade(stateName, 0.25f);
    }

    IEnumerator SittingSequence()
    {
        isInSequence = true;

        yield return PlayAndWait("StandToSit");
        yield return PlayAndWait("Sitting");

        yield return new WaitForSeconds(Random.Range(2f, 4f));

        yield return PlayAndWait("SitToType");
        yield return PlayAndWait("Typing");

        yield return new WaitForSeconds(Random.Range(8f, 18f));

        yield return PlayAndWait("TypeToSit");
        yield return PlayAndWait("Sitting");

        yield return new WaitForSeconds(Random.Range(2f, 4f));

        yield return PlayAndWait("SitToStand");

        isInSequence = false;
    }

    IEnumerator PlayAndWait(string stateName)
    {
        animator.CrossFade(stateName, 0.25f);

        // Wait until animation is actually playing
        yield return null;

        // Get current state info
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        float waitTime = stateInfo.length;

        // fallback safety
        if (waitTime <= 0f)
            waitTime = 1.5f;

        yield return new WaitForSeconds(waitTime);
    }
}