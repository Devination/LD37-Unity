using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveUtils {
	public static void Save () {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create( Application.persistentDataPath + "/savedGame.save" );
		bf.Serialize( file, SaveGame.CurrentSaveGame );
		file.Close();
	}

	public static bool Loaded () {
		return SaveGame.CurrentSaveGame != null;
	}

	public static void Load () {
		if ( File.Exists( Application.persistentDataPath + "/savedGame.save" ) ) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open( Application.persistentDataPath + "/savedGame.save", FileMode.Open );
			SaveGame.CurrentSaveGame = ( SaveGame )bf.Deserialize( file );
			file.Close();
		}
		else {
			SaveGame.CurrentSaveGame = new global::SaveGame();
		}
	}
}
