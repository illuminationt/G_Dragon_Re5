using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPUI : MonoBehaviour
{

    public Text hpLabel;
    public Text attack;
    public Image hpSprite;
    public Image moveSprite;

    private int maxHp;
    private int nowHp;
    private int moveHp;
    private Vector4 damageColor;
    private Vector4 healColor;
    private Dragon dragon;

    void Start()
    {
        damageColor = new Vector4(0.4f, 0.25f, 0, 1);
        healColor = new Vector4(0.3f, 1, 0.5f, 1);
        //dragon = GameObject.Find("Dragon").GetComponent<Dragon>();
        dragon = FindObjectOfType<Dragon>();

        maxHp = dragon.HP;
        nowHp = maxHp;
        moveHp = maxHp;
        hpSprite.fillAmount = (float)nowHp / (float)maxHp;
        moveSprite.fillAmount = (float)moveHp / (float)maxHp;
        hpLabel.text = nowHp + "/" + maxHp;
        attack.text = "G : " + GOD.Instance.dragonAttackGu + " C : " + GOD.Instance.dragonAttackChoki + " P : " + GOD.Instance.dragonAttackPar;
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
        moveHp = dragon.HP;
        if (nowHp != moveHp)
        {
            if (nowHp > moveHp)
            {
                // damage
                nowHp -= Mathf.FloorToInt(maxHp * Time.deltaTime * 0.8f);
                if (nowHp < moveHp)
                {
                    print(nowHp);
                    nowHp = moveHp;
                }
                moveSprite.color = damageColor;
                //print(nowHp);
                //print(moveHp);
                //print(maxHp);
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