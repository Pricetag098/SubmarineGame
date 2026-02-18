using UnityEngine;

public class ObjectWiggler : MonoBehaviour
{
    [SerializeField] private float _radius, speed;
    private Vector3 _position, target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        if(Vector3.Distance(transform.position, target) < .1)
        {
            target = _position + Random.insideUnitSphere * _radius;
        }
    }
}
