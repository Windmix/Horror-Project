using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Spike : UdonSharpBehaviour
{
    public GameObject healthManager;
    private PlayerHealthManager playerhealthScript;
    private VRCPlayerApi localPlayer;

    [UdonSynced] private Vector3 syncedBounceVector;
    [UdonSynced] private bool justHitSpike;
    private float hitTimer = 0.0f;
    public int spikeDmg = 10;
    public float bounceForce = 5.0f;  // Adjust this for stronger/slower bounce

    void Start()
    {
        playerhealthScript = healthManager.GetComponent<PlayerHealthManager>();
        localPlayer = Networking.LocalPlayer;
        justHitSpike = false;
    }
    void Update()
    {
        if (justHitSpike)
        {
            hitTimer += Time.deltaTime;
            if (hitTimer >= 0.4f)  // Stop force application after 0.4s
            {
                justHitSpike = false;
                localPlayer.SetVelocity(Vector3.zero);  // Stop movement after launch
            }
        }
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (!player.isLocal) return;  // Only process for the local player

        justHitSpike = true;
        hitTimer = 0.0f;

        // Damage the player
        if (!Networking.IsOwner(healthManager))
        {
            Networking.SetOwner(localPlayer, healthManager);
        }
        playerhealthScript.TakeDamage(spikeDmg);

        // Calculate bounce direction (away from the spike + upwards)
        Vector3 bounceDirection = (player.GetPosition() - transform.position).normalized + Vector3.up;

        // Sync only if owner
        if (Networking.IsOwner(gameObject))
        {
            syncedBounceVector = bounceDirection;
            RequestSerialization();
        }

        // Apply smooth physics-based launch
        player.SetVelocity(bounceDirection * bounceForce);
    }

    public override void OnDeserialization()
    {
        if (justHitSpike)
        {
            // Apply synced physics launch for other players
            localPlayer.SetVelocity(syncedBounceVector * bounceForce);
        }
    }
}
