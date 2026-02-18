using System.Collections.Generic;
using UnityEngine;

public class LidarScanner : MonoBehaviour
{
    [SerializeField] private float _scanRange = 100;
    [SerializeField] private float _sweepAngle = 30;
    [SerializeField] private float _angleFudging = 1;
    [SerializeField] private LayerMask _scanLayer;
    [SerializeField] private int _horizontalSweeps = 50, _verticalSweeps = 50;
    [SerializeField] private int _maxPoints = 1000000;
    private List<Vector3> _points = new List<Vector3>();
    public List<Vector3> Points => _points;
    [ContextMenu("Scan")]
    public void Scan()
    {
        for(int x = 0; x < _horizontalSweeps; x++)
        {
            var horizontalAngle = Mathf.LerpAngle(-_sweepAngle / 2, _sweepAngle / 2, (float)x / _horizontalSweeps) ;
            for(int y = 0; y < _horizontalSweeps; y++)
            {
                var verticalAngle = Mathf.LerpAngle(-_sweepAngle / 2, _sweepAngle / 2, (float)y / _verticalSweeps);
                var sweepDir = transform.rotation * Quaternion.Euler(verticalAngle + Random.Range(-1f, 1f) * _angleFudging, horizontalAngle + Random.Range(-1f, 1f) * _angleFudging, 0) * Vector3.forward;
                if(Physics.Raycast(transform.position, sweepDir, out var hitInfo, _scanRange, _scanLayer))
                {
                    _points.Add(hitInfo.point);
                }
            }
        }

        if(Points.Count > _maxPoints)
        {
            Points.RemoveRange(0, Points.Count - _maxPoints);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach(var point in _points)
        {
            Gizmos.DrawSphere(point, .02f);
        }
    }
}
