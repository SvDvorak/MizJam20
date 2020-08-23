using System;
using DG.Tweening;
using UnityEngine;

namespace Assets
{
	public interface ITimedTask
	{
		bool IsPlaying();
	}

	public abstract class TimedTask : MonoBehaviour, ITimedTask
	{
		public abstract void StartTask();
		public abstract void FinishNow();

		public event Action OnFinish;
		private bool isFinished;

		protected void Finish()
		{
			isFinished = true;
			OnFinish?.Invoke();
		}

		public bool IsPlaying()
		{
			return !isFinished;
		}
	}

	public class TweenTask : ITimedTask
	{
		private readonly Tween _tween;
		private bool isFinished;

		public TweenTask(Tween tween)
		{
			_tween = tween;
			_tween.onComplete += TweenComplete;
			_tween.onKill += TweenComplete;
		}

		private void TweenComplete()
		{
			_tween.onComplete -= TweenComplete;
			_tween.onKill -= TweenComplete;
			isFinished = true;
		}

		public bool IsPlaying()
		{
			return !isFinished;
		}
	}
}