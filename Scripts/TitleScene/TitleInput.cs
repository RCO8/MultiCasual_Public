using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField EnterName;
    [SerializeField] private RectTransform ErrorMessage;
    [SerializeField] private Button EnterGameButton;

    private void Awake()
    {
        ErrorMessage.gameObject.SetActive(false);
    }

    private void Start()
    {
        EnterGameButton.interactable = true;
    }

    public void EnterGame()
    {
        if (EnterName.text.Length > 0)  //닉네임이 입력되지 않으면
        {
            GameManager.Instance.UserName = EnterName.text;
            NetworkManager.Instance.JoinToServer();
            EnterGameButton.interactable = false;
        }
        else
        {
            ErrorMessage.gameObject.SetActive(true);
        }
    }

    public void CloseError()
    {
        ErrorMessage.gameObject.SetActive(false);
        EnterGameButton.interactable = true;
    }
}
