using System.Collections;
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
        }
    }
    private Submarine _submarine;

    [SerializeField] private LidarDisplay lidarDisplay;

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
}
