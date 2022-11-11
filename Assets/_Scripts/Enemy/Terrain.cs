using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    [SerializeField] GameObject terrainObject;
    MeshCollider col;

    float minX, maxX, minZ, maxZ;

    // Start is called before the first frame update
    void Start()
    {
        col = terrainObject.GetComponent<MeshCollider>();

        // Note: cannot call col.bounds in update()
        minX = col.bounds.min.x;
        maxX = col.bounds.max.x;
        minZ = col.bounds.min.z;
        maxZ = col.bounds.max.z;
    }

    // return random position within the terrain
    public Vector3 ClampPosition(Vector3 pos)
    {
        return new Vector3(Mathf.Clamp(pos.x, minX, maxX), pos.y, Mathf.Clamp(pos.z, minZ, maxZ));
    }
}
