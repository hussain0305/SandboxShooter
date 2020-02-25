using UnityEngine;

public class AutoNavFlyerProbe : MonoBehaviour
{
    public Direction myDirection;
    private AutoNavFlyer navSystem;

    private void Awake()
    {
        navSystem = GetComponentInParent<AutoNavFlyer>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!navSystem.PViewIsMine())
        {
            return;
        }
        navSystem.DirectionUnavailable(myDirection);
        navSystem.StopMoving();
        //navSystem.Redirect();
    }

    public void OnTriggerExit(Collider other)
    {
        if (!navSystem.PViewIsMine())
        {
            return;
        }

        navSystem.DirectionAvailable(myDirection);
    }
}
