using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forceAnimSwap : MonoBehaviour
{
    public RuntimeAnimatorController baseAnim;
    public List<RuntimeAnimatorController> animations = new List<RuntimeAnimatorController>();
    public Animator animator;
    public int animationIndex = 0;

    public void Start()
    {
        animationIndex = 0;
        animator = this.GetComponent<Animator>();
    }

    public void changeAnimation()
    {
        animationIndex++;

        if (animationIndex > animations.Count - 1)
            animationIndex = 0;

        animator.runtimeAnimatorController = animations[animationIndex];
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad9))
            changeAnimation();
    }
}
