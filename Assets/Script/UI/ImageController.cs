using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ImageController : MonoBehaviour
{
	/// <summary>
	/// イメージ
	/// </summary>
	[SerializeField]
	private Image m_Image = null;

	private void Start()
	{
		//transform.localScale = Vector3.zero;
	}

	/// <summary>
	/// アルファ値のフェード
	/// </summary>
	/// <param name="endValue"></param>
	/// <param name="fadeTime"></param>
	/// <param name="deley"></param>
	/// <returns></returns>
	public Tween AlphaFade(float endValue, float fadeTime, float deley = 0.0f)
	{
		return m_Image.DOFade(endValue, fadeTime).SetEase(Ease.Linear).SetDelay(deley);
	}

	/// <summary>
	/// スケール値のフェード
	/// </summary>
	/// <param name="endScale"></param>
	public Tween FadeScale(Vector3 endScale, float fadeTime, float delay = 0.0f)
	{
		return transform.DOScale(endScale, fadeTime).SetDelay(delay).SetEase(Ease.Linear);
	}

	/// <summary>
	/// 画像の点滅処理
	/// </summary>
	public Tween Flash(float flashSpan, float delay = 0.0f)
	{
		return m_Image.DOFade(0.0f, flashSpan).SetLoops(-1, LoopType.Yoyo).SetDelay(delay);
	}
}
