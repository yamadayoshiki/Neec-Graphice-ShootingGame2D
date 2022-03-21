using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyType02 : Enemy
{
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
	}

	protected override void Update()
	{
		//移動処理
		Move();

		//活動範囲内で処理を行う
		if (IsActiveArea())
		{
			//攻撃処理
			Attack();
		}

		//左画面奥に移動したら削除する
		if (MyScreen.BottomLeft.x + -100.0f >= m_Transform.position.x + Collider.radius)
		{
			Life.ApplayDamage(Life.MaxHitPoint);
		}
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
		//弾を一定間隔で打つ
		Shooter.Fire(Vector2.left);
	}

	/// <summary>
	/// 死亡処理
	/// </summary>
	protected override void Dead()
	{
		if (!Life.IsDead) return;

		//スコアを加算
		if (GamePlayManager.Instance != null) GamePlayManager.Instance.Score += 25;

		//エフェクトを生成
		CreateDamageEffect(m_ExplosionEffect, m_Transform.position, Vector3.right);

		//自分を削除
		Destroy(gameObject);
	}

	/// <summary>
	/// ダメージを受けた時のリアクション
	/// </summary>
	protected override void DamageReaction()
	{
		//自分にもダメージ時与えて死亡させる
		Life.ApplayDamage(Life.MaxHitPoint);
	}
}

