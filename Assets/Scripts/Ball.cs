using UnityEngine;

public class Ball : MonoBehaviour
{
    #region Variables

    // Is ball on ground ?
    bool onGround;
    // Components
    Rigidbody rb;
    MeshRenderer meshRenderer;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = ColorManager.Instance.GetMaterial();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            onGround = true;
            Vector3 point = new Vector3(collision.GetContact(0).point.x, collision.GetContact(0).point.y + .1f, collision.GetContact(0).point.z);
            GameObject splat = GameManager.Instance.splatPool.GetObjFromPool(point, collision.transform.rotation);
            splat.GetComponent<Splat>().LoadSplat(meshRenderer.material.color);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Underground"))
        {
            onGround = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Projectile3D.Instance.DestroyBall(gameObject);
        }
    }

    #endregion

    #region Other Methods

    public void LoadBall(Vector3 velocity)
    {
        rb.velocity = velocity;
    }

    #endregion
}
