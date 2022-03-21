using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public enum FadeType
{
	Normal,
	LayerMask
}

public class FadeController : MonoBehaviour
{
	//シングルトン
	public static FadeController Instance = null;

	/// <summary>
	/// フェード処理で使用するイメージ
	/// </summary>
	[SerializeField]
	private Image m_FadeImage = null;

	/// <summary>
	/// 切り抜き画像のトランスフォーム
	/// </summary>
	[SerializeField]
	private RectTransform m_UnMaskTransform = null;

	/// <summary>
	/// 初期化する色
	/// </summary>
	[SerializeField]
	private Color m_InitColor = Color.black;

	/// <summary>
	/// マスクフェードするときの拡大倍率
	/// </summary>
	[SerializeField]
	private float m_MaskFadeEndValue = 500;

	/// <summary>
	/// フェード処理を行っているトゥイーンを保持する
	/// </summary>
	private Tween m_CurrentFade = null;

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(this.gameObject);
		}

		Initilaize();
	}

	/// <summary>
	/// 初期化
	/// </summary>
	public void Initilaize()
	{
		m_FadeImage.color = m_InitColor;
		m_UnMaskTransform.localScale = Vector3.zero;
	}

	/// <summary>
	/// フェードイン処理
	/// </summary>
	/// <param name="duration"> フェードする時間 </param>
	/// <param name="compCallback"> フェード完了時に呼ぶ関数 </param>
	/// <param name="delayTime"> 遅延時間 </param>
	/// <param name="ease"> イージング </param>
	public void FadeIn(float duration, UnityAction compCallback = null, float delayTime = 0.0f, Ease ease = Ease.InQuint)
	{
		Fade(0.0f, duration, delayTime, ease, compCallback);
	}

	/// <summary>
	/// フェードアウト処理
	/// </summary>
	/// <param name="duration"> フェードする時間 </param>
	/// <param name="compCallback"> フェード完了時に呼ぶ関数 </param>
	/// <param name="delayTime"> 遅延時間 </param>
	/// <param name="ease"> イージング </param>
	public void FadeOut(float duration, UnityAction compCallback = null, float delayTime = 0.0f, Ease ease = Ease.OutQuint)
	{
		Fade(1.0f, duration, delayTime, ease, compCallback);
	}

	/// <summary>
	/// フェード処理
	/// </summary>
	/// <param name="endVlue"> フェードするゴールの値 </param>
	/// <param name="duration"> フェードする時間 </param>
	/// <param name="delayTime"> 遅延時間 </param>
	/// <param name="ease"> イージング </param>
	/// <param name="compCallback"> フェード完了時に呼ぶ関数 </param>
	public void Fade(float endVlue, float duration, float delayTime, Ease ease = Ease.InQuint, UnityAction compCallback = null)
	{
		var tween = m_FadeImage.DOFade(endVlue, duration)
			.SetDelay(delayTime)
			.SetEase(ease)
			.OnComplete(() =>
			{
				//再生完了コールバックを実行
				if (compCallback != null)
				{
					compCallback();
				}
			});

		SetFadeTweeen(tween);
	}

	/// <summary>
	/// マスクを使ったフェードイン処理(遅延処理あり)
	/// </summary>
	/// <param name="duration"> フェードする時間 </param>
	/// <param name="compCallback"> フェード完了時に呼ぶ関数 </param>
	/// <param name="delayTime"> 遅延時間 </param>
	/// <param name="ease"> イージング </param>
	public void FadeInMask(float duration, UnityAction compCallback = null, float delayTime = 0.0f, Ease ease = Ease.InQuint)
	{
		FadeMask(m_MaskFadeEndValue, duration, delayTime, ease, compCallback);
	}

	/// <summary>
	/// マスクを使ったフェードアウト処理
	/// </summary>
	/// <param name="duration"> フェードする時間 </param>
	/// <param name="compCallback"> フェード完了時に呼ぶ関数 </param>
	/// <param name="delayTime"> 遅延時間 </param>
	/// <param name="ease"> イージング </param>
	public void FadeOutMask(float duration, UnityAction compCallback = null, float delayTime = 0.0f, Ease ease = Ease.OutQuint)
	{
		FadeMask(0, duration, delayTime, ease, compCallback);
	}

	/// <summary>
	/// マスクを使ったフェード処理
	/// </summary>
	/// <param name="endVlue"> フェードするゴールの値 </param>
	/// <param name="duration"> フェードする時間 </param>
	/// <param name="delayTime"> 遅延時間 </param>
	/// <param name="ease"> イージング </param>
	/// <param name="compCallback"> フェード完了時に呼ぶ関数 </param>
	public void FadeMask(float endVlue, float duration, float delayTime, Ease ease = Ease.InQuint, UnityAction compCallback = null)
	{
		var value = Vector3.one * endVlue;
		var tween = m_UnMaskTransform.DOScale(value, duration)
			.SetDelay(delayTime)
			.SetEase(ease)
			.OnComplete(() =>
			{
				//再生完了コールバックを実行
				if (compCallback != null)
				{
					compCallback();
				}
			}
		);

		SetFadeTweeen(tween);
	}

	/// <summary>
	/// 一時停止
	/// </summary>
	public void Pause()
	{
		m_CurrentFade.Pause();
	}

	/// <summary>
	/// 一時停止解除
	/// </summary>
	public void UnPause()
	{
		m_CurrentFade.Play();
	}

	/// <summary>
	/// 処理中のフェードを即完了させる
	/// </summary>
	public void FadeComplite()
	{
		m_CurrentFade.Complete();
	}

	/// <summary>
	/// フェードのトゥイーンを設定
	/// </summary>
	/// <param name="tween"></param>
	private void SetFadeTweeen(Tween tween)
	{
		m_CurrentFade.Kill();
		m_CurrentFade = tween;
	}
}
