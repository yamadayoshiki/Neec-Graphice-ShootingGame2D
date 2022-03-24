using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPlayManager : MonoBehaviour
{
	/// <summary>
	/// デモイメージコントローラー
	/// </summary>
	[SerializeField]
	private ImageController m_DemoImageCtr = null;

	// Start is called before the first frame update
	void Start()
	{
		//点滅
		m_DemoImageCtr.Flash(1.0f);
	}

	// Update is called once per frame
	void Update()
	{
		//どれかのキーを押したらタイトルに戻る
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z))
		{
			SceneLoader.Load(SceneHandle.TITLE);
		}
	}
}
