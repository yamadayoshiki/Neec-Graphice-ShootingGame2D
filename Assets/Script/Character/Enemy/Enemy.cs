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

	/// <summary>
	/// �_���[�W�G�t�F�N�g
	/// </summary>
	[SerializeField]
	protected GameObject m_HitEffect = null;

	/// <summary>
	/// �����G�t�F�N�g
	/// </summary>
	[SerializeField]
	protected GameObject m_ExplosionEffect = null;

	protected virtual void OnEnable()
	{
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

	protected virtual void Update()
	{
		//�ړ�����
		Move();
		//�U������
		Attack();
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

		//��ʊO�ɏo�Ȃ��悤�ɕ␳
		nextPos = ScreenClampPosition(nextPos);

		//���W�𔽉f
		m_Transform.position = nextPos;
	}

	/// <summary>
	/// ��ʊO�ɏo�Ȃ��悤�ɕ␳����
	/// </summary>
	/// <param name="position"></param>
	/// <returns></returns>
	protected Vector3 ScreenClampPosition(Vector3 position)
	{
		//�ړ���̍��W
		Vector3 nextPos = position;

		/**** ��ʊO�ɏo�Ȃ��悤�ɕ␳���� ********************************************************/
		//�����蔻��̔��a���擾
		float radius = m_Collider.radius;
		//Z���̍��W���擾
		float positionZ = position.z;

		//��ʉE��̍��W���擾
		Vector3 topRight = MyScreen.TopRight;
		//��ʍ����̍��W���擾
		Vector3 bottomLeft = MyScreen.BottomLeft;

		//Y���̍��W����ʓ��ɕ␳����
		nextPos.y = Mathf.Clamp(position.y, bottomLeft.y + radius, topRight.y - radius);
		/******************************************************************************************/

		//���W��Ԃ�
		return nextPos;
	}

	/// <summary>
	/// �U������
	/// </summary>
	protected virtual void Attack()
	{

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

	/// <summary>
	/// �����͈͂��H
	/// </summary>
	/// <returns> �����͈͓��Ȃ�true��Ԃ� </returns>
	protected bool IsActiveArea()
	{
		float posX = m_Transform.position.x + m_Collider.radius;
		float posY = m_Transform.position.y + m_Collider.radius;
		if (posX >= MyScreen.BottomLeft.x && posX <= MyScreen.TopRight.x &&
			posY >= MyScreen.BottomLeft.y && posY <= MyScreen.TopRight.y)
		{
			return true;
		}
		return false;
	}

	/// <summary>
	///	�_���[�W�G�t�F�N�g�̐���
	/// </summary>
	/// <param name="effectPrefb"> ��������G�t�F�N�g�I�u�W�F�N�g </param>
	/// <param name="position"> �������W </param>
	/// <param name="moveDir"> �ړ����� </param>
	protected void CreateDamageEffect(GameObject effectPrefb, Vector3 position, Vector3 moveDir)
	{
		//�������̊p�x
		float angle = Random.Range(0, 360);
		Quaternion rotate = Quaternion.Euler(0, 0, angle);
		//�G�t�F�N�g�𐶐�
		var effect = Instantiate(effectPrefb, position, rotate).GetComponent<DamageEffect>();
		//�G�t�F�N�g�̈ړ�������ݒ�
		effect.MoveDirection = moveDir;
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
		}
	}
}
