using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
	/// 耐久値クラス
	/// </summary>
	private Life m_Life = null;
	public Life Life { get { return m_Life; } }

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
	/// カメラ
	/// </summary>
	[SerializeField]
	private Camera m_Camera = null;

	/// <summary>
	/// シューター
	/// </summary>
	[SerializeField]
	private Shooter m_Shooter = null;

	/// <summary>
	/// 点滅処理
	/// </summary>
	[SerializeField]
	private Flash m_DamageFlash = null;

	/// <summary>
	/// ダメージエフェクト
	/// </summary>
	[SerializeField]
	private GameObject m_HitEffect = null;

	/// <summary>
	/// 爆発エフェクト
	/// </summary>
	[SerializeField]
	private GameObject m_ExplosionEffect = null;

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

	/// <summary>
	/// フェード時間
	/// </summary>
	[SerializeField]
	private float m_FadeTime = 1.0f;

	private Vector3 m_InputDirection = Vector2.zero;

	private void Start()
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
		m_Life.DeadReaction = this.Dead;

		//初期化
		Initlaize();
	}

	/// <summary>
	/// 初期化
	/// </summary>
	public void Initlaize()
	{
		m_Life.Initialize();
		m_Shooter.Initialize();
		m_InputDirection = Vector2.zero;
	}

	private void Update()
	{
		//入力更新
		InputUpdate();
		//移動処理
		Move();
		//攻撃処理
		if (Input.GetButton("Fire1"))
		{
			m_Shooter.Fire(Vector2.right);
		}
		else
		{
			m_Shooter.Initialize();
		}

		//画像切り替え
		SwichImage();
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

	/// <summary>
	/// 死亡処理
	/// </summary>
	private void Dead()
	{
		//死亡フラグが立っていなければ処理しない
		if (!m_Life.IsDead) return;

		m_Damage.enabled = false;
		m_Default.DOFade(0.0f, m_FadeTime).OnComplete(() => {
			//フェードアウト完了後自分を削除
			Destroy(gameObject);
		});

		//爆発エフェクトを生成
		for(int i = 0;i < 3; i++)
		{
			//生成する座標
			Vector3 pos = m_Transform.position + new Vector3(Random.Range(-50.0f, 50.0f), Random.Range(-50.0f, 50.0f), 0);
			//生成時の角度
			float angle = Random.Range(0, 360);
			Quaternion rotate = Quaternion.Euler(0, 0, angle);
			//エフェクトを生成
			CreateDamageEffect(m_ExplosionEffect, pos, Vector3.left);
		}

	}

	/// <summary>
	/// ダメージを受けた時のリアクション
	/// </summary>
	private void DamageReaction()
	{
		Debug.Log("ダメージを受けた");
		m_DamageFlash.StartFlash();
	}

	/// <summary>
	/// ダメージ点滅
	/// </summary>
	private void SwichImage()
	{
		if (m_DamageFlash.IsActive)
		{
			m_Default.enabled = false;
			m_Damage.enabled = true;
		}
		else
		{
			m_Default.enabled = true;
			m_Damage.enabled = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//衝突位置を取得
		var hitPoint = collision.ClosestPoint(m_Transform.position);
		//エフェクトを生成
		CreateDamageEffect(m_HitEffect, hitPoint, Vector3.left);
	}

	/// <summary>
	///	ダメージエフェクトの生成
	/// </summary>
	/// <param name="effectPrefb"> 生成するエフェクトオブジェクト </param>
	/// <param name="position"> 生成座標 </param>
	/// <param name="moveDir"> 移動方向 </param>
	private void CreateDamageEffect(GameObject effectPrefb,Vector3 position,Vector3 moveDir)
	{
		//生成時の角度
		float angle = Random.Range(0, 360);
		Quaternion rotate = Quaternion.Euler(0, 0, angle);
		//エフェクトを生成
		var effect = Instantiate(effectPrefb, position, rotate).GetComponent<DamageEffect>();
		//エフェクトの移動方向を設定
		effect.MoveDirection = moveDir;
	}
}
