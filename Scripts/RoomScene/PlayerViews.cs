using TMPro;
using UnityEngine;

public class PlayerViews : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI UserNameText;
    [SerializeField] private TextMeshProUGUI ReadyText;

    public bool ImReady { get; private set; }

    private void Awake()
    {
        //자동 초기
        UserNameText = transform.GetChild(2).Find("UserText").GetComponent<TextMeshProUGUI>();
        ReadyText = transform.GetChild(2).Find("StateText").GetComponent<TextMeshProUGUI>();

        if(ReadyText.text.Equals("Ready"))
            ReadyText.gameObject.SetActive(false);
    }

    public void UpdateUserName(string name)
    {
        UserNameText.text = name;
    }

    public void SetReady(bool ready)
    {
        ImReady = ready;
        ReadyText.gameObject.SetActive(ready);
    }
}
