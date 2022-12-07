using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public int scoreValue = 10;

    public int minScoreValue;
    public int maxScoreValue;

    [SerializeField]
    private GameObject coin;

    CapsuleCollider collider;
    bool isDead;
    private Animator animator;
    private float timeUntilDestroy = 2f;

    private PopUp popup;
    [SerializeField] private Transform popupPosition;

    public delegate void TakeDamageDel();

    public TakeDamageDel takeDamage;

    //Items Consumibles
    [SerializeField] private float itemsRatio;
    [SerializeField] private GameObject[] items;

    // Enemy Type
    public enum EnemyType { WORKER, GUNSLINGER, GUNNER, EXPLOSIVE_SPIDERBOT, WORKER_SPIDERBOT, ATTACK_SPIDERBOT, FOREMAN }
    public EnemyType enemyType;

    // Player reference
    private GameObject playerRef;

    //private int timeAnimationDead = 1;
    private void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        currentHealth = startingHealth;
        //Debug.Log("Health: " + currentHealth);

        popup = GetComponent<PopUp>();
    }
    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log("Vida enemigo: " + currentHealth);

        playerRef.GetComponent<Player>().PlayHitMarker();

        if (takeDamage != null)
            takeDamage();

        popup.Create(popupPosition.position, amount, PopUp.TypePopUp.DAMAGE, false, 0.5f);

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        isDead = true;
        collider.enabled = false;

        //Look for a box collider in case its a melee enemy and deactivate
        BoxCollider coll = GetComponent<BoxCollider>();
        if (coll)
            coll.enabled = false;


        NavMeshAgent navMov = GetComponent<NavMeshAgent>();
        EnemyMovement eM = GetComponent<EnemyMovement>();
        WorkerBehavior mE = GetComponent<WorkerBehavior>();
        GunnerBehaviour gb = GetComponent<GunnerBehaviour>();
        RangedEnemy rE = GetComponent<RangedEnemy>();
        if (eM != null)
            eM.enabled = false;
        if (rE != null)
            rE.enabled = false;
        if (mE != null)
            mE.enabled = false;
        if (navMov != null)
            navMov.enabled = false;
        if (gb)
        {
            
            gb.enabled = false;

        }

        // Faltan sonidos y animaciones de muerte etc etc

        int r = Random.Range(0, 2);
        //Randomly choose death animation type
        switch (r)
        {
            case 0:
                animator.SetFloat("death_type", 0);
                break;
            case 1:
                animator.SetFloat("death_type", .5f);
                break;
            case 2:
                animator.SetFloat("death_type", 1);
                break;

        }
        animator.SetTrigger("death");


        // Suelta moneda
        GameObject moneda = Instantiate(coin, transform.position, Quaternion.identity);
        //moneda.gameObject.GetComponent<Moneda>().value = scoreValue;
        moneda.gameObject.GetComponent<Moneda>().value = Random.Range(minScoreValue, maxScoreValue);

        //Soltar items consumibles

        float aux = Random.Range(0f, 1f);
        if (aux <= itemsRatio)
        {
            int index = Random.Range(0, items.Length);
            GameObject item = Instantiate(items[index], transform.position + new Vector3(0.8f, 0f, -0.8f), Quaternion.identity);
        }

        // Add stats to player
        playerRef.GetComponent<PlayerStats>().numDefeatedEnemies++;
    }
    //Autamitacally call when death animation ended
    public void DestroyCallback()
    {
        Destroy(gameObject, timeUntilDestroy);
    }

    public void Heal(int hp) { 
        int hpHealed = (hp > startingHealth - currentHealth) ? startingHealth - currentHealth : hp;
        popup.Create(popupPosition.position, hpHealed, PopUp.TypePopUp.LIFE, true, 0.5f);
        currentHealth += hpHealed;
    }
}
