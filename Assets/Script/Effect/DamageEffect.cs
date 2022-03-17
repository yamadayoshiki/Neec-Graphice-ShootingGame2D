using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class DamageEffect : MonoBehaviour
{
	/// <summary>
	/// �X�v���C�g
	/// </summary>
	[SerializeField]
	private SpriteRenderer m_Sprite = null;

	/// <summary>
	/// �ړ�����
	/// </summary>
	[SerializeField]
	private Vector3 m_MoveDirection = Vector3.zero;
	private Vector3 MoveDirection
	{
		get { return m_MoveDirection; }
		set { m_MoveDirection = value; }
	}

	/// <summary>
	/// �ړ����x
	/// </summary>
	[SerializeField]
	private float m_MoveSpeed = 50.0f;



	void Start()
	{
		//�R���|�[�l���g���擾
		TryGetComponent(out m_Sprite);
		//�X�P�[���l��0�ɂ���
		transform.localScale = Vector3.zero;
		var seq = DOTween.Sequence();

		//�g��̃A�j���[�V����
		seq.Append(transform.DOScale(Vector2.one * 3.0f, 0.5f));
		//�t�F�[�h�A�E�g�̃A�j���[�V����
		seq.Insert(0.2f, m_Sprite.DOFade(0.0f, 0.5f));
		seq.OnComplete(() => {
			Destroy(gameObject);
		});
	}

	void Update()
	{
		//�ړ�����
		transform.position += m_MoveDirection.normalized * m_MoveSpeed * Time.deltaTime;
	}
}
