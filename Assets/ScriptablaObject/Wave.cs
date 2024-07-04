using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Waves", order = 1)]
public class Wave : ScriptableObject
{
    [SerializeField] private GameObject[] enemiesInWave;
    [SerializeField] private float timeBeforeThisWave;
    [SerializeField] private float numberToSpawn;
    [SerializeField] private float scoreThresholdForNextWave;

    public GameObject[] EnemiesInWave { get => enemiesInWave; }
    public float TimeBeforeThisWave { get => timeBeforeThisWave; }
    public float NumberToSpawn { get => numberToSpawn; }
    public float ScoreThresholdForNextWave { get => scoreThresholdForNextWave; }

    public void SetNumberToSpawn(float value) { numberToSpawn = value; }
    public void SetTimeBeforeThisWave(float value) { timeBeforeThisWave = value; }
}
