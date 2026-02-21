using UnityEngine;

public class EnemyFish : MonoBehaviour
{
    [SerializeField] private Transform model;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float detectionRange = 10;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private ConfigurableJoint joint;
    [SerializeField] private float wiggleRate, wiggleAngle;

    private Submarine submarine;
    private Vector3 targetPoint;
    private Quaternion initialRot;

    [SerializeField] private State state;
    enum State
    {
        Idle,
        Roam,
        Chasing
    }
    private void Awake()
    {
        initialRot = model.localRotation;
    }

    private void Update()
    {
        if (!submarine)
            submarine = FindAnyObjectByType<Submarine>();
        model.localRotation = initialRot * Quaternion.Euler(Vector3.forward * Mathf.Sin(Time.time * (Vector3.Dot(_rigidbody.linearVelocity, transform.forward) / moveSpeed) * Mathf.PI * wiggleRate) *wiggleAngle);
        transform.forward = targetPoint - transform.position;

        switch (state)
        {
            case State.Chasing:
                targetPoint = submarine.transform.position;
                break;  
        }
    }
    private void FixedUpdate()
    {
        joint.targetVelocity = transform.forward * moveSpeed;
    }
}
