using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnData",menuName = "Create/Data/EnemySpawnData")]
public class EnemySpawnData : ScriptableObject
{
	/// <summary>
	/// ��������Enemy�I�u�W�F�N�g
	/// </summary>
	[SerializeField]
	private GameObject m_EnemyPrefab = null;
	public GameObject EnemyPrefab { get { return m_EnemyPrefab; } }

	/// <summary>
	/// �������s���ő吔
	/// </summary>
	[SerializeField]
	private int m_MaxSpawn = 3;
	public int MaxSpawn
	{
		get { return m_MaxSpawn; }
		set { m_MaxSpawn = value; }
	}

	/// <summary>
	/// �����C���^�[�o��
	/// </summary>
	[SerializeField]
	private float m_SpawnInterval = 3.0f;
	public float SpawnInterval
	{
		get { return m_SpawnInterval; }
		set { m_SpawnInterval = value; }
	}
}
