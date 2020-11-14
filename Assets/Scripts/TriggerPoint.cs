using UnityEngine;

// Take over line
public class TriggerPoint : MonoBehaviour
{
    #region MonoBehaviour Callbacks

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("AI"))
            other.GetComponent<Character>().UseCannon();
    }

    #endregion
}
