using DG.Tweening;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    #region Variables

    [SerializeField] Vector2 yLimits;

    #endregion

    #region MonoBahaviour Callbacks

    private void Start()
    {
        MoveY();
    }

    #endregion

    #region Other Methods

    void MoveY()
    {
        transform.DOMoveY(yLimits.y, 2f, false).OnComplete(delegate {
            transform.DOMoveY(yLimits.x, 2f, false).OnComplete(delegate
            {
                MoveY();
            });
        });
    }

    #endregion
}
