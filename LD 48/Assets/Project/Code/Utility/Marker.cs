using UnityEngine;

namespace Assets.Project.Code.Utility {
	public class Marker : MonoBehaviour {

		[SerializeField]
		private float lineLength;
		
		public void OnDrawGizmos() {
			Gizmos.color = Color.cyan;
			float half = lineLength * 0.5f;
			Gizmos.DrawLine(transform.position + (Vector3.left * half), transform.position + (Vector3.right * half));
		}

	}
}