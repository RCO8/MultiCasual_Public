using System.Collections;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    //프리팹 로직
    [SerializeField] protected GameObject FirePrefab;
    [SerializeField] protected GameObject ExplosePrefab;
    [SerializeField] protected GameObject ExplosePrefabPower;
    private GameObject curExploseObject;

    //물리 및 파티클
    protected ParticleSystem exploseParticle;
    private Rigidbody rgdby;

    private float shootSpeed;
    protected int curIndex;   //자신이 쏜 불덩이가 자신에 맞지 못하게
    protected float curDamage;  //타 플레이어가 타격을 받을 데미지

    //슈팅시 지속시간
    private bool goAhead = false;   //폭발전 나갈 신호
    private bool isShoot = false;   //발사했다는 신호
    private float delayTime = 0f;
    private float maxTime = 0.5f;

    private Vector3 direc = Vector3.zero;   //쏘아진 방향

    private void Awake()
    {
        //프리팹 설정
        if (FirePrefab == null) //없으면 자동으로 설정
            FirePrefab = transform.GetChild(0).gameObject;
        if (ExplosePrefab != null || ExplosePrefabPower != null)  //폭발 프리팹 설정
        {
            //프리팹 생성 (자신의 transform을 자식으로 저장)
            curExploseObject = Instantiate(ExplosePrefab, transform);
            Instantiate(ExplosePrefabPower, transform).SetActive(false);
            curExploseObject.SetActive(false);

            //파티클 선언
            exploseParticle = curExploseObject.GetComponentInChildren<ParticleSystem>();
        }
        rgdby = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isShoot)    //발사시 일정시간 동안 앞으로
        {
            delayTime += Time.deltaTime;
            if(delayTime > maxTime) FireExplose();
        }
    }

    private void FixedUpdate()
    {
        rgdby.velocity = goAhead ? (direc * shootSpeed) : Vector2.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        int targetLayer = other.gameObject.layer;
        int playerLayer = LayerMask.NameToLayer("Player");
        int boxLayer = LayerMask.NameToLayer("Box");

        if (playerLayer.Equals(targetLayer))
        {
            PlayerBase target = other.gameObject.GetComponent<PlayerBase>();
            int checkIdx = target.CurIndex;

            if (checkIdx != curIndex)
            {
                target.GetDamage(curDamage);
                FireExplose();
            }
        }

        if(boxLayer.Equals(targetLayer))
        {
            BoxObject box = other.gameObject.GetComponent<BoxObject>();
            box.GetDamage(curDamage);
            FireExplose();
        }
    }

    /// <summary>
    /// 슈팅 신호 시작
    /// </summary>
    /// <param name="startPos">현재 위치</param>
    /// <param name="setTime">지속시간</param>
    /// <param name="setDamage">현재 데미지</param>
    public void Shooting(Transform startPos, float setTime, float setDamage, float setBoost, int setIdx)
    {
        if (!isShoot)
        {
            //발사시 플레이어 인덱스 발급
            curIndex = setIdx;

            //파이어볼 활성
            gameObject.SetActive(true);
            FirePrefab.SetActive(true);
            curExploseObject.SetActive(false);
            isShoot = true;
            goAhead = true;

            //방향 및 각도 설정
            transform.position = startPos.position;
            direc = startPos.forward;
            shootSpeed = setBoost;

            //지속시간 및 데미지 설정 (SetRange)
            maxTime = setTime;

            //파이어볼 위력 설정 (SetPower)
            curDamage = setDamage;
            SwapExplose(setDamage > 3f ? 2 : 1);
        }
    }

    /// <summary>
    /// 위력 설정 변경
    /// </summary>
    private void SwapExplose(int type)
    {
        curExploseObject = transform.GetChild(type).gameObject;
        exploseParticle = curExploseObject.GetComponentInChildren<ParticleSystem>();
        curExploseObject.SetActive(false);
    }

    /// <summary>
    /// 슈팅 지속시간이 끝날 때
    /// </summary>
    private void FireExplose()
    {
        //Debug.Log("아야..");

        goAhead = false;
        isShoot = false;
        delayTime = 0f;

        //파이어볼이 폭발하도록
        FirePrefab.SetActive(false);
        curExploseObject.SetActive(true);

        //폭발 기능 시작
        StartCoroutine(ParticleTime());
    }

    /// <summary>
    /// 파티클이 한사이클이 끝나면
    /// </summary>
    private void EndCycleParticle()
    {
        gameObject.SetActive(false);
    }
    private IEnumerator ParticleTime()
    {
        yield return new WaitForSeconds(exploseParticle.duration);

        EndCycleParticle();
    }
}
