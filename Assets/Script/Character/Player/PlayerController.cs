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
	/// �J����
	/// </summary>
	[SerializeField]
	private Camera m_Camera = null;

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
		if (m_Camera == null) m_Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		m_Transform = this.transform;

		m_Rigidbody.gravityScale = 0.0f;
		m_Collider.isTrigger = true;

		//������
		Initlaize();
	}

	/// <summary>
	/// ������
	/// </summary>
	public void Initlaize()
	{
		m_InputDirection = Vector2.zero;
	}

	private void Update()
	{
		//���͍X�V
		InputUpdate();
		//�ړ�����
		Move();
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
}
