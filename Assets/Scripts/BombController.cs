using System;
using UnityEngine;

/// <summary>
/// 爆弾を制御するコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BombController : MonoBehaviour
{
    [Tooltip("発射する時にかける力")]
    [SerializeField] float m_shootPower = 10f;
    [Tooltip("発射する時に回転させる力")]
    [SerializeField] float m_torquePower = 3f;
    [Tooltip("爆発の半径")]
    [SerializeField] float m_bombRadius = 4f;
    [Tooltip("爆発した時に表示するエフェクトのプレハブ")]
    [SerializeField] GameObject m_explosionPrefab = default;
    /// <summary>爆発したかどうか</summary>
    bool m_exploded = false;

    void Start()
    {
        // 上向きに力をかけて回転させる
        var rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * m_shootPower, ForceMode2D.Impulse);
        rb.AddTorque(UnityEngine.Random.Range(-1 * m_torquePower, m_torquePower), ForceMode2D.Impulse);
    }

    /// <summary>
    /// 爆発させる
    /// </summary>
    public void Bomb()
    {
        if (m_exploded) return;
        m_exploded = true;
        
        // 爆発半径内の全てのコライダーに対して処理する
        Array.ForEach(Physics2D.OverlapCircleAll(this.transform.position, m_bombRadius), col =>
        {
            if (col.gameObject.tag == "Enemy")  // 敵の場合
            {
                col.gameObject.GetComponent<EnemyController>()?.Hit();
            }
            else if (col.gameObject.tag == "Bomb")  // 爆弾の場合
            {
                col.gameObject.GetComponent<BombController>()?.Bomb();
            }
        });

        // エフェクトを出す
        if (m_explosionPrefab)
        {
            Instantiate(m_explosionPrefab, this.transform.position, Quaternion.identity);
        }

        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Bomb();
    }
}
