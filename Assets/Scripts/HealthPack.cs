
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HealthPack : UdonSharpBehaviour
{
    public GameObject playerHealthManager;
    private PlayerHealthManager playerhealth;
    public int healthAmmount;
    private float angle;
    private Vector3 startPosition;

    public GameObject child1;
    public GameObject child2;

    [UdonSynced] bool isRespawning;
    [UdonSynced] public float respawnTimer;

    MeshRenderer meshrendComp;
    MeshRenderer meshrendComp1;
    MeshRenderer meshrendComp2;
    BoxCollider colliderComp;
    void Start()
    {
        playerhealth = playerHealthManager.GetComponent<PlayerHealthManager>();
        meshrendComp = gameObject.GetComponent<MeshRenderer>();
        meshrendComp1 = child1.GetComponent<MeshRenderer>();
        meshrendComp2 = child2.GetComponent<MeshRenderer>();
        colliderComp = gameObject.GetComponent<BoxCollider>();
        startPosition = transform.position;
        isRespawning = false;
        respawnTimer = 10.0f;
        healthAmmount = 50;
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
        if (isRespawning)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0.0f)
            {
                respawnTimer = 10.0f;

                meshrendComp.enabled = true;
                colliderComp.enabled = true;
                meshrendComp1.enabled = true;
                meshrendComp2.enabled = true;
                isRespawning = false;
            }
           
        }
        RequestSerialization();

    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            if (playerhealth.currentHealth >= 100)
            {
                return;
            }
            else
            {
                playerhealth.currentHealth = Mathf.Min(playerhealth.currentHealth + healthAmmount, 100);
                meshrendComp.enabled = false;
                colliderComp.enabled = false;
                meshrendComp1.enabled = false;
                meshrendComp2.enabled = false;
                isRespawning = true;

            }
          
        }
        RequestSerialization();

    }
}
