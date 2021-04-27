using UnityEngine;

namespace Assets.Project.Code.Gameplay {
	public class PearlCollectible : MonoBehaviour {

		#region MonoBehaviour

		public void OnTriggerEnter2D(Collider2D collider2D) {
			GameManager.Instance.OnCollectible();
			Destroy(gameObject);
		}

		#endregion
		
	}
}