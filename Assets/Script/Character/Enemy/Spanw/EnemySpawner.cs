using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	/// <summary>
	/// Enemy生成データ
	/// </summary>
	[SerializeField]
	private EnemySpawnData m_SpawnData = null;
	public void SetEnemySpawnData(EnemySpawnData data)
	{
		m_SpawnData = data;
	}

	/// <summary>
	/// アクティブフラグ
	/// </summary>
	[SerializeField]
	private bool m_IsActive = true;
	public bool IsActive
	{
		get { return m_IsActive; }
		set { m_IsActive = value; }
	}

	/// <summary>
	/// 生成回数
	/// </summary>
	private int m_SpawnCount = 0;

	/// <summary>
	/// 生成タイマー
	/// </summary>
	private float m_SpawnTimer = 0.0f;

	/// <summary>
	/// 生成終了
	/// </summary>
	public bool IsSpawnEnd { get { return m_SpawnCount >= m_SpawnData.MaxSpawn; } }

	/// <summary>
	/// 初期化
	/// </summary>
	public void Initialize()
	{
		m_SpawnCount = 0;
		m_SpawnTimer = 0.0f;
	}

	private void Update()
	{
		//非アクティブなら処理しない
		if (!IsActive) return;
		//生成処理
		SpawnEnemy();
	}

	/// <summary>
	/// Enemyの生成
	/// </summary>
	public void SpawnEnemy()
	{
		//生成が終了していたら処理しない
		if (IsSpawnEnd) return;

		if(m_SpawnTimer >= m_SpawnData.SpawnInterval)
		{
			//生成
			Instantiate(m_SpawnData.EnemyPrefab, transform.position, Quaternion.identity);
			//生成カウントを更新
			m_SpawnCount++;
			//生成タイマーを初期化
			m_SpawnTimer = 0.0f;
		}

		//生成タイマーを更新
		m_SpawnTimer += Time.deltaTime;
	}
}

