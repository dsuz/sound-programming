using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField] GameObject m_prefab = default;
    [SerializeField] float m_minX = -10;
    [SerializeField] float m_maxX = 10;
    [SerializeField] float m_interval = 2;
    float m_timer = 0;

    void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer > m_interval)
        {
            m_timer = 0;
            Vector2 pos = new Vector2(Random.Range(m_minX, m_maxX), this.transform.position.y);
            Instantiate(m_prefab, pos, Quaternion.identity);
        }
    }
}
