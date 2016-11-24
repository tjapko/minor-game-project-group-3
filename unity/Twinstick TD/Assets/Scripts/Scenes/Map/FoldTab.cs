using UnityEngine;
using System.Collections;

/// <summary>
/// Class FoldTab
/// Functions to expand or fold tabs in UI
/// </summary>
public class FoldTab : MonoBehaviour {
    private Vector3 originalPosition;
    private Vector3 currentPosition;
    bool fold_position;

    //Constructer
    public void Awake()
    {
        originalPosition = GetComponent<RectTransform>().transform.position;
        currentPosition = originalPosition;
        fold_position = true;
    }

    public void MovetabHorizontal(int units)
    {
        //UI has been folded -> open
        if (fold_position)
        {
            //Open UI
            fold_position = false;  //UI is now open
            currentPosition[0] += units;
            GetComponent<RectTransform>().transform.position = currentPosition;
        } else
        {
            //Unfold UI
            fold_position = true;   //UI is now folded
            resetPosition();
        }
    }

    public void resetPosition()
    {
        currentPosition = originalPosition;
        GetComponent<RectTransform>().transform.position = currentPosition;
    }
}
