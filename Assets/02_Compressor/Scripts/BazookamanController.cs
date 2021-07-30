using UnityEngine;

/// <summary>
/// プレイヤーを制御するコンポーネント
/// Horizontal で左右に動き、Fire1 で爆弾を発射する
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BazookamanController : MonoBehaviour
{
    [Tooltip("左右に動く力")]
    [SerializeField] float m_movePower = 1f;
    [Tooltip("爆弾を生成するポイント")]
    [SerializeField] Transform m_muzzle = default;
    [Tooltip("爆弾として生成するプレハブ")]
    [SerializeField] GameObject m_bulletPrefab = default;
    [Tooltip("足下に表示する Particle")]
    [SerializeField] ParticleSystem m_footStepParticle = default;
    Rigidbody2D m_rb = default;
    Animator m_anim = default;
    float m_h = 0;

    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        m_h = Input.GetAxisRaw("Horizontal");

        // アニメーションと足下のパーティクルを制御する
        if (m_h == 0)
        {
            m_footStepParticle.Stop();
            m_anim?.Play("BazookamanDefault");
        }
        else if (m_footStepParticle.isStopped)
        {
            m_footStepParticle.Play();
            m_anim?.Play("BazookamanWalk");
        }

        // 爆弾を発射する
        if (m_bulletPrefab && Input.GetButtonDown("Fire1"))
        {
            Instantiate(m_bulletPrefab, m_muzzle.position, Quaternion.identity);
        }
    }

    private void FixedUpdate()
    {
        m_rb.AddForce(m_h * Vector2.right * m_movePower);
    }
}
