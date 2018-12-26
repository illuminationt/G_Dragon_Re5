using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPUI : MonoBehaviour {

    public Text hpLabel;
    public Text attack;
    public Image hpSprite;
    public Image moveSprite;

    private int maxHp;
    private int nowHp;
    private int moveHp;
    private Vector4 damageColor;
    private Vector4 healColor;
    private Enemy enemy;

    void Start()
    {
        damageColor = new Vector4(0.4f, 0.25f, 0, 1);
        healColor = new Vector4(0.3f, 1, 0.5f, 1);
       // enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
        enemy = FindObjectOfType<Enemy>();

        maxHp = enemy.HP;
        nowHp = maxHp;
        moveHp = maxHp;
        hpSprite.fillAmount = (float)nowHp / (float)maxHp;
        moveSprite.fillAmount = (float)moveHp / (float)maxHp;
        hpLabel.text = nowHp + "/" + maxHp;
        attack.text = "G : " + enemy.AttackGu + " C : " + enemy.AttackChoki + " P : " + enemy.AttackPar;
    }

    public void OnDamageClick()
    {
        int damage = Random.Range(20, 600);
        moveHp -= damage;
        if (moveHp < 0) moveHp = 0;
    }

    public void OnHealClick()
    {
        int heal = Random.Range(20, 600);
        moveHp += heal;
        if (moveHp > maxHp) moveHp = maxHp;

    }


    void Update()
    {
        moveHp = enemy.HP;
        if (nowHp != moveHp)
        {
            if (nowHp > moveHp)
            {
                // damage
                nowHp -= Mathf.FloorToInt(maxHp * Time.deltaTime * 0.3f);
                if (nowHp < moveHp)
                {
                    print(nowHp);
                    nowHp = moveHp;
                }
                moveSprite.color = damageColor;
                hpSprite.fillAmount = (float)moveHp / (float)maxHp;
                moveSprite.fillAmount = (float)nowHp / (float)maxHp;
            }
            else
            {
                // heal
                nowHp += Mathf.FloorToInt(maxHp * Time.deltaTime * 0.3f);
                if (nowHp > moveHp) nowHp = moveHp;

                moveSprite.color = healColor;
                hpSprite.fillAmount = (float)nowHp / (float)maxHp;
                moveSprite.fillAmount = (float)moveHp / (float)maxHp;

            }
            if (nowHp < 0) nowHp = 0;
            if (moveHp < 0) moveHp = 0;
            hpLabel.text = nowHp + "/" + maxHp;
        }

    }
}
