using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Life))]
public class Enemy : MonoBehaviour
{
	/// <summary>
	/// リジッドボディ
	/// </summary>
	private Rigidbody2D m_Rigidbody = null;
	public Rigidbody2D Rigidbody { get { return m_Rigidbody; } }

	/// <summary>
	/// コライダー
	/// </summary>
	private CircleCollider2D m_Collider = null;
	public CircleCollider2D Collider { get { return m_Collider; } }

	/// <summary>
	/// 耐久値クラス
	/// </summary>
	private Life m_Life = null;
	public Life Life { get { return m_Life; } }

	/// <summary>
	/// カメラ
	/// </summary>
	[SerializeField]
	private Camera m_Camera = null;

	/// <summary>
	/// シューター
	/// </summary>
	[SerializeField]
	private Shooter m_Shooter = null;
	public Shooter Shooter { get { return m_Shooter; } }

	/// <summary>
	/// 自分のトランスフォーム
	/// </summary>
	protected Transform m_Transform = null;

	/// <summary>
	/// 移動速度
	/// </summary>
	[SerializeField]
	private int m_MoveSpeed = 100;
	public int MoveSpeed
	{
		get { return m_MoveSpeed; }
		set { m_MoveSpeed = value * 100; }
	}

	/// <summary>
	/// ダメージ値
	/// </summary>
	[SerializeField]
	private int m_DamageValue = 1;
	public int DamageValue { get { return m_DamageValue; } }

	/// <summary>
	/// 移動方向
	/// </summary>
	[SerializeField]
	protected Vector3 m_MoveDirection = -Vector2.right;

	private void OnEnable()
	{
		//コンポーネントを取得
		TryGetComponent(out m_Rigidbody);
		TryGetComponent(out m_Collider);
		TryGetComponent(out m_Life);
		if (m_Camera == null) m_Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		if (!TryGetComponent(out m_Shooter)) m_Shooter = GetComponentInChildren<Shooter>();

		m_Transform = this.transform;

		m_Rigidbody.gravityScale = 0.0f;
		m_Collider.isTrigger = true;
		m_Life.DamageReaction = this.DamageReaction;

		//初期化
		Initlaize();
	}

	/// <summary>
	/// 初期化
	/// </summary>
	public virtual void Initlaize()
	{
		m_Life.Initialize();
		m_Shooter.Initialize();
	}

	private void Update()
	{
		//移動処理
		Move();
		//死亡処理
		Dead();
	}

	/// <summary>
	/// 移動処理
	/// </summary>
	protected virtual void Move()
	{
		//移動量を計算
		Vector3 velocity = m_MoveDirection * m_MoveSpeed;
		//移動先の座標を保持
		Vector3 nextPos = m_Transform.position + velocity * Time.deltaTime;

		/**** 画面外に出ないように補正処理 ********************************************************/
		//当たり判定の半径を取得
		float radius = m_Collider.radius;
		//Z軸の座標を取得
		float positionZ = nextPos.z;

		//画面右上の座標を取得
		Vector3 topRight = m_Camera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, positionZ));
		//画面左下の座標を取得
		Vector3 bottomLeft = m_Camera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, positionZ));

		//X軸の座標を画面内に補正する
		nextPos.x = Mathf.Clamp(nextPos.x, bottomLeft.x + radius, topRight.x - radius);
		//Y軸の座標を画面内に補正する
		nextPos.y = Mathf.Clamp(nextPos.y, bottomLeft.y + radius, topRight.y - radius);
		/******************************************************************************************/

		//座標を反映
		m_Transform.position = nextPos;
	}

	/// <summary>
	/// 死亡処理
	/// </summary>
	protected virtual void Dead()
	{
		if (!m_Life.IsDead) return;

		//自分を削除
		Destroy(gameObject);
	}

	/// <summary>
	/// ダメージを受けた時のリアクション
	/// </summary>
	protected virtual void DamageReaction()
	{
		Debug.Log("ダメージを受けた");
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
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
}
