using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelTile : MonoBehaviour
{
    public float VoxelSize = 0.1f;
    public int TileSideSize = 8;

    [HideInInspector] public byte[] ColorsRight;
    [HideInInspector] public byte[] ColorsForward;
    [HideInInspector] public byte[] ColorsLeft;
    [HideInInspector] public byte[] ColorsBack;

    public void CalculateSidesColors() {
        ColorsRight = new byte[TileSideSize * TileSideSize];
        ColorsForward = new byte[TileSideSize * TileSideSize];
        ColorsLeft = new byte[TileSideSize * TileSideSize];
        ColorsBack = new byte[TileSideSize * TileSideSize];

        for (int y = 0; y < TileSideSize; y++) {
            for (int i = 0; i < TileSideSize; i++) {
                ColorsRight[y * TileSideSize + i] = GetVoxelColor(y, i, Vector3.right);
                ColorsForward[y * TileSideSize + i] = GetVoxelColor(y, i, Vector3.forward);
                ColorsLeft[y * TileSideSize + i] = GetVoxelColor(y, i, Vector3.left);
                ColorsBack[y * TileSideSize + i] = GetVoxelColor(y, i, Vector3.back);
            }
        }
        // Debug.Log(string.Join(", ", ColorsRight));
        // Debug.Log(string.Join(", ", ColorsForward));
        // Debug.Log(string.Join(", ", ColorsLeft));
        // Debug.Log(string.Join(", ", ColorsBack));
    }

    private byte GetVoxelColor(int verticalLayer, int horizontalOffset, Vector3 direction)
    {
        var meshCollider = GetComponentInChildren<MeshCollider>();

        float vox = VoxelSize;
        float half = VoxelSize/2;

        Vector3 rayStart;
        if(direction == Vector3.right) {
            rayStart = meshCollider.bounds.min + new Vector3(-half, 0, half + horizontalOffset * vox);
        }
        else if(direction == Vector3.forward) {
            rayStart = meshCollider.bounds.min + new Vector3(half + horizontalOffset * vox, 0, -half);
        }
        else if(direction == Vector3.left) {
            rayStart = meshCollider.bounds.max + new Vector3(half, 0, -half - (TileSideSize - horizontalOffset - 1) * vox);
        }
        else if(direction == Vector3.back) {
            rayStart = meshCollider.bounds.max + new Vector3(-half - (TileSideSize - horizontalOffset - 1) * vox, 0, half);
        }
        else {
            throw new ArgumentException("Wrong direction value, should be Vector3.left/right/back/forward", nameof(direction));
        }

        rayStart.y = meshCollider.bounds.min.y + half + verticalLayer * vox;

        // Debug.DrawRay(rayStart, direction*.1f, Color.blue, 2);

        if(Physics.Raycast(new Ray(rayStart, Vector3.forward), out RaycastHit hit, vox))
        {
            Mesh mesh = meshCollider.sharedMesh;
            int hitTriangleVertex = mesh.triangles[hit.triangleIndex * 3];
            byte colorIndex = (byte)(mesh.uv[hitTriangleVertex].x * 256);
            return colorIndex;
        }
        return 0;
    }
}
