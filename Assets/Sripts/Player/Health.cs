using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color damageColor = new Color(1.0f, 0.0f, 0.0f, 0.1f);

    Player playerMovement; // Referencia a dicho script para desactivarlo si el jugador muere para que no se pueda mover.
    bool isDead;
    bool damaged;

    private void Awake()
    {
        playerMovement = GetComponent<Player>();
        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        if (damaged)
        {
            damageImage.color = damageColor;
        }
        else {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    public void TakeDamage(int amount) {
        damaged = true;

        currentHealth -= amount;
        Debug.Log("Vida actual: " + currentHealth);

        if (currentHealth <= 0 && !isDead) {
            Death();
        }
    }

    private void Death() { 
        isDead = true;
        playerMovement.enabled = false;
        // Faltar�a poner sistema de animaciones o audios, etc. Por eso est� esto en un m�todo a parte
    }
}