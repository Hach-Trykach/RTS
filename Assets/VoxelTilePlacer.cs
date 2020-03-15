using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class VoxelTilePlacer : MonoBehaviour
{
    public VoxelTile[] TilePrefabs;
    public Vector2Int MapSize = new Vector2Int(10, 10);

    private VoxelTile[,] spawnedTiles;
    void Start()
    {
        spawnedTiles = new VoxelTile[MapSize.x, MapSize.y];

        foreach(VoxelTile TilePrefab in TilePrefabs) {
            TilePrefab.CalculateSidesColors();
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.D)) {
            StopAllCoroutines();

            foreach (VoxelTile spawnedTile in spawnedTiles) {
                if(spawnedTile != null) Destroy(spawnedTile.gameObject);
            }
            StartCoroutine(Generate());
        }
    }

    public IEnumerator Generate() {
        // PlaceTile(MapSize.x /2, MapSize.y / 2);

        for(int x = 1; x < MapSize.x-1; x++) {
            for(int y = 1; y < MapSize.y-1; y++) {

        // while(true) {
        //     Vector2Int pos;
        //     while(true) {
        //         pos = new Vector2Int(Random.Range(1, MapSize.x - 1), Random.Range(1, MapSize.y - 1));

        //         if(spawnedTiles[pos.x, pos.y] == null && 
        //             (spawnedTiles[pos.x+1, pos.y] != null || 
        //             spawnedTiles[pos.x-1, pos.y] != null || 
        //             spawnedTiles[pos.x, pos.y+1] != null || 
        //             spawnedTiles[pos.x, pos.y-1] != null))
        //         {
        //             break;
        //         }
        //     }
            yield return new WaitForSeconds(0.1f);

            PlaceTile(x, y);
            }
        }
    }

    public void PlaceTile(int x, int y) {
        List<VoxelTile> availableTiles = new List<VoxelTile>();

        foreach(VoxelTile tilePrefab in TilePrefabs) {
            if(CanAppendTile(spawnedTiles[x-1,y], tilePrefab, Vector3.left) && 
            CanAppendTile(spawnedTiles[x+1,y], tilePrefab, Vector3.right) && 
            CanAppendTile(spawnedTiles[x,y-1], tilePrefab, Vector3.back) && 
            CanAppendTile(spawnedTiles[x,y+1], tilePrefab, Vector3.forward))
            {
                availableTiles.Add(tilePrefab);
            }
        }

        if(availableTiles.Count == 0) return;

        VoxelTile selectedTile = availableTiles[Random.Range(0, availableTiles.Count)];
        Vector3 position = new Vector3(x, 0, y) * selectedTile.VoxelSize * selectedTile.TileSideSize;
        spawnedTiles[x,y] = Instantiate(selectedTile, position, Quaternion.identity);
    }

    private bool CanAppendTile(VoxelTile existingTile, VoxelTile tileToAppend, Vector3 direction) {
        if(existingTile == null) return true;

        if(direction == Vector3.right) {
            return Enumerable.SequenceEqual(existingTile.ColorsRight, tileToAppend.ColorsLeft);
        }
        else if(direction == Vector3.left) {
            return Enumerable.SequenceEqual(existingTile.ColorsLeft, tileToAppend.ColorsRight);
        }
        else if(direction == Vector3.forward) {
            return Enumerable.SequenceEqual(existingTile.ColorsForward, tileToAppend.ColorsBack);
        }
        else if(direction == Vector3.back) {
            return Enumerable.SequenceEqual(existingTile.ColorsBack, tileToAppend.ColorsForward);
        }
        else {
            throw new ArgumentException("Wrong direction value, should be Vector3.left/right/back/forward", nameof(direction));
        }
    }
}
