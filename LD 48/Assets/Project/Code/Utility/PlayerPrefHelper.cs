using UnityEngine;

namespace Assets.Project.Code.Utility {
	public static class PlayerPrefHelper {

		public const string HAS_PLAYED_BEFORE = "Gameplay.HasPlayedBefore";
		public const string FURTHEST_DEPTH = "Gameplay.FurthestDepth";
		
		public static void SaveFloat(string name, float value) {
			PlayerPrefs.SetFloat(name, value);
		}
		
		public static void SaveBool(string name, int value) {
			PlayerPrefs.SetInt(name, value);
		}
		
		public static float LoadFloat(string name, float value = 0) {
			return PlayerPrefs.GetFloat(name, value);
		}
		
		public static bool LoadBool(string name, int value = 0) {
			return PlayerPrefs.GetInt(name, value) != 0;
		}

	}
}