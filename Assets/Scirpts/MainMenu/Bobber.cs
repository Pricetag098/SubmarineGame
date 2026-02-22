using UnityEngine;

public class Bobber : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] float bobRate;
    [SerializeField] float bobMagnitude;

    float timer;
    float yInitial;

    private void Start()
    {
        yInitial = transform.position.y;
    }

    private void Update()
    {
        timer = Mathf.Repeat(timer + Time.deltaTime, bobRate);

        float position = yInitial + (bobMagnitude * curve.Evaluate(timer/bobRate));

        transform.position = new Vector3(transform.position.x, position, transform.position.z);
    }

}
