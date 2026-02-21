using UnityEngine;

public class WheelNoise : MonoBehaviour
{
    [SerializeField] ControlLever helm;
    [SerializeField] AudioSource source;
    [SerializeField] float dieOffTime;
    [SerializeField] AnimationCurve dieOffCurve;
    [SerializeField] float requiredDelta;

    float timer;
    float currentDelta;
    float previousValue;


    private void Start()
    {
        helm.onValueChanged += WheelMoved;
    }


    private void Update()
    {
        if(timer > 0)
            timer -= Time.deltaTime;
        source.volume = dieOffCurve.Evaluate(timer/dieOffTime);
    }
    void WheelMoved(float wheelValue)
    {
        currentDelta += Mathf.Abs(wheelValue - previousValue);
        previousValue = wheelValue;

        if(currentDelta > requiredDelta)
        {
            currentDelta = 0;
            timer = dieOffTime;
        }

  
    }
}
