using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Variables

    // Singleton
    public static CameraFollow Instance;

    [SerializeField] Transform finishTable;
    [SerializeField] Transform characterTransform;
    [SerializeField] Transform ragdollTransform;
    [SerializeField] [Range(0, 1)] float smoothValue;
    [SerializeField] Vector3 offset;

    Transform currentTransform;
    Vector3 firstOffset;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        Instance = this;
        currentTransform = characterTransform;
        firstOffset = offset;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = currentTransform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothValue);
        transform.position = smoothedPosition;
        transform.LookAt(currentTransform);
    }

    #endregion

    #region Other Methods

    public void CannonView()
    {
        offset = new Vector3(offset.x, 15, 20);
    }

    public void CrashView()
    {
        currentTransform = ragdollTransform;
    }

    public void DefaultView()
    {
        currentTransform = characterTransform;
        offset = new Vector3(0,4,-12);
    }

    public void EndGame()
    {
        currentTransform = finishTable;
        offset = new Vector3(0, 2, -15);
    }

    #endregion
}
