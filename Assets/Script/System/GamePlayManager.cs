using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using YY.Sound;

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
	/// �t�F�[�h����
	/// </summary>
	[SerializeField]
	private float m_FadeTime = 2.0f;

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
	/// �V�[���J�ڃ^�C�}�[
	/// </summary>
	[SerializeField]
	private float m_SceneTransitionTime = 8.0f;

	/// <summary>
	/// �v���C���[
	/// </summary>
	[SerializeField]
	public PlayerController Player { get; set; }

	/// <summary>
	/// �{�X
	/// </summary>
	[SerializeField]
	public Enemy BossEnemy { get; set; }

	/// <summary>
	/// �^�C�}�[
	/// </summary>
	private float m_Timer = 0.0f;

	private void Start()
	{
		//������
		Initialize();

		//�t�F�[�h�R���g���[���[��������
		FadeController.Instance.Initilaize();
		FadeController.Instance.FadeIn(1.0f);
	}

	/// <summary>
	/// ������
	/// </summary>
	public void Initialize()
	{
		Score = 0;
		m_Timer = 0.0f;
		IsGameEnd = false;
	}

	private void Update()
	{
		//�f�o�b�O�R�}���h
		DebugCommand();

		if (IsGameEnd)
		{
			//���Ԃ��o������܂���Enter�L�[�������ꂽ��^�C�g���V�[���ɑJ��
			if (m_Timer >= m_SceneTransitionTime || Input.GetKeyDown(KeyCode.Return))
			{
				SceneLoader.Load(SceneHandle.TITLE);
			}
			m_Timer += Time.deltaTime;
		}
	}

	/// <summary>
	/// �Q�[���I������
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
	/// �Q�[���N���A����
	/// </summary>
	private void Clear()
	{
		Tween tween = m_ClaerImageCtr.FadeScale(Vector3.one, 1.0f, 2.0f);
		tween.OnStart(() => SoundManager.Instance.PlayMenuSE((int)SE.SE17_clear));
		m_ClaerImageCtr.Flash(1.0f, 3.0f);
	}

	/// <summary>
	/// �Q�[���I�[�o�[����
	/// </summary>
	private void Over()
	{
		//�t�F�[�h�A�E�g����
		FadeController.Instance.FadeOut(m_FadeTime);
		Tween tween = m_OverImageCtr.FadeScale(Vector3.one, 1.0f, 2.0f);
		tween.OnStart(() => SoundManager.Instance.PlayMenuSE((int)SE.SE16_over));
	}

	/// <summary>
	/// �f�o�b�O�R�}���h
	/// </summary>
	private void DebugCommand()
	{
		//�Q�[���I�[�o�[�ɂ���
		if(Input.GetKeyDown(KeyCode.O) && Player != null)
		{
			if(Player.TryGetComponent(out Life life)){
				life.ApplayDamage(life.MaxHitPoint);
			}
		}
		//�Q�[���N���A�ɂ���
		else if (Input.GetKeyDown(KeyCode.C))
		{
			GameEnd(true, GameEndType.Claer);
		}
	}
}
