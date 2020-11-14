using System.Collections;
using UnityEngine;

public class HitText : MonoBehaviour
{
    #region Variables

    Camera camera;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        transform.Translate(transform.up * 3 * Time.deltaTime, Space.World);
        transform.LookAt(camera.transform);
    }

    #endregion

    #region Other Methods

    public void LoadText()
    {
        StartCoroutine(DestroyText());
    }

    IEnumerator DestroyText()
    {
        yield return new WaitForSeconds(3f);
        GameManager.Instance.textPool.ReturnObjToPool(gameObject);
    }

    #endregion
}
