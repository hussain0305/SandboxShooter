using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    public GameObject dynamicBlock;
    public GameObject scoreboardRow;

    public float firstRowY;
    public float rowHeight;

    private float currentRowPosition;

    private void OnEnable()
    {
        currentRowPosition = firstRowY;
        //remove outdated entries
        foreach(Transform currChild in dynamicBlock.transform)
        {
            Destroy(currChild.gameObject);
        }
        EPlayerNetworkPresence[] allPlayers = GameObject.FindObjectsOfType<EPlayerNetworkPresence>();

        foreach(EPlayerNetworkPresence currPlayer in allPlayers)
        {
            GameObject currRow = Instantiate(scoreboardRow);
            Rect rT;
            
            rT = currRow.GetComponent<RectTransform>().rect;
            currRow.transform.GetChild(0).GetComponent<Text>().text = "" + currPlayer.GetID();
            currRow.transform.GetChild(1).GetComponent<Text>().text = "" + currPlayer.gameRecord.GetKills();
            currRow.transform.GetChild(2).GetComponent<Text>().text = "" + currPlayer.gameRecord.GetDeaths();
            currRow.transform.GetChild(3).GetComponent<Text>().text = "" + currPlayer.gameRecord.GetSpawnablesBroken();

            currRow.transform.SetParent(dynamicBlock.transform);
            currRow.transform.localScale = new Vector3(1, 1, 1);

            //rT.position = new Vector2(0, currentRowPosition);
            currRow.transform.localPosition = new Vector3(0, currentRowPosition, 0);
            currentRowPosition -= rowHeight;
            //currentRowPosition.y += rowHeight;
        }
    }
}
