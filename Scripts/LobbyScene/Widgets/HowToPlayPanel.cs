using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayPanel : MonoBehaviour
{
    [SerializeField] private RectTransform GameView;
    [SerializeField] private Button Prev, Next;
    private List<GameObject> ViewList = new List<GameObject>();

    private int curViewIdx = 0;

    private void Awake()
    {
        for (int i = 0; i < GameView.childCount; i++)
        {
            ViewList.Add(GameView.GetChild(i).gameObject);
            ViewList[i].SetActive(false);
        }

        //항목들이 1개 이하일때만 전후버튼 숨김
        if(ViewList.Count <= 1)
        {
            Prev.gameObject.SetActive(false);
            Next.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        curViewIdx = 0;
        for (int i = 0; i < ViewList.Count; i++)
            ViewList[i].SetActive(false);
        ViewList[curViewIdx].SetActive(true);

        Prev.interactable = false;
        Next.interactable = true;
    }

    #region 각 설명란을 페이지 별로 교체
    public void PrevPanel()
    {
        ViewList[curViewIdx].SetActive(false);
        ViewList[--curViewIdx].SetActive(true);

        Prev.interactable = curViewIdx > 0;

        Next.interactable = true;
    }
    public void NextPanel()
    {
        ViewList[curViewIdx].SetActive(false);
        ViewList[++curViewIdx].SetActive(true);

        Next.interactable = curViewIdx < GameView.childCount - 1;

        Prev.interactable = true;
    }
    #endregion
}
