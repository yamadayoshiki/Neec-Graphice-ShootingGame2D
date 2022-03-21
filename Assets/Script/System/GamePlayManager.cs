using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
	/// 初期化
	/// </summary>
	public void Initialize()
	{
		Score = 0;
		IsGameEnd = false;
	}

	/// <summary>
	/// ゲーム終了処理
	/// </summary>
	/// <param name="isEnd"></param>
	/// <param name="endType"></param>
	public void GameEnd(bool isEnd, GameEndType endType)
	{
		if (isEnd == false) return;
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
		m_ClaerImageCtr.FadeScale(Vector3.one, 1.0f, 2.0f);
		m_ClaerImageCtr.Flash(1.0f, 3.0f);
	}

	/// <summary>
	/// ゲームオーバー処理
	/// </summary>
	private void Over()
	{
		m_OverImageCtr.FadeScale(Vector3.one, 1.0f, 2.0f);
	}
}
