
using SimpleAI;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Lava : UdonSharpBehaviour
{
    public GameObject location;
    public GameObject Cube;
    public GameObject startPosition;
    public Animator animator;
    VRCPlayerApi Localplayer;
    bool enteredCollider = false;
    void Start()
    {
    }
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("FadeOut") && stateInfo.normalizedTime >= 1.0f && enteredCollider) 
        {
            Localplayer.TeleportTo(location.transform.position, Quaternion.AngleAxis(0.0f, Vector3.forward));

            animator.Play("FadeIn"); // Play FadeIn when FadeOut completes
            enteredCollider = false;
        }
    }
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if(player.isLocal)
        {
            animator.Play("FadeOut");
            Localplayer = player;
            enteredCollider = true;
            
        }

    }
    public void OnCollisionStay(Collision collision)
    {
        Cube.transform.SetPositionAndRotation(startPosition.transform.position, Quaternion.AngleAxis(90.0f, Vector3.forward));
    }
}
