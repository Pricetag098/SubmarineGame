using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Vector3 _moveSpeed;
    [SerializeField] private float _height;
    [SerializeField] private ConfigurableJoint _joint;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private LayerMask _groundLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, out var hit, _height, _groundLayer))
        {
            _joint.yMotion = ConfigurableJointMotion.Free;
            return;
        }

        _joint.targetPosition = hit.point + Vector3.up * _height;
    }
}
