using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public float flashSpeed = 5f;
    public Color damageColor = new Color(1.0f, 0.0f, 0.0f, 0.1f);

    Player playerMovement; // Referencia a dicho script para desactivarlo si el jugador muere para que no se pueda mover.
    bool isDead;

    //Perks barriers
    public bool electricBarrier;

    // Display health
    private RectTransform lifeScaler;
    private TextMeshProUGUI lifeText;

    // PopUp
    private PopUp popup;
    [SerializeField] private Transform popupPosition;

    public delegate void TakeDamageDel();

    public TakeDamageDel takeDamage;

    [SerializeField] private AudioClip healClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip[] hurtClips;

    [HideInInspector]
    private AudioSource source;

    private void Awake()
    {
        playerMovement = GetComponent<Player>();
        popup = GetComponent<PopUp>();
        currentHealth = maxHealth;
        source = GetComponents<AudioSource>()[2];
        electricBarrier = false;
    }

    private void Start()
    {
        lifeScaler = GameObject.Find("LifeScaler").GetComponent<RectTransform>();
        lifeText = GameObject.Find("LifeCounter").GetComponent<TextMeshProUGUI>();
        UpdateLifeUI();
    }


    private void Update()
    {
        //Debug.Log("Vida actual: " + currentHealth);
    }

    public void TakeDamage(float amount)
    {
        popup.Create(popupPosition.position, (int)amount, PopUp.TypePopUp.DAMAGE, false, 0.5f);
        if (takeDamage != null)
            takeDamage();

        if (currentHealth > amount)
            currentHealth -= amount;
        else
            currentHealth = 0;


        UpdateLifeUI();

        if (currentHealth <= 0 && !isDead)
        {
            PlaySound(deathClip);
            Death();
        } else if (!isDead)
        {
            PlaySound(hurtClips[Random.Range(0, hurtClips.Length)]);
        }
    }

    public void Heal(float amount) {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            popup.Create(popupPosition.position, (int)amount, PopUp.TypePopUp.LIFE, true, 0.5f);
        }
        PlaySound(healClip);
        UpdateLifeUI();
    }

    private void PlaySound(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    private void Death()
    {
        Debug.Log("LLEGA 1 ");
        isDead = true;
        int r = Random.Range(0, 2);
        //Randomly choose death animation type
        switch (r)
        {
            case 0:
                playerMovement.playerAnimator.SetFloat("death_type",0);
                break;
            case 1:
                playerMovement.playerAnimator.SetFloat("death_type", .5f);
                break;
            case 2:
                playerMovement.playerAnimator.SetFloat("death_type", 1);
                break;

        }
        playerMovement.playerAnimator.SetTrigger("death");
        playerMovement.enabled = false;



        // Faltar�a poner sistema de animaciones o audios, etc. Por eso est� esto en un m�todo a parte

        // Finish walking sound
        playerMovement.footSteps.Stop();

        // Tiempo de espera para el men� de resumen
        Invoke("LoadResume", 3);
    }

    public void LoadResume()
    {
        Debug.Log("LLEGA 2 ");
        int minutes = GameObject.Find("GameRegistry").GetComponent<GameRegistry>().minutes;
        int seconds = GameObject.Find("GameRegistry").GetComponent<GameRegistry>().seconds;
        int bulletsHit = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ShootSystem>().numBulletsHit;
        int bulletsMissed = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ShootSystem>().numBulletsMissed;
        GameObject.Find("InGameUI").GetComponent<GestorUIinGame>().FinishGame(minutes, seconds, bulletsHit, bulletsMissed);
        GameObject.Find("InGameMusic").GetComponent<InGameMusicManager>().SetGameOverMusic();
        //Destroy(gameObject);
    }
    

    public void UpdateLifeUI() {
        lifeScaler.localScale = new Vector3(currentHealth / maxHealth, 1, 1);
        lifeText.text = (int)currentHealth + " / " + maxHealth;
    }
}
