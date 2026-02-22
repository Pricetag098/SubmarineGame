
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEnder : MonoBehaviour
{
    public float TriggerPoint => triggerPoint;
    [SerializeField] float triggerPoint;

    [SerializeField] List<GameObject> deactivate = new List<GameObject>();
    [SerializeField] List<GameObject> activate = new List<GameObject>();
    [SerializeField] List<ControlLever> subControls = new List<ControlLever>();
    [SerializeField] List<AudioSource> allSounds = new List<AudioSource>();

    [SerializeField] TextMeshProUGUI display;
    [SerializeField] Color displayTextColour;

    [SerializeField] AudioSource loop;
    [SerializeField] SoundPlayer soundPlayer;

    [SerializeField] AudioClip impact;
    [SerializeField] AudioClip nessieDriveBy;
    [SerializeField] AudioClip alert;
    [SerializeField] AudioClip rebootLoop;
    [SerializeField] AudioClip alarm;
    [SerializeField] AudioClip alarm2;
    [SerializeField] AudioClip riser;

    [SerializeField] float descentSpeed;

    [SerializeField] float shutdownTime;
    [SerializeField] float rebootTime;
    [SerializeField] float hoverTime;
    [SerializeField] float attackTime;
    [SerializeField] float descentTime;

    [SerializeField] string scanString;

    [SerializeField] Image blackout;
    [SerializeField] CameraShake cameraShake;
    [SerializeField] Flicker alarmLight;


    float startDepth;
    float currentDepth;
    int stage;
    float timer;
    bool finalShake;

    [ContextMenu("Test")]
    public void Test()
    {
        BeginEnd(0);
    }

    public void BeginEnd(float subCurrentDepth)
    {
        startDepth = subCurrentDepth;

        foreach (GameObject go in deactivate)
            go.SetActive(false);

        foreach (GameObject go in activate)
            go.SetActive(true);

        foreach(ControlLever control in subControls)
            control.Disable();

        display.color = displayTextColour;
        display.text = "EE#%$RO#^R";

        soundPlayer.Play(impact);
        soundPlayer.Play(nessieDriveBy);

        cameraShake.Shake(.4f, .2f,1f);
        stage++;

    }

    string GetScanString(bool reverse, float t, float speed)
    {
        int position = Mathf.FloorToInt(t * speed);

        string scan = string.Empty;
        if (!reverse)
        {
            for (int i = 0; i < scanString.Length; ++i)
            {
                scan += scanString[(i + position + scanString.Length) % scanString.Length];
            }
        }
        else
        {
            for (int i = scanString.Length - 1; i >= 0; --i)
            {
                scan += scanString[(i + position + scanString.Length) % scanString.Length];
            }
        }

        return scan;
    }

    private void Update()
    {
        if (stage == 0)
            return;

        switch(stage)
        {
            case 1:
                timer += Time.deltaTime;
                if(timer > shutdownTime)
                {
                    soundPlayer.Play(alert);
                    loop.clip = rebootLoop;
                    loop.Play();
                    timer = 0;
                    stage++;
                }    

                break;

            case 2:

                timer += Time.deltaTime;


                string scanProgress = "";
                scanProgress += GetScanString(false, timer, 10f) + "\n";
                scanProgress += GetScanString(true, timer, 10f) + "\n";
                scanProgress += "REBOOTING:\n";
                scanProgress += GetScanString(false, timer, 10f) + "\n";
                scanProgress += GetScanString(true, timer, 10f) + "\n";


                display.text = scanProgress;

                if(timer >= rebootTime)
                {
                    timer = 0;
                    stage++;
                    loop.Stop();
                    display.text = "ASSESSING CONDITIONS";
                    soundPlayer.Play(alert);
                }

                break;

            case 3:

                timer += Time.deltaTime;
              

                if (timer > hoverTime)
                {
                    timer = 0;
                    display.fontSize = display.fontSize + .05f;
                    display.text = "PROXIMITY ALERT: COLOSSAL";

                    alarmLight.Flick(.5f, 4f);

                    loop.clip = alarm;
                    loop.Play();
                    stage++;
                }

            break;

            case 4:

                timer += Time.deltaTime;

                int tick = Mathf.FloorToInt(timer * 5);
                if (tick % 2 == 0)
                    display.text = "PROXIMITY ALERT: COLOSSAL";
                else
                    display.text = "";

                if (timer > attackTime)
                {
                    timer = 0;
                    display.text = "WARNING:\nUNCONTROLLED DESCENT";
                    loop.clip = alarm2;
                    loop.Play();
                    soundPlayer.Play(impact);
                    soundPlayer.Play(riser);

                    alarmLight.Flick(.4f, 15f);
                    cameraShake.Shake(.4f, .2f, 1f);
                  

                    currentDepth = startDepth;

                    stage++;
                }

                break;

            case 5:

                timer += Time.deltaTime;

                if(timer > 1f && !finalShake)
                {
                    finalShake = true;
                    cameraShake.Shake(.1f, .3f, 15f);
                }

                currentDepth += descentSpeed * Time.deltaTime;
                float displayDepth = startDepth - currentDepth;

                int tick2 = Mathf.FloorToInt(timer * 2);
                if (tick2 % 2 == 0)
                    display.text = "WARNING:\nUNCONTROLLED DESCENT\n" + displayDepth.ToString("00.0") + "m";
                else
                    display.text = "";

                if (timer > descentTime)
                {
                    timer = 0;
                    loop.Stop();
                    blackout.gameObject.SetActive(true);
                    foreach(AudioSource source in allSounds)
                        source.enabled = false;


                    stage++;
                }

                break;
        }
    }
}
