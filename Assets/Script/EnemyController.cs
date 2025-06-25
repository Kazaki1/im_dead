using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyAttackBase attackScript;

    void Start()
    {
        attackScript = GetComponent<EnemyAttackBase>();
    }

    public void StartAttack()
    {
        if (attackScript != null)
            attackScript.StartAttack();
    }

    public void StopAttack()
    {
        if (attackScript != null)
            attackScript.StopAttack();
    }
}
