using UnityEngine;

[CreateAssetMenu(fileName = "NewDifficulty", menuName = "Difficulty System/Difficulty")]
public class DifficultyGame : ScriptableObject
{
    [System.Serializable]
    public class DifficultyScript
    {
        [SerializeField] // Permite que o campo seja exibido no Inspector
        private Difficulty difficulty;

        public Difficulty Difficulty
        {
            get => difficulty;
            set => difficulty = value;
        }
    }

    public DifficultyScript difficultyData = new();
}

public enum Difficulty 
{
  Easy,
  Medium,
  Hard
}
