using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    private TextMeshProUGUI textPanel;
    private const string loading = "LOADING";
    private const string dot = ".";
    private string blink;

    private void Awake()
    {
        textPanel = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        blink = dot;
        textPanel.text = loading + dot;

        StartCoroutine(DotBlink());
    }

    private void OnEnable()
    {
        gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// ... 효과
    /// </summary>
    /// <returns></returns>
    private IEnumerator DotBlink()
    {
        while (true)
        {
            blink += dot;
            ApplyText();
            if (blink.Length > 3)
                blink = dot;

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void ApplyText()
    {
        textPanel.text = loading + blink;
    }
}
