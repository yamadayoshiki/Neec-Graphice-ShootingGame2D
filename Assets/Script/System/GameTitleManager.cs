using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using YY.Sound;

public class GameTitleManager : MonoBehaviour
{
	/// <summary>
	/// �^�C�g���C���[�W�R���g���[���[
	/// </summary>
	[SerializeField]
	private ImageController m_TitleImageCtr = null;

	/// <summary>
	/// �I�v�V�����C���[�W�R���g���[���[
	/// </summary>
	[SerializeField]
	private ImageController m_OptionImageCtr = null;

	/// <summary>
	/// SE�摜�؂�ւ��@�\
	/// </summary>
	[SerializeField]
	private SwitchImage m_SeImageSwithcer = null;

	/// <summary>
	/// �V�[���J�ڃ^�C�}�[
	/// </summary>
	[SerializeField]
	private float m_SceneTransitionTime = 15.0f;

	/// <summary>
	/// �f���V���[�^�[
	/// </summary>
	[SerializeField]
	private List<DemoShooter> m_DemoShooters = new List<DemoShooter>();

	/// <summary>
	/// �^�C�}�[
	/// </summary>
	private float m_Timer = 0.0f;

	/// <summary>
	/// �J�ډ\�t���O
	/// </summary>
	private bool m_IsTransition = false;

	//�~���[�g�t���O
	private bool m_IsMute = false;

	private void Start()
	{
		//�t�F�[�h�R���g���[���[��������
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
		//SEOnOff�؂�ւ�
		if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
		{
			//�摜�؂�ւ�
			m_SeImageSwithcer.Switch();
		}

		//�J�ډ\�łȂ���Ώ������Ȃ�
		if (!m_IsTransition) return;

		//�V�[���J��
		if (Input.GetKeyDown(KeyCode.Return))
		{
			SoundManager.Instance.PlayMenuSE((int)SE.SE00_Play);
			SceneLoader.Load(SceneHandle.PLAY);
		}
		else if (m_Timer >= m_SceneTransitionTime)
		{
			SceneLoader.Load(SceneHandle.DEMO);
		}

		//�^�C�}�[�X�V
		m_Timer += Time.deltaTime;
	}
}
