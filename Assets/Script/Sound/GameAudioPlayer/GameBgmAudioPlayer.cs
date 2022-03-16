using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YY.Sound
{
	/// <summary>
	/// ゲームBGM再生クラス
	/// </summary>
	public class GameBgmAudioPlayer : GameAudioPlayerBase
	{
		/// <summary>
		/// フェードする時間
		/// </summary>
		[SerializeField]
		private float m_FadeTime = 1.0f;

		protected override void Awake()
		{
			//オーディオソースの取得
			m_AudioSource = GetComponent<AudioSource>();
			//オーディオソースの初期化
			InitializeAudioSource();
		}

		protected override void Start()
		{
			base.Start();

			//ミキサーを設定
			m_AudioSource.outputAudioMixerGroup = SoundManager.Instance.GetAudioMixerManager().BgmGroup;
		}

		/// <summary>
		/// サウンドの再生
		/// </summary>
		public override void PlaySound()
		{
			//一時停止中は再生しない
			if (IsPaused) return;

			//オーディオクリップを取得
			AudioClip audioClip = m_AudioSource.clip;

			//nullチェック
			if (audioClip == null)
			{
				Debug.LogError("オーディオクリップがありません");
				return;
			}

			//フェードインしながら再生
			m_AudioSource.PlayWithFadeIn(audioClip, m_FadeTime);
		}

		/// <summary>
		/// サウンドの停止
		/// </summary>
		public override void StopSound()
		{
			m_AudioSource.StopWithFadeOut(m_FadeTime);
		}

		/// <summary>
		/// オーディオソースの初期化
		/// </summary>
		protected override void InitializeAudioSource()
		{
			base.InitializeAudioSource();

			if (m_3DSoundSetting == null) return;
			//3Dサウンドの比率にする
			m_AudioSource.spatialBlend = m_3DSoundSetting.SpatialBlend;
			//3Dサウンドの設定
			m_AudioSource.dopplerLevel = m_3DSoundSetting.DopplerLeveget;
			m_AudioSource.spread = m_3DSoundSetting.Spread;
			m_AudioSource.minDistance = m_3DSoundSetting.MinDistance;
			m_AudioSource.maxDistance = m_3DSoundSetting.MaxDistance;
		}
	}
}
