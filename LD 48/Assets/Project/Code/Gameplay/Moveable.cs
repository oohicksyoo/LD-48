using UnityEngine;

namespace Assets.Project.Code.Gameplay {
	public abstract class Moveable : MonoBehaviour {

		#region Serialized Fields

		[Header("Moveable")]
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		#endregion
		
		
		#region Properties

		protected Vector2 Velocity {
			get;
			set;
		}

		public bool IsFacingRight {
			get;
			protected set;
		} = true;

		#endregion


		#region Protected Functions

		public virtual void Update() {
			transform.position += new Vector3(this.Velocity.x, this.Velocity.y, 0);
			this.Velocity *= 0.9f;
		}

		protected virtual void Flip() {
			this.IsFacingRight = !this.IsFacingRight;
			spriteRenderer.flipX = !this.IsFacingRight;
		}

		#endregion

	}
}