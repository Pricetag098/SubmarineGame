using System.Collections.Generic;
using UnityEngine;

public class LidarDisplay : MonoBehaviour
{
    [SerializeField] private Mesh dotMesh;
    [SerializeField] private Material material;
    [SerializeField] private Transform sourcePosition;
    [SerializeField] private LidarScanner lidarScanner;
    [SerializeField] private MaterialPropertyBlock block;
    [SerializeField] private float drawBounds=1;
    [SerializeField] private float dotScale = 1;
    const int MAX_INSTANCES = 1023;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Matrix4x4[] matrices = new Matrix4x4[lidarScanner.Points.Count];
        material.SetVector("_WorldPoint", transform.position);
        material.SetFloat("_MaxDistance", drawBounds);
        var sourceWTL = sourcePosition.worldToLocalMatrix;
        var localLTW = transform.localToWorldMatrix;
        for(int i = 0; i < lidarScanner.Points.Count; i++)
        {
            var point = lidarScanner.Points[i];
            point = localLTW.MultiplyPoint(sourceWTL.MultiplyPoint(point));
            matrices[i] = Matrix4x4.TRS(point, Quaternion.identity, Vector3.one * dotScale);
        }

        int arrayIndex = 0;
        while(arrayIndex < matrices.Length)
        {
            if(arrayIndex + MAX_INSTANCES <= matrices.Length)
            {
                Matrix4x4[] drawCall = new Matrix4x4[MAX_INSTANCES];
                System.Array.Copy(matrices, arrayIndex, drawCall, 0, MAX_INSTANCES);
                Graphics.DrawMeshInstanced(dotMesh, 0, material, drawCall,MAX_INSTANCES, block ,UnityEngine.Rendering.ShadowCastingMode.Off, false);
                arrayIndex += MAX_INSTANCES;
            }
            else
            {
                Matrix4x4[] drawCall = new Matrix4x4[matrices.Length - arrayIndex];
                System.Array.Copy(matrices, arrayIndex, drawCall, 0, matrices.Length - arrayIndex);
                Graphics.DrawMeshInstanced(dotMesh, 0, material, drawCall, matrices.Length - arrayIndex, block, UnityEngine.Rendering.ShadowCastingMode.Off, false);
                arrayIndex += matrices.Length - arrayIndex;
            }
        }
       

        
                
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, drawBounds);
    }
}
