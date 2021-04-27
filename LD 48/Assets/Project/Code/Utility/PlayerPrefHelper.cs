using UnityEditor;
using UnityEngine;

namespace Assets.Project.Code.Utility {
	public static class PlayerPrefHelper {

		public const string HAS_PLAYED_BEFORE = "Gameplay.HasPlayedBefore";
		public const string FURTHEST_DEPTH = "Gameplay.FurthestDepth";
		public const string PEARL_COUNT = "Gameplay.PearlCount";
		public const string OXYGEN_AMOUNT = "Gameplay.OxygenAmount";
		public const string UPGRADE_LIGHT = "Gameplay.Light";
		public const string UPGRADE_RADAR = "Gameplay.Radar";
		
		
		public static void SaveFloat(string name, float value) {
			PlayerPrefs.SetFloat(name, value);
		}
		
		public static void SaveBool(string name, int value) {
			PlayerPrefs.SetInt(name, value);
		}
		
		public static void SaveInt(string name, int value) {
			PlayerPrefs.SetInt(name, value);
		}
		
		public static float LoadFloat(string name, float value = 0) {
			return PlayerPrefs.GetFloat(name, value);
		}
		
		public static bool LoadBool(string name, int value = 0) {
			return PlayerPrefs.GetInt(name, value) != 0;
		}
		
		public static int LoadInt(string name, int value = 0) {
			return PlayerPrefs.GetInt(name, value);
		}

		[MenuItem("LD48/Clear Playerprefs")]
		public static void ClearAll() {
			PlayerPrefs.DeleteAll();
		}

	}
}