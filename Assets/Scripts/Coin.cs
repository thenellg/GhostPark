using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int amount = 1;
    public Animator animator;
    public AudioClip soundEffect;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void end()
    {
        Destroy(this.gameObject);
    }

    public void hideCoin()
    {
        animator.SetTrigger("hideCoin");
        Invoke("end", 0.5f);
    }
}
