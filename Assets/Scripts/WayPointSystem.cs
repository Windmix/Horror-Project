
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class WayPointSystem : UdonSharpBehaviour
{
    private Vector3 localPlayerPos;
    public GameObject[] waypoints;  // Array of waypoints
    private int currentIndex = 0;  // Current waypoint index

    void Start()
    {

    }
    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentIndex].transform;
        waypoints[currentIndex].SetActive(true);
        localPlayerPos = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
        if (Vector3.Distance(localPlayerPos, target.position) < 4.0f)
        {
            waypoints[currentIndex].SetActive(false);
            currentIndex = (currentIndex + 1) % waypoints.Length; // Loop waypoints yes
        }
    }
}