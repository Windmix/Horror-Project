
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class character_ghost : UdonSharpBehaviour
{
    public GridNode gridnode; // Reference to the GridNode containing all nodes
    public float moveSpeed = 2.0f; // Movement speed of the ghost
    private Transform randomNode;
    [UdonSynced] private Vector3 syncedPosition;
    private bool isOwner => Networking.IsOwner(gameObject);

    public float radius = 0.5f;
    private bool randomNodeReached = false;
    private bool  closestNodeCalled  = false;

    void Start()
    {
        randomNode = GetRandomNode();
    }
    void Update()
    {
        if (randomNodeReached)
        {
            randomNode = GetRandomNode();
        } 
        if (randomNode != null)
        {
            MoveTowardsNode();
            RequestSerialization();
        }
    }
    Transform GetRandomNode()
    { 
        int childCount = gridnode.transform.childCount;
        if (childCount == 0)
        {
            Debug.LogWarning("No children found under gridnode.");
            return null;
        }

        int randomIndex = Random.Range(0, childCount);
        Transform randomChild = gridnode.transform.GetChild(randomIndex);
        return randomChild;
    }

    void MoveTowardsNode()
    {
        randomNodeReached = false;
        Vector3 direction = (randomNode.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, randomNode.position, moveSpeed * Time.deltaTime);

        // Optional: Rotate the ghost to face the direction of movement
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.deltaTime);
        }

        // Check if the ghost has reached the closest node
        if (Vector3.Distance(transform.position, randomNode.position) < radius)
        {
            // Reset and find a new closest node
            randomNodeReached = true;

        }
    }
}
