using UnityEngine;

namespace Assets.Project.Code.Gameplay {
	public class FishController : Moveable {

		#region Serialized Fields

		[Header("Fish")]
		[SerializeField]
		private float speed;

		[SerializeField]
		private Transform rightMarker;
		
		[SerializeField]
		private Transform leftMarker;

		[SerializeField]
		private bool useRightMarker = true;

		[Header("Motion")]
		[SerializeField]
		private float frequency;
		
		[SerializeField]
		private float amplitude;

		[SerializeField]
		[Range(0, 1)]
		private float phaseStart;
		
		#endregion
		
		
		#region Private Variables

		private float phase;

		#endregion
		
		
		private float Phase {
			get {
				return phase;
			}
			set {
				value %= 1;
				phase = value;
			}
		}


		#region MonoBehaviour

		public void Start() {
			this.Phase = phaseStart;
		}

		public override void Update() {

			this.Phase += Time.deltaTime * frequency;
			float theta = 2 * Mathf.PI * this.Phase;
			float sin = Mathf.Sin(theta);
			transform.position += new Vector3(0, sin * amplitude, 0);

			if (useRightMarker) {
				if (transform.position.x >= rightMarker.position.x) {
					var vect = transform.position;
					vect.x = leftMarker.position.x;
					transform.position =  vect;
				}
				this.Velocity += Vector2.right * speed * Time.deltaTime;
			} else {
				if (transform.position.x <= leftMarker.position.x) {
					var vect = transform.position;
					vect.x = rightMarker.position.x;
					transform.position =  vect;
				}
				this.Velocity -= Vector2.right * speed * Time.deltaTime;
			}
			base.Update();
		}

		#endregion
		

	}
}