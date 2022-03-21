using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ImageController : MonoBehaviour
{
	/// <summary>
	/// �C���[�W
	/// </summary>
	[SerializeField]
	private Image m_Image = null;

	private void Start()
	{
		transform.localScale = Vector3.zero;
	}

	/// <summary>
	/// �X�P�[���l�̃t�F�[�h
	/// </summary>
	/// <param name="endScale"></param>
	public Tween FadeScale(Vector3 endScale, float fadeTime, float delay = 0.0f)
	{
		return transform.DOScale(endScale, fadeTime).SetDelay(delay).SetEase(Ease.Linear);
	}

	/// <summary>
	/// �摜�̓_�ŏ���
	/// </summary>
	public Tween Flash(float flashSpan, float delay = 0.0f)
	{
		return m_Image.DOFade(0.0f, flashSpan).SetLoops(-1, LoopType.Yoyo).SetDelay(delay);
	}
}
