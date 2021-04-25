using System.Collections;
using UnityEngine;

namespace Assets.Project.Code.Gameplay {
	public class RopeAnimator : MonoBehaviour {

		#region Serialized Fields

		[SerializeField]
		private Material ropeMaterial;

		[SerializeField]
		private float start;

		[SerializeField]
		private float end;

		[SerializeField]
		private float animationLength;

		#endregion


		#region MoneBehaviour

		public void OnDestroy() {
			ResetRope();
		}

		#endregion
		

		#region Public Functions

		public void ResetRope() {
			ropeMaterial.SetFloat("_Visibility", start);
		}

		public void RunRopeAnimation() {
			StartCoroutine(OnRunRopeAnimation());
		}

		#endregion


		#region Private Functions

		private IEnumerator OnRunRopeAnimation() {
			ropeMaterial.SetFloat("_Visibility", start);
			for (float i = 0; i < animationLength; i += Time.deltaTime) {
				float t = Mathf.Lerp(start, end, i / animationLength);
				ropeMaterial.SetFloat("_Visibility", t);
				yield return null;
			}
			ropeMaterial.SetFloat("_Visibility", end);
		}

		#endregion

	}
}