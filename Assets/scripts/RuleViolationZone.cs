using UnityEngine;

public class RuleViolationZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Sadece Tag'i "Runner" olanlarla ilgileniyoruz
        if (other.CompareTag("Runner"))
        {
            Debug.Log("Kural ihlali");
        }
    }
}
