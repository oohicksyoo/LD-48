using Assets.Project.Code.Audio;
using Assets.Project.Code.Utility;
using UnityEngine;

namespace Assets.Project.Code.Gameplay {
	public class LightActivator : MonoBehaviour {

		#region Serialized Fields

		[SerializeField]
		private Transform lightActivatorPosition;

		[SerializeField]
		private GameObject light;

		#endregion


		#region Properties

		public bool IsActivated {
			get;
			private set;
		}

		#endregion


		#region MonoBehaviour

		public void Start() {
			ResetLight();
		}

		public void Update() {
			bool hasLightUpgrade = PlayerPrefHelper.LoadBool(PlayerPrefHelper.UPGRADE_LIGHT);
			if (!this.IsActivated && hasLightUpgrade && transform.position.y <= lightActivatorPosition.position.y) {
				AudioManager.Instance.PlaySFX(SFXList.LightClick);
				this.IsActivated = true;
				light.SetActive(true);
			}
		}

		#endregion


		#region Public Functions

		public void ResetLight() {
			light.SetActive(false);
			this.IsActivated = false;
		}

		#endregion
		
		
	}
}