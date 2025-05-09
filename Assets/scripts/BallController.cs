using UnityEngine;

public class BallController : MonoBehaviour

{
    public GameObject player;
    private RunnerController runnerController;

    void Start()
    {
        if (player != null)
        {
            runnerController = player.GetComponent<RunnerController>();
        }
        else
        {
            Debug.LogWarning("Player GameObject not assigned in BallController.");
        }
    }


    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wall") || other.CompareTag("Ground") || other.CompareTag("Enemy") )
        {
            Destroy(gameObject);
            
            
        }

        else if (other.CompareTag("Runner"))
        {
            Debug.Log("ya herro ya merro");

            RunnerController rc = other.GetComponent<RunnerController>();
            if (rc != null)
            {
                rc.isAlive = false;
            }
            else
            {
                Debug.LogWarning("RunnerController component not found on Runner object!");
            }
        }

    }
}
