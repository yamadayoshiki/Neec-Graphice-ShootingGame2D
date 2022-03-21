using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YY.Sound;

public class Shooter : MonoBehaviour
{
	/// <summary>
	/// 弾プレハブ
	/// </summary>
	[SerializeField]
	private GameObject m_BulletPrefab = null;

	/// <summary>
	/// 連射間隔
	/// </summary>
	[SerializeField]
	private float m_RapidIntarval = 0.01f;

	/// <summary>
	/// 連射タイマー
	/// </summary>
	private float m_RapidTimer = 0.0f;

	/// <summary>
	/// 初期化
	/// </summary>
	public void Initialize()
	{
		m_RapidTimer = 0.0f;
	}

	/// <summary>
	/// 射撃
	/// </summary>
	public void Fire(Vector2 direction, int seHandle = -1, int damageValue = 1)
	{
		if (m_RapidTimer <= 0.0f)
		{
			//SEを再生
			if(seHandle != -1) SoundManager.Instance.PlayMenuSE(seHandle);
			//弾を生成
			CreateBullet(direction, damageValue);
			//初期化
			ResetTimer();
		}

		//連射タイマーの更新
		m_RapidTimer -= Time.deltaTime;
	}

	/// <summary>
	/// 弾の生成
	/// </summary>
	private void CreateBullet(Vector2 direction, int damegeValue = 1)
	{
		var bullet = Instantiate(m_BulletPrefab, this.transform.position, Quaternion.identity).GetComponent<Bullet>();
		bullet.DamageValue = damegeValue;
		bullet.MoveDirection = direction;
		bullet.Initialize();

	}

	/// <summary>
	/// 射撃タイマーの初期化
	/// </summary>
	private void ResetTimer()
	{
		m_RapidTimer = m_RapidIntarval;
	}
}

