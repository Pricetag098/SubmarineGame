using UnityEngine;

public class Submarine : MonoBehaviour
{
    public LidarScanner scanner;
    public ThermalRenderer thermalCamera;
    public PlayerConsole console;
    public float collisionDamageThreshold;
    public AnimationCurve damageCurve;
    [Range(-1,1)]public float thrustControl, bouyancyControl, turnControl;
    [SerializeField] private float thrustOverheatThreshold;
    [SerializeField] private float heatGainRate;
    [SerializeField] private DamageType m_heatDamageType;
    [SerializeField] private ConfigurableJoint joint;

    [SerializeField] private float moveSpeed, floatSpeed, turnRate;
    [SerializeField] private float driveValue;

    SegmentedHealthbar.Damage heatDamage;

    private void Start()
    {
        heatDamage = new SegmentedHealthbar.Damage();
        heatDamage.Type = m_heatDamageType;
        heatDamage.Amount = 0;
        SegmentedHealthbar.Instance.AddDamage(heatDamage);
    }
    private void OnDestroy()
    {
        SegmentedHealthbar.Instance.ClearDamage(heatDamage);
    }

    private void Update()
    {
        joint.targetVelocity = transform.forward * moveSpeed * thrustControl + transform.up * bouyancyControl * floatSpeed;
        joint.targetAngularVelocity = Vector3.up * turnRate * turnControl;
        var drive = new JointDrive()
        {
            positionDamper = driveValue,
            maximumForce = float.PositiveInfinity,
        };
        joint.xDrive = drive;
        joint.yDrive = drive;
        joint.zDrive = drive;

        joint.slerpDrive = drive;

        if(Mathf.Abs(thrustControl) > thrustOverheatThreshold)
        {
            heatDamage.Amount += heatGainRate * Time.deltaTime;
        }
        else
        {
            heatDamage.Amount -= heatGainRate * Time.deltaTime;
            heatDamage.Amount = Mathf.Max(0, heatDamage.Amount);
        }
    }

    public void DealHullDamage(float amount)
    {
        console.DealHullDamage(amount);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude < collisionDamageThreshold)
            return;

        DealHullDamage(damageCurve.Evaluate(collision.relativeVelocity.magnitude - collisionDamageThreshold));
    }
}