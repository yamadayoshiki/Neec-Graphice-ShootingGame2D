using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using YY.Sound;

public class GameTitleManager : MonoBehaviour
{
	/// <summary>
	/// タイトルイメージコントローラー
	/// </summary>
	[SerializeField]
	private ImageController m_TitleImageCtr = null;

	/// <summary>
	/// オプションイメージコントローラー
	/// </summary>
	[SerializeField]
	private ImageController m_OptionImageCtr = null;

	/// <summary>
	/// SE画像切り替え機能
	/// </summary>
	[SerializeField]
	private SwitchImage m_SeImageSwithcer = null;

	/// <summary>
	/// シーン遷移タイマー
	/// </summary>
	[SerializeField]
	private float m_SceneTransitionTime = 15.0f;

	/// <summary>
	/// デモシューター
	/// </summary>
	[SerializeField]
	private List<DemoShooter> m_DemoShooters = new List<DemoShooter>();

	/// <summary>
	/// タイマー
	/// </summary>
	private float m_Timer = 0.0f;

	/// <summary>
	/// 遷移可能フラグ
	/// </summary>
	private bool m_IsTransition = false;

	//ミュートフラグ
	private bool m_IsMute = false;

	private void Start()
	{
		//フェードコントローラーを初期化
		FadeController.Instance.Initilaize();
		FadeController.Instance.FadeIn(0.5f);

		MyScreen.Initialize();

		m_TitleImageCtr.FadeScale(Vector3.zero, 0.01f);
		m_OptionImageCtr.AlphaFade(0.0f, 0.01f);
		m_IsTransition = false;
		m_Timer = 0.0f;

		Tween title = m_TitleImageCtr.FadeScale(Vector3.one, 2.0f, 2.0f);
		title.OnStart(() => SoundManager.Instance.PlayMenuSE((int)SE.SE14_title));
		Tween option = m_OptionImageCtr.AlphaFade(1.0f, 1.0f, 5.0f);
		option.OnComplete(() =>
		{
			m_IsTransition = true;
			m_DemoShooters.ForEach(e => e.IsActive = true);
		});
	}

	private void Update()
	{
		//SEOnOff切り替え
		if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
		{
			//画像切り替え
			m_SeImageSwithcer.Switch();
		}

		//遷移可能でなければ処理しない
		if (!m_IsTransition) return;

		//シーン遷移
		if (Input.GetKeyDown(KeyCode.Return))
		{
			SoundManager.Instance.PlayMenuSE((int)SE.SE00_Play);
			SceneLoader.Load(SceneHandle.PLAY);
		}
		else if (m_Timer >= m_SceneTransitionTime)
		{
			SceneLoader.Load(SceneHandle.DEMO);
		}

		//タイマー更新
		m_Timer += Time.deltaTime;
	}
}
