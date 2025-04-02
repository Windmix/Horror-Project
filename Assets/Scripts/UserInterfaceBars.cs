using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using TMPro;
using VRC.Udon;

public class UserInterfaceBars : UdonSharpBehaviour
{
    public Canvas canvas;
    public float distance = 0.5f;
    public GameObject playerHealthManager;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI respawnText;

    private PlayerHealthManager playerhealthScript;
    private float respawnTimer = 10.0f; // Only needed locally

    void Start()
    {
        AttachToCamera();
        playerhealthScript = playerHealthManager.GetComponent<PlayerHealthManager>();
    }

    private void UpdateHealthDisplay()
    {
        healthText.text = "Health: " + playerhealthScript.currentHealth;
    }

    private void UpdateRespawnTimer()
    {
        respawnText.text = "YOU ARE DEAD! \n respawning in: " + respawnTimer.ToString("F0");
        respawnTimer -= Time.deltaTime;

        if (respawnTimer <= 0.0f)
        {
            respawnTimer = 10.0f;
            playerhealthScript.currentHealth = 100; // Reset only locally
            respawnText.text = "";
        }
    }



    public void LateUpdate()
    {
        // Attach UI to the player's camera
        Vector3 headPos = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
        Quaternion headRot = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
        canvas.transform.position = headPos + headRot * Vector3.forward * distance;
        canvas.transform.rotation = Quaternion.LookRotation(headRot * Vector3.forward);

        // Update UI locally
        UpdateHealthDisplay();

        if (playerhealthScript.currentHealth <= 0)
        {
            UpdateRespawnTimer();
        }
    }

    private void AttachToCamera()
    {
        if (Networking.LocalPlayer != null)
        {
            Vector3 headPos = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
            Quaternion headRot = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
            canvas.transform.position = headPos;
            canvas.transform.rotation = headRot;
            canvas.transform.localPosition = new Vector3(0, 0, 0.5f);
            canvas.transform.localRotation = Quaternion.identity;
        }
    }
}
