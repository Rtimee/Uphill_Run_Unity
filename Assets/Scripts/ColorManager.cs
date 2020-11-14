using UnityEngine;

public class ColorManager : MonoBehaviour
{
    #region Variables

    // Singleton
    public static ColorManager Instance;

    [SerializeField] Material[] materials;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    #region Other Methods

    public Material GetMaterial()
    {
        int random = Random.Range(0, materials.Length);
        Material newMaterial = materials[random];

        return newMaterial;
    }

    #endregion
}
