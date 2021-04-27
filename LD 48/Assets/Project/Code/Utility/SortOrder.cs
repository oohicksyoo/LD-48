using UnityEngine;

namespace Assets.Project.Code.Utility {
	public class SortOrder : MonoBehaviour {

		#region Serialized Fields

		[SerializeField]
		private MeshRenderer meshRenderer;

		[SerializeField]
		private string layer = "Default";

		[SerializeField]
		private int order;
		
		#endregion


		#region MonoBehaviour

		public void Awake() {
			meshRenderer.sortingLayerName = layer;
			meshRenderer.sortingOrder = order;
		}

		#endregion

	}
}