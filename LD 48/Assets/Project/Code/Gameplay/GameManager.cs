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
		private Follower followerComponent;

		[SerializeField]
		private UIAnimator uiAnimator;

		[SerializeField]
		private RopeAnimator ropeAnimator;

		[SerializeField]
		private LightActivator lightActivator;

		[SerializeField]
		private TextMeshPro furthestDepthText;

		[Header("Gameplay")]
		[SerializeField]
		private float distanceMultiplier = 1;

		#endregion
		
		
		#region Properties

		public GameState GameState {
			get;
			private set;
		} = GameState.None;

		public float Depth {
			get;
			private set;
		}

		#endregion


		#region MonoBehaviour

		public void Start() {
			CheckDepth();
		}

		public void Update() {
			StateUpdate();
		}

		#endregion


		#region Private Functions

		private void StateUpdate() {
			switch (this.GameState) {
				case GameState.None:
					SwitchState(GameState.Shop);
					break;
				case GameState.Shop:
					if (Input.GetKeyDown(KeyCode.Space)) {
						SwitchState(GameState.Transition);
					}
					break;
				case GameState.Transition:
					playerSub.position += Vector3.down * 0.5f * Time.deltaTime;
					if (playerSub.position.y < releasePoint.position.y) {
						SwitchState(GameState.Game);	
					}
					break;
				case GameState.Game:
					CalculateDistance();
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
					ropeAnimator.ResetRope();
					lightActivator.ResetLight();
					playerSub.position = spawnPoint.position;
					playerSub.rotation = Quaternion.Euler(0,0,0);
					playerSub.GetComponent<BubbleSpawner>().enabled = false;
					playerSub.GetComponent<PlayerController>().enabled = false;
					followerComponent.transform.position = Vector3.zero;
					followerComponent.enabled = false;
					this.Depth = 0;
					uiAnimator.RunAnimationIn();
					break;
				case GameState.Transition:
					uiAnimator.RunAnimationOut();
					ropeAnimator.RunRopeAnimation();
					break;
				case GameState.Game:
					playerSub.GetComponent<BubbleSpawner>().enabled = true;
					playerSub.GetComponent<PlayerController>().enabled = true;
					playerSub.GetComponent<PlayerController>().OnDeath = () => {
						SwitchState(GameState.Dead);
					};
					followerComponent.enabled = true;
					ropeAnimator.RunRopePullUpAnimation();
					break;
				case GameState.Dead:
					CheckDepth();
					SwitchState(GameState.Shop);
					break;
			}
		}

		private void CalculateDistance() {
			float d = Mathf.Abs(playerSub.position.y - waterLine.position.y);
			this.Depth = d * distanceMultiplier;
			Debug.Log($"Distance: {d} = {d * distanceMultiplier}");
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

		#endregion

	}
}