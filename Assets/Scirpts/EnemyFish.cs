using UnityEngine;

public class EnemyFish : MonoBehaviour
{
    [SerializeField] private Transform model;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float detectionRange = 10;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private ConfigurableJoint joint;
    [SerializeField] private float wiggleRate, wiggleAngle;
    [SerializeField] private float damageOnHit;
    [SerializeField] private float damageCooldown;
    [SerializeField] private float damageRange;
    [SerializeField] private float agroRange, dropAgroRange;

    [SerializeField] private SoundPlayer soundPlayer;

    private Vector3 startPos;
    private float timeLastAttacked;
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
        startPos = transform.position;
    }

    private void Update()
    {
        if (!submarine)
        {
            submarine = FindAnyObjectByType<Submarine>();
            return;
        }
            
        model.localRotation = initialRot * Quaternion.Euler(Vector3.forward * Mathf.Sin(Time.time * (Vector3.Dot(_rigidbody.linearVelocity, transform.forward) / moveSpeed) * Mathf.PI * wiggleRate) *wiggleAngle);
        
        
        var distance = Vector3.Distance(transform.position, targetPoint);
        if(distance > .5)
            transform.forward = targetPoint - transform.position;

        switch (state)
        {
            case State.Chasing:
                targetPoint = submarine.transform.position;
                distance = Vector3.Distance(transform.position, targetPoint);
                if (distance < damageRange)
                {
                    targetPoint = transform.position;
                    if(Time.timeSinceLevelLoad - timeLastAttacked > damageCooldown)
                    {
                        state = State.Idle;
                        submarine.DealHullDamage(damageOnHit);
                        timeLastAttacked = Time.timeSinceLevelLoad;
                        Debug.Log("Hit");
                    }
                }
                if (distance > dropAgroRange)
                    state = State.Idle;
                break;
            case State.Idle:
                targetPoint = startPos;
                if (Vector3.Distance(transform.position, submarine.transform.position) < agroRange && Time.timeSinceLevelLoad - timeLastAttacked > damageCooldown)
                {
                    state = State.Chasing;
                    soundPlayer.Play();
                }
                break;
        }
    }
    private void FixedUpdate()
    {
        joint.targetVelocity = (targetPoint - transform.position).normalized * moveSpeed;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPoint, 1);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, agroRange);
        Gizmos.DrawWireSphere(transform.position, dropAgroRange);
        Gizmos.DrawWireSphere(transform.position, damageRange);
    }
}
