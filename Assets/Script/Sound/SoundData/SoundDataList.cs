using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YY.Sound
{
	/// <summary>
	/// サウンドデータリストクラス
	/// </summary>
	public class SoundDataList : ScriptableObject
	{
		/// <summary>
		/// サウンドの種類
		/// </summary>
		[SerializeField]
		private SoundType m_SoundType = SoundType.NONE;
		public SoundType SoundType
		{
			get { return m_SoundType; }
			set { m_SoundType = value; }
		}

		/// <summary>
		/// サウンドデータリスト
		/// </summary>
		[SerializeField]
		private List<SoundData> m_SoundDataLists = new List<SoundData>();
		public List<SoundData> GetSoundDataList { get { return m_SoundDataLists; } }

		/// <summary>
		/// サウンドデータの設定
		/// </summary>
		/// <param name="soundData"> 設定するサウンドデータ </param>
		public void SetSoundData(SoundData soundData)
		{
			//サウンドデータリストに追加
			m_SoundDataLists.Add(soundData);
		}

		/// <summary>
		/// サウンドデータリストの削除
		/// </summary>
		public void Clear()
		{
			//リストの中身を消す
			m_SoundDataLists.Clear();
		}

		/// <summary>
		/// オーディオクリップの取得
		/// </summary>
		/// <param name="soundID"> サウンドデータのID </param>
		/// <returns></returns>
		public AudioClip GetAudioClip(int soundID)
		{
			//IDと一致するものを配列から取得
			SoundData soundData = m_SoundDataLists.FirstOrDefault(data => data.ID == soundID);
			//サウンドがなければ処理を行わない
			if (soundData == null)
			{
				//ログにエラーメッセージを出す
				Debug.LogError("サウンドデータがありません。ID:" + soundID);
				return null;
			}
			//オーディオクリップを返す
			return soundData.AudioClip;
		}
	}
}