using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    #region Variables

    // Ragdoll rigidbodies
    List<Rigidbody> rigidbodies;
    // Ragdoll colliders
    List<Collider> colliders;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>().ToList();
        colliders = GetComponentsInChildren<Collider>().ToList();
    }

    private void Start()
    {
        PassiveRagdoll();
    }

    #endregion

    #region Other Methods

    public void ActiveRagdoll()
    {
        RigidbodyState(false);
        ColliderState(true);
    }

    public void PassiveRagdoll()
    {
        RigidbodyState(true);
        ColliderState(false);
    }

    void RigidbodyState(bool state)
    {
        // index 0 = parent so we start from 1
        for (int i = 1; i < rigidbodies.Count; i++)
        {
            rigidbodies[i].isKinematic = state;
        }
    }

    void ColliderState(bool state)
    {
        // index 0 = parent so we start from 1
        for (int i = 1; i < colliders.Count; i++)
        {
            colliders[i].enabled = state;
        }
    }

    #endregion
}
