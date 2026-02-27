using System.Collections;
using UnityEngine;

public class PlayerInvisible : MonoBehaviour
{
    [SerializeField] private GameObject viewObject;
    [SerializeField] private Collider collider;

    private void Awake()
    {
        viewObject = transform.GetChild(0).gameObject;
        collider = GetComponent<Collider>();
    }

    public void StartInvisible(float time)
    {
        StartCoroutine(EnableTime(time));
    }

    private void SetInvisible(bool enable)
    {
        viewObject.SetActive(enable);
        collider.enabled = enable;
    }

    private IEnumerator EnableTime(float t)
    {
        float curTime = 0f;
        SetInvisible(true);
        while (curTime < t)
        {
            curTime += Time.deltaTime;
            yield return null;
        }
        SetInvisible(false);
    }
}
