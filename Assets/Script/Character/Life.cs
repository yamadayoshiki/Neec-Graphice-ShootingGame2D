using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
	/// <summary>
	/// 最大耐久値
	/// </summary>
	[SerializeField]
	private int m_MaxHitPoint = 1;
	public int MaxHitPoint
	{
		get { return m_MaxHitPoint; }
		set { m_MaxHitPoint = Math.Max(value, 0); }
	}

	/// <summary>
	/// 現在の耐久値
	/// </summary>
	[SerializeField]
	private int m_HitPoint = 0;
	public int HitPoint
	{
		get { return m_HitPoint; }
		set { m_HitPoint = Mathf.Clamp(value, 0, 100); }
	}

	/// <summary>
	/// 耐久値の割合
	/// </summary>
	public float LifeRate
	{
		get
		{
			return (float)m_HitPoint / (float)m_MaxHitPoint;
		}
	}

	/// <summary>
	/// 無敵時間をしようするか
	/// </summary>
	[SerializeField]
	private bool m_IsUseInvincible = true;

	/// <summary>
	/// 無敵フラグ
	/// </summary>
	[SerializeField]
	private bool m_IsInvincible = false;
	public bool IsInvicible 
	{
		get { return m_IsInvincible; }
		set { m_IsInvincible = value; }
	}

	/// <summary>
	/// 無敵時間
	/// </summary>
	[SerializeField]
	private float m_InvincibleTime = 0.3f;

	/// <summary>
	/// 無敵待機時間
	/// </summary>
	private WaitForSeconds m_InvincibleTimeWait = null;

	/// <summary>
	/// 死亡しているか
	/// </summary>
	public bool IsDead { get { return HitPoint <= 0; } }

	/// <summary>
	/// ダメージを受けた時のリアクション
	/// </summary>
	public UnityAction DamageReaction = null;

	/// <summary>
	/// ダメージを受けた時のリアクション
	/// </summary>
	public UnityAction DeadReaction = null;

	private void Start()
	{
		//初期化
		Initialize();
		m_InvincibleTimeWait = new WaitForSeconds(m_InvincibleTime);
	}

	/// <summary>
	/// 初期化
	/// </summary>
	public void Initialize()
	{
		HitPoint = MaxHitPoint;
	}

	/// <summary>
	/// ダメージを適用する
	/// </summary>
	/// <param name="damage"></param>
	public void ApplayDamage(int damage)
	{
		//無敵状態又は死亡しているなら処理しない
		if (IsInvicible || IsDead) return;

		//耐久値からダメージ値を引く
		HitPoint -= damage;

		//死亡時のリアクションを行う
		if (IsDead && DeadReaction != null) DeadReaction();
		//ダメージを受けた時のリアクションを行う
		else if (DamageReaction != null) DamageReaction();

		if (m_IsUseInvincible)
		{	//無敵処理を行う
			StartCoroutine(nameof(Invincible));
		}
	}

	/// <summary>
	/// 無敵処理
	/// </summary>
	/// <returns></returns>
	private IEnumerator Invincible()
	{
		IsInvicible = true;

		yield return m_InvincibleTimeWait;

		IsInvicible = false;
	}
}

