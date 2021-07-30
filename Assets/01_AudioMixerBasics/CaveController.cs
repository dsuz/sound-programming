using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;    // AudioMixer の API を使うのに必要です

/// <summary>
/// 洞窟に入ったか、洞窟から出たかを判定する。
/// 洞窟に入った、とは「Tag が Cave のトリガーと接触している時」とする。
/// 洞窟に出入りすると、ライトをオフ/オンにし、AudioMixer のスナップショットを切り替える
/// </summary>
public class CaveController : MonoBehaviour
{
    /// <summary>洞窟内に入った時にオンにするライト</summary>
    [SerializeField] Light m_light;
    /// <summary>洞窟外に居る時に有効にするスナップショット</summary>
    [SerializeField] AudioMixerSnapshot m_audioSettingsInField;
    /// <summary>洞窟内に居る時に有効にするスナップショット</summary>
    [SerializeField] AudioMixerSnapshot m_audioSettingsInCave;
    /// <summary>スナップショット切り替えにかける時間（秒）</summary>
    [SerializeField] float m_transitionSeconds = 0.3f;

    void Start()
    {
        m_light.enabled = false;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cave")
        {
            m_light.enabled = true;
            
            if (m_audioSettingsInCave)
            {
                m_audioSettingsInCave.TransitionTo(m_transitionSeconds);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Cave")
        {
            m_light.enabled = false;

            if (m_audioSettingsInField)
            {
                m_audioSettingsInField.TransitionTo(m_transitionSeconds);
            }
        }
    }
}
