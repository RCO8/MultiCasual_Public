using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;

public class MapStage : MonoBehaviour
{
    private List<BoxObject> boxesOnStage = new List<BoxObject>();
    private int boxCount;

    //서버로 보낼 데이터
    private Hashtable itemPacks;
    private ItemPackage itemPackages = new ItemPackage();

    private bool hasItem;
    private float itemType;
    private float randomItem;
    private int itemIndex;

    private void Awake()
    {
        itemPacks = new Hashtable()
        {
            {"BoxIndex", 0 },   //박스 인덱스
            {"HasItem", false }, //아이템 여부
            {"ItemType", 0f },  //아이템 타입 (물약 or 미사일)
            {"RandomItem", 0f },//아이템 고를 확률
            {"ItemIndex", 0 }   //아이템 랜덤
        };
    }

    private void Start()
    {
        //자식 오브젝트 안에 있는 박스들의 수
        boxCount = transform.childCount;

        for (int i = 0; i < boxCount; i++)  //박스 오브젝트의 컴포넌트 저장
            boxesOnStage.Add(transform.GetChild(i).GetComponent<BoxObject>());
        //Debug.Log("박스 개수 : " + boxesOnStage.Count);
    }

    public void SendDataToServer()
    {
        SettingRandomData();
    }

    /// <summary>
    /// 박스내 아이템을 랜덤한 수로 설정
    /// </summary>
    private void SettingRandomData()
    {
        for (int i = 0; i < boxCount; i++)
        {
            //랜덤값 설정
            hasItem = Random.Range(0f, 1f) > 0.5f;
            itemType = Random.Range(0f, 5f);
            randomItem = Random.Range(0f, 1f);
            itemIndex = Random.Range(1, 8);

            //CustomPropertie에 전달
            itemPacks["BoxIndex"] = i;
            itemPacks["HasItem"] = hasItem;
            itemPacks["ItemType"] = itemType;
            itemPacks["RandomItem"] = randomItem;
            itemPacks["ItemIndex"] = itemIndex;

            PhotonNetwork.SetPlayerCustomProperties(itemPacks);
        }
    }

    //서버에서 랜덤한 확률을 받아서 아이템 추가 (has, item)
    public void InsertItem(ItemPackage pack)
    {
        //현재 스테이지의 박스들의 아이템을 설정
        int idx = pack.BoxIndex;
        //pack.ShowValues();
        boxesOnStage[idx].InitItem(pack);
    }
}
