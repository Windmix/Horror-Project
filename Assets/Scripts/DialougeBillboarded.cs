
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DialougeBillboarded : UdonSharpBehaviour
{
    private Vector3 playerCamera;
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
        }
    }
}
