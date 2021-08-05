using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RockController : MonoBehaviour
{
    [Tooltip("敵がやられるエフェクトのプレハブ")]
    [SerializeField] GameObject m_hitPrefab = default;

    /// <summary>
    /// 攻撃が当たった時に呼ぶ
    /// </summary>
    public void Hit()
    {
        if (m_hitPrefab) Instantiate(m_hitPrefab, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
