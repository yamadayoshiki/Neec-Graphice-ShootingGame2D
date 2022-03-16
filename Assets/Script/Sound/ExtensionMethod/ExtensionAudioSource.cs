using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

/// <summary>
/// AudioSoruce関数拡張クラス
/// </summary>
public static class ExtensionAudioSource
{
	/// <summary>
	/// Play関数の拡張
	/// １行でaudioClip, ボリューム、再生位置のランダマイズまで指定できるようにする
	/// </summary>
	/// <param name="audioClip"> サウンドデータ </param>
	/// <param name="volime"> 音量 0.0f~1.0f </param>
	/// <param name="isRandomStartTime"> 再生位置をランダムにするか </param>
	public static void Play(this AudioSource audioSource, AudioClip audioClip, float volume, bool isRandomStartTime = false)
	{
		//オーディオクリップがnullなら処理しない
		if (audioClip == null) return;

		//オーディオソースにオーディオクリップと音量を設定
		audioSource.clip = audioClip;
		audioSource.volume = volume;
		//再生位置のランダムが有効なら処理する
		if (isRandomStartTime)
		{
			audioSource.time = UnityEngine.Random.Range(0f, audioClip.length - 0.01f);
			//結果がlengthと同値になるとシークエラーを起こすため -0.01秒する//
		}
		//音を再生
		audioSource.Play();
	}

	/// <summary>
	/// Play関数の拡張
	/// audioClip, ボリューム,再生完了時に実行する関数を設定
	/// </summary>
	/// <param name="audioClip"> サウンドデータ </param>
	/// <param name="volime"> 音量 0.0f~1.0f </param>
	/// <param name="compCallback"> 再生完了時に実行する関数 </param>
	public static Tween PlayWithCompCallback(this AudioSource audioSource, AudioClip audioClip, float volume = 1f, UnityAction compCallback = null)
	{
		float timer = 0.0f; //タイマーの用意
		//DOtweenを使って再生完了時にコールバックを行う
		Tween tween = DOTween.To(
			() => 0.0f, //初期値を設定
			t => timer = t, //タイマーの更新
			Time.deltaTime, //タイマーに加算する値
			audioClip.length) //終了値
			.OnStart(() => audioSource.Play(audioClip, volume)) //トゥイーン開始時に音を再生
			.OnComplete(() => //トゥイーン完了時に実行
			{	//コールバックが設定されていなければ処理しない
				if (compCallback != null)
				{
					//再生完了コールバックを実行
					compCallback();
				}
			}
		);
		//トゥイーンを返す
		return tween;
	}

	/// <summary>
	/// Play関数の拡張
	/// 指定した時間後に再生する
	/// </summary>
	/// <param name="audioClip"> サウンドデータ </param>
	/// <param name="delayTime"> 遅延時間 </param>
	/// <param name="volime"> 音量 0.0f~1.0f </param>
	public static void PlayDelay(this AudioSource audioSource, AudioClip audioClip, float delayTime, float volume = 1.0f)
	{
		PlayWithCompCallback(audioSource, audioClip, volume).SetDelay(delayTime);
	}

	/// <summary>
	/// Play関数の拡張
	/// フェード処理
	/// </summary>
	/// <param name="targetVolume"> 目標のボリューム </param>
	/// <param name="fadeTime"> フェードする時間 </param>
	/// <param name="ease"> イージングタイプ </param>
	/// <param name="compCallback"> フェード完了時に実行する関数 </param>
	private static Tween VolumeFade(this AudioSource audioSource, float targetVolume = 1f, float fadeTime = 1f, Ease ease = Ease.Linear, UnityAction compCallback = null)
	{
		//トゥイーンを生成
		//DOFadeを使って音量を操作する
		Tween tween = audioSource.DOFade(targetVolume, fadeTime)
			.SetEase(ease) //イージングを設定
			.OnComplete(() =>//トゥイーン完了時に実行
			{   //コールバックが設定されていなければ処理しない
				if (compCallback != null)
				{
					//再生完了コールバックを実行
					compCallback();
				}
			});
		//トゥイーンを返す
		return tween;
	}

	/// <summary>
	/// Play関数の拡張
	/// フェードイン処理
	/// </summary>
	/// <param name="audioClip"> サウンドデータ </param>
	/// <param name="targetVolume"> 目標のボリューム </param>
	/// <param name="fadeTime"> フェードする時間 </param>
	/// <param name="ease"> イージングタイプ </param>
	/// <param name="isRandomStartTime"> 再生位置をランダムにするか </param>
	/// <param name="compCallback"> 再生完了時に実行する関数 </param>
	public static Tween PlayWithFadeIn(this AudioSource audioSource, AudioClip audioClip, float fadeTime = 1f, float startVolume = 0.0f, float targetVolume = 1f, Ease ease = Ease.Linear, bool isRandomStartTime = false, UnityAction compCallback = null)
	{
		//目標ボリュームが0以下の場合は再生をキャンセル
		if (targetVolume <= 0.0f) return null;
		//音の再生
		audioSource.Play(audioClip, startVolume, isRandomStartTime);
		//フェードイン処理を行う
		return audioSource.VolumeFade(targetVolume, fadeTime, ease, compCallback);
	}

	/// <summary>
	/// Play関数の拡張
	/// フェードアウト処理
	/// </summary>
	/// <param name="audioClip"> サウンドデータ </param>
	/// <param name="targetVolume"> 目標のボリューム </param>
	/// <param name="fadeTime"> フェードする時間 </param>
	/// <param name="ease"> イージングタイプ </param>
	/// <param name="isRandomStartTime"> 再生位置をランダムにするか </param>
	public static Tween StopWithFadeOut(this AudioSource audioSource, float fadeTime = 1f, float targetVolume = 0f, Ease ease = Ease.Linear, bool isRandomStartTime = false, UnityAction compCallback = null)
	{
		//再生されてなければ処理しない
		if (audioSource.isPlaying == false) return null;
		//フェードアウト処理を行う
		return audioSource.VolumeFade(targetVolume, fadeTime, ease
			, () =>
			{   //再生を停止
				audioSource.Stop();
				//再生完了コールバックを実行
				if (compCallback != null)
				{
					compCallback();
				}
			}
		);
	}
}
