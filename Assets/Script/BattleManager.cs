using UnityEngine;

public enum BattleState
{
    PlayerTurn,
    EnemyTurn,
    Busy
}

public class BattleManager : MonoBehaviour
{
    public BattleState state = BattleState.PlayerTurn;

    [Header("UI Elements")]
    public GameObject fightMenu;     // Panel chứa các nút: Fight / Act / Item / Mercy
    public GameObject fightbar;      // GameObject chứa thanh Slider và FightbarController

    [Header("Enemy")]
    public EnemyController enemy;    // Script điều khiển quái vật

    void Start()
    {
        StartPlayerTurn();
    }

    // -------------------------
    // PLAYER TURN
    // -------------------------
    void StartPlayerTurn()
    {
        Debug.Log("🔁 Player's Turn");
        state = BattleState.PlayerTurn;

        fightMenu.SetActive(true);
        fightbar.SetActive(false);
    }

    public void OnPlayerChooseFight()
    {
        if (state != BattleState.PlayerTurn) return;

        Debug.Log("🗡️ Player chose FIGHT");

        state = BattleState.Busy;

        fightMenu.SetActive(false);
        fightbar.SetActive(true);

    }

    public void EndPlayerTurn()
    {
        Debug.Log("✅ Player ends turn");
        fightbar.SetActive(false);

        StartEnemyTurn();
    }

    void StartEnemyTurn()
    {
        Debug.Log("👾 Enemy's Turn");
        state = BattleState.EnemyTurn;

        if (enemy != null)
            enemy.StartAttack();

        // Có thể điều chỉnh thời gian enemy attack tùy pattern
        Invoke(nameof(EndEnemyTurn), 3f); // hoặc enemy tự gọi khi kết thúc pattern
    }

    void EndEnemyTurn()
    {
        Debug.Log("💤 Enemy ends turn");

        if (enemy != null)
            enemy.StopAttack();

        StartPlayerTurn();
    }
}
