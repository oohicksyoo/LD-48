using System.Collections;
using Assets.Project.Code.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Project.Code.Gameplay {
	public class UIAnimator : MonoBehaviour {

		#region Serialized Fields

		[Header("Selection UI")]
		[SerializeField]
		private RectTransform buttonContainer;
		
		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private TextMeshProUGUI mainText;
		
		[Header("Game UI")]
		[SerializeField]
		private RectTransform gameUIContainer;
		
		[SerializeField]
		private CanvasGroup gameUICanvasGroup;

		[Header("Animation")]
		[SerializeField]
		private float yPosition;

		[SerializeField]
		private float animationLength;

		[SerializeField]
		private AnimationCurve animationCurve;

		[Header("Buttons")]
		[SerializeField]
		private Button oxygenButton;
		
		[SerializeField]
		private Image oxygenLockImage;
		
		[SerializeField]
		private Button lightButton;
		
		[SerializeField]
		private Image lightLockImage;
		
		[SerializeField]
		private Button radarButton;
		
		[SerializeField]
		private Image radarLockImage;

		#endregion


		#region Properties

		private Vector2 SpawnPositionSelection {
			get;
			set;
		}
		
		private Vector2 SpawnPositionGameUI {
			get;
			set;
		}

		#endregion
		

		#region MonBehaviour

		public void Start() {
			canvasGroup.alpha = 0;
			gameUICanvasGroup.alpha = 0;
			this.SpawnPositionSelection = buttonContainer.anchoredPosition;
			this.SpawnPositionGameUI = gameUIContainer.anchoredPosition;
		}

		#endregion
		
		
		#region Public Functions

		public void UpdateMainText() {
			int count = GameManager.Instance.PearlCount;
			bool hasLightUpgrade = PlayerPrefHelper.LoadBool(PlayerPrefHelper.UPGRADE_LIGHT);
			bool hasRadarUpgrade = PlayerPrefHelper.LoadBool(PlayerPrefHelper.UPGRADE_RADAR);
			mainText.text = $"You have <color=#17224B>{count}</color> Pearl{(count != 1 ? "s":"")} to spend";
			
			//Handle locking and of grabbed UI
			oxygenButton.interactable = !(count < 2);
			oxygenLockImage.gameObject.SetActive(count < 2);
			
			lightButton.interactable = !(count < 6) && !hasLightUpgrade;
			lightLockImage.gameObject.SetActive(count < 6 || hasLightUpgrade);
			
			radarButton.interactable = !(count < 10) && !hasRadarUpgrade;
			radarLockImage.gameObject.SetActive(count < 10 || hasRadarUpgrade);
		}
		
		public void RunAnimationIn() {
			UpdateMainText();
			StartCoroutine(OnRunAnimationIn());
		}

		public void RunAnimationOut() {
			StartCoroutine(OnRunAnimationOut());
		}
		
		public void RunGameUIAnimationIn() {
			StartCoroutine(OnRunGameUIAnimationIn());
		}

		public void RunGameUIAnimationOut() {
			StartCoroutine(OnRunGameUIAnimationOut());
		}

		#endregion


		#region Private Functions

		private IEnumerator OnRunAnimationIn() {
			canvasGroup.alpha = 0;
			buttonContainer.anchoredPosition = this.SpawnPositionSelection + new Vector2(0, yPosition);
			
			for (float i = 0; i < animationLength; i += Time.deltaTime) {
				float t = animationCurve.Evaluate(i / animationLength);
				canvasGroup.alpha = Mathf.Lerp(0, 1, t);
				buttonContainer.anchoredPosition = Vector2.Lerp(this.SpawnPositionSelection + new Vector2(0, yPosition), this.SpawnPositionSelection, t);
				yield return null;
			}
			
			canvasGroup.alpha = 1;
			buttonContainer.anchoredPosition = this.SpawnPositionSelection;
			canvasGroup.interactable = true;
		}
		
		private IEnumerator OnRunAnimationOut() {
			canvasGroup.interactable = false;
			canvasGroup.alpha = 1;
			buttonContainer.anchoredPosition = this.SpawnPositionSelection;
			
			for (float i = 0; i < animationLength; i += Time.deltaTime) {
				float t = animationCurve.Evaluate(i / animationLength);
				canvasGroup.alpha = Mathf.Lerp(1, 0, t);
				buttonContainer.anchoredPosition = Vector2.Lerp(this.SpawnPositionSelection, this.SpawnPositionSelection + new Vector2(0, yPosition), t);
				yield return null;
			}
			
			canvasGroup.alpha = 0;
			buttonContainer.anchoredPosition = this.SpawnPositionSelection + new Vector2(0, yPosition);
		}
		
		private IEnumerator OnRunGameUIAnimationIn() {
			gameUICanvasGroup.alpha = 0;
			gameUIContainer.anchoredPosition = this.SpawnPositionGameUI + new Vector2(0, yPosition);
			
			for (float i = 0; i < animationLength; i += Time.deltaTime) {
				float t = animationCurve.Evaluate(i / animationLength);
				gameUICanvasGroup.alpha = Mathf.Lerp(0, 1, t);
				gameUIContainer.anchoredPosition = Vector2.Lerp(this.SpawnPositionGameUI + new Vector2(0, yPosition), this.SpawnPositionGameUI, t);
				yield return null;
			}
			
			gameUICanvasGroup.alpha = 1;
			gameUIContainer.anchoredPosition = this.SpawnPositionGameUI;
		}
		
		private IEnumerator OnRunGameUIAnimationOut() {
			gameUICanvasGroup.alpha = 1;
			gameUIContainer.anchoredPosition = this.SpawnPositionGameUI;
			
			for (float i = 0; i < animationLength; i += Time.deltaTime) {
				float t = animationCurve.Evaluate(i / animationLength);
				gameUICanvasGroup.alpha = Mathf.Lerp(1, 0, t);
				gameUIContainer.anchoredPosition = Vector2.Lerp(this.SpawnPositionGameUI, this.SpawnPositionGameUI + new Vector2(0, yPosition), t);
				yield return null;
			}
			
			gameUICanvasGroup.alpha = 0;
			gameUIContainer.anchoredPosition = this.SpawnPositionGameUI + new Vector2(0, yPosition);
		}

		#endregion

	}
}