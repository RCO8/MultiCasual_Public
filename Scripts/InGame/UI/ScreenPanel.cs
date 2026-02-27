using UnityEngine;

public class ScreenPanel : MonoBehaviour
{
    private ResultPanel resultPanel;
    private LoadingPanel loadingPanel;

    public ResultPanel Result { get => resultPanel; }

    private void Awake()
    {
        //자식 오브젝트에 있는 컴포넌트
        resultPanel = GetComponentInChildren<ResultPanel>();
        loadingPanel = GetComponentInChildren<LoadingPanel>();
    }

    private void Start()
    {
        //GameManager 필드 등록
        GameManager.Instance.GameSetPanel = this;

        GameReady();
    }

    private void GameReady()
    {
        resultPanel.gameObject.SetActive(false);
        loadingPanel.gameObject.SetActive(true);
    }

    public void GameStart()
    {
        resultPanel.gameObject.SetActive(false);
        loadingPanel.gameObject.SetActive(false);
    }

    public void GameSet()
    {
        resultPanel.gameObject.SetActive(true);
        loadingPanel.gameObject.SetActive(false);
    }
}
