using UnityEngine;

namespace Assets.Project.Code.Gameplay {
	public class Monster : MonoBehaviour {

		public void Start() {
			GameManager.Instance.Monsters.Add(this);	
		}

		public void OnDestroy() {
			GameManager.Instance.Monsters.Remove(this);
		}
	}
}