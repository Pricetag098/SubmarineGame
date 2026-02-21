using UnityEngine;

public class SubEngineSoundController : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] ControlLever thrustControl;
    [SerializeField] AnimationCurve curve;
    [SerializeField] float rampTime;

    float timer;

    private void Update()
    {
        if(thrustControl.controlPercentage != 0)
            timer = Mathf.Clamp(timer + Time.deltaTime, 0f, rampTime);
 
        else
            timer = Mathf.Clamp(timer - Time.deltaTime, 0f, rampTime);

        source.volume = curve.Evaluate(timer / rampTime);
    }

}
