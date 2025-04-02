using SimpleAI;
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerPool : UdonSharpBehaviour
{
    public GameObject healthManagerPrefab;// Prefab to be assigned
    public GameObject UserInterfaceBars;
    public int maxPlayers = 80; // VRChat's max players


    public GameObject[] healthManagers;
    public GameObject[] userInterfaceBars;

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

            userInterfaceBars[i] = Object.Instantiate(UserInterfaceBars);
            userInterfaceBars[i].SetActive(false); // Hide until assigned
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

                userInterfaceBars[i].SetActive(true);// Activate the UI manager
                Networking.SetOwner(player, userInterfaceBars[i]); // Give ownership
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
                userInterfaceBars[i].SetActive(false);
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
