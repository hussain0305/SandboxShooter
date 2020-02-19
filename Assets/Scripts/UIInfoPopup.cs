using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoPopup : MonoBehaviour
{
    public Text spawnableName;
    public Text description;
    public Image spawnableImage;
    public Text attackValue;
    public Text healthValue;
    public Text dpSecondValue;
    public Text dpShotValue;

    public Vector2 cursorOffset;

    private float centerX;
    private float centerY;

    void Start()
    {
        centerX = Screen.width / 2;
        centerY = Screen.height / 2;
    }
    void Update()
    {
        transform.position = GetAnchor(Input.mousePosition);
    }

    public Vector2 GetAnchor(Vector3 mousePos)
    {
        if (mousePos.y > centerY)
        {
            //Top Left
            if (mousePos.x < centerX)
            {
                GetComponent<RectTransform>().pivot = new Vector2(0, 1);
                return new Vector2(mousePos.x + cursorOffset.x, mousePos.y + cursorOffset.y);
            }

            //Top Right
            else
            {
                GetComponent<RectTransform>().pivot = new Vector2(1, 1);
                return new Vector2(mousePos.x - cursorOffset.x, mousePos.y + cursorOffset.y);
            }
        }
        else
        {
            //Bottom Left
            if (Input.mousePosition.x < centerX)
            {
                GetComponent<RectTransform>().pivot = new Vector2(0, 0);
                return new Vector2(mousePos.x + cursorOffset.x, mousePos.y - cursorOffset.y);
            }

            //Bottom Right
            else
            {
                GetComponent<RectTransform>().pivot = new Vector2(1, 0);
                return new Vector2(mousePos.x - cursorOffset.x, mousePos.y - cursorOffset.y);
            }

        }

        //return new Vector2(1, 1);
    }
}
