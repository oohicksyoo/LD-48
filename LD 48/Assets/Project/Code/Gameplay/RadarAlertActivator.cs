using System.Collections.Generic;
using System.Linq;
using Assets.Project.Code.Audio;
using Assets.Project.Code.Utility;
using UnityEngine;

namespace Assets.Project.Code.Gameplay {
	public class RadarAlertActivator : MonoBehaviour {

		#region Serialized Fields

		[SerializeField]
		private GameObject alert;

		#endregion


		#region Properties

		private bool IsPlayingAudio {
			get;
			set;
		}

		#endregion

		
		#region MonoBehaviour

		public void Start() {
			ResetAlert();
		}

		public void Update() {
			bool hasRadarUpgrade = PlayerPrefHelper.LoadBool(PlayerPrefHelper.UPGRADE_RADAR);
			var list = GameManager.Instance.Monsters.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).ToList();
			if (list.Any()) {
				if (hasRadarUpgrade && Vector3.Distance(transform.position, list[0].transform.position) <= 9 && transform.position.y > list[0].transform.position.y) {
					
					alert.SetActive(true);
					if (!this.IsPlayingAudio) {
						this.IsPlayingAudio = true;
						AudioManager.Instance.PlaySFX(SFXList.Radar);
					}
				} else {
					ResetAlert();
				}
			} else {
				ResetAlert();
			}
		}

		#endregion


		#region Public Functions

		public void ResetAlert() {
			alert.SetActive(false);
			this.IsPlayingAudio = false;
			AudioManager.Instance.StopSFX(SFXList.Radar);
		}

		#endregion
		
	}
}