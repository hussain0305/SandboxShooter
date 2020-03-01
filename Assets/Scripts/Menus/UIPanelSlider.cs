﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelSlider : MonoBehaviour
{
    public float slideSpeed;
    private float panelWidth;
    private float panelHeight;
    private float workingPanelWidth;
    private RectTransform rTransform;

    private Vector2 outAnchoredPosition;
    private Vector2 inAnchoredPosition;
    // Start is called before the first frame update
    void Start()
    {
        FetchComponentsAndOrient();
    }

    void FetchComponentsAndOrient()
    {
        rTransform = GetComponent<RectTransform>();
        panelWidth = rTransform.rect.width;
        panelHeight = rTransform.rect.height;
        workingPanelWidth = panelWidth + 30;
        outAnchoredPosition = new Vector2(workingPanelWidth / 2, 0);
        inAnchoredPosition = new Vector2(-workingPanelWidth / 2, 0);
        rTransform.anchoredPosition = outAnchoredPosition;
    }

    public void MoveIn()
    {
        StopAllCoroutines();
        StartCoroutine(SlideIn());
    }

    public void MoveOut()
    {
        StopAllCoroutines();
        StartCoroutine(SlideOut());

    }

    IEnumerator SlideIn()
    {
        while(Vector2.Distance(rTransform.anchoredPosition, inAnchoredPosition) > 0.2f)
        {
            rTransform.anchoredPosition = Vector2.Lerp(rTransform.anchoredPosition, inAnchoredPosition, Time.deltaTime * slideSpeed);
            yield return new WaitForEndOfFrame();
        }
        rTransform.anchoredPosition = inAnchoredPosition;
    }

    IEnumerator SlideOut()
    {
        while (Vector2.Distance(rTransform.anchoredPosition, outAnchoredPosition) > 0.2f)
        {
            rTransform.anchoredPosition = Vector2.Lerp(rTransform.anchoredPosition, outAnchoredPosition, Time.deltaTime * slideSpeed);
            yield return new WaitForEndOfFrame();
        }
        rTransform.anchoredPosition = outAnchoredPosition;

    }

}