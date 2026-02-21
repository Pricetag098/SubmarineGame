using UnityEngine;

public class ControlAdjustSound : MonoBehaviour
{
    [SerializeField] ControlLever control;
    [SerializeField] SoundPlayer soundPlayer;

    //play sound when changing value
    float value;
    bool primed;
    private void Start()
    {
        value = control.controlPercentage;
        control.onInteract += SetPrimed;
        control.onValueChanged += DoNoise;
    }

    void DoNoise(float newValue)
    {
        if(!primed)
            return;

        soundPlayer.Play();
        primed = false;

    }

    void SetPrimed(bool interacting)
    {
        primed = interacting;
    }
}
