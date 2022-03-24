using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using YY.Sound;

public class GamePlayManager : SingletonMonoBehaviour<GamePlayManager>
{
	/// <summary>
	/// ゲーム終了の種類
	/// </summary>
	public enum GameEndType
	{
		Claer,
		Over
	}

	/// <summary>
	/// スコア
	/// </summary>
	public int Score { get; set; }

	/// <summary>
	/// ゲーム終了フラグ
	/// </summary>
	[SerializeField]
	private bool m_IsGameEnd = false;
	public bool IsGameEnd
	{
		get { return m_IsGameEnd; }
		set { m_IsGameEnd = value; }
	}

	/// <summary>
	/// フェード時間
	/// </summary>
	[SerializeField]
	private float m_FadeTime = 2.0f;

	/// <summary>
	/// ゲームクリアイメージコントローラー
	/// </summary>
	[SerializeField]
	private ImageController m_ClaerImageCtr = null;

	/// <summary>
	/// ゲームオーバーイメージコントローラー
	/// </summary>
	[SerializeField]
	private ImageController m_OverImageCtr = null;

	/// <summary>
	/// シーン遷移タイマー
	/// </summary>
	[SerializeField]
	private float m_SceneTransitionTime = 8.0f;

	/// <summary>
	/// プレイヤー
	/// </summary>
	[SerializeField]
	public PlayerController Player { get; set; }

	/// <summary>
	/// ボス
	/// </summary>
	[SerializeField]
	public Enemy BossEnemy { get; set; }

	/// <summary>
	/// タイマー
	/// </summary>
	private float m_Timer = 0.0f;

	private void Start()
	{
		//初期化
		Initialize();

		//フェードコントローラーを初期化
		FadeController.Instance.Initilaize();
		FadeController.Instance.FadeIn(1.0f);
	}

	/// <summary>
	/// 初期化
	/// </summary>
	public void Initialize()
	{
		Score = 0;
		m_Timer = 0.0f;
		IsGameEnd = false;
	}

	private void Update()
	{
		//デバッグコマンド
		DebugCommand();

		if (IsGameEnd)
		{
			//時間が経ったらまたはEnterキーを押されたらタイトルシーンに遷移
			if (m_Timer >= m_SceneTransitionTime || Input.GetKeyDown(KeyCode.Return))
			{
				SceneLoader.Load(SceneHandle.TITLE);
			}
			m_Timer += Time.deltaTime;
		}
	}

	/// <summary>
	/// ゲーム終了処理
	/// </summary>
	/// <param name="isEnd"></param>
	/// <param name="endType"></param>
	public void GameEnd(bool isEnd, GameEndType endType)
	{
		if (isEnd == false || m_IsGameEnd == true) return;
		m_IsGameEnd = isEnd;
		switch (endType)
		{
			case GameEndType.Claer:
				Clear();
				break;
			case GameEndType.Over:
				Over();
				break;
		}
	}

	/// <summary>
	/// ゲームクリア処理
	/// </summary>
	private void Clear()
	{
		Tween tween = m_ClaerImageCtr.FadeScale(Vector3.one, 1.0f, 2.0f);
		tween.OnStart(() => SoundManager.Instance.PlayMenuSE((int)SE.SE17_clear));
		m_ClaerImageCtr.Flash(1.0f, 3.0f);
	}

	/// <summary>
	/// ゲームオーバー処理
	/// </summary>
	private void Over()
	{
		//フェードアウトする
		FadeController.Instance.FadeOut(m_FadeTime);
		Tween tween = m_OverImageCtr.FadeScale(Vector3.one, 1.0f, 2.0f);
		tween.OnStart(() => SoundManager.Instance.PlayMenuSE((int)SE.SE16_over));
	}

	/// <summary>
	/// デバッグコマンド
	/// </summary>
	private void DebugCommand()
	{
		//ゲームオーバーにする
		if(Input.GetKeyDown(KeyCode.O) && Player != null)
		{
			if(Player.TryGetComponent(out Life life)){
				life.ApplayDamage(life.MaxHitPoint);
			}
		}
		//ゲームクリアにする
		else if (Input.GetKeyDown(KeyCode.C))
		{
			GameEnd(true, GameEndType.Claer);
		}
	}
}
