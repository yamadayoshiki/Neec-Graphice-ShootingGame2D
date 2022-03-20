﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossEnemy : Enemy
{
	/// <summary>
	/// ボスの状態
	/// </summary>
	public enum BossState
	{
		Appearance, //登場
		Normal,     //通常行動
		Dead        //死亡
	}

	/// <summary>
	/// ボスの現在の状態
	/// </summary>
	private BossState m_State = BossState.Appearance;
	public BossState State { get { return m_State; } }


	/// <summary>
	/// 通常画像
	/// </summary>
	[SerializeField]
	private SpriteRenderer m_Default = null;

	/// <summary>
	/// ダメージ画像
	/// </summary>
	[SerializeField]
	private SpriteRenderer m_Damage = null;

	/// <summary>
	/// 点滅処理
	/// </summary>
	[SerializeField]
	private Flash m_DamageFlash = null;

	/// <summary>
	/// 登場時の移動先
	/// </summary>
	[SerializeField]
	private Transform m_AppearanceMovePoint = null;

	/// <summary>
	/// フェード時間
	/// </summary>
	[SerializeField]
	private float m_FadeTime = 1.0f;

	/// <summary>
	/// 射撃時間
	/// </summary>
	[SerializeField]
	private float m_ShotTime = 1.0f;

	/// <summary>
	/// 射撃インターバル
	/// </summary>
	[SerializeField]
	private float m_ShotInterval = 3.0f;

	/// <summary>
	/// 射撃タイマー
	/// </summary>
	private float m_ShotTimer = 0.0f;

	/// <summary>
	/// インターバルタイマー
	/// </summary>
	private float m_ShotIntrvalTimer = 0.0f;

	/// <summary>
	/// 初期化
	/// </summary>
	public override void Initlaize()
	{
		base.Initlaize();
		//Y軸方向をランダムで決める
		float dirY = Random.Range(-1, 1);
		if (dirY < 0) m_MoveDirection.y = dirY;
		else m_MoveDirection.y = 1.0f;

		m_ShotTimer = 0.0f;
		m_ShotIntrvalTimer = m_ShotInterval / 2;
	}

	protected override void Update()
	{
		switch (m_State)
		{
			case BossState.Appearance:
				Appearance();
				break;
			case BossState.Normal:
				Normal();
				break;
			case BossState.Dead:
				Deaded();
				break;
		}

		//画像切り替え
		SwichImage();
	}

	/// <summary>
	/// 登場時の行動
	/// </summary>
	private void Appearance()
	{
		//移動先のベクトルを求める
		m_MoveDirection = (m_AppearanceMovePoint.position - m_Transform.position).normalized;
		//移動量を計算
		Vector3 velocity = m_MoveDirection * MoveSpeed;
		//移動先の座標を保持
		Vector3 nextPos = m_Transform.position + velocity * Time.deltaTime;
		//座標を反映
		m_Transform.position = nextPos;

		//目的位置との距離を求める
		float distance = Vector3.Distance(m_AppearanceMovePoint.position, m_Transform.position);
		//目的位置との距離が10ピクセル以下なら状態を通常行動にする
		if (distance <= 10f)
		{
			//移動方向を上に設定する
			m_MoveDirection = Vector3.up;
			m_State = BossState.Normal;
		}

	}

	//通常行動
	private void Normal()
	{
		//移動処理
		Move();

		//活動範囲内で処理を行う
		if (IsActiveArea())
		{
			//攻撃処理
			Attack();
		}

		//死亡フラグが立ったら死亡状態にする
		if (Life.IsDead)
		{
			m_State = BossState.Dead;
		}
	}

	/// <summary>
	/// 死亡行動
	/// </summary>
	private void Deaded()
	{
		//点滅処理を停止
		m_DamageFlash.StopFlash();
	}

	/// <summary>
	/// 移動処理
	/// </summary>
	protected override void Move()
	{
		//移動量を計算
		Vector3 velocity = m_MoveDirection * MoveSpeed;
		//移動先の座標を保持
		Vector3 nextPos = m_Transform.position + velocity * Time.deltaTime;

		//画面の上下端まで移動したらY軸移動方向を反転させる
		float posY = Mathf.Abs(nextPos.y) + Collider.radius;
		if (Mathf.Abs(MyScreen.BottomLeft.y) <= posY)
		{
			m_MoveDirection.y *= -1;
		}

		//画面外に出ないように補正
		nextPos = ScreenClampPosition(nextPos);

		//座標を反映
		m_Transform.position = nextPos;
	}

	/// <summary>
	/// 攻撃処理
	/// </summary>
	protected override void Attack()
	{
		if (m_ShotIntrvalTimer >= m_ShotInterval)
		{
			//弾を一定間隔で打つ
			Shooter.Fire(Vector2.left);
			if(m_ShotTimer >= m_ShotTime)
			{
				m_ShotIntrvalTimer = 0.0f;
			}
			m_ShotTimer += Time.deltaTime;
		}
		else if(m_ShotIntrvalTimer <= m_ShotInterval)
		{
			m_ShotTimer = 0.0f;
			m_ShotIntrvalTimer += Time.deltaTime;
		}
	}

	/// <summary>
	/// 死亡処理
	/// </summary>
	protected override void Dead()
	{
		if (!Life.IsDead) return;

		m_Damage.enabled = false;
		m_Default.DOFade(0.0f, m_FadeTime).OnComplete(() => {
			//フェードアウト完了後機能を停止
			Collider.enabled = false;
		});

		//爆発エフェクトを生成
		for (int i = 0; i < 5; i++)
		{
			float randomRange = 75.0f;
			//生成する座標
			Vector3 pos = m_Transform.position + new Vector3(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange), 0);
			//生成時の角度
			float angle = Random.Range(0, 360);
			Quaternion rotate = Quaternion.Euler(0, 0, angle);
			//エフェクトを生成
			CreateDamageEffect(m_ExplosionEffect, pos, Vector3.right);
		}
	}

	/// <summary>
	/// ダメージを受けた時のリアクション
	/// </summary>
	protected override void DamageReaction()
	{
		//点滅処理
		m_DamageFlash.StartFlash();

		//生成時の角度
		float angle = Random.Range(0, 360);
		Quaternion rotate = Quaternion.Euler(0, 0, angle);
		//エフェクトを生成
		CreateDamageEffect(m_HitEffect, m_Transform.position, Vector3.right);
	}

	/// <summary>
	/// ダメージ点滅
	/// </summary>
	private void SwichImage()
	{
		if (m_DamageFlash.IsActive)
		{
			m_Default.enabled = false;
		}
		else
		{
			m_Default.enabled = true;
		}
	}
}

