
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BillboardRespawnerTime : UdonSharpBehaviour
{
    public TextMeshProUGUI RespawnText;
    public HealthPack healthPack;
    private Vector3 playerCamera;
    public float baseScale = 1.0f;
    public float scaleMultiplier = 1.0f;

    void Start()
    {
        if (Networking.LocalPlayer != null)
        {
            playerCamera = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
        }
    }

    void Update()
    {
        playerCamera = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
        if (playerCamera != null)
        {
            transform.LookAt(playerCamera);
            transform.Rotate(0, 180, 0); // Flip the UI to face the player properly

            transform.localScale = Vector3.one * baseScale * scaleMultiplier;
            RespawnText.text = $"Respawning... \n{healthPack.respawnTimer:F0} s";
        }
    }
}
