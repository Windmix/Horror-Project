
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Lava : UdonSharpBehaviour
{
    public GameObject location;
    public GameObject Cube;
    public Vector3 startPosition;
    void Start()
    {
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if(player.isLocal)
        {
            player.TeleportTo(location.transform.position, Quaternion.AngleAxis(0.0f, Vector3.forward));
        }

    }
    public void OnCollisionStay(Collision collision)
    {
        Cube.transform.SetPositionAndRotation(startPosition, Quaternion.AngleAxis(90.0f, Vector3.forward));
    }
}
