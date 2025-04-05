
using UdonSharp;
using UnityEngine;
using UnityEngine.UIElements;
using VRC.SDKBase;
using VRC.Udon;

public class Coin : UdonSharpBehaviour
{
    private float angle;
    private Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
        angle = 0.0f;
    }
    public void Update()
    {
        angle += Time.deltaTime * 100f;
        // Clockwise rotation
        Quaternion rotation = Quaternion.Euler(0, angle, 0);

        //Hovering effect(smooth up and down motion)
        Vector3 upAndDown = startPosition + Vector3.up * Mathf.Sin(Time.time * 2f) * 0.3f;
        transform.SetPositionAndRotation(upAndDown, rotation);


    }
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player.isLocal)
        {

        }
    }
}
