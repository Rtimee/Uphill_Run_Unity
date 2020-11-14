using System.Collections;
using UnityEngine;

public class AI : Character
{
    #region Variables

    float timer;
    float turningRate;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        base.Awake();

        turningRate = Random.Range(2f, 5f);
    }

    private void Update()
    {
        base.Update();
        if (currentState == PlayerStates.Running)
        {
            if (timer <= turningRate)
                timer += Time.deltaTime;
            else
                AutoRotate();
        }
    }

    #endregion

    #region Other Methods

    void AutoRotate()
    {
        float randomYValue = Random.Range(-45, 45);
        float yValue = transform.localEulerAngles.y + randomYValue;
        yValue = Mathf.Clamp(yValue, -45, 45);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yValue, transform.localEulerAngles.z);
        timer = 0;
        Invoke("TurnToHill", turningRate / 2);
    }

    void TurnToHill()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
    }

    #endregion
}
