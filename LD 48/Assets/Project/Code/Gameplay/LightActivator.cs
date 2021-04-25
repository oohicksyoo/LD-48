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
			if (!this.IsActivated && transform.position.y <= lightActivatorPosition.position.y) {
				this.IsActivated = true;
				light.SetActive(true);
			}
		}

		#endregion


		#region Public Functions

		public void ResetLight() {
			light.SetActive(false);
		}

		#endregion
		
		
	}
}