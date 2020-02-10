using Photon.Pun;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    const int damage = 5000;

    private int summonerID;
    private PhotonView pView;
    private Rigidbody rBody;

    private void Start()
    {
        pView = GetComponent<PhotonView>();
        rBody = GetComponent<Rigidbody>();

        rBody.velocity = transform.forward * 250;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!pView.IsMine)
        {
            return;
        }

        if (other.gameObject.GetComponent<SpawnableHealth>())
        {
            other.gameObject.GetComponent<SpawnableHealth>().TakeDamage(damage, summonerID);
        }
    }

    public void SetSummonerID(int id)
    {
        summonerID = id;
    }
}
