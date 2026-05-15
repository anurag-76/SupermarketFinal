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
        SetNextAction();
    }

    void Update()
    {
        if (!isInSequence)
        {
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
                    animator.CrossFade(chosen, 0.25f);

                    if (chosen == "Wave")
                        nextActionTime = GetClipLength("Wave") + Random.Range(4f, 8f);
                    else
                        nextActionTime = GetClipLength(chosen) + Random.Range(2f, 5f);

                    timer = 0f;
                }
            }
        }
    }

    IEnumerator SittingSequence()
    {
        isInSequence = true;

        animator.CrossFade("StandToSit", 0.25f);
        yield return new WaitForSeconds(GetClipLength("StandToSit"));

        animator.CrossFade("Sitting", 0.25f);
        yield return new WaitForSeconds(Random.Range(2f, 4f));

        animator.CrossFade("SitToType", 0.25f);
        yield return new WaitForSeconds(GetClipLength("SitToType"));

        animator.CrossFade("Typing", 0.3f);
        yield return new WaitForSeconds(Random.Range(8f, 18f));

        animator.CrossFade("TypeToSit", 0.25f);
        yield return new WaitForSeconds(GetClipLength("TypeToSit"));

        animator.CrossFade("Sitting", 0.25f);
        yield return new WaitForSeconds(Random.Range(2f, 4f));

        animator.CrossFade("SitToStand", 0.25f);
        yield return new WaitForSeconds(GetClipLength("SitToStand"));

        yield return new WaitForSeconds(Random.Range(2f, 4f));

        isInSequence = false;
        SetNextAction();
    }

    float GetClipLength(string clipName)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            if (clip.name == clipName) return clip.length;

        Debug.LogWarning("Clip not found: " + clipName);
        return 1.5f;
    }

    void SetNextAction()
    {
        timer = 0f;
        nextActionTime = Random.Range(6f, 12f);
    }
}