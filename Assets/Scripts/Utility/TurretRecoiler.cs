using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRecoiler : MonoBehaviour
{
    public int numRecoilAnims;

    private Animator animator;
    private int alternator = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Recoil()
    {
        alternator = ((alternator) % numRecoilAnims) + 1;
        animator.SetTrigger(("Fire" + alternator));
    }
}
