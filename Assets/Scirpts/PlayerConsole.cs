using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConsole : MonoBehaviour
{
    public Submarine submarine
    {
        get => _submarine;
        set
        {
            _submarine = value;
            lidarDisplay.lidarScanner = submarine.scanner;
            lidarDisplay.sourcePosition = submarine.transform;
            thermalDisplay.camera = submarine.thermalCamera;
            thermalDisplay.Setup();
        }
    }
    private Submarine _submarine;

    [SerializeField] private LidarDisplay lidarDisplay;
    [SerializeField] private ThermalDisplay thermalDisplay;
    [SerializeField] private DamageSource hullDamagePrefab;
    [SerializeField] private List<Transform> sparkPoints;

    private void Start()
    {
        StartCoroutine(RunCamTest());
    }

    private void Update()
    {
        
    }

    IEnumerator RunCamTest()
    {
        while (true)
        {
            lidarDisplay.lidarScanner.Scan();
            yield return new WaitForSeconds(.5f);
        }
        
    }
    public void DealHullDamage(float amount)
    {
        var point = sparkPoints[Random.RandomRange(0, sparkPoints.Count)];
        var spawnedDamageThing = (Instantiate(hullDamagePrefab) as DamageSource);
        spawnedDamageThing.Amount = amount;
        spawnedDamageThing.transform.position = point.position;
        spawnedDamageThing.transform.rotation = point.rotation;
    }
}
