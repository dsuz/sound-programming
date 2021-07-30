using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームを管理するコンポーネント
/// </summary>
public class GameManager : MonoBehaviour
{
    [Tooltip("チェックを入れるとテストモードになり、ゲームオーバーにならなくなる")]
    [SerializeField] bool m_testMode = false;
    [Tooltip("コンボの有効期間（秒）")]
    [SerializeField] float m_comboDuration = 1f;
    [Tooltip("ベースとなる得点")]
    [SerializeField] int m_baseScore = 100;
    [Tooltip("スコアを表示する Text")]
    [SerializeField] Text m_scoreText = default;
    [Tooltip("コンボ情報を表示する Text")]
    [SerializeField] Text m_comboText = default;
    [Tooltip("敵を生成する GameObject。ゲームオーバー時に破棄する")]
    [SerializeField] GameObject m_enemyGenerator = default;
    /// <summary>現在のコンボ数</summary>
    public static int s_comboCounter = 0;
    /// <summary>現在ゲームに残っているターゲットの数</summary>
    public static int s_targetCounter = 0;
    /// <summary>得点</summary>
    int m_score = 0;
    /// <summary>コンボタイマー</summary>
    float m_comboTimer = 0;

    private void Start()
    {
        s_targetCounter = GameObject.FindGameObjectsWithTag("Target").Length;
    }

    void Update()
    {
        // コンボ処理
        if (s_comboCounter > 0)
        {
            m_comboTimer += Time.deltaTime;
            m_comboText.text = $"Combo: x{s_comboCounter} {s_comboCounter * s_comboCounter * m_baseScore}";

            // コンボ期間が終わったら
            if (m_comboTimer > m_comboDuration)
            {
                // コンボをリセットして得点を追加する
                m_score += s_comboCounter * s_comboCounter * m_baseScore;
                m_comboTimer = 0;
                s_comboCounter = 0;
                m_comboText.text = "";
                m_scoreText.text = m_score.ToString("D8");
            }
        }

        // ゲームオーバー処理
        if (s_targetCounter < 1 && !m_testMode)
        {
            // 敵生成器を破棄して敵をもう出なくする
            Destroy(m_enemyGenerator);
            // 残っている敵は全て逃げる
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            Array.ForEach(enemies, e => e.GetComponent<EnemyController>()?.Run());
        }
    }
}
