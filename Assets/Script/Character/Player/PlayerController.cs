using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerController : MonoBehaviour
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
	/// �J����
	/// </summary>
	[SerializeField]
	private Camera m_Camera = null;

	/// <summary>
	/// �V���[�^�[
	/// </summary>
	[SerializeField]
	private Shooter m_Shooter = null;

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

	private Vector3 m_InputDirection = Vector2.zero;

	private void Start()
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
	public void Initlaize()
	{
		m_Life.Initialize();
		m_Shooter.Initialize();
		m_InputDirection = Vector2.zero;
	}

	private void Update()
	{
		//���͍X�V
		InputUpdate();
		//�ړ�����
		Move();
		//�U������
		if (Input.GetButton("Fire1"))
		{
			m_Shooter.Fire(Vector2.right);
		}
		else
		{
			m_Shooter.Initialize();
		}
	}

	/// <summary>
	/// ���͍X�V
	/// </summary>
	private void InputUpdate()
	{
		//���͕����x�N�g�����擾
		m_InputDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0).normalized;
	}

	/// <summary>
	/// �ړ�����
	/// </summary>
	private void Move()
	{
		//�ړ��ʂ��v�Z
		Vector3 velocity = m_InputDirection * m_MoveSpeed;
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
	/// �_���[�W���󂯂����̃��A�N�V����
	/// </summary>
	private void DamageReaction()
	{
		Debug.Log("�_���[�W���󂯂�");
	}
}
