using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rigidbody を使ってプレイヤーを動かすコンポーネント
/// 入力を受け取り、それに従ってオブジェクトを動かす
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class RigidbodyPlayerControllerWithSound : MonoBehaviour
{
    /// <summary>動く速さ</summary>
    [SerializeField] float m_movingSpeed = 5f;
    /// <summary>ターンの速さ</summary>
    [SerializeField] float m_turnSpeed = 3f;
    /// <summary>ジャンプ力</summary>
    [SerializeField] float m_jumpPower = 5f;
    /// <summary>接地判定の際、中心 (Pivot) からどれくらいの距離を「接地している」と判定するかの長さ</summary>
    [SerializeField] float m_isGroundedLength = 1.1f;
    /// <summary>効果音を再生するための AudioSource</summary>
    [SerializeField] AudioSource m_audioSource;
    /// <summary>ジャンプした時の効果音</summary>
    [SerializeField] AudioClip m_jump;
    /// <summary>着地した時（実際は衝突した時）の効果音</summary>
    [SerializeField] AudioClip m_landing;
    /// <summary>Fire1 を押した時の効果音</summary>
    [SerializeField] AudioClip m_fire1;
    /// <summary>Fire2 を押した時の効果音</summary>
    [SerializeField] AudioClip m_fire2;
    Rigidbody m_rb;


    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 方向の入力を取得し、方向を求める
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        // 入力方向のベクトルを組み立てる
        Vector3 dir = Vector3.forward * v + Vector3.right * h;
        Move(dir);

        // ジャンプの入力を取得し、接地している時に押されていたらジャンプする
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Fire1();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Fire2();
        }
    }

    /// <summary>
    /// 地面に接触しているか判定する
    /// </summary>
    /// <returns></returns>
    bool IsGrounded()
    {
        // Physics.Linecast() を使って足元から線を張り、そこに何かが衝突していたら true とする
        Vector3 start = this.transform.position;   // start: オブジェクトの Pivot
        Vector3 end = start + Vector3.down * m_isGroundedLength;  // end: start から真下の地点
        Debug.DrawLine(start, end); // 動作確認用に Scene ウィンドウ上で線を表示する
        bool isGrounded = Physics.Linecast(start, end); // 引いたラインに何かがぶつかっていたら true とする
        return isGrounded;
    }

    /// <summary>
    /// Rigidbody を使って入力した方向にオブジェクトを移動させる
    /// </summary>
    /// <param name="dir">入力した方向</param>
    void Move(Vector3 dir)
    {
        if (dir == Vector3.zero)
        {
            // 方向の入力がニュートラルの時は、y 軸方向の速度を保持するだけ
            m_rb.velocity = new Vector3(0f, m_rb.velocity.y, 0f);
        }
        else
        {
            // カメラを基準に入力が上下=奥/手前, 左右=左右にキャラクターを向ける
            dir = Camera.main.transform.TransformDirection(dir);    // メインカメラを基準に入力方向のベクトルを変換する
            dir.y = 0;  // y 軸方向はゼロにして水平方向のベクトルにする

            // 入力方向に滑らかに回転させる
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * m_turnSpeed);

            Vector3 velo = dir.normalized * m_movingSpeed; // 入力した方向に移動する
            velo.y = m_rb.velocity.y;   // ジャンプした時の y 軸方向の速度を保持する
            m_rb.velocity = velo;   // 計算した速度ベクトルをセットする
        }
    }

    /// <summary>
    /// ジャンプする。
    /// </summary>
    void Jump()
    {
        m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
        m_audioSource.PlayOneShot(m_jump);
    }

    void Fire1()
    {
        m_audioSource.PlayOneShot(m_fire1);
    }

    void Fire2()
    {
        m_audioSource.PlayOneShot(m_fire2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_audioSource.PlayOneShot(m_landing);
    }
}
