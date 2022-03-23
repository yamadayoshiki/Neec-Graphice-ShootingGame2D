using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shooter))]
public class DemoShooter : MonoBehaviour
{
	/// <summary>
	/// �V���[�^�[�N���X
	/// </summary>
	[SerializeField]
	private Shooter m_Shooter = null;

	/// <summary>
	/// ������
	/// </summary>
	[SerializeField]
	private Vector3 m_ShotDirection = Vector3.right;

	/// <summary>
	/// �ˌ���
	/// </summary>
	[SerializeField]
	private SE m_ShotSe = SE.SE07_shot;

	/// <summary>
	/// �ړ�����
	/// </summary>
	[SerializeField]
	private Vector3 m_MoveDirection = Vector3.up;

	/// <summary>
	/// �ړ����x
	/// </summary>
	[SerializeField]
	private float m_MoveSpeed = 2.0f;

	/// <summary>
	/// ���a
	/// </summary>
	[SerializeField]
	private float m_Radius = 32.0f;

	[SerializeField]
	private bool m_IsActive = true;
	public bool IsActive
	{
		get { return m_IsActive; }
		set { m_IsActive = value; }
	}

	//�����̃g�����X�t�H�[��
	private Transform m_Transform = null;

	// Start is called before the first frame update
	void Start()
	{
		//�R���|�[�l���g���擾
		TryGetComponent(out m_Shooter);

		m_Transform = this.transform;
		//��A�N�e�B�u�ɂ���
		IsActive = false;
	}

	// Update is called once per frame
	void Update()
	{
		//��A�N�e�B�u�Ȃ珈�����Ȃ�
		if (!IsActive) return;

		//�ˌ�
		m_Shooter.Fire(m_ShotDirection,(int)m_ShotSe);

		//�ړ�����
		Move();
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

		//��ʂ̏㉺�[�܂ňړ�������Y���ړ������𔽓]������
		float posY = Mathf.Abs(nextPos.y) + m_Radius;
		if (Mathf.Abs(MyScreen.BottomLeft.y) <= posY)
		{
			m_MoveDirection.y *= -1;
		}

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

		//��ʉE��̍��W���擾
		Vector3 topRight = MyScreen.TopRight;
		//��ʍ����̍��W���擾
		Vector3 bottomLeft = MyScreen.BottomLeft;

		//Y���̍��W����ʓ��ɕ␳����
		nextPos.y = Mathf.Clamp(position.y, bottomLeft.y + m_Radius, topRight.y - m_Radius);
		/******************************************************************************************/

		//���W��Ԃ�
		return nextPos;
	}
}
