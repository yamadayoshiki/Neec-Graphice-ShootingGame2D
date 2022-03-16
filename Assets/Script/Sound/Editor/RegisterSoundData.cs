using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using YY.Sound;

/// <summary>
/// サウンドデータ登録クラス
/// </summary>
public static class RegisterSoundData
{
	/// <summary>
	/// 取得先
	/// </summary>
	private const string m_LoadFilePath = "Sound/";

	/// <summary>
	/// enumファイルの作成場所ばでのパス
	/// </summary>
	private const string m_EnumFileExportPath = "Assets/Script/Sound/Enum/";

	/// <summary>
	/// オーディオクリップの読み込み
	/// </summary>
	/// <param name="filePath"> 読み込み先ファイルパス </param>
	/// <returns> オーディオクリップリストを返す </returns>
	private static List<AudioClip> LoadAudioClips(string filePath)
	{
		//オーディオクリップリストを作成
		List<AudioClip> audioClips = new List<AudioClip>();
		//リソースフォルダから読み込み
		audioClips.AddRange(Resources.LoadAll<AudioClip>(filePath));
		return audioClips;
	}

	/// <summary>
	/// サウンドデータの登録
	/// </summary>
	/// <param name="fileName"> サウンドデータリストの名前 </param>
	/// <param name="exportFilePath"> 保存先のファイルパス </param>
	/// <param name="soundType"> サウンドの種類 </param>
	/// <param name="loadAudioFilePath"> オーディオクリップの読み込み先ファイルパス </param>
	/// <returns> 作成したサウンドデータリストを返す </returns>
	public static SoundDataList Register(string fileName, string exportFilePath, string loadAudioFilePath, SoundType soundType)
	{
		//サウンドの種類が選択されてなければ登録処理をしない
		if (soundType == SoundType.NONE)
		{
			Debug.LogError("サウンドの種類が選択されていません");
			return null;
		}

		//オーディオクリップの読み込み
		List<AudioClip> audioClips = LoadAudioClips(loadAudioFilePath);

		//サウンドデータリストを取得
		var datas = GetSoundDataList(exportFilePath, fileName);
		//データが未作成なら作成する
		if (datas == null)
		{
			//サウンドデータリストを作成
			datas = CreateSoundDataList.Create(exportFilePath, fileName);
		}

		//中身を初期化
		datas.Clear();
		//サウンドデータリストにサウンドタイプを登録
		datas.SoundType = soundType;

		//サウンドデータリストにオーディオクリップとIDを登録
		for (int i = 0; i < audioClips.Count; i++)
		{
			//サウンドデータを作成
			SoundData soundData = new SoundData(i, audioClips[i]);
			//リストに追加する
			datas.SetSoundData(soundData);
		}

		//変更があった事を記録する
		EditorUtility.SetDirty(datas);
		//保存する
		AssetDatabase.SaveAssets();

		//サウンドデータリストを返す
		return datas;
	}

	/// <summary>
	/// サウンドIDのEnumを作成
	/// </summary>
	/// <param name="fileName"> Enumのファイルの名前 </param>
	/// <param name="soundDictionary"> Dictionaryデータ </param>
	/// <param name="exportPath"> 保存先のファイルパス </param>
	public static void CreateEnum(string fileName, SoundDataList soundDataList, string exportPath)
	{
		//ディクショナリーに仮登録
		var soundDictionary = ConvartDic(soundDataList.GetSoundDataList);
		//enumの作成
		EnumCreator.Create(fileName, soundDictionary, m_EnumFileExportPath + fileName + ".cs");
	}

	/// <summary>
	/// サウンドデータリストを取得
	/// </summary>
	/// <param name="fileName"></param>
	/// <param name="soundType"></param>
	/// <returns> サウンドデータリストを返す。なければnullが返る </returns>
	private static SoundDataList GetSoundDataList(string filePath, string fileName)
	{
		//サウンドデータリストを読み込み
		var datas = Resources.Load<SoundDataList>(fileName);
		return datas;
	}

	/// <summary>
	/// サウンドデータをDictinaryに登録
	/// </summary>
	/// <param name="soundDatas"></param>
	/// <returns> Dictionaryで返す </returns>
	private static Dictionary<string, int> ConvartDic(List<SoundData> soundDatas)
	{
		//ディクショナリを生成
		Dictionary<string, int> dic = new Dictionary<string, int>();
		//リストをforeachで回す
		foreach (var data in soundDatas)
		{
			//オーディオクリップの名前をエラーが出ないように変換
			string name = ConvartRegisterName(data.AudioClip.name);
			//ディクショナリに登録
			dic[name] = data.ID;
		}
		return dic;
	}

	/// <summary>
	/// オーディオクリップの名前をEnumに登録しやすいように変換
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	private static string ConvartRegisterName(string name)
	{
		//文字列を空白で分割
		string[] str = name.Split(' ');
		//文字列を空白なしで結合
		string newName = String.Join("", str);
		//一番先頭の文字を取得
		string first = newName[0].ToString();
		bool isNumberIncluded = false;

		//先頭の文字列に数字が含まれているか確認
		for (int i = 0; i < 10; i++)
		{
			if (first == i.ToString())
			{
				//先頭の文字に数字が含まれていたらフラグを立てる
				isNumberIncluded = true;
			}
		}
		//先頭の数字文字を最後尾にする
		if (isNumberIncluded)
		{
			//数字文字列を保持する
			string numberString = "_";
			//文字列内の数字文字を調べる
			for (int j = 0; j < newName.Length; j++)
			{
				for (int i = 0; i < 10; i++)
				{
					int num = newName.IndexOf(i.ToString());
					if (num == -1) continue;
					numberString += newName[num];
					newName = newName.Remove(num, 1);
				}
			}
			//先頭の数字文字列を最後尾にする
			newName += numberString;
		}

		//扱えない記号を"_"で置換
		newName = newName.Replace(".", "_");
		newName = newName.Replace("-", "_");
		newName = newName.Replace("(", "_");
		//扱えない記号を""で置換
		newName = newName.Replace(")", "");

		return newName;
	}
}
