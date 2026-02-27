using TMPro;
using UnityEngine;

public class MyUserLabel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MyUserName;
    [SerializeField] private TextMeshProUGUI ConnectState;

    private string _userName;
    public string UserName
    {
        get => _userName;
        set
        {
            if(value != null) MyUserName.text = value;
        }
    }

    private void Start()
    {
        UserName = GameManager.Instance.UserName;
    }
}