using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyBattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Slider hpSlider;

    public void SetHUD(MyUnit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lv"+unit.unitLevel.ToString();
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
    }

    public void SetHp(int hp)
    {
        hpSlider.value = hp;
    }
}
