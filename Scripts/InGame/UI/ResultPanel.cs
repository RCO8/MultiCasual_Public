using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TopTextField; //Nµî
    [SerializeField] private TextMeshProUGUI BottomTextField; //3ÃÊ ÈÄ

    private void Awake()
    {
        TopTextField = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        BottomTextField = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void ApplyRankText(string txt, Color col)
    {
        TopTextField.text = txt;
        TopTextField.color = col;
    }

    public void ApplyWaitText(int sec)
    {
        string wait = $"Wait for {sec} seconds...";
        BottomTextField.text = wait;
    }
}
