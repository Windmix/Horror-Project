
using SimpleAI;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using static UnityEditor.FilePathAttribute;

public class grave : UdonSharpBehaviour
{
    public GameObject teleportObject;
    public GameObject previous;
    public Animator animator;

    public override void Interact()
    {
        if(Networking.LocalPlayer.isLocal)
        {
            Networking.LocalPlayer.TeleportTo(teleportObject.transform.position, Networking.LocalPlayer.GetRotation());
            //animator.Play("FadeOut");
           
        }
    }
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("FadeOut") && stateInfo.normalizedTime >= 1.0f)
        {
          

         
        }
    }
}
