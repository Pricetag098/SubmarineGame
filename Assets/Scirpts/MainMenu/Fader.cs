using UnityEngine;

public class Fader : MonoBehaviour
{

    [SerializeField] float fadeTime;
    [SerializeField] AnimationCurve fadeCurve;
    [SerializeField] CanvasGroup canvasGroup;

    float timer;

    void Update()
    {
        if(timer >= fadeTime)
            return;

        timer += Time.deltaTime;

        canvasGroup.alpha = fadeCurve.Evaluate(timer/fadeTime);

    }
}
