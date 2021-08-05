using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour
{
    [SerializeField] float m_movePower = 10f;
    [SerializeField] float m_jumpVelocity = 3f;
    [SerializeField] int m_maxJumpCount = 2;
    [SerializeField] Collider2D m_attackRange = default;
    [SerializeField] Collider2D m_attackRangeMidAir = default;
    Rigidbody2D m_rb = default;
    Animator m_anim = default;
    float m_h = 0;
    float m_lastHorizontalInput = -1;
    bool m_isGrounded = false;
    int m_jumpCount = 0;
    bool m_isFrozen = false;

    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (m_isFrozen)
        {
            if (m_isGrounded)
            {
                m_h = 0;
                m_rb.velocity = Vector2.zero;
            }

            return;
        }

        m_h = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && m_jumpCount < m_maxJumpCount)
        {
            m_jumpCount++;
            m_isGrounded = false;
            m_rb.velocity += m_jumpVelocity * Vector2.up;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            m_anim.SetTrigger("Attack");
        }

        if (m_h * m_lastHorizontalInput < 0)
        {
            Vector3 scale = this.transform.localScale;
            scale.x = scale.x * -1;
            this.transform.localScale = scale;
        }

        if (m_h != 0) m_lastHorizontalInput = m_h;
    }

    private void FixedUpdate()
    {
        m_rb.AddForce(m_h * m_movePower * Vector2.right);
    }

    private void LateUpdate()
    {
        m_anim.SetFloat("WalkSpeed", Mathf.Abs(m_rb.velocity.x));
        m_anim.SetBool("IsGrounded", m_isGrounded);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            m_jumpCount = 0;
            m_isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            m_isGrounded = false;
        }
    }

    void Freeze()
    {
        m_isFrozen = true;
    }

    void UnFreeze()
    {
        m_isFrozen = false;
    }

    void Attack()
    {
        List<Collider2D> result = new List<Collider2D>(); ;
        if (m_attackRange.OverlapCollider(new ContactFilter2D(), result) > 0)
        {
            result.ForEach(x =>
            {
                if (x.gameObject.tag.Equals("Enemy"))
                {
                    x.GetComponent<EnemyController>()?.Hit();
                }
                else if (x.gameObject.tag.Equals("Rock"))
                {
                    x.GetComponent<RockController>()?.Hit();
                }
            });
        }
    }

    void AttackMidAir()
    {
        List<Collider2D> result = new List<Collider2D>(); ;
        if (m_attackRangeMidAir.OverlapCollider(new ContactFilter2D(), result) > 0)
        {
            result.ForEach(x =>
            {
                if (x.gameObject.tag.Equals("Enemy"))
                {
                    x.GetComponent<EnemyController>()?.Hit();
                }
                else if (x.gameObject.tag.Equals("Rock"))
                {
                    x.GetComponent<RockController>()?.Hit();
                }
            });
        }
    }
}
