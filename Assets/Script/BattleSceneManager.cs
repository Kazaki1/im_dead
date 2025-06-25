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
        // Lượt Player bắt đầu: ẩn soul, ẩn fightbar, mở nút
        if (soul != null) soul.SetActive(false);
        if (fightbar != null) fightbar.SetActive(false);
        if (fightButton != null) fightButton.onClick.AddListener(OnFightButtonClicked);

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

        if (fightbar != null) fightbar.SetActive(true);
        fightButton.interactable = false;
        isFightBarActive = true;
    }

    void AttackEnemy()
    {
        Debug.Log("Attacking enemy...");

        // Ẩn fightbar (kết thúc Player Turn)
        if (fightbar != null) fightbar.SetActive(false);
        isFightBarActive = false;

        // **Hiện soul** ngay để Player có thể né đạn
        if (soul != null) soul.SetActive(true);

        // Gọi Enemy Turn
        if (enemy != null)
        {
            // Đăng ký callback khi enemy attack xong
            var bulletPattern = enemy.GetComponent<BulletHellAttack>();
            if (bulletPattern != null)
                bulletPattern.OnAttackFinished = OnEnemyAttackFinished;

            enemy.StartAttack();
        }
    }

    void OnEnemyAttackFinished()
    {
        Debug.Log("Enemy attack finished. Back to player turn.");

        // Ẩn soul, chuyển lại cho Player
        if (soul != null) soul.SetActive(false);

        // Cho phép Player bấm Fight lại
        if (fightButton != null) fightButton.interactable = true;
    }
}
