using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outerRing : MonoBehaviour
{

    [SerializeField] private ProspectorBehaviour m_Prospector;
    [SerializeField] private LayerMask m_LayerMask;

    private SphereCollider sC;

    private void Start()
    {
        sC = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") { 
            Debug.Log("Anillo grande entra.");
            m_Prospector.insideOuterRing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") { 
            Debug.Log("Anillo grande sale.");
            m_Prospector.insideOuterRing = true;
            m_Prospector.fromOutside = true;
            m_Prospector.fromInside = false;
        }
    }

    public int checkNumEnemiesInRange()
    {
        // Checks for number of enemies
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, sC.radius, m_LayerMask);
        return hitColliders.Length;
    }

    public float checkEnemiesHealthStatus() {
        // TODO: Arreglar el n�mero de colisionadores

        float healthRatio = 0.0f;
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, sC.radius, m_LayerMask);

        // Checks for number of enemies
        foreach (Collider eC in hitColliders) {
            EnemyHealth eH = eC.gameObject.GetComponent<EnemyHealth>();
            if (eH.enemyType != EnemyHealth.EnemyType.FOREMAN)
            {
                // Health ratio only based on surrounding enemies
                healthRatio += ((float)eH.currentHealth / (float)eH.startingHealth);
                //Debug.Log("Current Health: " + eH.currentHealth + ". Max Health: " + eH.startingHealth+ ". HealthRatio: " + healthRatio);
            }
        }


        if(hitColliders.Length > 1)
            healthRatio /= (float)(hitColliders.Length - 1);
        else
            healthRatio = 1.0f; // 1 is returned in case no enemies in Foremans range for the mathematical function in ProspectorBehavour when calculating VU.
        Debug.Log("HealthRatio: " + healthRatio);

        return healthRatio;
    }

    public Collider[] GetEnemiesInRange() { 
        return Physics.OverlapSphere(gameObject.transform.position, sC.radius, m_LayerMask);
    }
}
