using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomContents : MonoBehaviour
{
    private RectTransform listLength;
    private RectTransform contentsSize;
    [SerializeField] private GameObject RoomNodePrefab;

    private float heightGrid = 100f;

    List<RoomNode> listInRooms = new List<RoomNode>();

    private void Awake()
    {
        listLength = GetComponent<RectTransform>();
        contentsSize = RoomNodePrefab.GetComponent<RectTransform>();
    }
}
