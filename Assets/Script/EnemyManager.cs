using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public Image enemyImageUI;               // gán Image trong Canvas
    public EnemyData[] enemyOptions;         // danh sách enemy trong Inspector
    public Transform patternSpawnPoint;      // nơi spawn script pattern

    private EnemyAttackBase currentPattern;

    public void SetupRandomEnemy()
    {
        int index = Random.Range(0, enemyOptions.Length);
        EnemyData selected = enemyOptions[index];

        enemyImageUI.sprite = selected.image;

        GameObject randomPattern = selected.patternPrefabs[Random.Range(0, selected.patternPrefabs.Length)];

        if (currentPattern != null)
            Destroy(currentPattern.gameObject);

        GameObject patternObj = Instantiate(randomPattern, patternSpawnPoint.position, Quaternion.identity);
        currentPattern = patternObj.GetComponent<EnemyAttackBase>();

        FindObjectOfType<BattleManager>().enemy = patternObj.GetComponent<EnemyController>();
    }
}