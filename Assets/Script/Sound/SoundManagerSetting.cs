using UnityEngine;

namespace YY.Sound
{
	/// <summary>
	/// サウンドマネージャーの設定データクラス
	/// </summary>
	[CreateAssetMenu(fileName = "SoundManagerSetting", menuName = "Create/Sound/SoundManagerSetting")]
	public class SoundManagerSetting : ScriptableObject
	{
		[SerializeField, Header("BGMの再生可能数")]
		private int m_BgmAudioPlayerNum = 2;
		public int BgmAudioPlayerNum
		{
			get { return m_BgmAudioPlayerNum; }
		}

		[SerializeField, Header("MenuSEの再生可能数")]
		private int m_MenuSEAudioPlayerNum = 3;
		public int MenuSEAudioPlayerNum
		{
			get { return m_MenuSEAudioPlayerNum; }
		}

		[SerializeField, Header("サウンドデータの読み込み先ファイルパス")]
		private string m_SoundDataFilePth = "Data/Sound/";
		public string SoundDataFilePath { get { return m_SoundDataFilePth; } }

	}
}