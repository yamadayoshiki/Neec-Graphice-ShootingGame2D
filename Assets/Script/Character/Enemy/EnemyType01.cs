using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyType01 : Enemy
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
		//攻撃処理
		Attack();
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

	}

	/// <summary>
	/// 死亡処理
	/// </summary>
	protected override void Dead()
	{
		if (!Life.IsDead) return;

		//自分を削除
		Destroy(gameObject);
	}

	/// <summary>
	/// ダメージを受けた時のリアクション
	/// </summary>
	protected override void DamageReaction()
	{
		Debug.Log("ダメージを受けた");
	}
}

