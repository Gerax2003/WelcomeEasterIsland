///-----------------------------------------------------------------
/// Author : Adrien Lemaire
/// Date : 06/09/2022 11:04
///-----------------------------------------------------------------

using Com.DefaultCompany.EasterIsland;
using Com.DefaultCompany.EasterIsland.EasterIsland;
using Com.DefaultCompany.EasterIsland.EasterIsland.Gameplay;
using Com.DefaultCompany.EasterIsland.EasterIsland.Gameplay.Player;
using Com.DefaultCompany.EasterIsland.EasterIsland.UI;
using Com.Isartdigital.EasterIsland.EasterIsland.Gameplay;
using Com.Isartdigital.EasterIsland.EasterIsland.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.Isartdigital.EasterIsland.EasterIsland.Managers {

	[DisallowMultipleComponent]
	public class GameManager : MonoBehaviour 
	{
		public static readonly string groundTag = "Ground";
		public static readonly string playerTag = "Player";
		public static readonly string pauseControlName = "echap";

		public static GameManager Instance = default;
		public static float _gravity { get; private set; }

		[Header("UI")]
		[SerializeField] private TitleCard titleCard = default;
		[SerializeField] private GameOver gameOver = default;
		[SerializeField] private GameWin gameWin = default;
		[SerializeField] private Pause pause = default;
		[SerializeField] private Hud hud = default;

		[Header("Objects")]
		[SerializeField] private Ocean ocean = default;
		[SerializeField] private PlayerMovement startingPlayer = default;
		[SerializeField] private List<LevelParams> humanList = new List<LevelParams>();
		[SerializeField] private GameObject player = default;

		[Header("Values")]
		[SerializeField] private float gravity = 0f;
		[SerializeField] private List<OceanParams> oceanLevels = new List<OceanParams>();
		private int currentOceanLevel = 0;
		private int currentHumanNumber = 0;

		private float indexCheat = 0f;

		[SerializeField]
		Cinemachine.CinemachineFreeLook cameraController;

		private Action DoAction;

		#region Unity Methods
		private void Awake()
		{
			Instance = this;
			_gravity = gravity;
			
			cameraController = FindObjectOfType<Cinemachine.CinemachineFreeLook>();

			TitleCard.OnPlay += TitleCard_OnPlay;
            GameOver.OnQuitGameOver += GameOver_OnQuitGameOver;
            GameOver.OnRetry += GameOver_OnRetry;
            GameWin.OnNext += GameWin_OnNext;
            Pause.OnResume += Pause_OnResume;
            Pause.OnQuit += Pause_OnQuit;
			Human.OnDeath += Human_OnDeath;
            TouristDeathAnim.OnExit += TouristDeathAnim_OnExit;
            StatueUnlock.OnGetCloseToStatue += StatueUnlock_OnGetCloseToStatue;
            StatueUnlock.OnGetFarFromStatue += StatueUnlock_OnGetFarFromStatue;

			//Instantiate(player, new Vector3(184.81f, 67.22f, 349.03f), Quaternion.identity);

			SetModeUI();

			SetFirstScreen();
		}

        private void Update()
		{
			DoAction();
		}
		#endregion

		#region State Machine
		private void SetModePlay()
        {
			if (cameraController) cameraController.gameObject.SetActive(true);
			PlayerMovement.dontMove = false;
            Cursor.visible = false;

			if (startingPlayer) startingPlayer.GetComponent<Rigidbody>().isKinematic = false;
			if (startingPlayer) startingPlayer.GetComponent<Rigidbody>().useGravity = true;

			if (hud) hud.StartActivate();

			PlayerAttack.isUpdate = true;
			PlayerAttackJumper.isUpdate = true;

			DoAction = DoActionPlay;
		}

		private void SetModeUI()
        {
			Debug.Log("entering ui");
			cameraController.gameObject.SetActive(false);
			PlayerMovement.dontMove = true;
            Cursor.visible = true;

			if (startingPlayer) startingPlayer.GetComponent<Rigidbody>().isKinematic = true;
			if (startingPlayer) startingPlayer.GetComponent<Rigidbody>().useGravity = false;

			hud.StartDeactivate();

			PlayerAttack.isUpdate = false;
			PlayerAttackJumper.isUpdate = false;

			DoAction = DoActionUI;
        }

		private void DoActionPlay()
        {
			Cheat();

            for (int i = 0; i < humanList[currentOceanLevel].list.Count; i++)
            {
                humanList[currentOceanLevel].list[i].GameLoop();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
				PauseGame();
		}

		private void DoActionUI(){
			for (int i = 0; i < humanList[currentOceanLevel].list.Count; i++)
			{
				humanList[currentOceanLevel].list[i].GameLoop();
			}
		}
		#endregion

		#region Events
		private void TitleCard_OnPlay(BaseScreen sender)
		{
			InitGame();
		}

		private void GameOver_OnRetry(BaseScreen sender)
		{
			DestroyGame();
			InitGame();
		}

		private void GameOver_OnQuitGameOver(BaseScreen sender)
		{
			QuitGame();
		}

		private void GameWin_OnNext(BaseScreen sender)
		{
			QuitGame();
		}

		private void Pause_OnResume(BaseScreen sender)
		{
			ResumeGame();
		}

		private void Pause_OnQuit(BaseScreen sender)
		{
			QuitGame();
		}

		private void Human_OnDeath(Human sender)
		{
			currentHumanNumber -= 1;

			hud.SetCounter(currentHumanNumber);

			CheckLastHuman(sender);
		}

		private void TouristDeathAnim_OnExit(TouristDeathAnim sender, Human human)
		{
			human.EndAnimDie();
		}

		private void StatueUnlock_OnGetFarFromStatue(StatueUnlock sender)
		{
			hud.SetKeyE(false);
		}

		private void StatueUnlock_OnGetCloseToStatue(StatueUnlock sender)
		{
			hud.SetKeyE(true);
		}
		#endregion

		#region Game State
		private void InitGame()
		{
			currentOceanLevel = 0;

			//Remettre les statues à leur place

            for (int i = 0; i < humanList[currentOceanLevel].list.Count; i++)
            {
				if (humanList[currentOceanLevel].list[i])
					humanList[currentOceanLevel].list[i].gameObject.SetActive(true);
            }

			currentHumanNumber = humanList[currentOceanLevel].list.Count;

			hud.SetCounter(currentHumanNumber);

			SetModePlay();
		}

		private void PauseGame()
        {
			SetModeUI();
			pause.StartActivate();
		}

		private void ResumeGame()
        {
			pause.StartDeactivate(SetModePlay);
		}

		private void QuitGame()
		{
			DestroyGame();
			titleCard.StartActivate();

			SetModeUI();
		}

		private void DestroyGame()
        {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
		#endregion

		private void Cheat()
        {
			if (Input.GetKeyDown(KeyCode.F1) && indexCheat == 0)
            {
				currentOceanLevel = 1;
				ocean.PullDownOcean(oceanLevels[currentOceanLevel].levelHeight,oceanLevels[currentOceanLevel].levelDownTime,oceanLevels[currentOceanLevel].delay);

				for (int i = 0; i < humanList[currentOceanLevel].list.Count; i++)
                {
					humanList[currentOceanLevel].list[i].gameObject.SetActive(true);
                }

				indexCheat += 1;
			}
			else if (Input.GetKeyDown(KeyCode.F2) && indexCheat == 1)
            {
				currentOceanLevel = 2;
				ocean.PullDownOcean(oceanLevels[currentOceanLevel].levelHeight, oceanLevels[currentOceanLevel].levelDownTime, oceanLevels[currentOceanLevel].delay);

				for (int i = 0; i < humanList[currentOceanLevel].list.Count; i++)
				{
					humanList[currentOceanLevel].list[i].gameObject.SetActive(true);
				}

				indexCheat += 1;
			}
			else if (Input.GetKeyDown(KeyCode.F3) && indexCheat == 2)
            {
				currentOceanLevel = 3;
				ocean.PullDownOcean(oceanLevels[currentOceanLevel].levelHeight, oceanLevels[currentOceanLevel].levelDownTime, oceanLevels[currentOceanLevel].delay);

				for (int i = 0; i < humanList[currentOceanLevel].list.Count; i++)
				{
					humanList[currentOceanLevel].list[i].gameObject.SetActive(true);
				}

				indexCheat += 1;
			}
        }

		private void CheckLastHuman(Human human)
        {
			humanList[currentOceanLevel].list.Remove(human);

			if (humanList[currentOceanLevel].list.Count == 0)
				NextLevel();
		}

		private void NextLevel()
        {
			currentOceanLevel += 1;

			List<Human> _humanList = humanList[currentOceanLevel].list;

			currentHumanNumber = _humanList.Count;

            hud.SetCounter(currentHumanNumber);

            if (oceanLevels.Count >= currentOceanLevel + 1)
			{
				for (int i = 0; i < _humanList.Count; i++)
				{
					_humanList[i].gameObject.SetActive(true);
				}

				OceanParams oceanParams = oceanLevels[currentOceanLevel];

				ocean.PullDownOcean(oceanParams.levelHeight, oceanParams.levelDownTime, oceanParams.delay);
			}
			else
			{
				SetModeUI();
				gameWin.StartActivate();
			}
		}

		private void SetFirstScreen()
		{
			titleCard.GetComponent<Animator>().SetTrigger(BaseScreen.active);
			titleCard.ActivateButtons();
		}
	}

	[Serializable]
	public class LevelParams
    {
		public List<Human> list = new List<Human>();
    }
}
