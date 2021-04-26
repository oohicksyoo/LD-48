using System;
using UnityEngine;

namespace Assets.Project.Code.Gameplay {
	public class Bubble : MonoBehaviour {

		#region Serialized Fields

		[SerializeField]
		private float lifeTime;

		[SerializeField]
		[Range(0, 50)]
		private float raisingSpeed;

		[SerializeField]
		private AnimationCurve bubbleSize;
		
		[SerializeField]
		private AnimationCurve bubbleAlpha;

		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[Header("Sin Wave")]
		[SerializeField]
		[Range(0, 5)]
		private float frequency = 1;

		[SerializeField]
		[Range(0, 5)]
		private float amplitude = 1;

		#endregion


		#region Private Variables

		private float phase;

		#endregion


		#region Properties

		public Action OnCompleteAction {
			get;
			set;
		}

		public float StartingPhase {
			get;
			set;
		}

		private float Phase {
			get {
				return phase;
			}
			set {
				if (value > 1) {
					//Did one cycle
				}

				value %= 1;
				phase = value;
			}
		}

		private float ElaspedTime {
			get;
			set;
		}

		#endregion


		#region MonoBehaviour

		public void Start() {
			this.Phase = this.StartingPhase;
			transform.localScale = Vector3.zero;
		}

		public void FixedUpdate() {
			this.ElaspedTime += Time.fixedDeltaTime;

			if (this.ElaspedTime > lifeTime) {
				DestroyImmediate(gameObject);
				return;
			}

			this.Phase += Time.fixedDeltaTime * frequency;
			float theta = 2 * Mathf.PI * this.Phase;
			float sin = Mathf.Sin(theta);
			transform.position += new Vector3(sin * amplitude, Time.fixedDeltaTime * raisingSpeed, 0);

			float t = this.ElaspedTime / lifeTime;
			float s = bubbleSize.Evaluate(t);
			transform.localScale = new Vector3(s, s, s);

			Color c = Color.white;
			c.a = bubbleAlpha.Evaluate(t);
			spriteRenderer.color = c;
		}

		#endregion
	}
}