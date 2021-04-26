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
		private Follower followerComponent;

		[SerializeField]
		private UIAnimator uiAnimator;

		[SerializeField]
		private RopeAnimator ropeAnimator;

		[SerializeField]
		private LightActivator lightActivator;

		#endregion
		
		
		#region Properties

		public GameState GameState {
			get;
			private set;
		} = GameState.None;

		#endregion


		#region MonoBehaviour

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
					SwitchState(GameState.Shop);
					break;
			}
		}

		#endregion

	}
}