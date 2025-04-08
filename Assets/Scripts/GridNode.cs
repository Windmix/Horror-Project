
using UdonSharp;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using VRC.SDKBase;
using VRC.Udon;

public class GridNode : UdonSharpBehaviour
{
     [Header("Grid Settings")]
    public Vector3Int gridSize = new Vector3Int(5, 5, 5);  // X, Y, Z dimensions
    public Vector3 spacing = new Vector3(2f, 2f, 2f);     // Distance between spheres
    public Vector3 startOffset = Vector3.zero;            // Starting offset from the object's position

    [Header("Sphere Settings")]
    public GameObject spherePrefab;                       // Assign your sphere prefab here

    void Start()
    {
        if (spherePrefab == null)
        {
            Debug.LogError("Sphere prefab is not assigned.");
            return;
        }

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int z = 0; z < gridSize.z; z++)
                {
                    // Calculate position for each sphere
                    Vector3 position = new Vector3(
                        x * spacing.x,
                        y * spacing.y,
                        z * spacing.z
                    ) + startOffset;

                    // Instantiate the sphere prefab
                    GameObject sphere = VRCInstantiate(spherePrefab);
                    sphere.transform.position = transform.position + position;

                    // Optional: Parent the sphere under this object for better hierarchy organization
                    sphere.transform.SetParent(transform);
                }
            }
        }
    }

}
