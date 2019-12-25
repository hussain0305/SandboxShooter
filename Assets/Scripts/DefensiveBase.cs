using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveBase : MonoBehaviour
{
    public Projectile projectile;
    public bool hasProximityDetector;

    public float frequency;
    protected EPlayerController owner;
    protected List<TestEnemy> opponentsInVicinity;
    /*
     * DO NOT DELETE
     * 
    protected List<EPlayerController> opponentsInVicinity;
    */
    void Start()
    {
        /*
         * DO NO DELETE
         * 
        opponentsInVicinity = new List<EPlayerController>();
        */
        opponentsInVicinity = new List<TestEnemy>();
    }
    public EPlayerController GetOwner()
    {
        return owner;
    }

    public void SetOwner(EPlayerController own)
    {
        owner = own;
        if (hasProximityDetector)
        {
            GetComponentInChildren<ProximityDetector>().SetOwner(owner);
            GetComponentInChildren<ProximityDetector>().SetStructure(this);
        }
    }

    /*
     * DO NOT DELETE
     * 
    public void OpponentDetected(EPlayerController opponent)
    */
    public void OpponentDetected(TestEnemy opponent)
    {
        /*
         * DO NO DELETE
         * 
        opponentsInVicinity.Add(opponent);
        */
        opponentsInVicinity.Add(opponent);
        PerformAttack();
    }

    public void OpponentLeft(TestEnemy opponent)
    {
        /*
         * DO NO DELETE
         * 
        opponentsInVicinity.Remove(opponent);
        */
        if (opponentsInVicinity.Contains(opponent))
        {
            opponentsInVicinity.Remove(opponent);
        }
        StopAttack();
    }

    public IEnumerator AttackAgainAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        PerformAttack();
    }

    public virtual void PerformAttack() { }

    public virtual void StopAttack() { }
}
