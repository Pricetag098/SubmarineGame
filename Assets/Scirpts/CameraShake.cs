using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] AnimationCurve motionCurve;
    [SerializeField] AnimationCurve baseIntensityCurve;

    float magnitude;
    float duration;
    float frequency;
    float timer;
    AnimationCurve intensity;

    public void Shake(float magnitude, float frequency, float duration)
    {
        this.magnitude = magnitude;
        this.frequency = frequency;
        this.duration = duration;
        timer = duration;
        intensity = baseIntensityCurve;
    }


    private void Update()
    {
        if (timer <= 0)
            return;

        timer -= Time.deltaTime;

        float t = 1f - (timer/duration);

        //shake gets bigger over time
        float currentAmplitude = intensity.Evaluate(t) * magnitude;

        //frequency gets smaller over time
        float effectiveFrequency = frequency * baseIntensityCurve.Evaluate(1 - t);

        float frequencyTimer = Mathf.Repeat(duration - timer, effectiveFrequency);

        if (effectiveFrequency > 0)

            transform.localPosition = new Vector3(0,motionCurve.Evaluate(frequencyTimer/effectiveFrequency) * currentAmplitude, 0);

        if(timer <= 0)
            transform.localPosition = Vector3.zero;
    }


}
