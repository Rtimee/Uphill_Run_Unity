using System.Collections;
using UnityEngine;

public class Splat : MonoBehaviour
{
    #region Variables

    [SerializeField] SpriteRenderer sprite;

    #endregion

    #region Other Methods

    public void LoadSplat(Color color)
    {
        sprite.color = color;
        StartCoroutine(DestroySplat());
    }

    IEnumerator DestroySplat()
    {
        yield return new WaitForSeconds(5f);
        GameManager.Instance.splatPool.ReturnObjToPool(gameObject);
    }

    #endregion
}
