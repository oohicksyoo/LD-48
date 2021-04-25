using System.Collections;
using UnityEngine;

namespace Assets.Project.Code.Gameplay {
	public class UIAnimator : MonoBehaviour {

		#region Serialized Fields

		[SerializeField]
		private RectTransform buttonContainer;

		[SerializeField]
		private float yPosition;

		[SerializeField]
		private float animationLength;

		[SerializeField]
		private AnimationCurve animationCurve;

		[SerializeField]
		private CanvasGroup canvasGroup;

		#endregion


		#region Properties

		private Vector2 SpawnPosition {
			get;
			set;
		}

		#endregion
		

		#region MonBehaviour

		public void Start() {
			canvasGroup.alpha = 0;
			this.SpawnPosition = buttonContainer.anchoredPosition;
		}

		#endregion
		
		
		#region Public Functions

		public void RunAnimationIn() {
			StartCoroutine(OnRunAnimationIn());
		}

		public void RunAnimationOut() {
			StartCoroutine(OnRunAnimationOut());
		}

		#endregion


		#region Private Functions

		private IEnumerator OnRunAnimationIn() {
			canvasGroup.alpha = 0;
			buttonContainer.anchoredPosition = this.SpawnPosition + new Vector2(0, yPosition);
			
			for (float i = 0; i < animationLength; i += Time.deltaTime) {
				float t = animationCurve.Evaluate(i / animationLength);
				canvasGroup.alpha = Mathf.Lerp(0, 1, t);
				buttonContainer.anchoredPosition = Vector2.Lerp(this.SpawnPosition + new Vector2(0, yPosition), this.SpawnPosition, t);
				yield return null;
			}
			
			canvasGroup.alpha = 1;
			buttonContainer.anchoredPosition = this.SpawnPosition;
		}
		
		private IEnumerator OnRunAnimationOut() {
			canvasGroup.alpha = 1;
			buttonContainer.anchoredPosition = this.SpawnPosition;
			
			for (float i = 0; i < animationLength; i += Time.deltaTime) {
				float t = animationCurve.Evaluate(i / animationLength);
				canvasGroup.alpha = Mathf.Lerp(1, 0, t);
				buttonContainer.anchoredPosition = Vector2.Lerp(this.SpawnPosition, this.SpawnPosition + new Vector2(0, yPosition), t);
				yield return null;
			}
			
			canvasGroup.alpha = 0;
			buttonContainer.anchoredPosition = this.SpawnPosition + new Vector2(0, yPosition);
		}

		#endregion

	}
}