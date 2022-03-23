using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンのハンドル
/// シーンの登録時のIDに注意
/// </summary>
public enum SceneHandle
{
	NONE = -1,
	TITLE = 0,
	PLAY ,
	DEMO,
	RESULT ,
}

/// <summary>
/// シーン読み込み機能
/// </summary>
public static class SceneLoader
{
	/// <summary>
	/// シーンの読み込み
	/// </summary>
	/// <param name="scene"> 遷移先シーン </param>
	public static void Load(int scene)
	{
		SceneManager.LoadScene(scene);
	}

	/// <summary>
	/// シーンの読み込み
	/// </summary>
	/// <param name="sceneType"> 遷移先シーンのタイプID </param>
	public static void Load(SceneHandle sceneType)
	{
		if (sceneType == SceneHandle.NONE)
		{
			Debug.LogError("シーンの種類が選択されていません");
			return;
		}
		Load((int)sceneType);
	}
}

