using System;
using UnityEngine;

namespace Assets.Project.Code.Gameplay {
	public class PlayerController : Moveable {

		#region Serialized Fields

		[Header("Sub")]
		[SerializeField]
		private Vector2 speed;

		[SerializeField]
		private float tiltSpeed;

		[SerializeField]
		private CircleCollider2D collider2D;

		#endregion


		#region Properties

		public float Vertical {
			get;
			private set;
		}

		public Action OnDeath {
			get;
			set;
		}

		#endregion
		
		
		#region MonoBehaviour

		public override void Update() {
			float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");
			vertical = Mathf.Clamp(vertical, -1, 0);
			this.Vertical = vertical;

			//Dont allow just down only angled down
			if (Mathf.Abs(horizontal) <= 0.1f) {
				vertical = 0;
			}

			//Slow down the diagonal movement
			if (vertical < 0 && horizontal != 0) {
				vertical *= 0.5f;
				horizontal *= 0.5f;
				
				//Tilt
				transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - horizontal * tiltSpeed * Time.deltaTime);
				if (!this.IsFacingRight) {
					if (transform.rotation.eulerAngles.z > 20) {
						transform.rotation = Quaternion.Euler(0,0,20);
					}
				} else {
					if (transform.rotation.eulerAngles.z - 360 < -20 && transform.rotation.eulerAngles.z - 360 > -25) {
						transform.rotation = Quaternion.Euler(0,0,-20);
					} 
				}
			} else {
				//Tilt back towards 0
				if (this.IsFacingRight) {
					float angle = transform.rotation.eulerAngles.z;
					transform.rotation = Quaternion.Euler(0, 0, angle + 1 * tiltSpeed * Time.deltaTime);
					if (angle < 1 || angle > 359) {
						transform.rotation = Quaternion.Euler(0,0,0);
					}
				} else {
					float angle = transform.rotation.eulerAngles.z;
					transform.rotation = Quaternion.Euler(0, 0, angle + -1 * tiltSpeed * Time.deltaTime);
					if (angle < 1 || angle > 359) {
						transform.rotation = Quaternion.Euler(0,0,0);
					}
				}
			}

			this.Velocity += new Vector2(horizontal, vertical) * speed * Time.deltaTime;

			if (this.Velocity.x < 0 && this.IsFacingRight || this.Velocity.x > 0 && !this.IsFacingRight) {
				Flip();

				if (vertical < 0 && horizontal > 0) {
					//Flip Tilt
					transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z * -1);
				}
			}
			
			
			base.Update();
		}

		public void OnCollisionEnter2D(Collision2D collision2D) {
			Debug.Log($"Collided");
			this.Velocity = Vector2.zero;
			this.OnDeath();
		}

		#endregion


		#region Protected Overrides

		protected override void Flip() {
			base.Flip();
			collider2D.offset *= new Vector2(-1, 1);
		}

		#endregion
		
	}
}