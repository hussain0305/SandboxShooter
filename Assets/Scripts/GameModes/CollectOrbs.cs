using System.Collections;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class CollectOrbs : ModeManager
{
    public static CollectOrbs orbMode;

    public string[] orbPath;
    public float totalRoundTime;
    public float orbSpawnsPerRound;
    public Transform orbSpawnPoints;

    [Header("Scoreboard Related")]
    public Transform scoreboardHeader;
    public Color[] allColors;

    public TextMesh timer;

    private int[] scores;

    private float timeBetweenOrbSpawns;
    private float orbLifetime;
    private float cooldownTime;

    private float spawnBuffer = 2;
    private PhotonView pView;

    private float currentRowPosition;

    private bool coroutineRunning;
    private Coroutine spawningCoroutine;
    // Start is called before the first frame update

    private void Awake()
    {
        if (orbMode != null && orbMode != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            CollectOrbs.orbMode = this;
        }

        pView = GetComponent<PhotonView>();
    }

    void Start()
    {
        pView = GetComponent<PhotonView>();
        scores = new int[8];
        coroutineRunning = false;

        if (!pView.IsMine)
        {
            return;
        }

        SetTimes();
        spawningCoroutine = StartCoroutine(SpawnOrb());
        StartCoroutine(Timer(totalRoundTime));
        StartCoroutine(TrackEndOfGame(totalRoundTime));
    }

    void SetTimes()
    {
        timeBetweenOrbSpawns = (totalRoundTime - (orbSpawnsPerRound * spawnBuffer)) / orbSpawnsPerRound;
        orbLifetime = (3.0f / 4) * timeBetweenOrbSpawns;
        cooldownTime = (1.0f / 4) * timeBetweenOrbSpawns;
    }

    IEnumerator SpawnOrb()
    {
        coroutineRunning = true;
        yield return new WaitForSeconds(spawnBuffer);
        GameObject tOrb = PhotonNetwork.Instantiate(Path.Combine(orbPath), GetOrbSpawnLocation(), Quaternion.identity, 0);
        tOrb.GetComponent<CollectibleOrb>().SetLifetime(orbLifetime - 0.2f);
        tOrb.GetComponent<CollectibleOrb>().parentMode = this;
        yield return new WaitForSeconds(orbLifetime);
        coroutineRunning = false;
        StartCoroutine(SpawnCoolDown());
    }

    IEnumerator SpawnCoolDown()
    {
        yield return new WaitForSeconds(cooldownTime);
        spawningCoroutine = StartCoroutine(SpawnOrb());

    }

    Vector3 GetOrbSpawnLocation()
    {
        return orbSpawnPoints.GetChild(Random.Range(0, orbSpawnPoints.childCount)).position;
    }

    public void OrbPicked(int pickerID)
    {
        if (coroutineRunning)
        {
            StopCoroutine(spawningCoroutine);
        }
        StartCoroutine(SpawnCoolDown());

        pView.RPC("RPC_PickedUpOnClients", RpcTarget.All, pickerID);

    }

    [PunRPC]
    void RPC_PickedUpOnClients(int pickerID)
    {
        scoreboardHeader.GetComponent<TextMesh>().text = "Scoreboard";

        int pickerIndex = pickerID / 1000;
        scores[pickerIndex] += 1;

        DisplayScores();
    }

    void DisplayScores()
    {
        currentRowPosition = -75;
        int loop = 0;
        foreach(Transform currChild in scoreboardHeader)
        {
            Destroy(currChild.gameObject);
        }
        foreach (int currentScore in scores)
        {
            if (currentScore != 0)
            {
                GameObject newRow = Instantiate(new GameObject());
                newRow.transform.SetParent(scoreboardHeader);
                newRow.transform.localPosition = Vector3.zero;
                newRow.transform.localRotation = Quaternion.identity;
                newRow.transform.localPosition = new Vector3(0, currentRowPosition, 0);
                currentRowPosition -= 75;

                newRow.AddComponent<TextMesh>();
                newRow.GetComponent<TextMesh>().characterSize = 5;
                newRow.GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
                newRow.GetComponent<TextMesh>().alignment = TextAlignment.Center;
                newRow.GetComponent<TextMesh>().fontSize = 50;
                newRow.GetComponent<TextMesh>().text = "Player " + loop + "\n" + scores[loop];
                newRow.GetComponent<TextMesh>().color = allColors[loop];
            }
            loop++;
        }


    }

    IEnumerator TrackEndOfGame(float duration)
    {
        yield return new WaitForSeconds(duration + 2);
        pView.RPC("RPC_EndGameOnClients", RpcTarget.All);
    }

    [PunRPC]
    void RPC_EndGameOnClients()
    {
        pView.RPC("RPC_DestroyTimerOnClients", RpcTarget.All);

        StopAllCoroutines();

        DetermineWinner();
    }

    void DetermineWinner()
    {
        foreach (Transform currChild in scoreboardHeader)
        {
            Destroy(currChild.gameObject);
        }

        if (!pView.IsMine)
        {
            return;
        }

        int loop = 0;
        int highestScore = 0;
        int highestIndex = 0;
        bool isDraw = false;

        foreach (int currentScore in scores)
        {
            if (currentScore > highestScore)
            {
                highestScore = currentScore;
                highestIndex = loop;
                isDraw = false;
            }
            else if(currentScore == highestScore)
            {
                isDraw = true;
            }

            loop++;
        }


        if (isDraw)
        {
            pView.RPC("RPC_DeclareOnClients", RpcTarget.All, "It's a tie");
        }
        else
        {
            pView.RPC("RPC_DeclareOnClients", RpcTarget.All, ("Player " + highestIndex + " won"));
        }
    }

    IEnumerator Timer(float duration)
    {
        float currTime = duration;
        while (currTime > 0)
        {
            pView.RPC("RPC_ServerTime", RpcTarget.All, currTime);
            currTime--;

            yield return new WaitForSeconds(1);
        }
        pView.RPC("RPC_DestroyTimerOnClients", RpcTarget.All);
    }

    [PunRPC]
    void RPC_DestroyTimerOnClients()
    {
        if (timer)
        {
            Destroy(timer.gameObject);
        }
    }


    [PunRPC]
    void RPC_ServerTime(float time)
    {
        timer.text = "" + time;
    }

    //Unfortunately, Photon doesn't allow calling RPCs on base classes, so we can't put this common
    //function in a base class. Has to be declared in every mode script
    [PunRPC]
    void RPC_DeclareOnClients(string msg)
    {
        scoreboardHeader.GetComponent<TextMesh>().text = msg;
    }

}
