
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Surveyor : MonoBehaviour
{
    [SerializeField] List<PointOfInterest> pointsOfInterest = new List<PointOfInterest>();
    [SerializeField] TextMeshProUGUI display;
    [SerializeField] float scannerRange;
    [SerializeField] float scanningTime;
    [SerializeField] float scanCooldown;

    [SerializeField] string scanString;

    [SerializeField] SoundPlayer soundPlayer;
    [SerializeField] AudioClip alert;
    [SerializeField] AudioClip fail;
    [SerializeField] AudioClip succeed;
    [SerializeField] AudioSource scanLoop;

    enum State { Idle, Scanning, Cooldown}
    State state;

    string[] alpha = { "A", "B", "C", "D", "E", "F", "G" };
    Submarine submarine;
    float timer;
    bool alertReady;

    [ContextMenu("Scan")]
    public void StartScan()
    {
        timer = 0f;

        if (!InRange())
        {
            state = State.Cooldown;
            display.text = "NO SITE IN RANGE";
            scanLoop.volume = 0f;
            soundPlayer.Play(fail);
        }

        else
        {
            state = State.Scanning;
            scanLoop.volume = 1f;
        }
    }

    private void Update()
    {
        if (submarine == null)
        {
            submarine = FindAnyObjectByType<Submarine>();
            return;
        }

        foreach (PointOfInterest point in pointsOfInterest)
            point.currentDistance = Vector3.Distance(submarine.transform.position, point.position);

        switch (state)
        {
            case State.Idle:

                if(!InRange())
                    alertReady = true;
                else if(alertReady)
                {
                    soundPlayer.Play(alert);
                    alertReady = false;
                }

                display.text = GetOutput();

                break;

            case State.Scanning:

                if(!InRange())
                {
                    timer = 0;
                    state = State.Cooldown;
                    display.text = "SCAN ABORTED";
                    scanLoop.volume = 0f;
                    soundPlayer.Play(fail);
                }

                timer += Time.deltaTime;

                int position = Mathf.FloorToInt(timer * 10);
                string scanProgress = "SCANNING:\n\n";
                for(int i = 0; i < scanString.Length; ++i)
                {
                    scanProgress += scanString[(i + position + scanString.Length) % scanString.Length];
                }    

                display.text = scanProgress;


                if(timer >= scanningTime)
                {
                    timer = 0;
                    state = State.Cooldown;

                    foreach(PointOfInterest point in pointsOfInterest)
                        if(point.currentDistance < scannerRange)
                            point.isScanned = true;

                    display.text = "SCAN SUCCESSFUL";
                    scanLoop.volume = 0f;
                    soundPlayer.Play(succeed);
                }    

                break;

            case State.Cooldown:

                timer += Time.deltaTime;
                if(timer >= scanCooldown)
                {
                    timer = 0;
                    state = State.Idle;
                }    

                break;

   
        }
    }


    public string GetOutput()
    {
        string output = string.Empty;

        if(InRange())
        {
            return "READY TO SCAN";
        }


        for (int i = 0; i < pointsOfInterest.Count; ++i)
        {
            if (!pointsOfInterest[i].isScanned)
                output += "SITE " + alpha[i] + ": " + pointsOfInterest[i].currentDistance.ToString("0.00") + "\n";
        }

        return output;

    }

    bool InRange()
    {
        foreach (PointOfInterest point in pointsOfInterest)
            if(!point.isScanned && point.currentDistance < scannerRange)
                return true;

        return false;
    }

    [System.Serializable]
    class PointOfInterest
    {
        public Vector3 position;
        public float currentDistance;
        public bool isScanned;
    }
}
