using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelTile : MonoBehaviour {

    public float VoxelSize = 0.1f;
    void Start() {
        GetVoxelColor(0, 0);
    }
    
    void Update() {
        
    }

    private void GetVoxelColor(int verticalLayer, int horizontalLayer)
    {
        var meshCollider = GetComponentInChildren<MeshCollider>();

        Vector3 rayStart = meshCollider.bounds.min;
        rayStart.x += 0.5f * VoxelSize;
        rayStart.y += 0.5f * VoxelSize;
        rayStart.z -= 0.5f * VoxelSize;

        Debug.DrawRay(rayStart, Vector3.forward, Color.blue, 2);

        if(Physics.Raycast(new Ray(rayStart, Vector3.forward), out RaycastHit hit))
        {
            Mesh mesh = meshCollider.sharedMesh;
            int hitTriangleVertex = mesh.triangles[hit.triangleIndex * 3];
            Debug.Log(mesh.uv[hitTriangleVertex]);
        }
    }
}
