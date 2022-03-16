using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerController : MonoBehaviour
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
	/// カメラ
	/// </summary>
	[SerializeField]
	private Camera m_Camera = null;

	/// <summary>
	/// 自分のトランスフォーム
	/// </summary>
	private Transform m_Transform = null;

	/// <summary>
	/// 移動速度
	/// </summary>
	[SerializeField]
	private int m_MoveSpeed = 3;
	public int MoveSpeed
	{
		get { return m_MoveSpeed; }
		set { m_MoveSpeed = value * 100; }
	}

	private Vector3 m_InputDirection = Vector2.zero;

	private void Start()
	{
		//コンポーネントを取得
		TryGetComponent(out m_Rigidbody);
		TryGetComponent(out m_Collider);
		if (m_Camera == null) m_Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		m_Transform = this.transform;

		m_Rigidbody.gravityScale = 0.0f;
		m_Collider.isTrigger = true;

		//初期化
		Initlaize();
	}

	/// <summary>
	/// 初期化
	/// </summary>
	public void Initlaize()
	{
		m_InputDirection = Vector2.zero;
	}

	private void Update()
	{
		//入力更新
		InputUpdate();
		//移動処理
		Move();
	}

	/// <summary>
	/// 入力更新
	/// </summary>
	private void InputUpdate()
	{
		//入力方向ベクトルを取得
		m_InputDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0).normalized;
	}

	/// <summary>
	/// 移動処理
	/// </summary>
	private void Move()
	{
		//移動量を計算
		Vector3 velocity = m_InputDirection * m_MoveSpeed;
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
}
