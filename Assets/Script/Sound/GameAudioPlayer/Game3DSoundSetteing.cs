using UnityEngine;

namespace YY.Sound
{
	/// <summary>
	/// 3Dサウンドの設定データ
	/// </summary>
	[CreateAssetMenu(fileName = "Game3DSoundSetting", menuName = "Create/Sound/Game3DSoundSetting")]
	public class Game3DSoundSetteing : ScriptableObject
	{
		/// <summary>
		/// 3D立体化の計算の影響度
		/// 0.0なら完全に2D、1.0なら完全に3D
		/// </summary>
		[SerializeField, Header("3D立体化の計算の影響度"), Range(0.0f, 1.0f)]
		private float m_SpatialBlend = 1.0f;
		public float SpatialBlend { get { return m_SpatialBlend; } }

		/// <summary>
		/// ドップラー効果の度合
		/// </summary>
		[SerializeField, Header("ドップラー効果の度合"), Range(0.0f, 5.0f)]
		private float m_DopplerLevel = 1.0f;
		public float DopplerLeveget { get { return m_DopplerLevel; } }

		/// <summary>
		/// スピーカー空間で3Dステレオやマルチチャンネルサウンドに対する広がりの角度
		/// </summary>
		[SerializeField, Header("スピーカー空間で3Dステレオやマルチチャンネルサウンドに対する広がりの角度"), Range(0, 360)]
		private int m_Spread = 0;
		public int Spread { get { return m_Spread; } }

		/// <summary>
		/// 音量が減衰されない最低距離
		/// </summary>
		[SerializeField, Header("音量が減衰されない最低距離")]
		private float m_MinDistance = 1.0f;
		public float MinDistance { get { return m_MinDistance; } }

		/// <summary>
		/// 音が弱まるのを止める距離
		/// </summary>
		[SerializeField, Header("音が弱まるのを止める距離")]
		private float m_MaxDistance = 500.0f;
		public float MaxDistance { get { return m_MaxDistance; } }
	}
}