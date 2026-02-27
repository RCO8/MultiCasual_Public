using UnityEngine;

public class BoxObject : MonoBehaviour
{
    [SerializeField] private GameObject BoxModel;
    [SerializeField] private GameObject InItem;
    private Collider collider;

    //아이템 속성
    private string itemPath = "Prefabs/ItemObjects/";

    #region 아이템 열거형
    private enum PotionType { SpeedUpItem = 1, PowerUpItem, RangeUpItem, BoostUpItem, HiddenItem }
    private enum MissileType { MiniMissile = 1, SpeedMissile, ChaserMissile, AlphaMissile, PowerMissile, MineMissile, NuclearMissile }
    #endregion

    private float curBoxHP = 8f;
    private ItemPackage setItemPack;

    private void Awake()
    {
        //박스 모델 등록
        BoxModel = transform.Find("BoxReady").gameObject;
        collider = GetComponent<BoxCollider>();
    }

    #region 아이템 생성
    public void InitItem(ItemPackage pack)
    {
        setItemPack = pack;
        SettingItem();
    }

    private void SettingItem()
    {
        //20%확률의 미사일
        if (setItemPack.ItemType > 4f)
        {
            itemPath += "Missiles/";
            InitMissile(setItemPack.RandomItem);
        }
        else
        {
            itemPath += "Potions/";
            InitPotion(setItemPack.RandomItem);
        }

        //Debug.Log(itemPath);
    }
    private void InitPotion(float r)
    {
        PotionType curPotion;
        int randomPotion;

        //속도, 파워, 가속, 길이 : 80%
        //투명 : 20%
        if (r > 0.2f) randomPotion = Mathf.Clamp(setItemPack.ItemIndex, 1, 4);
        else randomPotion = Mathf.Clamp(setItemPack.ItemIndex, 5, 5);

        //프리팹 추가
        curPotion = (PotionType)randomPotion;
        itemPath += curPotion.ToString();
        //Debug.Log(itemPath);

        CreateItem();
    }
    private void InitMissile(float r)
    {
        MissileType curMissile;
        int randomMissile;

        //60% 확률의 평범한 미사일
        if (r > 0.4f) randomMissile = Mathf.Clamp(setItemPack.ItemIndex, 1, 3);
        else if (r > 0.1f) randomMissile = Mathf.Clamp(setItemPack.ItemIndex, 4, 5);
        else randomMissile = Mathf.Clamp(setItemPack.ItemIndex, 6, 7);

        curMissile = (MissileType)randomMissile;
        itemPath += randomMissile + "_" + curMissile.ToString();
        //Debug.Log(itemPath);

        CreateItem();
    }
    private void CreateItem()
    {
        InItem = Resources.Load<GameObject>(itemPath);
        if (InItem != null)
        {
            Instantiate(InItem, transform);
            InItem = transform.GetChild(1).gameObject;
            InItem.SetActive(false);
        }
        itemPath = "Prefabs/ItemObjects/";
    }
    #endregion

    #region 박스 데미지
    public void GetDamage(float dmg)
    {
        curBoxHP -= dmg;
        if (curBoxHP < 0) BoxCrash();
    }
    private void BoxCrash()
    {
        //아이템 있으면 드랍
        if (InItem != null) InItem.SetActive(true);
        BoxModel.SetActive(false);
        collider.enabled = false;
    }
    #endregion
}
