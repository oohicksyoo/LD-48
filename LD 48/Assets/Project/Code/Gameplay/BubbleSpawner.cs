using System.Collections.Generic;
using UnityEngine;

namespace Assets.Project.Code.Gameplay {
	public class BubbleSpawner : MonoBehaviour {

		#region Serialized Fields

		[Header("Bubbles")]
		[SerializeField]
		private GameObject bubblePrefab;

		[SerializeField]
		private Transform bubbleContainer;

		[SerializeField]
		private Transform bubbleSpawnLocation;

		[SerializeField]
		private float bubbleSpawnTime;

		#endregion


		#region Private Variables

		private List<Bubble> bubbles;

		#endregion


		#region Properties

		private List<Bubble> Bubbles {
			get {
				return bubbles = (bubbles != null) ? bubbles : new List<Bubble>();
			}
		}

		private float BubbleSpawnTime {
			get;
			set;
		}

		#endregion


		#region MonoBehaviour

		public void Start() {
			
		}

		public void Update() {
			this.BubbleSpawnTime += Time.deltaTime;
			if (this.BubbleSpawnTime >= bubbleSpawnTime) {
				this.BubbleSpawnTime = 0;
				SpawnBubble();
			}
		}

		#endregion


		#region Private Functions

		private void SpawnBubble() {
			GameObject go = Instantiate(bubblePrefab, bubbleSpawnLocation.position, Quaternion.identity, bubbleContainer);
			Bubble bubble = go.GetComponent<Bubble>();
			bubble.OnCompleteAction = () => {
				//TODO: SpawnBubble
			};
			this.Bubbles.Add(bubble);
		}

		#endregion

	}
}