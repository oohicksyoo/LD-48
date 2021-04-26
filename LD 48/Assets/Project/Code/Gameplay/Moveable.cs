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

		protected virtual void Flip() {
			this.IsFacingRight = !this.IsFacingRight;
			spriteRenderer.flipX = !this.IsFacingRight;
		}

		#endregion

	}
}