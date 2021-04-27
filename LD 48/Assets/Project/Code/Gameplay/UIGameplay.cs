using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Project.Code.Gameplay {
	public class UIGameplay : MonoBehaviour {

		[Header("Oxygen")]
		[SerializeField]
		private Image oxygenFill;

		[SerializeField]
		private TextMeshProUGUI oxygenText;
		
		public void Update() {
			var amount = GameManager.Instance.CurrentOxygen;
			oxygenFill.fillAmount = amount / GameManager.Instance.OxygenAmount;
			oxygenText.text = $"{(int)amount} second{((int)amount != 1 ? "s" : "")} left";
		}

	}
}