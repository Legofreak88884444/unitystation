﻿using UnityEngine;
using UnityEngine.SceneManagement;
using Lobby;

public class PlayerManager : MonoBehaviour
{
	private static PlayerManager playerManager;
	public static GameObject LocalPlayer { get; private set; }

	public static Equipment Equipment { get; private set; }

	public static PlayerScript LocalPlayerScript { get; private set; }

	//For access via other parts of the game
	public static PlayerScript PlayerScript { get; private set; }

	public static bool HasSpawned { get; private set; }

	public static string PlayerNameCache => CurrentCharacterSettings.Name;

	public static CharacterSettings CurrentCharacterSettings { get; set; }

	public static PlayerManager Instance
	{
		get
		{
			if (!playerManager)
			{
				playerManager = FindObjectOfType<PlayerManager>();
			}

			return playerManager;
		}
	}

	void Start()
	{
		if(!PlayerPrefs.HasKey("currentcharacter")){
			PlayerPrefs.SetString("currentcharacter", JsonUtility.ToJson(new CharacterSettings()));
			PlayerPrefs.Save();
		}

		CurrentCharacterSettings = JsonUtility.FromJson<CharacterSettings>(PlayerPrefs.GetString("currentcharacter"));
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		Reset();
	}

	public static void Reset()
	{
		HasSpawned = false;
	}

	public static void SetPlayerForControl(GameObject playerObjToControl)
	{
		LocalPlayer = playerObjToControl;
		LocalPlayerScript = playerObjToControl.GetComponent<PlayerScript>();

		PlayerScript =
			LocalPlayerScript; // Set this on the manager so it can be accessed by other components/managers
		Camera2DFollow.followControl.target = LocalPlayer.transform.Find("Sprites");

		HasSpawned = true;
	}

	public static bool PlayerInReach(Transform transform)
	{
		if (PlayerScript != null)
		{
			return PlayerScript.IsInReach(transform.position);
		}
		return false;
	}
}