using SimpleAI;
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerPool : UdonSharpBehaviour
{
    public GameObject healthManagerPrefab;// Prefab to be assigned

    public int maxPlayers = 16; // VRChat's max players


    public GameObject[] healthManagers;
  

    public VRCPlayerApi[] assignedPlayers;


    void Start()
    {
        healthManagers = new GameObject[maxPlayers];
        assignedPlayers = new VRCPlayerApi[maxPlayers];

        // Pre-instantiate health managers
        for (int i = 0; i < maxPlayers; i++)
        {
           
            healthManagers[i] = Object.Instantiate(healthManagerPrefab);
            healthManagers[i].SetActive(false); // Hide until assigned

  
        }
    }

    public void AssignManagers(VRCPlayerApi player)
    {
        for (int i = 0; i < maxPlayers; i++)
        {
            if (assignedPlayers[i] == null) // Find an available slot
            {
                assignedPlayers[i] = player;
                healthManagers[i].SetActive(true); // Activate the health manager
                Networking.SetOwner(player, healthManagers[i]); // Give ownership

                break;
            }
        }
    }
  
    public void RemoveManagers(VRCPlayerApi player)
    {
        for (int i = 0; i < maxPlayers; i++)
        {
            if (assignedPlayers[i] != null && assignedPlayers[i].playerId == player.playerId)
            {
                healthManagers[i].SetActive(false); // Disable the health manager
                assignedPlayers[i] = null; // Free up the slot

                break;
            }
        }
    }
   

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            AssignManagers(player);
           
        }

      
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        RemoveManagers(player);

    }
}
