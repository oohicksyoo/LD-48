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
			StopAllCoroutines();
			ropeMaterial.SetFloat("_Visibility", start);
		}

		public void RunRopeAnimation() {
			StartCoroutine(OnRunRopeAnimation());
		}

		public void RunRopePullUpAnimation() {
			StopAllCoroutines();
			StartCoroutine(OnRunRopePullUpAnimation());
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
		
		private IEnumerator OnRunRopePullUpAnimation() {
			ropeMaterial.SetFloat("_Visibility", end);
			for (float i = 0; i < 4; i += Time.deltaTime) {
				float t = Mathf.Lerp(end, start, i / 4);
				ropeMaterial.SetFloat("_Visibility", t);
				yield return null;
			}
			ropeMaterial.SetFloat("_Visibility", start);
		}

		#endregion

	}
}