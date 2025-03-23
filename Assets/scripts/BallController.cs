using UnityEngine;

public class BallController : MonoBehaviour
{
    
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wall") || other.CompareTag("Ground") || other.CompareTag("Enemy") )
        {
            
            Destroy(gameObject);
            
            
        }
    }

  

}
