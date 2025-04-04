using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using TMPro;
using VRC.Udon;
using UnityEngine.UI;

public class UserInterfaceBars : UdonSharpBehaviour
{
    public Canvas canvas;
    public float distance = 0.5f;
    public GameObject playerHealthManager;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI respawnText;
    public Animator animator;

   private PlayerHealthManager playerhealthScript;
   public float respawnTimer = 10.0f; // Only needed locally

    private Vector3 savedDeathLocation;
    public GameObject respawnGameObject;

    void Start()
    {
        AttachToCamera();
        playerhealthScript = playerHealthManager.GetComponent<PlayerHealthManager>();
        
    }

    public void UpdateHealthDisplay()
    {
        healthText.text = "Health: " + playerhealthScript.currentHealth;
    }
    public void UpdateRespawnTimer()
    {
        respawnText.text = "YOU ARE DEAD! \n respawning in: " + respawnTimer.ToString("F0");
        respawnTimer -= Time.deltaTime;

        //freeze player
        Networking.LocalPlayer.TeleportTo(savedDeathLocation, Networking.LocalPlayer.GetRotation());

        if (respawnTimer <= 0.0f)
        {
            animator.Play("FadeOut");

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("FadeOut") && stateInfo.normalizedTime >= 1.0f)
            {
                Networking.LocalPlayer.TeleportTo(respawnGameObject.transform.position, Quaternion.AngleAxis(0.0f, Vector3.forward));

                animator.Play("FadeIn"); // Play FadeIn when FadeOut completes
            }

            if (stateInfo.IsName("FadeIn") && stateInfo.normalizedTime >= 1.0f)
            {
                respawnTimer = 10.0f;
                playerhealthScript.currentHealth = 100; // Reset only locally
                respawnText.text = "";
            }
            
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
            return;
            
        }
        savedDeathLocation = Networking.LocalPlayer.GetPosition();


    }
    public void AttachToCamera()
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
    public override void OnPlayerJoined(VRCPlayerApi player)
    {

    }

}
