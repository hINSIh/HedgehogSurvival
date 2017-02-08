using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Stage {
	public Sprite map;
	public Round[] rounds;
	public Enemy[] enemys;
}

[System.Serializable]//inspecter뷰에서 수정할 수 없음
public class Round
{
    public int monsterCount;
	public int survivalTime;
    public float spawnDelay;
	public float enemyStrength = 1;
}

public class RoundManager : MonoBehaviour
{
	private enum State {
		Spawning, AllKill, Survive, Idle
	}

    public ItemManager itemManager;

    [Header("UI")]
	public Text roundText;
	public Text deathProgressText;
	public Text timeProgressText;

	[Header("Random Spawn")]
	public float spawnViewportMargin;

	[Header("Player")]
	public Transform player;

	[Header("Map")]
	public SpriteRenderer background;

	public FadeOutLoadScene fadeOutLoadScene;

	public Stage[] stages;

	public float timeCounterDelay = 0.2f;

	private int stageIndex, roundIndex;
	private TitleManager titleManager;

	private const string titleRoundFormat = "Round {0} - {1}";
	private TitleSchedule messageAllKill = new TitleSchedule("All kill !!", 0.01f, 1.2f);
	private TitleSchedule messageSurvive = new TitleSchedule("Survive !!", 0.01f, 1.2f);
	private TitleSchedule messageRound = new TitleSchedule("", 0.01f, 1.5f);

	private Round currentRound;
	private float currentRoundTime;
	private int currentRoundKill;

	private float totalSurvivalTime;
	private int totalKill;

	private State roundState;

	private Transform enemyStorage;
	// Use this for initialization
	void Start()
	{
		Manager.RegisterManager(this);

		titleManager = Manager.Get<TitleManager>();
		enemyStorage = new GameObject("EnemyStorage").transform;

		CheckRounds();
		ResetRound();
		StartCoroutine(StartGame());
	}

	public void KillEnemy(int count) {
		currentRoundKill += count;
		totalKill += count;

		float percent = GetPercent(currentRoundKill, currentRound.monsterCount);
		deathProgressText.text = GetPercentFormat(percent);

		if (currentRoundKill >= currentRound.monsterCount) {
			roundState = State.AllKill;
		}
	}

	public IEnumerator GameOver() {
		Enemy enemy;

        for (int i = 0; i < enemyStorage.childCount; i++) {
			enemy = enemyStorage.GetChild(i).GetComponent<Enemy>();
			Destroy(enemy);
		}

		titleManager.AddSchedule(new TitleSchedule("Game Over...", 0.07f, 1.5f));
		yield return titleManager.Run();

		GameOverData data = new GameOverData();
		data.survivalTime = Mathf.RoundToInt(totalSurvivalTime);
		data.kill = totalKill;
		Manager.RegisterManager(data);

		fadeOutLoadScene.OnLoad();
		StopAllCoroutines();
        itemManager.gameSituation = false;//여기로 진입하면 멈춤
    }

	private IEnumerator StartGame()
	{
		for (stageIndex = 0; stageIndex < stages.Length; stageIndex++) {
			Stage stage = stages[stageIndex];
			background.sprite = stage.map;
			yield return StartRound(stage);
		}
        itemManager.gameSituation = false;//여기로 진입하면 멈춤
    }

	private IEnumerator StartRound(Stage stage) {
        for (roundIndex = 0; roundIndex < stage.rounds.Length; roundIndex++)
        { 
            roundText.text = string.Format("{0} - {1}", stageIndex + 1, roundIndex + 1);
			messageRound.message = string.Format(titleRoundFormat, stageIndex + 1, roundIndex + 1);
			titleManager.AddSchedule(messageRound);
			yield return titleManager.Run();

			currentRound = stage.rounds[roundIndex];
			roundState = State.Spawning;

			StartCoroutine(StartTimeCounter());
            itemManager.gameSituation = true; //여기로 진입하면 멈춤
            yield return StartMonsterSpawn(stage, currentRound);

            while (roundState == State.Spawning) {
				yield return null;
			}

			if (roundState == State.AllKill) {
				titleManager.AddSchedule(messageAllKill);
			} else {
				titleManager.AddSchedule(messageSurvive);
			}

			ResetRound();
		}
	}

	private IEnumerator StartMonsterSpawn(Stage stage, Round round) { 
		WaitForSeconds spawnDelay = new WaitForSeconds(round.spawnDelay);

		for (int count = 0; count < round.monsterCount; count++) {
			yield return spawnDelay;
			int randomMonb = Random.Range(0, stage.enemys.Length);
			Enemy enemy = stage.enemys[randomMonb];

			Enemy enemyObject = Instantiate(enemy.gameObject).GetComponent<Enemy>();
			enemyObject.name = "Enemy_" + (roundIndex + 1) + "R";
			enemyObject.transform.position = GetRandomSpawnPoint();
			enemyObject.transform.SetParent(enemyStorage);

			enemyObject.playerTransform = player;

			enemyObject.SetStrength(round.enemyStrength);
		}
	}

	private IEnumerator StartTimeCounter()
	{
		float percent = 0;
		while (percent < 100) {
			if (roundState == State.Idle) {
				yield break;
			}

			currentRoundTime += timeCounterDelay;
			totalSurvivalTime += timeCounterDelay;
			percent = GetPercent(currentRoundTime, currentRound.survivalTime);
			timeProgressText.text = GetPercentFormat(percent);
			yield return new WaitForSeconds(timeCounterDelay);
		}

		roundState = State.Survive;
	}

	private void ResetRound() {
		currentRoundTime = 0;
		currentRoundKill = 0;
		roundState = State.Idle;

		deathProgressText.text = GetPercentFormat(0);
		timeProgressText.text = GetPercentFormat(0);

		Enemy enemy;
		for (int i = 0; i < enemyStorage.childCount; i++) {
			enemy = enemyStorage.GetChild(i).GetComponent<Enemy>();
			enemy.Death(Enemy.DeathReason.RoundClear);
		}
	}

	private float GetPercent(float value, float maxValue) { 
		return Mathf.Min(100, value / maxValue * 100);
	}

	private string GetPercentFormat(float value) { 
		return string.Format("{0} %", Mathf.RoundToInt(value));
	}

	private void CheckRounds() {
		Stage stage;
		Round round;

		float survivalTime;
		float max;
		for (int stageIndex = 0; stageIndex < stages.Length; stageIndex++) {
			stage = stages[stageIndex];
			for (int roundIndex = 0; roundIndex < stage.rounds.Length; roundIndex++) {
				round = stage.rounds[roundIndex];
				survivalTime = round.survivalTime;
				max = Mathf.Max(survivalTime, round.spawnDelay * round.monsterCount);

				if (survivalTime < max) {
					Debug.LogWarningFormat("Value input warning !\n" +
					                       "stage: {0}\n" +
					                       "round: {1}\n" +
					                       "min survavalTime: {2}\n" +
					                       "current survavalTime: {3}", 
					                       stageIndex, roundIndex, max, survivalTime);
					round.survivalTime = Mathf.RoundToInt(max);
				}
			}
		}
	}

	#region Random point

	private Vector3 GetRandomSpawnPoint()
	{
		Vector3 result;
		float x, y;
		float randomAxis = GetRandomAxis();
		float randomPosition = GetRandomPosition();

		randomAxis += 0.5f;
		randomPosition += 0.5f;

		RandomInit(randomAxis, randomPosition, out x, out y);

		result = Camera.main.ViewportToWorldPoint(new Vector2(x, y));
		result.z = 0;
		return result;
	}

	private void RandomInit(float value1, float value2, out float a, out float b)
	{
		if (Random.Range(0, 2) == 0)
		{
			float temp = value1;
			value1 = value2;
			value2 = temp;
		}

		a = value1;
		b = value2;
	}

	private float GetRandomAxis() {
		float result = spawnViewportMargin;
		if (Random.Range(0, 2) == 0) {
			result *= -1;
		}

		return result;
	}

	private float GetRandomPosition() { 
		float result = Random.Range(0, spawnViewportMargin * 2) - spawnViewportMargin;
		return result;
	}
	#endregion
}

