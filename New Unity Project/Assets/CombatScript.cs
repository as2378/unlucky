using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatScript : MonoBehaviour {

	// Update is called once per frame
	public void Combate (GameObject Attacker, GameObject Defender) {

        Sector AttackSector = Attacker.GetComponent<Sector>();
        Sector DefenderSector = Defender.GetComponent<Sector>();
        int AttackerA = AttackSector.Attack;
        int DefenderD = DefenderSector.Defence;
        //SceneManager.LoadScene("SliderGame");
        float attack = Random.Range(1f, AttackerA);
        float defence = Random.Range(1f, DefenderD);
        float result = defence - attack;

        print(attack + "=attack " + defence + "=defence " + result + "=result");



    }
}
