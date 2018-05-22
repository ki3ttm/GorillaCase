using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowSetting : MonoBehaviour {

	[SerializeField]
	TextMesh mText;

	// Use this for initialization
	void Start () {
		Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
	}
	
	// Update is called once per frame
	void Update () {
		mText.text = "";

		if(Input.GetKeyDown(KeyCode.Z)) {
			SaveScreenShot();
		}
	}

	void OnGUI() {

		GUILayout.BeginVertical();

		/*
		if (GUILayout.Button("ScreenShot", GUILayout.Width(300.0f), GUILayout.Height(100.0f))) {
			SaveScreenShot();
		}
		*/

		/*
		int i = 0;
		while (i < 5) {
			int lStageNum = i + 1;
			string lStageName = "Stage1-" + lStageNum;
			if (GUILayout.Button(lStageName, GUILayout.Width(300.0f), GUILayout.Height(100.0f))) {
				UnityEngine.SceneManagement.SceneManager.LoadScene(lStageName);
			}
			i++;
		}
		*/
		GUILayout.EndVertical();
	}

	void SaveScreenShot() {

		if (!System.IO.Directory.Exists(GetScreenShotPath())) {
			System.IO.Directory.CreateDirectory(GetScreenShotPath());
		};

		float max = Mathf.Max(Screen.width, Screen.height);
		int scale = Mathf.RoundToInt(1960 / max);
		string lNowDate = string.Format("{0:00}{1:00}-{2:00}{3:00}-{4:00}{5:00}", System.DateTime.Now.Month, System.DateTime.Now.Day, System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second, System.DateTime.Now.Millisecond / 10);
		string lFileName = string.Format("{0}/MassShift_{1}.png", GetScreenShotPath(), lNowDate);

		//CaptureScreenShotAlpha(lFileName);
		Application.CaptureScreenshot(lFileName, scale);
	}

	string GetScreenShotPath() {

		string lSavePath = Application.dataPath + "/../";
		//string lSavePath = Application.pernantokaPath + "/";
#if UNITY_EDITOR
		lSavePath = "";
#endif

		string lSaveFolderPath = lSavePath + "ScreenShot";

		return lSaveFolderPath;
	}

	void CaptureScreenShotAlpha(string aFilePath) {

		var lHeight = Screen.height;
		var lWidth = Screen.width;
		var lTex = new Texture2D(lWidth, lHeight, TextureFormat.ARGB32, false);
		lTex.ReadPixels(new Rect(0, 0, lWidth, lHeight), 0, 0);
		lTex.Apply();

		var lBytes = lTex.EncodeToPNG();
		Destroy(lTex);

		System.IO.File.WriteAllBytes(aFilePath, lBytes);
	}
}
