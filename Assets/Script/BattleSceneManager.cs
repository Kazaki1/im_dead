using UnityEngine;
using UnityEngine.UI;

public class BattleSceneManager : MonoBehaviour
{
    public GameObject soul;             
    public Button fightButton;          
    public GameObject fightbar;         
    public EnemyController enemy;       

    private bool isFightBarActive = false;

    void Start()
    {
        if (soul != null)
            soul.SetActive(false);

        if (fightbar != null)
            fightbar.SetActive(false);

        if (fightButton != null)
            fightButton.onClick.AddListener(OnFightButtonClicked);

        fightButton.interactable = true;
    }

    void Update()
    {
        if (isFightBarActive && Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Player confirmed attack with Enter.");
            AttackEnemy();
        }
    }

    void OnFightButtonClicked()
    {
        Debug.Log("Fight button clicked!");

        if (fightbar != null)
            fightbar.SetActive(true);

        fightButton.interactable = false;
        isFightBarActive = true;
    }

    void AttackEnemy()
    {
        Debug.Log("Attacking enemy...");

        if (fightbar != null)
            fightbar.SetActive(false);

        if (soul != null)
            soul.SetActive(false);

        isFightBarActive = false;

        if (enemy != null)
        {
            var bulletPattern = enemy.GetComponent<BulletHellAttack>();
            if (bulletPattern != null)
                bulletPattern.OnAttackFinished = OnEnemyAttackFinished;

            enemy.StartAttack();
        }
    }

    void OnEnemyAttackFinished()
    {
        Debug.Log("Enemy attack finished. Player's turn begins again.");

        if (soul != null)
            soul.SetActive(true);

        if (fightButton != null)
            fightButton.interactable = true;
    }
}
