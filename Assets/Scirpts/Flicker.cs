using UnityEngine;

public class Flicker : MonoBehaviour
{
    [SerializeField] Light lightSource;
    [SerializeField] AnimationCurve curve;

    float baseIntensity;
    float frequency;
    float duration;
    float timer;

    private void Start()
    {
        baseIntensity = lightSource.intensity;
    }

    public void Flick(float frequency, float duration)
    {
        this.duration = duration;
        this.frequency = frequency;
        timer = duration;
    }

    private void Update()
    {
        if (timer <= 0)
            return;

        timer -= Time.deltaTime;

        float t = Mathf.Repeat(duration - timer, frequency);

        lightSource.intensity = baseIntensity * curve.Evaluate(t);

        if (timer <= 0)
            lightSource.intensity = baseIntensity;

    }
}
