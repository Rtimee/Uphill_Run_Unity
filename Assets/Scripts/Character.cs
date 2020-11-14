using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{
    #region Variables

    public PlayerStates currentState;

    [Header("Others")]
    public float score;

    [Header("Attributes")]
    [SerializeField] float runSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] float maxRotationValue;

    [Header("Spawn")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform checkPoint;
    Transform currentSpawnPoint;

    [Header("FX")]
    [SerializeField] GameObject boostFx;

    [Header("UI")]
    [SerializeField] Text scoreText;
    [SerializeField] GameObject nameText;

    // Ragdoll script
    RagdollController ragdoll;
    // Components
    Rigidbody rb;
    Animator anim;
    // Others
    float currentSpeed;

    #endregion

    #region States

    public enum PlayerStates
    {
        Waiting,
        Running,
        Crashed,
        UsingCannon,
        EndGame
    }

    #endregion

    #region MonoBehaviour Callbacks

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        ragdoll = GetComponent<RagdollController>();
        currentSpawnPoint = spawnPoint;
        currentSpeed = runSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ball"))
        {
            // If character collide with ball when running
            if (currentState == PlayerStates.Running)
            {
                if (Cannon.Instance.whoIsUsing != null && Cannon.Instance.whoIsUsing.gameObject.CompareTag("Player"))
                {
                    GameObject hit = GameManager.Instance.textPool.GetObjFromPool(transform.position, Quaternion.identity);
                    hit.GetComponent<HitText>().LoadText();
                    GameManager.Instance.EarnCoin();
                }
                ActiveRagdoll();
                rb.AddForce((transform.position - collision.transform.position).normalized * 20000);
            }
        }
    }

    public void Update()
    {
        if (currentState == PlayerStates.Running)
            Running();
        else if (currentState == PlayerStates.UsingCannon)
            UpdateScore();
    }

    #endregion

    #region Other Methods

    // When ball hit
    public void Crash()
    {
        nameText.SetActive(false);
        currentState = PlayerStates.Crashed;
        if (boostFx.active)
            boostFx.SetActive(false);
        anim.SetBool("Run", false);
        anim.enabled = false;
        if(gameObject.CompareTag("Player"))
            CameraFollow.Instance.CrashView();
        Invoke("Respawn", 3f);
    }

    public void PassiveRagdoll()
    {
        ragdoll.PassiveRagdoll();
    }

    public void ActiveRagdoll()
    {
        Crash();
        ragdoll.ActiveRagdoll();
    }

    void ResetCharacter()
    {
        anim.enabled = true;
        transform.position = currentSpawnPoint.position;
        transform.rotation = currentSpawnPoint.rotation;
        rb.isKinematic = true;
        Invoke("StartRunning", 1f);
    }

    public void Respawn()
    {
        if (gameObject.CompareTag("Player"))
        {
            CameraFollow.Instance.DefaultView();          
        }
        currentState = PlayerStates.Waiting;
        PassiveRagdoll();
        ResetCharacter();
        nameText.SetActive(true);
    }

    public void StartRunning()
    {
        rb.isKinematic = false;
        anim.SetBool("Run", true);
        currentState = PlayerStates.Running;
    }

    public void Running()
    {
        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
    }

    public void UseCannon()
    {
        if (currentState == PlayerStates.Running)
        {
            anim.SetBool("Run", false);
            Cannon.Instance.UseCannon(transform);
            currentState = PlayerStates.UsingCannon;
            if (gameObject.CompareTag("Player"))
                CameraFollow.Instance.CannonView();
        }
    }

    public void CannonTakenOver()
    {
        if (gameObject.CompareTag("Player"))
            InputManager.Instance.ResetValues();
        Cannon.Instance.LoseCannon(gameObject.transform);
        currentSpawnPoint = spawnPoint;
        Respawn();
        currentState = PlayerStates.Waiting;
        Invoke("Boost", 1f);
    }

    public void Boost()
    {
        boostFx.SetActive(true);
        anim.speed = 1.25f;
        float additionalSpeed = Random.Range(3f, 6f);
        currentSpeed += additionalSpeed;
        Invoke("DefaultSpeed", 2.5f);
    }

    void DefaultSpeed()
    {
        currentSpeed = runSpeed;
        boostFx.SetActive(false);
        anim.speed = 1f;
    }

    void UpdateScore()
    {
        score += Time.deltaTime;
        scoreText.text = ((int)score).ToString();
    }

    public void PassCheckPoint()
    {
        currentSpawnPoint = checkPoint;
    }

    // Getter methods -------------------
    public float GetRotationSpeed()
    {
        return rotateSpeed;
    }

    public float GetMaxRotation()
    {
        return maxRotationValue;
    }

    // Getter methods -------------------

    #endregion
}
