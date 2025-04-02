using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class VerticalPlatform : UdonSharpBehaviour
{
    private bool movingUp;

    // Networking
    [UdonSynced] private Vector3 syncedPosition;
    [UdonSynced] private bool syncedMovingUp;
    private bool isOwner => Networking.IsOwner(gameObject);

    // Player & Objects
    private VRCPlayerApi localPlayer;
    private bool localPlayerOnPlatform;
    private bool localObject;
    private Collider specificCollider;

    [UdonSynced] public float Speed = 2.0f;
    [UdonSynced] public float offset = 5.0f;
    private Vector3 startPosition;
    private Vector3 lastPosition;
    private float lerpSpeed = 10f; // Interpolation speed for non-owners

    void Start()
    {
        localPlayer = Networking.LocalPlayer;
        startPosition = transform.localPosition;
        lastPosition = startPosition;

        if (isOwner)
        {
            movingUp = Random.Range(0, 2) == 1;
            syncedMovingUp = movingUp;
            RequestSerialization();
        }
    }

    void Update()
    {
        if (isOwner)
        {
            MovePlatform();
            syncedPosition = transform.localPosition;
            syncedMovingUp = movingUp;
            RequestSerialization();
        }
        else
        {
            // Smooth interpolation for non-owners instead of instant jumps
            transform.localPosition = Vector3.Lerp(transform.localPosition, syncedPosition, Time.deltaTime * lerpSpeed);
            movingUp = syncedMovingUp;
        }

        MoveAttachedObjects();
        lastPosition = transform.localPosition;
    }

    private void MovePlatform()
    {
        float moveStep = Speed * Time.deltaTime;
        Vector3 moveDir = movingUp ? Vector3.up : Vector3.down;

        transform.localPosition += moveDir * moveStep;

        if (movingUp && transform.localPosition.y >= startPosition.y + offset)
        {
            movingUp = false;
        }
        else if (!movingUp && transform.localPosition.y <= startPosition.y - offset)
        {
            movingUp = true;
        }
    }

    private void MoveAttachedObjects()
    {
        Vector3 delta = transform.localPosition - lastPosition;

        if (localPlayerOnPlatform)
        {
            localPlayer.TeleportTo(localPlayer.GetPosition() + delta, localPlayer.GetRotation());
        }

        if (localObject && specificCollider != null)
        {
            specificCollider.transform.position += delta;
        }
    }

    public override void OnDeserialization()
    {
        syncedPosition = transform.localPosition;
        movingUp = syncedMovingUp;
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            localPlayerOnPlatform = true;
        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            localPlayerOnPlatform = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Cube" && specificCollider == null)
        {
            specificCollider = other;
            localObject = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.name == "Cube" && specificCollider == other)
        {
            specificCollider = null;
            localObject = false;
        }
    }
}
