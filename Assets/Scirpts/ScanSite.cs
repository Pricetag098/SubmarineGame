using UnityEngine;

public class ScanSite : MonoBehaviour
{
    [SerializeField] float range;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
