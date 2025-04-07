
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class fireRing : UdonSharpBehaviour
{
    [UdonSynced] private Vector3 syncedPosition;
    private bool isOwner => Networking.IsOwner(gameObject);

    public float speed = 1.0f;
    
    void Start()
    {
        speed = speed * Random.Range(1, 2);
    }
    void Update()
    {

        if (isOwner)
        {
            transform.Rotate(Vector3.back, speed * Time.deltaTime);
            RequestSerialization();
        }       
    }
}
