using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    #region MonoBehaviour Callbacks

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("AI"))
            other.GetComponent<Character>().PassCheckPoint();
    }

    #endregion
}
