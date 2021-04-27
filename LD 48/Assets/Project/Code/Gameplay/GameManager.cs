using System.Collections.Generic;
using Assets.Project.Code.Audio;
using Assets.Project.Code.Utility;
using TMPro;
using UnityEngine;

namespace Assets.Project.Code.Gameplay {
	public class GameManager : MonoBehaviour {

		#region Serialized Fields

		[SerializeField]
		private Transform playerSub;

		[SerializeField]
		private Transform spawnPoint;

		[SerializeField]
		private Transform releasePoint;

		[SerializeField]
		private Transform waterLine;
		
		[SerializeField]
		private Transform uiFadePoint;

		[SerializeField]
		private Follower followerComponent;

		[SerializeField]
		private UIAnimator uiAnimator;

		[SerializeField]
		private RopeAnimator ropeAnimator;

		[SerializeField]
		private LightActivator lightActivator;

		[SerializeField]
		private RadarAlertActivator radarAlertActivator;

		[SerializeField]
		private InstructionUI instructionUI;

		[SerializeField]
		private TrackCreator trackCreator;

		[SerializeField]
		private TextMeshPro furthestDepthText;

		[Header("Gameplay")]
		[SerializeField]
		private float distanceMultiplier = 1;

		#endregion


		#region Private Variables

		private List<Monster> monsters;

		#endregion
		
		
		#region Properties
		
		public static GameManager Instance {
			get;
			private set;
		}

		public GameState GameState {
			get;
			private set;
		} = GameState.None;

		public float Depth {
			get;
			private set;
		}

		public int PearlCount {
			get;
			private set;
		}

		public float CurrentOxygen {
			get;
			private set;
		}

		public float OxygenAmount {
			get;
			private set;
		}

		public List<Monster> Monsters {
			get {
				return monsters = (monsters != null) ? monsters : new List<Monster>();
			}
		}

		private bool UIFadedIn {
			get;
			set;
		}
		
		private bool CanTakeOxygen {
			get;
			set;
		}

		#endregion


		#region MonoBehaviour
		
		public void Awake() {
			Instance = this;
		}

		public void Start() {
			this.PearlCount = PlayerPrefHelper.LoadInt(PlayerPrefHelper.PEARL_COUNT);
			this.OxygenAmount = PlayerPrefHelper.LoadFloat(PlayerPrefHelper.OXYGEN_AMOUNT, 30);
			CheckDepth();
		}

		public void Update() {
			StateUpdate();
		}

		#endregion
		
		
		#region Public Functions

		public void OnSkipButton() {
			if (this.GameState == GameState.Shop) {
				SwitchState(GameState.Transition);
			}
		}

		public void OnOxygenUpgradeButton() {
			var pearlCount = PlayerPrefHelper.LoadInt(PlayerPrefHelper.PEARL_COUNT);
			this.PearlCount = pearlCount - 2;
			PlayerPrefHelper.SaveInt(PlayerPrefHelper.PEARL_COUNT, this.PearlCount);
			this.OxygenAmount = PlayerPrefHelper.LoadFloat(PlayerPrefHelper.OXYGEN_AMOUNT, 30);
			PlayerPrefHelper.SaveFloat(PlayerPrefHelper.OXYGEN_AMOUNT, this.OxygenAmount + 30);
			uiAnimator.UpdateMainText();
		}

		public void OnLightUpgradeButton() {
			var pearlCount = PlayerPrefHelper.LoadInt(PlayerPrefHelper.PEARL_COUNT);
			this.PearlCount = pearlCount - 6;
			PlayerPrefHelper.SaveInt(PlayerPrefHelper.PEARL_COUNT, this.PearlCount);
			PlayerPrefHelper.SaveBool(PlayerPrefHelper.UPGRADE_LIGHT, 1);
			uiAnimator.UpdateMainText();
		}

		public void OnRadarUpgradeButton() {
			var pearlCount = PlayerPrefHelper.LoadInt(PlayerPrefHelper.PEARL_COUNT);
			this.PearlCount = pearlCount - 10;
			PlayerPrefHelper.SaveInt(PlayerPrefHelper.PEARL_COUNT, this.PearlCount);
			PlayerPrefHelper.SaveBool(PlayerPrefHelper.UPGRADE_RADAR, 1);
			uiAnimator.UpdateMainText();
		}

		public void OnCollectible() {
			AudioManager.Instance.PlaySFX(SFXList.Collectible);
			var pearlCount = PlayerPrefHelper.LoadInt(PlayerPrefHelper.PEARL_COUNT);
			this.PearlCount = pearlCount + 1;
			PlayerPrefHelper.SaveInt(PlayerPrefHelper.PEARL_COUNT, this.PearlCount);
		}

		#endregion


		#region Private Functions

		private void StateUpdate() {
			switch (this.GameState) {
				case GameState.None:
					SwitchState(GameState.Shop);
					break;
				case GameState.Shop:
					break;
				case GameState.Transition:
					playerSub.position += Vector3.down * 0.5f * Time.deltaTime;
					if (playerSub.position.y < releasePoint.position.y) {
						SwitchState(GameState.Game);	
					}
					break;
				case GameState.Game:
					CalculateDistance();
					CheckUIMarker();
					CheckInstructions();
					TakeOxygen();
					break;
				case GameState.Dead:
					break;
			}
		}

		private void SwitchState(GameState state) {
			if (state == this.GameState) {
				return;
			}

			this.GameState = state;

			switch (state) {
				case GameState.None:
					break;
				case GameState.Shop:
					instructionUI.ResetInstructions();
					ropeAnimator.ResetRope();
					lightActivator.ResetLight();
					radarAlertActivator.ResetAlert();
					trackCreator.ResetTrackCreator();
					playerSub.position = spawnPoint.position;
					playerSub.rotation = Quaternion.Euler(0,0,0);
					playerSub.GetComponent<BubbleSpawner>().enabled = false;
					playerSub.GetComponent<PlayerController>().enabled = false;
					followerComponent.transform.position = Vector3.zero;
					followerComponent.enabled = false;
					this.Depth = 0;
					this.UIFadedIn = false;
					this.CanTakeOxygen = false;
					uiAnimator.RunAnimationIn();
					break;
				case GameState.Transition:
					uiAnimator.RunAnimationOut();
					ropeAnimator.RunRopeAnimation();
					break;
				case GameState.Game:
					this.OxygenAmount = PlayerPrefHelper.LoadFloat(PlayerPrefHelper.OXYGEN_AMOUNT, 30);
					this.CurrentOxygen = this.OxygenAmount;
					AudioManager.Instance.StopSFX(SFXList.Crank);
					AudioManager.Instance.PlaySFX(SFXList.Fan);
					playerSub.GetComponent<BubbleSpawner>().enabled = true;
					playerSub.GetComponent<PlayerController>().enabled = true;
					playerSub.GetComponent<PlayerController>().OnDeath = () => {
						AudioManager.Instance.PlaySFX(SFXList.DieCrash);
						SwitchState(GameState.Dead);
					};
					followerComponent.enabled = true;
					ropeAnimator.RunRopePullUpAnimation();
					
					bool isInstructions = PlayerPrefHelper.LoadBool(PlayerPrefHelper.HAS_PLAYED_BEFORE);
					if (!isInstructions) {
						instructionUI.RunAnimationIn();
					}
					break;
				case GameState.Dead:
					AudioManager.Instance.StopSFX(SFXList.Crank);
					AudioManager.Instance.StopSFX(SFXList.Fan);
					CheckDepth();
					SwitchState(GameState.Shop);
					uiAnimator.RunGameUIAnimationOut();
					break;
			}
		}

		private void CalculateDistance() {
			float d = Mathf.Abs(playerSub.position.y - waterLine.position.y);
			this.Depth = d * distanceMultiplier;
		}

		private void CheckDepth() {
			float v = PlayerPrefHelper.LoadFloat(PlayerPrefHelper.FURTHEST_DEPTH);
			float highest = v > this.Depth ? v : this.Depth;
			PlayerPrefHelper.SaveFloat(PlayerPrefHelper.FURTHEST_DEPTH, (int)highest);
			SetFurthestTextBoard((int)highest);
		}

		private void SetFurthestTextBoard(int v) {
			furthestDepthText.text = $"{v:000000000}";
		}

		private void CheckUIMarker() {
			if (!this.UIFadedIn && playerSub.position.y < uiFadePoint.position.y) {
				this.UIFadedIn = true;
				uiAnimator.RunGameUIAnimationIn();
				this.CanTakeOxygen = true;
			}
		}

		private void CheckInstructions() {
			bool isInstructions = PlayerPrefHelper.LoadBool(PlayerPrefHelper.HAS_PLAYED_BEFORE);
			if (!isInstructions && playerSub.GetComponent<PlayerController>().Vertical < 0) {
				instructionUI.RunAnimationOut();
				PlayerPrefHelper.SaveBool(PlayerPrefHelper.HAS_PLAYED_BEFORE, 1);
			}
		}

		private void TakeOxygen() {
			if (this.CanTakeOxygen) {
				this.CurrentOxygen -= Time.deltaTime;
				if (this.CurrentOxygen <= 0) {
					AudioManager.Instance.PlaySFX(SFXList.DieAir);
					SwitchState(GameState.Dead);
				}
			}
		}

		#endregion

	}
}