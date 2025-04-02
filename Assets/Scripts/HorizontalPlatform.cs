using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HorizontalPlatform : UdonSharpBehaviour
{
    private bool movingRight;

    // Networking
    [UdonSynced] private Vector3 syncedPosition;
    [UdonSynced] private bool syncedMovingRight;
    private bool isOwner => Networking.IsOwner(gameObject);

    private VRCPlayerApi localPlayer;
    private Vector3 lastPlatformPosition;
    private bool localPlayerOnPlatform;

    private bool localObject;
    private Collider specificCollider;

    [UdonSynced] public float Speed;
    [UdonSynced] public float offset;
    [UdonSynced] private Vector3 startPosition;

    private float syncTimer = 0f;
    private float syncInterval = 0.1f; // Sync every 0.1s (10 times per second)

    void Start()
    {
        localPlayer = Networking.LocalPlayer;
        Speed = 2.0f;
        offset = 2.0f;

        startPosition = transform.localPosition;
        lastPlatformPosition = startPosition;

        if (isOwner)
        {
            movingRight = Random.Range(0, 2) == 1;
            syncedMovingRight = movingRight;
            syncedPosition = transform.localPosition;
            RequestSerialization();
        }
    }

    void Update()
    {
        if (isOwner)
        {
            MovePlatform();

            // Only sync position at intervals
            syncTimer += Time.deltaTime;
            if (syncTimer >= syncInterval)
            {
                syncedPosition = transform.localPosition;
                syncedMovingRight = movingRight;
                RequestSerialization();
                syncTimer = 0f; // Reset timer
            }
        }
        else
        {
            // Smoothly interpolate to the synced position
            transform.localPosition = Vector3.Lerp(transform.localPosition, syncedPosition, Time.deltaTime * 10f);
            movingRight = syncedMovingRight;
        }

        MoveLocalPlayer();
        MoveAttachedObjects();
        lastPlatformPosition = transform.localPosition;
    }

    private void MovePlatform()
    {
        if (movingRight)
        {
            transform.localPosition += Vector3.right * Speed * Time.deltaTime;
            if (transform.localPosition.x >= startPosition.x + offset)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.localPosition += Vector3.left * Speed * Time.deltaTime;
            if (transform.localPosition.x <= startPosition.x - offset)
            {
                movingRight = true;
            }
        }
    }

    private void MoveLocalPlayer()
    {
        if (localPlayerOnPlatform)
        {
            Vector3 delta = transform.localPosition - lastPlatformPosition;
            localPlayer.TeleportTo(localPlayer.GetPosition() + delta, localPlayer.GetRotation());
        }
    }

    private void MoveAttachedObjects()
    {
        if (localObject && specificCollider != null)
        {
            Vector3 deltaCol = transform.localPosition - lastPlatformPosition;
            specificCollider.transform.position += deltaCol;
        }
    }

    public override void OnDeserialization()
    {
        // Smooth interpolation instead of snapping
        transform.localPosition = Vector3.Lerp(transform.localPosition, syncedPosition, Time.deltaTime * 10f);
        movingRight = syncedMovingRight;
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
