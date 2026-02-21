
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Surveyor : MonoBehaviour
{
    [SerializeField] float exitDepth;
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

    [SerializeField] GameEnder gameEnder;
    enum State { Awating, Idle, Scanning, Cooldown, Final}
    State state;

    string[] alpha = { "A", "B", "C", "D", "E", "F", "G" };
    Submarine submarine;
    float timer;
    bool alertReady;

    [ContextMenu("End Game")]
    public void CheatEnding()
    {
        soundPlayer.Play(alert);
        state = State.Final;
    }
    public void StartScan()
    {
        if(state == State.Awating)
        {
            soundPlayer.Play(succeed);
            state = State.Cooldown;
            display.text = "LOADING...";
            return;
        }

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
        {
            point.currentDistance = Vector3.Distance(submarine.transform.position, point.position);

            Vector2 subPosition = new Vector2(submarine.transform.position.x, submarine.transform.position.z);
            Vector2 pointPosition = new Vector2(point.position.x, point.position.z);
            Vector2 directionToPoint = (pointPosition - subPosition).normalized;
            Vector2 subHeading = new Vector2(submarine.transform.forward.x, submarine.transform.forward.z);

            //heading is the angle between our forward vector and the direction to the point
            float angle = Vector2.Angle(subHeading, directionToPoint);

            point.currentHeading = angle;
        }

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
                    break;
                }

                timer += Time.deltaTime;

                int position = Mathf.FloorToInt(timer * 20);
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
                    if(!CompletedGame())
                        state = State.Idle;
                    else
                    {
                        soundPlayer.Play(alert);
                        state = State.Final;
                    }
                }    

                break;

            case State.Final:

                float distance = exitDepth - submarine.transform.position.y;

                display.text = "OBJECTIVE COMPLETE\nASCEND TO EXIT\n" + distance.ToString("00.0") + "m";

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
            {
                output += "SITE " + alpha[i] + ": " + pointsOfInterest[i].currentDistance.ToString("0.00") + "m";
                output += " | " + pointsOfInterest[i].currentHeading.ToString("0") + "°";
                output += "\n";
            }
        }

        return output;

    }

    bool CompletedGame()
    {
        foreach (PointOfInterest point in pointsOfInterest)
            if (!point.isScanned)
                return false;

        return true;
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
        public float currentHeading;
        public bool isScanned;
    }
}
