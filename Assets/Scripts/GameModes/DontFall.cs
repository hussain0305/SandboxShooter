using Photon.Pun;
using System.Collections;
using UnityEngine;

public class DontFall : BaseGameMode
{
    const int LEVEL_SPECIFIC_VALUE = 2000;

    public static DontFall dontFallMode;

    public float totalRoundTime;

    [Header("Scoreboard Related")]
    public Transform scoreboardHeader;
    public Color[] allColors;

    public TextMesh timer;

    private int[] scores;

    private PhotonView pView;

    private float currentRowPosition;

    private void Awake()
    {
        if (dontFallMode != null && dontFallMode != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontFall.dontFallMode = this;
        }

        pView = GetComponent<PhotonView>();
    }

    void Start()
    {
        pView = GetComponent<PhotonView>();
        scores = new int[8];

        for (int loop = 0; loop < scores.Length; loop++)
        {
            scores[loop] = LEVEL_SPECIFIC_VALUE;
        }

        if (!pView.IsMine)
        {
            return;
        }

        StartCoroutine(Timer(totalRoundTime));
        StartCoroutine(TrackEndOfGame(totalRoundTime));
    }

    public void PlayerFell(int fallenID)
    {
        pView.RPC("RPC_FellOnClients", RpcTarget.All, fallenID);
    }

    [PunRPC]
    void RPC_FellOnClients(int fallenID)
    {
        scoreboardHeader.GetComponent<TextMesh>().text = "Scoreboard";

        int fallenIndex = fallenID / 1000;
        if(scores[fallenIndex] == LEVEL_SPECIFIC_VALUE)
        {
            scores[fallenIndex] = 0;
        }
        scores[fallenIndex] += 1;

        DisplayScores();
    }

    void DisplayScores()
    {
        currentRowPosition = -75;
        int loop = 0;
        foreach (Transform currChild in scoreboardHeader)
        {
            Destroy(currChild.gameObject);
        }
        foreach (int currentScore in scores)
        {
            if (currentScore != LEVEL_SPECIFIC_VALUE)
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

        int index = 0;
        int lowestScore = LEVEL_SPECIFIC_VALUE;
        int lowestIndex = 0;
        bool isDraw = false;

        foreach(EPlayerController currPlayer in GameObject.FindObjectsOfType<EPlayerController>())
        {
            index = currPlayer.GetNetworkID() / 1000;

            if(scores[index] == LEVEL_SPECIFIC_VALUE)
            {
                scores[index] = 0;
            }

            if (scores[index] < lowestScore)
            {
                lowestScore = scores[index];
                lowestIndex = index;
                isDraw = false;
            }
            else if (scores[index] == lowestScore)
            {
                isDraw = true;
            }

        }

        if (isDraw)
        {
            pView.RPC("RPC_DeclareOnClients", RpcTarget.All, "It's a tie");
        }
        else
        {
            pView.RPC("RPC_DeclareOnClients", RpcTarget.All, ("Player " + lowestIndex + " won"));
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
