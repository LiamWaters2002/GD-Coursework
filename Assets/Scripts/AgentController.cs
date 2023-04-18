using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour 
{
    public GameObject[] prisoners;
    public GameObject targetPrisoner;
    public float speed = 5.0f;

    
    public NavMeshAgent agent;

    private void Start()
    {
        prisoners = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
	{
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