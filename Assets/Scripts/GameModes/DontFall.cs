using Photon.Pun;
using System.Collections;
using UnityEngine;

public class DontFall : BaseGameMode
{
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
            if (currentScore != 0)
            {
                GameObject newRow = Instantiate(new GameObject());
                newRow.transform.SetParent(scoreboardHeader);
                newRow.transform.localPosition = new Vector3(0, 0, 0);
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
            else if (currentScore == highestScore)
            {
                isDraw = true;
            }

            loop++;
        }

        foreach (Transform currChild in scoreboardHeader)
        {
            Destroy(currChild.gameObject);
        }

        if (isDraw)
        {
            scoreboardHeader.GetComponent<TextMesh>().text = "It's a tie";
        }
        else
        {
            scoreboardHeader.GetComponent<TextMesh>().text = "Player " + highestIndex + " won";
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
}
