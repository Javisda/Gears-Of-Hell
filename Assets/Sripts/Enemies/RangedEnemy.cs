using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : MonoBehaviour
{
    public float sightRange;
    public float attackRange;
    public float attackSpeed;
    public float bulletSpeed;

    private bool alreadyAttacked = false;

    public GameObject bullet;

    Transform player;
    NavMeshAgent agent;

    private void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        transform.LookAt(player.position);

        if (distance <= attackRange)
        {
            Attack();
        }
        else if (distance <= sightRange)
        {
            Chase();
        }
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            GameObject b = Instantiate(bullet, transform.position,Quaternion.identity);
            b.transform.LookAt(player.transform);
            b.GetComponent<BulletBehaviour>().setForce(bulletSpeed);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), attackSpeed);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}