using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Life))]
public class Enemy : MonoBehaviour
{
	/// <summary>
	/// ���W�b�h�{�f�B
	/// </summary>
	private Rigidbody2D m_Rigidbody = null;
	public Rigidbody2D Rigidbody { get { return m_Rigidbody; } }

	/// <summary>
	/// �R���C�_�[
	/// </summary>
	private CircleCollider2D m_Collider = null;
	public CircleCollider2D Collider { get { return m_Collider; } }

	/// <summary>
	/// �ϋv�l�N���X
	/// </summary>
	private Life m_Life = null;
	public Life Life { get { return m_Life; } }

	/// <summary>
	/// �J����
	/// </summary>
	[SerializeField]
	private Camera m_Camera = null;

	/// <summary>
	/// �V���[�^�[
	/// </summary>
	[SerializeField]
	private Shooter m_Shooter = null;
	public Shooter Shooter { get { return m_Shooter; } }

	/// <summary>
	/// �����̃g�����X�t�H�[��
	/// </summary>
	protected Transform m_Transform = null;

	/// <summary>
	/// �ړ����x
	/// </summary>
	[SerializeField]
	private int m_MoveSpeed = 100;
	public int MoveSpeed
	{
		get { return m_MoveSpeed; }
		set { m_MoveSpeed = value * 100; }
	}

	/// <summary>
	/// �_���[�W�l
	/// </summary>
	[SerializeField]
	private int m_DamageValue = 1;
	public int DamageValue { get { return m_DamageValue; } }

	/// <summary>
	/// �ړ�����
	/// </summary>
	[SerializeField]
	protected Vector3 m_MoveDirection = -Vector2.right;

	private void OnEnable()
	{
		//�R���|�[�l���g���擾
		TryGetComponent(out m_Rigidbody);
		TryGetComponent(out m_Collider);
		TryGetComponent(out m_Life);
		if (m_Camera == null) m_Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		if (!TryGetComponent(out m_Shooter)) m_Shooter = GetComponentInChildren<Shooter>();

		m_Transform = this.transform;

		m_Rigidbody.gravityScale = 0.0f;
		m_Collider.isTrigger = true;
		m_Life.DamageReaction = this.DamageReaction;

		//������
		Initlaize();
	}

	/// <summary>
	/// ������
	/// </summary>
	public virtual void Initlaize()
	{
		m_Life.Initialize();
		m_Shooter.Initialize();
	}

	private void Update()
	{
		//�ړ�����
		Move();
		//���S����
		Dead();
	}

	/// <summary>
	/// �ړ�����
	/// </summary>
	protected virtual void Move()
	{
		//�ړ��ʂ��v�Z
		Vector3 velocity = m_MoveDirection * m_MoveSpeed;
		//�ړ���̍��W��ێ�
		Vector3 nextPos = m_Transform.position + velocity * Time.deltaTime;

		/**** ��ʊO�ɏo�Ȃ��悤�ɕ␳���� ********************************************************/
		//�����蔻��̔��a���擾
		float radius = m_Collider.radius;
		//Z���̍��W���擾
		float positionZ = nextPos.z;

		//��ʉE��̍��W���擾
		Vector3 topRight = m_Camera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, positionZ));
		//��ʍ����̍��W���擾
		Vector3 bottomLeft = m_Camera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, positionZ));

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
	protected virtual void Dead()
	{
		if (!m_Life.IsDead) return;

		//�������폜
		Destroy(gameObject);
	}

	/// <summary>
	/// �_���[�W���󂯂����̃��A�N�V����
	/// </summary>
	protected virtual void DamageReaction()
	{
		Debug.Log("�_���[�W���󂯂�");
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			//����̑ϋv�l�N���X���擾���ă_���[�W��K�p����
			if (collision.gameObject.TryGetComponent(out Life life))
			{
				life.ApplayDamage(DamageValue);
			}
			//�������폜
			Destroy(gameObject);
		}
	}
}
