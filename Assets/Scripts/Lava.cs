
using SimpleAI;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Lava : UdonSharpBehaviour
{
    public GameObject location;
    public GameObject Cube;
    public Vector3 startPosition;
    public Animator animator;
    VRCPlayerApi Localplayer;
    void Start()
    {
    }
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("FadeOut") && stateInfo.normalizedTime >= 1.0f)
        {
            Localplayer.TeleportTo(location.transform.position, Quaternion.AngleAxis(0.0f, Vector3.forward));

            animator.Play("FadeIn"); // Play FadeIn when FadeOut completes
        }
    }
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if(player.isLocal)
        {
            animator.Play("FadeOut");
            Localplayer = player;
            
        }

    }
    public void OnCollisionStay(Collision collision)
    {
        Cube.transform.SetPositionAndRotation(startPosition, Quaternion.AngleAxis(90.0f, Vector3.forward));
    }
}
