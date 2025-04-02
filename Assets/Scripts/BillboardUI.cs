
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

public class BillboardUI : UdonSharpBehaviour
{
    public TextMeshProUGUI distanceText;
    private Vector3 playerCamera;
    public float baseScale = 1.0f;
    public float scaleMultiplier = 1.0f;
    public float distance;

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

            distance = Vector3.Distance(transform.position, playerCamera);
            transform.localScale = Vector3.one * baseScale * distance * scaleMultiplier;
            distanceText.text = $"Hello world\n{distance:F0}m";
        }
    }
}