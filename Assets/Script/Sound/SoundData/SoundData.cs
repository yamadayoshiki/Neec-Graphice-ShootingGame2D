using UnityEngine;

namespace YY.Sound
{
	/// <summary>
	/// サウンドデータ
	/// </summary>
	[System.Serializable]
	public class SoundData
	{
		/// <summary>
		/// ID
		/// </summary>
		[SerializeField]
		private int m_ID = 0;
		public int ID { get { return m_ID; } }

		/// <summary>
		/// オーディオデータ
		/// </summary>
		[SerializeField]
		private AudioClip m_AudioClip = null;
		public AudioClip AudioClip { get { return m_AudioClip; } }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="id"> 識別ID </param>
		/// <param name="audioClip"> オーディオデータ </param>
		public SoundData(int id, AudioClip audioClip)
		{
			m_ID = id;
			m_AudioClip = audioClip;
		}
	}
}