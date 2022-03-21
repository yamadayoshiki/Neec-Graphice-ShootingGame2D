using System.Collections.Generic;
using UnityEngine;

namespace YY.Sound
{
	/// <summary>
	/// ゲームSE再生プレイヤークラス
	/// </summary>
	public class GameSeAudioPlayer : GameAudioPlayerBase
	{
		/// <summary>
		/// ゲームSEの情報クラス
		/// </summary>
		public class GameSeInfo
		{
			//オーディオデータ
			public AudioClip AudioClip;
			//オーディオの名前
			public string AudioName = "";
			//再生済みかのフラグ　true:再生済み / false:未再生
			public bool IsDone = false;
			//再生候補になってからの経過フレーム数
			public int FrameCount;

			//コンストラクタ
			public GameSeInfo(AudioClip audioClip)
			{
				AudioClip = audioClip;
				AudioName = audioClip.name;
				FrameCount = 0;
				IsDone = false;
			}
		}

		/// <summary>
		/// 同じ音が同時再生数以上のリクエストされた時に再生を遅延させるフレーム数
		/// </summary>
		[SerializeField, Range(1, 10)]
		private int m_DelayFrameCount = 2;

		/// <summary>
		/// SE再生予定キューに登録できる最大要素数
		/// </summary>
		[SerializeField, Range(1, 10)]
		private int m_MaxQueuItemCount = 4;

		/// <summary>
		/// SEの再生テーブル
		/// </summary>
		private readonly Dictionary<string, Queue<GameSeInfo>> m_PlaySeTable = new Dictionary<string, Queue<GameSeInfo>>();

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
			m_AudioSource.outputAudioMixerGroup = SoundManager.Instance.GetAudioMixerManager().GameSeGroup;
		}

		private void Update()
		{
			foreach (var q in m_PlaySeTable.Values)
			{
				//要素がなければコンティニュー
				if (q.Count == 0) continue;

				while (true)
				{
					//要素がなければループから抜ける
					if (q.Count == 0) break;

					//再生済みなら要素から取り出して削除
					if (q.Peek().IsDone)
					{
						var _ = q.Dequeue();
					}
					else { break; }
				}

				//要素がなければコンティニュー
				if (q.Count == 0) continue;

				//未再生でキューの先頭の1件に対して
				var info = q.Peek();
				info.FrameCount++;
				//時間が経過していたら再生して削除
				if (info.FrameCount > m_DelayFrameCount)
				{
					//一時停止中は再生しない
					if (IsPaused) return;
					//再生
					m_AudioSource.PlayOneShot(info.AudioClip);
					var _ = q.Dequeue();
				}
			}
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

			//情報体を生成
			var info = new GameSeInfo(audioClip);

			//登録されているか
			if (!m_PlaySeTable.ContainsKey(info.AudioName))
			{
				m_AudioSource.PlayOneShot(audioClip);
				info.IsDone = true;//再生済みにする

				//情報体を登録
				var q = new Queue<GameSeInfo>();
				q.Enqueue(info);
				m_PlaySeTable[info.AudioName] = q;
			}
			else
			{
				var infoList = m_PlaySeTable[info.AudioName];
				//再生予定キュー登録数を超えてないか
				if (infoList.Count <= m_MaxQueuItemCount)
				{
					m_PlaySeTable[info.AudioName].Enqueue(info);
				}
				else
				{
					Debug.LogError("効果音の最大登録数を超えています。" + info.AudioName);
				}
			}
		}

		/// <summary>
		/// サウンドの停止
		/// </summary>
		public override void StopSound()
		{
			m_AudioSource.Stop();
		}

		/// <summary>
		/// ゲームSEの再生
		/// AudioSource PlayOneShotを使用
		/// </summary>
		public void PlayGameSe(int soundID)
		{
			//一時停止中は再生しない
			if (IsPaused) return;

			//オーディオクリップを取得
			AudioClip audioClip = SoundManager.Instance.GetSEAudioClip(soundID);

			//nullチェック
			if (audioClip == null)
			{
				Debug.LogError("サウンドデータがありません。SoundId:" + soundID);
				return;
			}

			//情報体を生成
			var info = new GameSeInfo(audioClip);

			//登録されているか
			if (!m_PlaySeTable.ContainsKey(info.AudioName))
			{
				m_AudioSource.PlayOneShot(audioClip);
				info.IsDone = true;//再生済みにする

				//情報体を登録
				var q = new Queue<GameSeInfo>();
				q.Enqueue(info);
				m_PlaySeTable[info.AudioName] = q;
			}
			else
			{
				var infoList = m_PlaySeTable[info.AudioName];
				//再生予定キュー登録数を超えてないか
				if (infoList.Count <= m_MaxQueuItemCount)
				{
					m_PlaySeTable[info.AudioName].Enqueue(info);
				}
				else
				{
					Debug.LogError("効果音の最大登録数を超えています。" + info.AudioName);
				}
			}
		}

		/// <summary>
		/// ゲームSEの再生
		/// </summary>
		/// <param name="audioClip"></param>
		public void PlayGameSe(AudioClip audioClip)
		{
			//一時停止中は再生しない
			if (IsPaused) return;

			m_AudioSource.PlayOneShot(audioClip);
		}

		/// <summary>
		/// オーディオソースの初期化
		/// </summary>
		protected override void InitializeAudioSource()
		{
			base.InitializeAudioSource();
			if (m_3DSoundSetting == null) return;
			//3Dサウンドの比率にする
			m_AudioSource.spatialBlend = 1.0f;
			//3Dサウンドの設定
			m_AudioSource.dopplerLevel = m_3DSoundSetting.DopplerLeveget;
			m_AudioSource.spread = m_3DSoundSetting.Spread;
			m_AudioSource.minDistance = m_3DSoundSetting.MinDistance;
			m_AudioSource.maxDistance = m_3DSoundSetting.MaxDistance;
		}
	}
}