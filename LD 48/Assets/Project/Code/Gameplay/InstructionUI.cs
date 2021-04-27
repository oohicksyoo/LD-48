using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;

namespace Assets.Project.Code.Gameplay {
	public class InstructionUI : MonoBehaviour {

		#region Serialized Fields

		[SerializeField]
		private List<SpriteRenderer> sprites;

		[SerializeField]
		private List<TextMeshPro> textFields;

		#endregion


		#region Public Functions

		public void ResetInstructions() {
			Color c = Color.white;
			c.a = 0;
			SetAllToAlpha(c);
		}

		public void RunAnimationIn() {
			StartCoroutine(OnRunAnimationIn());
		}
		
		public void RunAnimationOut() {
			StartCoroutine(OnRunAnimationOut());
		}

		#endregion


		#region Private Functions

		private IEnumerator OnRunAnimationIn() {
			Color c = Color.white;
			for (float i = 0; i < 0.5f; i += Time.deltaTime) {
				c.a = Mathf.Lerp(0, 1, i / 0.5f);
				SetAllToAlpha(c);
				yield return null;
			}
			
		}
		
		private IEnumerator OnRunAnimationOut() {
			Color c = Color.white;
			for (float i = 0; i < 0.5f; i += Time.deltaTime) {
				c.a = Mathf.Lerp(1, 0, i / 0.5f);
				SetAllToAlpha(c);
				yield return null;
			}
		}

		private void SetAllToAlpha(Color c) {
			foreach (var spriteRenderer in sprites) {
				spriteRenderer.color = c;
			}	
			
			foreach (var textMeshPro in textFields) {
				textMeshPro.color = c;
			}
		}

		#endregion

	}
}