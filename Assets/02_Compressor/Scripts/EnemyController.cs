using UnityEngine;

/// <summary>
/// 敵を制御するコンポーネント
/// ゆらゆら揺れながらターゲット（リンゴ）に向かう
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [Tooltip("上下に揺れる力")]
    [SerializeField] float m_floatPower = 16f;
    [Tooltip("上下に揺れる力が変化する速度")]
    [SerializeField] float m_noiseChangeSpeed = 1f;
    [Tooltip("上下振動の速さ")]
    [SerializeField] float m_frequency = 1f;
    [Tooltip("ターゲットを横方向に追いかける力")]
    [SerializeField] float m_targetChasePower = 1f;
    [Tooltip("敵がやられるエフェクトのプレハブ")]
    [SerializeField] GameObject m_enemyHitPrefab = default;
    [Tooltip("敵がリンゴを取って逃げるプレハブ")]
    [SerializeField] GameObject m_enemyRunPrefab = default;
    Rigidbody2D m_rb = default;
    /// <summary>敵が向かうターゲット</summary>
    Transform m_target = default;
    
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // ターゲットがない場合はセットする
        if (!m_target)
        {
            m_target = PickupTarget();
        }

        // 上下に揺れながら、ターゲットに向かう
        float x = (m_target.position.x - this.transform.position.x) * m_targetChasePower;
        float y = Mathf.PerlinNoise(Time.time * m_noiseChangeSpeed, 0) * Mathf.Sin(Time.time * m_frequency) * m_floatPower;
        Vector2 dir = new Vector2(x, y);
        m_rb.AddForce(dir);
    }

    /// <summary>
    /// 攻撃が当たった時に呼ぶ
    /// </summary>
    public void Hit()
    {
        Instantiate(m_enemyHitPrefab, this.transform.position, Quaternion.identity);
        GameManager.s_comboCounter++;
        Destroy(this.gameObject);
    }

    /// <summary>
    /// ターゲットを選ぶ
    /// 画面内の Target タグがついたオブジェクトをランダムに取ってくる。
    /// ない場合は、自分自身をターゲットとすることにより、真下に揺れながら落ちてくる
    /// </summary>
    /// <returns></returns>
    Transform PickupTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        Transform target = targets.Length > 0 ? targets[Random.Range(0, targets.Length)].transform : this.transform;
        return target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            // ターゲットにぶつかったら、逃げてターゲットの数を一つ減らす
            Run();
            GameManager.s_targetCounter--;
            Destroy(collision.gameObject);
        }
    }

    /// <summary>
    /// 敵が逃げるエフェクトを表示する時に呼ぶ
    /// </summary>
    public void Run()
    {
        Instantiate(m_enemyRunPrefab, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
