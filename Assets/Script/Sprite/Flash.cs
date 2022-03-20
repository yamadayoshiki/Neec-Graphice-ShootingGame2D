using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Flash : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer m_SpriteRenderer = null;

	/// <summary>
	/// �_�ŊԊu
	/// </summary>
	[SerializeField]
	private float m_FlashSpan = 0.2f;

	/// <summary>
	/// �_�ŉ�
	/// </summary>
	[SerializeField]
	private int m_FlashTimes = 3;
	public int FlashTimes { get { return m_FlashTimes; } }

	/// <summary>
	/// �A�N�e�B�u���H
	/// </summary>
	public bool IsActive { get; set; }

	private Tween m_Tween = null;

	private void Start()
	{
		IsActive = false;
		m_SpriteRenderer.DOFade(1.0f, 0.01f);
		m_SpriteRenderer.enabled = false;
	}

	/// <summary>
	/// �_�ł��J�n����
	/// </summary>
	public void StartFlash()
	{
		if (IsActive) return;
		IsActive = true;
		m_SpriteRenderer.enabled = true;
		m_Tween = m_SpriteRenderer.DOFade(0.0f, m_FlashSpan).SetLoops(m_FlashTimes, LoopType.Yoyo).OnComplete(() => {
			m_SpriteRenderer.DOFade(1.0f, 0.01f);
			m_SpriteRenderer.enabled = false;
			IsActive = false;
		});
	}

	/// <summary>
	/// �_�ł��~�߂�
	/// </summary>
	public void StopFlash()
	{
		m_Tween.Kill();
	}
}
