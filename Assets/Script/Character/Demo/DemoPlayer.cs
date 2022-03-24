using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DemoPlayer : MonoBehaviour
{
	/// <summary>
	/// ���W�b�h�{�f�B
	/// </summary>
	private Rigidbody2D m_Rigidbody = null;

	/// <summary>
	/// �R���C�_�[
	/// </summary>
	private CircleCollider2D m_Collider = null;

	/// <summary>
	/// �ϋv�l�N���X
	/// </summary>
	private Life m_Life = null;
	public Life Life { get { return m_Life; } }

	/// <summary>
	/// �ʏ�摜
	/// </summary>
	[SerializeField]
	private SpriteRenderer m_Default = null;

	/// <summary>
	/// �_���[�W�摜
	/// </summary>
	[SerializeField]
	private SpriteRenderer m_Damage = null;

	/// <summary>
	/// �V���[�^�[
	/// </summary>
	[SerializeField]
	private Shooter m_Shooter = null;

	/// <summary>
	/// �_�ŏ���
	/// </summary>
	[SerializeField]
	private Flash m_DamageFlash = null;

	/// <summary>
	/// �_���[�W�G�t�F�N�g
	/// </summary>
	[SerializeField]
	private GameObject m_HitEffect = null;

	/// <summary>
	/// �����G�t�F�N�g
	/// </summary>
	[SerializeField]
	private GameObject m_ExplosionEffect = null;

	/// <summary>
	/// �����̃g�����X�t�H�[��
	/// </summary>
	private Transform m_Transform = null;

	/// <summary>
	/// �ړ����x
	/// </summary>
	[SerializeField]
	private int m_MoveSpeed = 3;
	public int MoveSpeed
	{
		get { return m_MoveSpeed; }
		set { m_MoveSpeed = value * 100; }
	}

	/// <summary>
	/// �t�F�[�h����
	/// </summary>
	[SerializeField]
	private float m_FadeTime = 1.0f;

	/// <summary>
	/// �ړ������x�N�g��
	/// </summary>
	private Vector3 m_MoveDirection = Vector3.up;

	private void Start()
	{
		MyScreen.Initialize();

		//�R���|�[�l���g���擾
		TryGetComponent(out m_Rigidbody);
		TryGetComponent(out m_Collider);
		TryGetComponent(out m_Life);
		if (!TryGetComponent(out m_Shooter)) m_Shooter = GetComponentInChildren<Shooter>();
		m_Transform = this.transform;

		m_Rigidbody.gravityScale = 0.0f;
		m_Collider.isTrigger = true;
		//�_���[�W���Ǝ��S��������o�^
		m_Life.DamageReaction = this.DamageReaction;
		m_Life.DeadReaction = this.Dead;

		//�ϋv�l�Q�[�W�Ɏ����̑ϋv�l�N���X��n��
		var uiObj = GameObject.Find("PlayerGauge");
		if (uiObj != null)
		{
			if (uiObj.TryGetComponent(out LifeGauge lifeGauge))
			{
				lifeGauge.Life = m_Life;
			}
		}

		//������
		Initlaize();
	}

	/// <summary>
	/// ������
	/// </summary>
	public void Initlaize()
	{
		m_Life.Initialize();
		m_Shooter.Initialize();
	}

	private void Update()
	{
		//�ړ�����
		Move();
		//�U������
		m_Shooter.Fire(Vector2.right, (int)SE.SE07_shot);

		//�摜�؂�ւ�
		SwichImage();
	}

	/// <summary>
	/// �ړ�����
	/// </summary>
	private void Move()
	{
		//�ړ��ʂ��v�Z
		Vector3 velocity = m_MoveDirection * m_MoveSpeed;
		//�ړ���̍��W��ێ�
		Vector3 nextPos = m_Transform.position + velocity * Time.deltaTime;

		//�����蔻��̔��a���擾
		float radius = m_Collider.radius;

		//��ʂ̏㉺�[�܂ňړ�������Y���ړ������𔽓]������
		float posY = Mathf.Abs(nextPos.y) + radius;
		if (Mathf.Abs(MyScreen.BottomLeft.y) <= posY)
		{
			m_MoveDirection.y *= -1;
		}

		/**** ��ʊO�ɏo�Ȃ��悤�ɕ␳���� ********************************************************/


		//��ʉE��̍��W���擾
		Vector3 topRight = MyScreen.TopRight;
		//��ʍ����̍��W���擾
		Vector3 bottomLeft = MyScreen.BottomLeft;

		//X���̍��W����ʓ��ɕ␳����
		nextPos.x = Mathf.Clamp(nextPos.x, bottomLeft.x + radius, topRight.x - radius);
		//Y���̍��W����ʓ��ɕ␳����
		nextPos.y = Mathf.Clamp(nextPos.y, bottomLeft.y + radius, topRight.y - radius);
		/******************************************************************************************/

		//���W�𔽉f
		m_Transform.position = nextPos;
	}

	/// <summary>
	/// ���S����
	/// </summary>
	private void Dead()
	{
		//���S�t���O�������Ă��Ȃ���Ώ������Ȃ�
		if (!m_Life.IsDead) return;

		m_Damage.enabled = false;
		m_Default.DOFade(0.0f, m_FadeTime).OnComplete(() => {
			//�t�F�[�h�A�E�g�����㎩�����폜
			Destroy(gameObject);
		});

		//�����G�t�F�N�g�𐶐�
		for (int i = 0; i < 3; i++)
		{
			//����������W
			Vector3 pos = m_Transform.position + new Vector3(Random.Range(-50.0f, 50.0f), Random.Range(-50.0f, 50.0f), 0);
			//�������̊p�x
			float angle = Random.Range(0, 360);
			Quaternion rotate = Quaternion.Euler(0, 0, angle);
			//�G�t�F�N�g�𐶐�
			CreateDamageEffect(m_ExplosionEffect, pos, Vector3.left);
		}

		if (GamePlayManager.Instance != null)
		{
			//�Q�[���I���������s��
			GamePlayManager.Instance.GameEnd(true, GamePlayManager.GameEndType.Over);
		}

	}

	/// <summary>
	/// �_���[�W���󂯂����̃��A�N�V����
	/// </summary>
	private void DamageReaction()
	{
		Debug.Log("�_���[�W���󂯂�");
		m_DamageFlash.StartFlash();
	}

	/// <summary>
	/// �_���[�W�_��
	/// </summary>
	private void SwichImage()
	{
		if (m_DamageFlash.IsActive)
		{
			m_Default.enabled = false;
			m_Damage.enabled = true;
		}
		else
		{
			m_Default.enabled = true;
			m_Damage.enabled = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//�Փˈʒu���擾
		var hitPoint = collision.ClosestPoint(m_Transform.position);
		//�G�t�F�N�g�𐶐�
		CreateDamageEffect(m_HitEffect, hitPoint, Vector3.left);
	}

	/// <summary>
	///	�_���[�W�G�t�F�N�g�̐���
	/// </summary>
	/// <param name="effectPrefb"> ��������G�t�F�N�g�I�u�W�F�N�g </param>
	/// <param name="position"> �������W </param>
	/// <param name="moveDir"> �ړ����� </param>
	private void CreateDamageEffect(GameObject effectPrefb, Vector3 position, Vector3 moveDir)
	{
		//�������̊p�x
		float angle = Random.Range(0, 360);
		Quaternion rotate = Quaternion.Euler(0, 0, angle);
		//�G�t�F�N�g�𐶐�
		var effect = Instantiate(effectPrefb, position, rotate).GetComponent<DamageEffect>();
		//�G�t�F�N�g�̈ړ�������ݒ�
		effect.MoveDirection = moveDir;
	}
}
