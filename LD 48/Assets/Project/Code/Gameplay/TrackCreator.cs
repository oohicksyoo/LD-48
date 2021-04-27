using System.Collections.Generic;
using UnityEngine;

namespace Assets.Project.Code.Gameplay {
	public class TrackCreator : MonoBehaviour {

		#region Serialized Fields

		[SerializeField]
		private Transform trackMiddle;

		[SerializeField]
		private float trackOffset;

		[SerializeField]
		private Transform player;

		[SerializeField]
		private Transform container;

		[Header("Pieces")]
		[SerializeField]
		private List<GameObject> trackPrefabs;

		#endregion


		#region Private Variables

		private Queue<GameObject> trackPieces;

		#endregion


		#region Properties

		private Queue<GameObject> TrackPieces {
			get {
				return trackPieces = (trackPieces != null) ? trackPieces : new Queue<GameObject>();
			}
		}

		private Vector3 StartPosition {
			get;
			set;
		}

		#endregion
		
		
		#region MonoBehaviour

		public void Start() {
			this.StartPosition = trackMiddle.position;
		}

		public void Update() {
			//Check if player y pos if < trackMiddle
			//If < then move track middle down by offset and generate a new piece
			if (player.position.y < trackMiddle.position.y) {
				trackMiddle.position -= new Vector3(0, trackOffset, 0);
				GameObject go = Instantiate(trackPrefabs[Random.Range(0, trackPrefabs.Count - 1)], trackMiddle.position, Quaternion.identity, container);
				this.TrackPieces.Enqueue(go);
				
				//Cleanup old track pieces off screen
				if (this.TrackPieces.Count > 2) {
					DestroyImmediate(this.trackPieces.Dequeue());
				}
			}
		}

		#endregion


		#region Public Functions

		public void ResetTrackCreator() {
			trackMiddle.position = this.StartPosition;
			while (this.TrackPieces.Count > 0) {
				Destroy(this.trackPieces.Dequeue());
			}
		}

		#endregion
		
	}
}