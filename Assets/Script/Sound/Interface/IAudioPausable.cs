namespace YY.Sound
{
	/// <summary>
	/// 一時停止インターフェイス
	/// </summary>
	public interface IAudioPausable
	{
		/// <summary>
		/// 一時停止
		/// </summary>
		void Pause();

		/// <summary>
		/// 再生再開
		/// </summary>
		void Resume();

		/// <summary>
		/// 一時停止フラグ
		/// </summary>
		bool IsPaused { get; }
	}
}