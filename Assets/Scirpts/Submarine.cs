using UnityEngine;

public class Submarine : MonoBehaviour
{
    public LidarScanner scanner;
    public ThermalRenderer thermalCamera;
    [Range(-1,1)]public float thrustControl, bouyancyControl, turnControl;
    [SerializeField] private ConfigurableJoint joint;

    [SerializeField] private float moveSpeed, floatSpeed, turnRate;
    [SerializeField] private float driveValue;

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
    }
}