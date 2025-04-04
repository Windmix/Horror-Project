
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Padistal : UdonSharpBehaviour
{
    public GameObject Cube;
    public GameObject wayPointSys;
    public GameObject CubeWayPoint;
    public GameObject MorePlatforms;
    void Start()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        wayPointSys.SetActive(false);
        CubeWayPoint.SetActive(false);
        MorePlatforms.SetActive(true);
    }
}
