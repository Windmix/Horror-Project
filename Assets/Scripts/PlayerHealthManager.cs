using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using static UnityEditor.FilePathAttribute;

public class PlayerHealthManager : UdonSharpBehaviour
{
    public int currentHealth = 100;
    public int maxHealth = 100;
    public Animator animator;

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
        if (0 < currentHealth)
        {
            animator.Play("hurt", 1);
        }
        
    }
}
