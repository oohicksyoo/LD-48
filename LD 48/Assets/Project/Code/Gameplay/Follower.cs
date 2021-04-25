using UnityEngine;

namespace Assets.Project.Code.Gameplay {
	public class Follower : MonoBehaviour {

		[SerializeField]
		private Transform follower;
		
		public void Update() {
			transform.position = new Vector3(0, follower.position.y, 0);
		}
	}
}