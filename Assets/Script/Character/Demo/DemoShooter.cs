using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shooter))]
public class DemoShooter : MonoBehaviour
{
	/// <summary>
	/// シュータークラス
	/// </summary>
	[SerializeField]
	private Shooter m_Shooter = null;

	/// <summary>
	/// 撃つ方向
	/// </summary>
	[SerializeField]
	private Vector3 m_ShotDirection = Vector3.right;

	/// <summary>
	/// 射撃音
	/// </summary>
	[SerializeField]
	private SE m_ShotSe = SE.SE07_shot;

	/// <summary>
	/// 移動方向
	/// </summary>
	[SerializeField]
	private Vector3 m_MoveDirection = Vector3.up;

	/// <summary>
	/// 移動速度
	/// </summary>
	[SerializeField]
	private float m_MoveSpeed = 2.0f;

	/// <summary>
	/// 半径
	/// </summary>
	[SerializeField]
	private float m_Radius = 32.0f;

	[SerializeField]
	private bool m_IsActive = true;
	public bool IsActive
	{
		get { return m_IsActive; }
		set { m_IsActive = value; }
	}

	//自分のトランスフォーム
	private Transform m_Transform = null;

	// Start is called before the first frame update
	void Start()
	{
		//コンポーネントを取得
		TryGetComponent(out m_Shooter);

		m_Transform = this.transform;
		//非アクティブにする
		IsActive = false;
	}

	// Update is called once per frame
	void Update()
	{
		//非アクティブなら処理しない
		if (!IsActive) return;

		//射撃
		m_Shooter.Fire(m_ShotDirection,(int)m_ShotSe);

		//移動処理
		Move();
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

		//画面の上下端まで移動したらY軸移動方向を反転させる
		float posY = Mathf.Abs(nextPos.y) + m_Radius;
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
	/// 画面外に出ないように補正処理
	/// </summary>
	/// <param name="position"></param>
	/// <returns></returns>
	protected Vector3 ScreenClampPosition(Vector3 position)
	{
		//移動先の座標
		Vector3 nextPos = position;

		/**** 画面外に出ないように補正処理 ********************************************************/

		//画面右上の座標を取得
		Vector3 topRight = MyScreen.TopRight;
		//画面左下の座標を取得
		Vector3 bottomLeft = MyScreen.BottomLeft;

		//Y軸の座標を画面内に補正する
		nextPos.y = Mathf.Clamp(position.y, bottomLeft.y + m_Radius, topRight.y - m_Radius);
		/******************************************************************************************/

		//座標を返す
		return nextPos;
	}
}
