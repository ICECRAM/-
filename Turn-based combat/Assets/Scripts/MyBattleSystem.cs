using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MyBattleState { START,PLAYERTURN,ENMYTURN,WON,LOST  }

public class MyBattleSystem : MonoBehaviour
{
    public GameObject playerPerfab;
    public GameObject enemyPerfab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public Text dialogueText;

    MyUnit playerUnit;
    MyUnit enemyUnit;

    public MyBattleHUD playerHUD;
    public MyBattleHUD enemyHUD;

    MyBattleState state;

    private void Start()
    {
        state = MyBattleState.START;
        StartCoroutine(SetHUD());
    }

     IEnumerator SetHUD()
    {
        GameObject playerGo = Instantiate(playerPerfab, playerBattleStation);
        playerUnit = playerGo.GetComponent<MyUnit>();

        GameObject EnemyGo = Instantiate(enemyPerfab, enemyBattleStation);
        enemyUnit = EnemyGo.GetComponent<MyUnit>();

        dialogueText.text = "A wild " + enemyUnit.unitName + "approaches...";
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);
        state = MyBattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.SetHp(enemyUnit.currentHP);
        dialogueText.text = "The attack is successful!";

        yield return new WaitForSeconds(2.0f);

        if (isDead)
        {
            state = MyBattleState.WON;
            EndBattle();
        }
        else
        {
            state = MyBattleState.ENMYTURN;
            StartCoroutine(EnemyAttack());
        }
    }

    IEnumerator EnemyAttack()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";
        yield return new WaitForSeconds(1f);
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHp(playerUnit.currentHP);
        yield return new WaitForSeconds(1.0f);

        if (isDead)
        {
            state = MyBattleState.LOST;
            EndBattle();
        }
        else
        {
            state = MyBattleState.PLAYERTURN;
           PlayerTurn();
        }

    }

    private void EndBattle()
    {
        if (state == MyBattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        }
        else if (state == MyBattleState.LOST)
        {
            dialogueText.text = "You were defeated!";
        }

    }

    public void PlayerTurn()
    {
        dialogueText.text = "Choose an action: ";
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(10);
        playerHUD.SetHp(playerUnit.currentHP);
        dialogueText.text = "You feel renewed strength!";
        yield return new WaitForSeconds(2.0f);

        state = MyBattleState.ENMYTURN;
        StartCoroutine(EnemyAttack());
    }

    public void OnAttackButton()
    {

        if (state != MyBattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerAttack());
    }
    public void OnHeal()
    {
        if (state != MyBattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerHeal());
    }

}
