using UnityEngine;
using YY.Sound;

public class DebugController : MonoBehaviour
{
	/// <summary>
	/// ポーズフラグ
	/// </summary>
	[SerializeField]
	private bool m_IsPause = false;

	/// <summary>
	/// ミュートフラグ
	/// </summary>
	[SerializeField]
	private bool m_IsMute = false;

	private void Update()
	{
		//Pキーを押すとポーズ
		if (Input.GetKeyDown(KeyCode.P))
		{
			m_IsPause = !m_IsPause;
		}
		//ポーズフラグがtrueならタイムスケールを0にしてゲームを停止する
		if (m_IsPause) Time.timeScale = 0.0f;
		else Time.timeScale = 1.0f;

		//ミュート処理
		Mute();
	}

	public void Mute()
	{
		//SEOnOff切り替え
		if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
		{
			m_IsMute = !m_IsMute;
		}

		//音量調整
		if (m_IsMute) SoundManager.Instance.GetAudioMixerManager().MasterVolumeByLinear = 0.0f;
		else SoundManager.Instance.GetAudioMixerManager().MasterVolumeByLinear = 1.0f;
	}
}

