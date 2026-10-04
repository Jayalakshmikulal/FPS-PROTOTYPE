using UnityEngine;

public class EnemyDeathReporter : MonoBehaviour
{
    public EnemySpawner enemySpawner;

    private void OnDestroy()
    {
        if (enemySpawner != null)
        {
            enemySpawner.ReportEnemyDeath();
        }
    }
}