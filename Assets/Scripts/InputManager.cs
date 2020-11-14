using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Variables

    // Singleton
    public static InputManager Instance;

    [SerializeField] Character target;

    Vector3 startPos;
    Vector3 lastPos;

    // Trajectory value
    Vector3 distanceAmount;
    // Rotation value
    float rotationAmount;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        CalculateDistance();
    }

    #endregion

    #region Other Methods

    void CalculateDistance()
    {
        if (GameManager.Instance.isGameStarted)
        {
            if (Input.GetMouseButtonDown(0))
                startPos = Input.mousePosition;
            if (Input.GetMouseButton(0))
            {
                lastPos = Input.mousePosition;
                Vector3 distance = lastPos - startPos;
                switch (target.currentState)
                {
                    case Character.PlayerStates.UsingCannon:
                        distanceAmount -= distance / 25;

                        // Trajectory Limits
                        distanceAmount.x = Mathf.Clamp(distanceAmount.x, -17.5f, 17.5f);
                        distanceAmount.y = Mathf.Clamp(distanceAmount.y, 0, 8);
                        distanceAmount.z = Mathf.Clamp(distanceAmount.y, 0, -8);

                        UseCannon(distanceAmount);
                        break;
                    case Character.PlayerStates.Running:
                        rotationAmount += target.GetRotationSpeed() * distance.x;
                        // Rotation Limit
                        rotationAmount = Mathf.Clamp(rotationAmount, -target.GetMaxRotation(), target.GetMaxRotation());
                        Turning();
                        break;
                }
                startPos = lastPos;
            }
        }
    }

    void UseCannon(Vector3 velocity)
    {
        Projectile3D.Instance.SetVelocity(velocity);
    }

    void Turning()
    {
        target.transform.localEulerAngles = new Vector3(target.transform.localEulerAngles.x, rotationAmount, target.transform.localEulerAngles.z);
    }

    public void ResetValues()
    {
        distanceAmount = Vector3.zero;
        rotationAmount = 0;
    }

    #endregion
}
