using UnityEngine;

public class WheelNoise : MonoBehaviour
{
    [SerializeField] ControlLever helm;
    [SerializeField] AudioSource source;
    [SerializeField] float dieOffTime;
    [SerializeField] AnimationCurve dieOffCurve;

    float timer;

    private void Start()
    {
        helm.onValueChanged += SetVolume;
    }


    private void Update()
    {
        if(timer > 0)
            timer -= Time.deltaTime;
        source.volume = dieOffCurve.Evaluate(timer/dieOffTime);
    }
    void SetVolume(float _)
    {
        timer = dieOffTime;
    }
}
