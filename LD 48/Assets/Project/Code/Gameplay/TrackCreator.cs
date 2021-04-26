﻿using System.Collections.Generic;
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

		#endregion
		
		
		#region MonoBehaviour

		public void Update() {
			//Check if player y pos if < trackMiddle
			//If < then move track middle down by offset and generate a new piece
			if (player.position.y < trackMiddle.position.y) {
				trackMiddle.position -= new Vector3(0, trackOffset, 0);
				GameObject go = Instantiate(trackPrefabs[0], trackMiddle.position, Quaternion.identity, container);
				this.TrackPieces.Enqueue(go);
				
				//Cleanup old track pieces off screen
				if (this.TrackPieces.Count > 2) {
					DestroyImmediate(this.trackPieces.Dequeue());
				}
			}
		}

		#endregion
		
	}
}