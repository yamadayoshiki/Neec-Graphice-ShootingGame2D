using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using YY.Sound;

[RequireComponent(typeof(SpriteRenderer))]
public class DamageEffect : MonoBehaviour
{
	/// <summary>
	/// スプライト
	/// </summary>
	[SerializeField]
	private SpriteRenderer m_Sprite = null;

	/// <summary>
	/// 移動方向
	/// </summary>
	[SerializeField]
	private Vector3 m_MoveDirection = Vector3.zero;
	public Vector3 MoveDirection
	{
		get { return m_MoveDirection; }
		set { m_MoveDirection = value; }
	}

	/// <summary>
	/// 再生SEハンドル
	/// </summary>
	[SerializeField]
	private SE m_SeHandle;

	/// <summary>
	/// 移動速度
	/// </summary>
	[SerializeField]
	private float m_MoveSpeed = 50.0f;

	/// <summary>
	/// スケール値
	/// </summary>
	[SerializeField]
	private float m_Scale = 1.0f;


	void Start()
	{
		//コンポーネントを取得
		TryGetComponent(out m_Sprite);
		//スケール値を0にする
		transform.localScale = Vector3.zero;
		var seq = DOTween.Sequence();

		//SEを再生
		SoundManager.Instance.PlayMenuSE((int)m_SeHandle);

		//拡大のアニメーション
		seq.Append(transform.DOScale(Vector2.one * m_Scale, 0.5f));
		//フェードアウトのアニメーション
		seq.Insert(0.2f, m_Sprite.DOFade(0.0f, 0.5f));
		seq.OnComplete(() => {
			Destroy(gameObject);
		});
	}

	void Update()
	{
		//移動処理
		transform.position += m_MoveDirection.normalized * m_MoveSpeed * Time.deltaTime;
	}
}
