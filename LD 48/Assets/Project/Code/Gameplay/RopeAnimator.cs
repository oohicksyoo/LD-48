using System.Collections;
using Assets.Project.Code.Audio;
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

		[SerializeField]
		private Transform cog;

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
			cog.rotation = Quaternion.Euler(0,0,0);
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
			bool playedSubHitWaterAudio = false;
			AudioManager.Instance.PlaySFX(SFXList.Crank);
			ropeMaterial.SetFloat("_Visibility", start);
			for (float i = 0; i < animationLength; i += Time.deltaTime) {
				float t = Mathf.Lerp(start, end, i / animationLength);
				ropeMaterial.SetFloat("_Visibility", t);
				cog.rotation = Quaternion.Euler(0,0,cog.rotation.eulerAngles.z + 30 * Time.deltaTime);
				
				if (t > 0.3f) {
					if (!playedSubHitWaterAudio) {
						playedSubHitWaterAudio = true;
						AudioManager.Instance.PlaySFX(SFXList.SubHitWater);
						//TODO: Spawn a shit ton of bubbles
					}
				}
				
				yield return null;
			}
			ropeMaterial.SetFloat("_Visibility", end);
			AudioManager.Instance.StopSFX(SFXList.Crank);
		}
		
		private IEnumerator OnRunRopePullUpAnimation() {
			bool isPlayingCrankAudio = false;
			ropeMaterial.SetFloat("_Visibility", end);
			for (float i = 0; i < 4; i += Time.deltaTime) {
				float t = Mathf.Lerp(end, start, i / 4);
				ropeMaterial.SetFloat("_Visibility", t);

				if (i / 4 > 0.4f) {
					if (!isPlayingCrankAudio) {
						isPlayingCrankAudio = true;
						AudioManager.Instance.PlaySFX(SFXList.Crank);
					}

					cog.rotation = Quaternion.Euler(0, 0, cog.rotation.eulerAngles.z - 30 * Time.deltaTime);
				}

				yield return null;
			}
			ropeMaterial.SetFloat("_Visibility", start);
			AudioManager.Instance.StopSFX(SFXList.Crank);
		}

		#endregion

	}
}