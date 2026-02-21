using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] AnimationCurve motionCurve;
    [SerializeField] AnimationCurve intensityCurve;

    float magnitude;
    float duration;
    float frequency;
    float timer;
    bool invertIntensity;
    bool endMode;

    //you f-ed this up by not subtracting during mathf repeat
    //you can fix it if you want to do so that badly
    public void Shake(float magnitude, float frequency, float duration, bool invertIntesity = false)
    {
        this.magnitude = magnitude;
        this.frequency = frequency;
        this.duration = duration;
        this.invertIntensity = invertIntesity;
        timer = duration;
    }

    //it is 4.48am
    public void EndMode()
    {
        endMode = true;
    }

    private void Update()
    {
        if (timer <= 0)
            return;

        timer -= Time.deltaTime;

        float t = invertIntensity ? timer/duration : 1 - (timer/duration);
        t = endMode ? 1 - t : t;

        float currentIntensity = intensityCurve.Evaluate(t) * magnitude;


        float effectiveFrequency = frequency * intensityCurve.Evaluate(t);

        float frequencyTimer = Mathf.Repeat(timer, effectiveFrequency);

        if (effectiveFrequency > 0)
            transform.localPosition = new Vector3(0,motionCurve.Evaluate(frequencyTimer/effectiveFrequency) * currentIntensity, 0);

        if(timer <= 0)
            transform.localPosition = Vector3.zero;
    }


}
