using DG.Tweening;
using UnityEngine;

public class TurningBlock : MonoBehaviour
{
    #region Variables

    [SerializeField] int turningDirection;
    [SerializeField] float turningSpeed;

    #endregion

    #region MonoBehaviour Callbacks

    private void Update()
    {
        Turning();
    }

    #endregion

    #region Other Methods

    void Turning()
    {
        transform.Rotate(transform.up, turningSpeed * turningDirection * Time.deltaTime,Space.World);
    }

    #endregion
}
