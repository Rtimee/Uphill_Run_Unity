using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text3D : MonoBehaviour
{
    #region MonoBehaviour Callbacks

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

    #endregion
}
