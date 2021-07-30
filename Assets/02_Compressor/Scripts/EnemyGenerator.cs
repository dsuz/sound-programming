using UnityEngine;

/// <summary>
/// 敵の生成を制御するコンポーネント
/// </summary>
public class EnemyGenerator : MonoBehaviour
{
    [Tooltip("敵として生成するプレハブ")]
    [SerializeField] GameObject m_enemyPrefab = default;
    [Tooltip("敵を生成する場所")]
    [SerializeField] Transform[] m_spawnPoints = default;
    [Tooltip("敵を生成する間隔")]
    [SerializeField] float m_interval = 1f;
    float m_timer = 0;

    void Start()
    {
        Generate();
    }

    void Update()
    {
        // インターバルごとに敵を生成する
        m_timer += Time.deltaTime;

        if (m_timer > m_interval)
        {
            m_timer = 0f;
            Generate();
        }
    }

    /// <summary>
    /// 敵を生成する
    /// </summary>
    void Generate()
    {
        if (m_enemyPrefab)
        {
            Instantiate(m_enemyPrefab, m_spawnPoints[Random.Range(0, m_spawnPoints.Length)].position, Quaternion.identity);
        }
    }
}
