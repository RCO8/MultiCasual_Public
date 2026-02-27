using TMPro;
using UnityEngine;

public class UserNode : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI UserNameText;
    [SerializeField] private TextMeshProUGUI UserGameState;

    private string curName;
    public string Name { get => curName; }

    //서버로부터 데이터 수신
    public void ReceiveState(string name, string state)
    {
        curName = name;

        UserNameText.text = name;
        UserGameState.text = state;
    }
}
