using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour 
{
    public GameObject[] prisoners;
    public GameObject targetPrisoner;
    public float speed = 5.0f;

    public float zombieHealth;

    public float attackDistance = 1.5f;
    public float attackDelay = 5f; 
    public int attackDamage = 10;
    private bool isAttacking = false;


    public NavMeshAgent agent;

    private void Awake()
    {
        
    }

    private void Start()
    {
        prisoners = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
	{
        FindPlayerTarget();

        //Check if player is in range.
        if (Vector3.Distance(transform.position, targetPrisoner.transform.position) < attackDistance)
        {
            StartCoroutine(Attack());
        } 
    }

    IEnumerator Attack()
    {
        isAttacking = true; // set attacking to true

        //add animation for attack here....

        yield return new WaitForSeconds(attackDelay);

        if (Vector3.Distance(transform.position, targetPrisoner.transform.position) < attackDistance)
        {
            // Check if the player is still within attack range
            targetPrisoner.GetComponent<PlayerHealth>().TakeDamage();
        }

        isAttacking = false;
    }

    public void TakeDamange()
    {
        zombieHealth = zombieHealth - 12;
        //increase team score here (team points)
    }

    public void FindPlayerTarget() {
        if (targetPrisoner != null)
        {
            agent.SetDestination(targetPrisoner.transform.position);
        }

        if (prisoners.Length > 0)
        {
            float minDistance = Mathf.Infinity; //start with a high distance

            GameObject closestPrisoner = null;

            foreach (GameObject prisoner in prisoners)
            {
                float distance = Vector3.Distance(transform.position, prisoner.transform.position);

                //If target's distance is shorter than current minimum distance, update it.
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPrisoner = prisoner;
                }
            }

            if (closestPrisoner != null && !closestPrisoner.Equals(targetPrisoner))
            {
                targetPrisoner = closestPrisoner;
            }

        }
        else
        {
            prisoners = GameObject.FindGameObjectsWithTag("Player");
        }
    }
}