using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerHealthManager : UdonSharpBehaviour
{
    public int currentHealth = 100; 
    public int maxHealth = 100;

    void Start()
    {
        if (Networking.IsOwner(gameObject))
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!Networking.IsOwner(gameObject)) return;

        currentHealth = Mathf.Max(currentHealth - damage, 0);
    }
}
