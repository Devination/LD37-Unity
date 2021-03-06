﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {
    public void LoadLevel ( int levelIndex ) {
		if ( !SaveUtils.Loaded() ) {
			SaveUtils.Load();
		}
        SceneManager.LoadScene( levelIndex );
    }
}
