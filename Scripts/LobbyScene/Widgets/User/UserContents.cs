using System.Collections.Generic;
using UnityEngine;

public class UserContents : MonoBehaviour
{
    private RectTransform listLength;
    private RectTransform contentsSize;
    [SerializeField] private GameObject UserNodePrefab;

    private float heightGrid = 70f;

    List<UserNode> anotherUsers = new List<UserNode>();

    private void Awake()
    {
        listLength = GetComponent<RectTransform>();
        contentsSize = UserNodePrefab.GetComponent<RectTransform>();
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            anotherUsers.Add(transform.GetChild(i).GetComponent<UserNode>());
        }
        UpdateSize();
    }

    private void LateUpdate()
    {
        UpdateSize();
    }

    /// <summary>
    /// UserNode수 만큼 사이즈 갱신
    /// </summary>
    private void UpdateSize()
    {
        listLength.sizeDelta = new Vector2(340f, heightGrid * transform.childCount);
    }

    //로비 서버에서 유저 목록을 가져오기
    private void GetUsersFromServer()
    {
        //사용자가 로비 서버에 접속하면
        //anotherUsers에 추가하고 ContentsUI에 UserNode를 생성후 표시
    }
}