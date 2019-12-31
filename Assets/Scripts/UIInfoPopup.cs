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

    private float recHeight;
    private float recWidth;

    private float centerX;
    private float centerY;

    void Start()
    {
        recHeight = GetComponent<RectTransform>().rect.height;
        recWidth = GetComponent<RectTransform>().rect.width;
        centerX = Screen.width / 2;
        centerY = Screen.height / 2;
    }
    void Update()
    {
        transform.position = GetAnchor();
    }

    public Vector2 GetAnchor()
    {
        if (Input.mousePosition.y > centerY)
        {
            //Top Left
            if (Input.mousePosition.x < centerX)
            {
                return new Vector2(Input.mousePosition.x + cursorOffset.x, Input.mousePosition.y + cursorOffset.y);
            }

            //Top Right
            else
            {
                return new Vector2(Input.mousePosition.x - (cursorOffset.x + recWidth), Input.mousePosition.y + cursorOffset.y);
            }
        }
        else
        {
            //Bottom Left
            if (Input.mousePosition.x < centerX)
            {
                return new Vector2(Input.mousePosition.x + cursorOffset.x, Input.mousePosition.y - cursorOffset.y + recHeight);
            }

            //Bottom Right
            else
            {
                return new Vector2(Input.mousePosition.x - (cursorOffset.x + recWidth), Input.mousePosition.y - cursorOffset.y + recHeight);
            }

        }
        //return new Vector2(1, 1);
    }
}
