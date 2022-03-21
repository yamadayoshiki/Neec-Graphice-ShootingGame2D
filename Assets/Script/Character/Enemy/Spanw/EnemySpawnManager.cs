using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
	/// <summary>
	/// アクティブフラグ
	/// </summary>
	[SerializeField]
	private bool m_IsActive = true;
	public bool IsActive
	{
		get { return m_IsActive; }
		set 
		{
			m_IsActive = value;
			m_EnemySpawners.ForEach((spawner) => spawner.IsActive = m_IsActive);
		}
	}

	/// <summary>
	/// Enemy生成データリスト
	/// </summary>
	[SerializeField]
	private List<EnemySpawnData> m_SpawnDataList = new List<EnemySpawnData>();

	/// <summary>
	/// EnemyBoss生成データ
	/// </summary>
	[SerializeField]
	private EnemySpawnData m_BossSpawnData = null;

	/// <summary>
	/// Enemy生成者
	/// </summary>
	[SerializeField]
	private List<EnemySpawner> m_EnemySpawners = new List<EnemySpawner>();

	/// <summary>
	/// 生成カウンター
	/// </summary>
	private int m_SpawnCounter = 0;

	private void Start()
	{
		//初期化
		Initialize();
	}

	/// <summary>
	/// 初期化
	/// </summary>
	public void Initialize()
	{
		//生成者たちを取得
		if(m_EnemySpawners.Count <= 0)
		{
			var spawners = this.transform.GetComponentsInChildren<EnemySpawner>();
			m_EnemySpawners.AddRange(spawners);
		}

		//カウンターの初期化
		m_SpawnCounter = 0;
		//生成データを設定する
		m_EnemySpawners.ForEach((spawner) => spawner.SetEnemySpawnData(m_SpawnDataList[m_SpawnCounter]));
	}

	private void Update()
	{
		//非アクティブなら処理しない
		if (!IsActive)
		{
			IsActive = false;
			return;
		}
		else
		{
			IsActive = true;
		}

		//すべての生成が終了したか調べる
		bool isEnd = true;
		m_EnemySpawners.ForEach((spawner) => isEnd &= spawner.IsSpawnEnd);

		//生成が終了していたら次の生成データを設定する
		if (isEnd)
		{
			//生成カウンターの更新
			m_SpawnCounter++;
			//生成回数が設定されたデータ数以上ならボスを生成して生成処理を停止する
			if (m_SpawnCounter >= m_SpawnDataList.Count)
			{
				//BossEnemyを生成
				Instantiate(m_BossSpawnData.EnemyPrefab, transform.position, Quaternion.identity);
				IsActive = false;
				return;
			}
			//生成データを設定する
			m_EnemySpawners.ForEach((spawner) => spawner.SetEnemySpawnData(m_SpawnDataList[m_SpawnCounter]));
			//初期化
			m_EnemySpawners.ForEach((spawner) => spawner.Initialize());
		}
	}
}

