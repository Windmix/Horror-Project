
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class OrbTrigger : UdonSharpBehaviour
{
    [UdonSynced] public bool isTriggered;
    public GameObject parent;
    public Renderer objectRenderer;
    [UdonSynced] public Color triggeredColor;

    public MaterialPropertyBlock propBlock;
    void Start()
    {
        isTriggered = false;
        propBlock = new MaterialPropertyBlock();
    }
    public override void Interact()
    {
        Debug.Log("Player interacted with this object!");
        triggeredColor = Color.blue * 2;
        propBlock.SetColor("_Color", triggeredColor);
        objectRenderer.SetPropertyBlock(propBlock);
        
       
        isTriggered = true;
        var parentScript = parent.GetComponent<RoomActivationController>();
        parentScript.checkIfEverythingIsActive();
        RequestSerialization();

    }
}
