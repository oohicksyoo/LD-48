using System.Collections.Generic;
using UnityEngine;

namespace Assets.Project.Code.Audio {
	public class AudioManager : MonoBehaviour {

		#region Serialized Fields

		[SerializeField]
		private List<SFXEntry> entries;

		#endregion
		
		
		#region Properties

		public static AudioManager Instance {
			get;
			private set;
		}

		#endregion


		#region MonoBehaviour

		public void Awake() {
			Instance = this;
		}

		#endregion


		#region Public Functions

		public void PlaySFX(int type) {
			PlaySFX((SFXList)type);
		}

		public void PlaySFX(SFXList type) {
			foreach (var entry in entries) {
				if (entry.type == type) {
					entry.sound.Play();
				}
			}
		}

		public void StopSFX(SFXList type) {
			foreach (var entry in entries) {
				if (entry.type == type) {
					entry.sound.Stop();
				}
			}
		}

		#endregion
		
	}
}