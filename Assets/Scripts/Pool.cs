using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    #region Variables

    // Pool
    public List<GameObject> objectPool;
    // Size
    public int poolSize;
    // Prefab
    public GameObject Prefab;

    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        InitPool();
    }

    #endregion

    #region Other Methods

    public void InitPool()
    {
        objectPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            objectPool.Add(Instantiate(Prefab,transform));
            objectPool[i].SetActive(false);
        }
    }

    public GameObject GetObjFromPool(Vector3 pos, Quaternion rot)
    {
        GameObject newObject = objectPool[objectPool.Count - 1];
        newObject.SetActive(true);
        newObject.transform.position = pos;
        newObject.transform.rotation = rot;
        objectPool.RemoveAt(objectPool.Count - 1);
        return newObject;
    }

    public void ReturnObjToPool(GameObject go)
    {
        go.SetActive(false);
        go.transform.eulerAngles = Vector3.zero;
        objectPool.Add(go);
    }

    #endregion
}
