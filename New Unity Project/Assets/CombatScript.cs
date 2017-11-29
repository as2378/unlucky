using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatScript : MonoBehaviour {

    // Update is called once per frame
    public void Combat(GameObject Attacker, GameObject Defender)
    {

        Sector AttackSector = Attacker.GetComponent<Sector>();
        Sector DefenderSector = Defender.GetComponent<Sector>();
        AttackSector.Attack = 10;
        DefenderSector.Defence = 5;
        print(DefenderSector.Defence + "=Defence" + AttackSector.Attack + "=Attack ");
        //this is to test it with expected values
        int AttackerA = AttackSector.Attack;
        int DefenderD = DefenderSector.Defence;
        //SceneManager.LoadScene("SliderGame");
        float attack = Random.Range(1f, AttackerA);
        float defence = Random.Range(1f, DefenderD);
        float result = defence - attack;
        int DamageDone = Mathf.RoundToInt(result);
        print(attack + "=attack " + defence + "=defence " + result + "=result " + DamageDone);
        if (result > 0)
        {
            AttackSector.Attack = AttackerA - DamageDone;
            if (result > AttackSector.Attack)
            {

            }
        }
        else
        {
            DefenderSector.Defence = DefenderD + DamageDone;
            if (result > DefenderSector.Defence)
            {

            }
        }

        print(DefenderSector.Defence + "=Defence" + AttackSector.Attack + "=Attack ");

    }
}