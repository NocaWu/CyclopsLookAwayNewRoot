using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public static class UTIL_SceneLoader  {

	public static void loadScene(string sceneName){
		//SceneManager.LoadScene (sceneName);
		SceneManager.LoadSceneAsync(sceneName);
	}


}
