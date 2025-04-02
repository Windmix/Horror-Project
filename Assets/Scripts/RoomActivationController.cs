
using System.Numerics;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RoomActivationController : UdonSharpBehaviour
{
  public GameObject obj1;
  public GameObject obj2;
  public GameObject obj3;
  public GameObject obj4;
  public GameObject obj5;

  public GameObject goal;

    // Check if all booleans are true
    [UdonSynced] public bool allTriggered;

   private GameObject[] objects;
    public void Start()
    {
        objects = new GameObject[] { obj1, obj2, obj3, obj4, obj5 };
    }

    public void checkIfEverythingIsActive()
    {
        foreach (GameObject obj in objects)
        {
            if (obj == null) continue; // Skip null objects

            var scriptComp = obj.GetComponent<OrbTrigger>();
            
            if (!scriptComp.isTriggered)
            {
                allTriggered = false;
                return;
            }
            allTriggered = true;
        }
        
        if (allTriggered)
        {
            goal.SetActive(true);
        }

    }
}
