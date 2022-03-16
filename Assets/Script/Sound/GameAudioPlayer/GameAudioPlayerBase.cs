using UnityEngine;

namespace YY.Sound
{
	/// <summary>
	/// Game内で使うSEやBGMを鳴らす基底クラス
	/// 3Dサウンドにも対応できるように制作
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public abstract class GameAudioPlayerBase : MonoBehaviour, IAudioPausable
	{
		/// <summary>
		/// オーディオソース
		/// </summary>
		protected AudioSource m_AudioSource = null;

		/// <summary>
		/// 3Dサウンドの設定データ
		/// </summary>
		[SerializeField]
		protected Game3DSoundSetteing m_3DSoundSetting = null;

		/// <summary>
		/// 実行時に再生を開始するか
		/// </summary>
		[SerializeField]
		protected bool m_IsPlayOnAwake = false;

		/// <summary>
		/// 一時停止しているか
		/// </summary>
		public bool IsPaused { get; private set; }

		protected virtual void Awake()
		{
			//オーディオソースの取得
			m_AudioSource = GetComponent<AudioSource>();
			//オーディオソースの初期化
			InitializeAudioSource();
		}

		protected virtual void Start()
		{
			//サウンドマネージャーのAudioPausebleListに追加
			SoundManager.Instance.SetAudioPausebleList(this);

			//フラグがtrueなら再生
			if (m_IsPlayOnAwake)
			{
				PlaySound();
			}
		}

		/// <summary>
		/// サウンドの再生
		/// </summary>
		public abstract void PlaySound();

		/// <summary>
		/// サウンドの停止
		/// </summary>
		public abstract void StopSound();

		/// <summary>
		/// オーディオを一時停止
		/// </summary>
		public virtual void Pause()
		{
			IsPaused = true;
			m_AudioSource.Pause();
		}

		/// <summary>
		/// オーディオの再生を再開
		/// </summary>
		public virtual void Resume()
		{
			IsPaused = false;
			m_AudioSource.UnPause();
		}

		public virtual void OnDestroy()
		{
			//サウンドマネージャーのAudioPausebleListから削除
			SoundManager.Instance.RemoveAudioPausebleList(this);
		}

		/// <summary>
		/// オーディオソースの初期化
		/// </summary>
		protected virtual void InitializeAudioSource()
		{
			//PlayOnAwakeで再生した場合のために一度停止の処理を呼ぶ
			m_AudioSource.Stop();
			m_AudioSource.playOnAwake = false;
			m_AudioSource.volume = 1.0f;
			m_AudioSource.pitch = 1.0f;
			m_AudioSource.spatialBlend = 0.0f;
		}
	}
}