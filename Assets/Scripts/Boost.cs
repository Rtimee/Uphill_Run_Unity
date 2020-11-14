using UnityEngine;

public class Boost : MonoBehaviour
{
    #region MonoBehaviour Callbacks

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AI") || other.CompareTag("Player"))
        {
            other.GetComponent<Character>().Boost();
        }
    }

    #endregion
}
