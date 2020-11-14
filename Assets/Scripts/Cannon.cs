using UnityEngine;

public class Cannon : MonoBehaviour
{
    #region Variables

    // Singleton
    public static Cannon Instance;

    [HideInInspector]
    public Character whoIsUsing;

    [SerializeField] Transform usePoint;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    #region Other Methods

    public void UseCannon(Transform character)
    {
        if (whoIsUsing != null)
            whoIsUsing.CannonTakenOver();
        whoIsUsing = character.GetComponent<Character>();
        character.position = usePoint.position;
        character.rotation = usePoint.rotation;
        character.parent = usePoint;
    }

    public void LoseCannon(Transform character)
    {
        character.parent = null;
    }

    #endregion
}
