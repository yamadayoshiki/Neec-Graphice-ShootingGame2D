using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GamePlayManager : SingletonMonoBehaviour<GamePlayManager>
{
	/// <summary>
	/// �Q�[���I���̎��
	/// </summary>
	public enum GameEndType
	{
		Claer,
		Over
	}

	/// <summary>
	/// �X�R�A
	/// </summary>
	public int Score { get; set; }

	/// <summary>
	/// �Q�[���I���t���O
	/// </summary>
	[SerializeField]
	private bool m_IsGameEnd = false;
	public bool IsGameEnd
	{
		get { return m_IsGameEnd; }
		set { m_IsGameEnd = value; }
	}

	/// <summary>
	/// �Q�[���N���A�C���[�W�R���g���[���[
	/// </summary>
	[SerializeField]
	private ImageController m_ClaerImageCtr = null;

	/// <summary>
	/// �Q�[���I�[�o�[�C���[�W�R���g���[���[
	/// </summary>
	[SerializeField]
	private ImageController m_OverImageCtr = null;


	/// <summary>
	/// ������
	/// </summary>
	public void Initialize()
	{
		Score = 0;
		IsGameEnd = false;
	}

	/// <summary>
	/// �Q�[���I������
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
	/// �Q�[���N���A����
	/// </summary>
	private void Clear()
	{
		m_ClaerImageCtr.FadeScale(Vector3.one, 1.0f, 2.0f);
		m_ClaerImageCtr.Flash(1.0f, 3.0f);
	}

	/// <summary>
	/// �Q�[���I�[�o�[����
	/// </summary>
	private void Over()
	{
		m_OverImageCtr.FadeScale(Vector3.one, 1.0f, 2.0f);
	}
}
