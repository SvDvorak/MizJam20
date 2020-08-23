using System;
using System.Collections;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets
{
	public abstract class TimedTask : MonoBehaviour
	{
		public abstract void StartTask();
		public abstract void FinishNow();

		public event Action OnFinish;

		protected void Finish()
		{
			OnFinish?.Invoke();
		}
	}

	public enum PrintPerWait
	{
		Letter,
		Line
	}

	[RequireComponent(typeof(TMP_Text))]
	public class TextLinePresenter : TimedTask
	{
		public PrintPerWait PrintPerWait;
		public float LinesOrLettersPerSecond = 20;
		public float PausersTimeSeconds = 0.3f;
		public float SpeedUpLettersPerSecond = 5;
		public AudioSource WriteSound;

		private TMP_Text _textLine;

		private readonly char[] _pausers = { '.', ',', '!', '?', ':', ';' };
		private readonly char _hidePauser = '^';
		private readonly char _speed = '>';
		private readonly char _slow = '<';
		private int _speedChange;
		private Coroutine _task;

		public void Awake()
		{
			_textLine = GetComponent<TMP_Text>();
			SetText(_textLine.text);
		}

		public void ShowText(string text)
		{
			SetText(text);
			StartTask();
		}

		public void SetText(string text)
		{
			_textLine.text = text
				.Replace(_speed.ToString(), "")
				.Replace(_slow.ToString(), "");
			_textLine.maxVisibleCharacters = 0;
		}

		public override void StartTask()
		{
			_task = StartCoroutine(AnimateText());
		}

		public override void FinishNow()
		{
			StopCoroutine(_task);
			_textLine.maxVisibleCharacters = int.MaxValue;
			Finish();
		}

		private IEnumerator AnimateText()
		{
			yield return DialogueWait(0.5f);

			int letterIndex = 0;
			foreach(var letter in _textLine.text)
			{
				var speedChanged = UpdateSpeedChange(letter);
				if(speedChanged)
					continue;

				var hiddenPause = letter == _hidePauser;
				if(hiddenPause)
				{
					_textLine.text = _textLine.text.Remove(letterIndex, 1);
					yield return DialogueWait(1);
					continue;
				}

				letterIndex += 1;
				_textLine.maxVisibleCharacters = letterIndex;
				PlaySound();
				if(_pausers.Contains(letter))
					yield return DialogueWait(PausersTimeSeconds);

				if(PrintPerWait == PrintPerWait.Letter || PrintPerWait == PrintPerWait.Line && letter == '\n')
					yield return DialogueWait(1 / (LinesOrLettersPerSecond + _speedChange * SpeedUpLettersPerSecond));
			}

			yield return DialogueWait(1);

			Finish();
		}

		private bool UpdateSpeedChange(char letter)
		{
			int change = 0;
			if (letter == _slow)
				change -= 1;
			if (letter == _speed)
				change += 1;

			_speedChange += change;
			return change != 0;
		}

		private void PlaySound()
		{
			if(WriteSound != null && !WriteSound.isPlaying)
			{
				WriteSound.pitch = 0.9f + Random.value * 0.2f;
				WriteSound.Play();
			}
		}

		private static WaitForSeconds DialogueWait(float seconds)
		{
			//return Input.GetMouseButton(0) ? null : new WaitForSeconds(seconds);
			return new WaitForSeconds(seconds);
		}
	}
}
