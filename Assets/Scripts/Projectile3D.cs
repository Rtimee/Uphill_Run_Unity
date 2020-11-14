using System.Collections;
using UnityEngine;

public class Projectile3D : MonoBehaviour
{
    #region Variables

    public static Projectile3D Instance;

    [Header("Others")]
    [SerializeField] Character player;

    [Header("Cannon & Projectile")]
    [SerializeField] Transform turret;
    [SerializeField] Transform cursor;
    [SerializeField] Pool ballPool;
    [SerializeField] Vector2 xLimits;
    [SerializeField] Vector2 zLimits;
    [SerializeField] float fireCooldown;
    [SerializeField] Animator anim;

    [Header("Line renderer veriables")]
    [SerializeField] LineRenderer line;
    [Range(2, 30)]
    [SerializeField] int resolution;

    [Header("Formula variables")]
    [SerializeField] float yLimit;
    private float g;

    [Header("Linecast variables")]
    [Range(2, 30)]
    [SerializeField] int linecastResolution;
    [SerializeField] LayerMask canHit;

    float timer;
    bool canShoot;

    Vector3 currentVo;
    Vector3 previousVo;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        Instance = this;

        g = Mathf.Abs(Physics.gravity.y);
        previousVo = GenerateRandomPoint();
    }

    private void Update()
    {
        if (GameManager.Instance.isGameStarted)
        {
            if (timer <= fireCooldown)
                timer += Time.deltaTime;
            else
            {
                if (!canShoot)
                {
                    canShoot = true;

                    if (GameManager.Instance.isGameStarted && player.currentState != Character.PlayerStates.UsingCannon)
                        FireBall(GenerateRandomPoint());
                    else if (player.currentState == Character.PlayerStates.UsingCannon)
                        FireBall(previousVo);
                }
            }
            StartCoroutine(RenderArc());
        }
    }

    #endregion

    #region Other Methods

    private IEnumerator RenderArc()
    {
        line.positionCount = resolution + 1;
        line.SetPositions(CalculateLineArray());
        previousVo = Vector3.Lerp(previousVo, currentVo, .01f);
        turret.transform.rotation = Quaternion.LookRotation(-previousVo);
        yield return null;
    }

    private Vector3[] CalculateLineArray()
    {
        Vector3[] lineArray = new Vector3[resolution + 1];

        var lowestTimeValueX = MaxTimeX() / resolution;
        var lowestTimeValueZ = MaxTimeZ() / resolution;
        var lowestTimeValue = lowestTimeValueX > lowestTimeValueZ ? lowestTimeValueZ : lowestTimeValueX;

        for (int i = 0; i < lineArray.Length; i++)
        {
            var t = lowestTimeValue * i;
            lineArray[i] = CalculateLinePoint(t);
        }
        return lineArray;
    }

    private Vector3 HitPosition()
    {
        var lowestTimeValue = MaxTimeY() / linecastResolution;

        for (int i = 0; i < linecastResolution + 1; i++)
        {
            RaycastHit rayHit;

            var t = lowestTimeValue * i;
            var tt = lowestTimeValue * (i + 1);

            if (Physics.Linecast(CalculateLinePoint(t), CalculateLinePoint(tt), out rayHit, canHit))
            {
                cursor.position = rayHit.point + rayHit.normal * 0.001f;
                cursor.transform.LookAt(rayHit.point + rayHit.normal);
                return rayHit.point;
            }
        }

        return CalculateLinePoint(MaxTimeY());
    }

    private Vector3 CalculateLinePoint(float t)
    {
        float x = previousVo.x * t;
        float z = previousVo.z * t;
        float y = (previousVo.y * t) - (g * Mathf.Pow(t, 2) / 2);
        return new Vector3(x + transform.position.x, y + transform.position.y, z + transform.position.z);
    }

    private float MaxTimeY()
    {
        var v = previousVo.y;
        var vv = v * v;

        var t = (v + Mathf.Sqrt(vv + 2 * g * (transform.position.y - yLimit))) / g;
        return t;
    }

    private float MaxTimeX()
    {
        if (IsValueAlmostZero(previousVo.x))
            SetValueToAlmostZero(ref previousVo.x);

        var x = previousVo.x;

        var t = (HitPosition().x - transform.position.x) / x;
        return t;
    }

    private float MaxTimeZ()
    {
        if (IsValueAlmostZero(previousVo.z))
            SetValueToAlmostZero(ref previousVo.z);

        var z = previousVo.z;

        var t = (HitPosition().z - transform.position.z) / z;
        return t;
    }

    private bool IsValueAlmostZero(float value)
    {
        return value < 0.0001f && value > -0.0001f;
    }

    private void SetValueToAlmostZero(ref float value)
    {
        value = 0.0001f;
    }

    public void SetVelocity(Vector3 velocity)
    {
        previousVo = currentVo;
        currentVo = velocity;
    }

    void FireBall(Vector3 velocity)
    {
        anim.Play("Fire");
        GameObject ball = ballPool.GetObjFromPool(transform.position, transform.rotation);
        ball.GetComponent<Ball>().LoadBall(velocity);
        canShoot = false;
        timer = 0;
    }

    Vector3 GenerateRandomPoint()
    {
        Vector3 targetPosition = new Vector3(Random.Range(xLimits.x, xLimits.y), 0, Random.Range(zLimits.x, zLimits.y));
        Vector3 Vo = CalculateVelocity(targetPosition, transform.position, 2.5f);
        SetVelocity(Vo);
        return previousVo;
    }

    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = (Sy / time) + (.5f * Mathf.Abs(Physics.gravity.y) * time);

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

    public void DestroyBall(GameObject ball)
    {
        ballPool.ReturnObjToPool(ball);
    }

    public void ActiveLine()
    {
        line.enabled = true;
    }
    public void PassiveLine()
    {
        line.enabled = false;
        cursor.gameObject.SetActive(false);
    }

    #endregion
}
