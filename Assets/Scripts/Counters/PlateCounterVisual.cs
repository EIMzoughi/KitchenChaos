using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour
{
    [SerializeField] private Transform Spawn;
    [SerializeField] private Transform plateVisualPrefab;
    [SerializeField] private PlateCounter plateCounter;

    private List<GameObject> plateVisualGameObjectList;

    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
    }
    private void Start()
    {
        plateCounter.onPlateSpawn += PlateCounter_onPlateSpawn;
        plateCounter.onPlateGrab += PlateCounter_onPlateGrab;
    }

    private void PlateCounter_onPlateGrab(object sender, System.EventArgs e)
    {
        GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
        plateVisualGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void PlateCounter_onPlateSpawn(object sender, System.EventArgs e)
    {
        Transform plateVisualTranform = Instantiate(plateVisualPrefab,Spawn);

        float plateOffsetY = .1f;
        plateVisualTranform.localPosition = new Vector3(0,plateOffsetY * plateVisualGameObjectList.Count, 0);

        plateVisualGameObjectList.Add(plateVisualTranform.gameObject);
    }
}
