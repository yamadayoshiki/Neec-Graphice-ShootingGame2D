using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Bullet : MonoBehaviour
{
	/// <summary>
	/// リジッドボディ
	/// </summary>
	private Rigidbody2D m_Rigidbody = null;

	/// <summary>
	/// コライダー
	/// </summary>
	private CircleCollider2D m_Collider = null;

	/// <summary>
	/// 移動方向
	/// </summary>
	public Vector2 MoveDirection { get; set; }

	/// <summary>
	/// ダメージ値
	/// </summary>
	public int DamageValue { get; set; }

	/// <summary>
	/// 移動速度
	/// </summary>
	[SerializeField]
	private float m_MoveSpeed = 600.0f;

	/// <summary>
	/// 自分のトランスフォーム
	/// </summary>
	private Transform m_Transform = null;

	private void OnEnable()
	{
		//コンポーネントを取得
		TryGetComponent(out m_Rigidbody);
		TryGetComponent(out m_Collider);
		m_Transform = this.transform;

		m_Rigidbody.gravityScale = 0.0f;
		m_Collider.isTrigger = true;
		MoveDirection = Vector2.right; //初期値。右方向
	}

	/// <summary>
	/// 初期化
	/// </summary>
	public void Initialize()
	{
		//回転を計算して適用する
		CalucRotation();
	}

	private void Update()
	{
		//移動量を求める
		Vector3 velocity = MoveDirection.normalized * m_MoveSpeed;
		//現在の座標に移動量を足す
		m_Transform.position += velocity * Time.deltaTime;
	}

	/// <summary>
	/// 回転を計算する
	/// </summary>
	private void CalucRotation()
	{
		float angle = Vector2.Angle(MoveDirection, Vector2.right);
		Quaternion rotate = Quaternion.Euler(0, 0, angle);
		m_Transform.rotation = rotate;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//相手の耐久値クラスを取得してダメージを適用する
		if (collision.gameObject.TryGetComponent(out Life life))
		{
			life.ApplayDamage(DamageValue);
		}
		//自分を削除
		Destroy(gameObject);
	}
}
