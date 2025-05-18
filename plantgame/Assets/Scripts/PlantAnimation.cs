using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantAnimation : MonoBehaviour
{
    // Animator 

    private Animator animator;

    // Hashes for animation
    int isHappyHash;
    int isSadHash;
    int isIdleHash;

    private void Start()
    {
        animator = GetComponent<Animator>();

        isHappyHash = Animator.StringToHash("Stage2Happy");
        isSadHash = Animator.StringToHash("Stage2Sad");
        isIdleHash = Animator.StringToHash("Stage2Idle");
    }

    public void HandleAnimation(int hash)
    {
        int currentHash = isIdleHash;

        if(hash == 0)
        {
            animator.SetTrigger("IsIdle");
            currentHash = isIdleHash;
        }
        else if (hash == 1)
        {
            animator.SetTrigger("IsHappy");
            currentHash = isHappyHash;
        }
        else if (hash == 2)
        {
            animator.SetTrigger("IsSad"); 
            currentHash = isSadHash;
        }

        animator.Play(currentHash);
    }

    public void UnsetAnimation(int hash)
    {
        if (hash == 0)
        {
            animator.ResetTrigger("IsIdle");
        }
        else if (hash == 1)
        {
            animator.ResetTrigger("IsHappy");
        }
        else if (hash == 2)
        {
            animator.ResetTrigger("IsSad");
        }
    }



}
