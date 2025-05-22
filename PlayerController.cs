using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float dashSpeed = 40f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 4f;
    static public int health = 100;

    //public Animator PlayerAnimator;
    private Rigidbody rb;
    private bool isDashing = false;
    private bool canDash = true;

    private CinemachineImpulseSource impulseSource;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        if (!isDashing)
        {
            Move();
        }
        //RotateToMouse();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        if(!canDash) yield break;
        //PlayerAnimator.SetInteger("state", 2);
        isDashing = true;
        canDash = false;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        Vector3 dashDirection = new Vector3(moveX, 0, moveZ).normalized;

        if (dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward;
        }
        rb.linearVelocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void Move()
    {
        if(isDashing) return;
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 targetVelocity = new Vector3(moveX, 0, moveZ).normalized * moveSpeed;
        //rb.linearVelocity = moveDir * moveSpeed;
       /* if (targetVelocity == Vector3.zero)
        {
            rb.linearVelocity = Vector3.zero;
            PlayerAnimator.SetInteger("state", 0); // 정지 상태 애니메이션
        }
        else
        {
            PlayerAnimator.SetInteger("state", 1); // 이동 상태 애니메이션
        }*/
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, Time.deltaTime * 3f);
    }


    public void TakeDamage(int damage)
    {
        CameraManager.instance.CameraShake(impulseSource);
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject); // 체력이 0이면 파괴
            GameManager.instance.GameOver();
        }
        Debug.Log("Player Health: " + health);
    }

    public void HealHP()
    {
        health = 100;
    }
}
