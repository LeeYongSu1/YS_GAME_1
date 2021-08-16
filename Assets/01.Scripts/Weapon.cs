using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Define.WeaponType weaponType;

    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public GameObject[] particle;

    public void Use()
    {
        if(weaponType == Define.WeaponType.Melee)
        {
            StartCoroutine(Swing());
        }
    }

    public void Use2()
    {
        if (weaponType == Define.WeaponType.Melee)
        {
            StartCoroutine(Swing2());
        }
    }

    IEnumerator Swing()
    {
        
        yield return new WaitForSeconds(0.2f);
        foreach (GameObject slash in particle)
        {
            slash.SetActive(true);
        }
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.3f);
        foreach (GameObject slash in particle)
        {
            slash.SetActive(false);
        }
        yield return new WaitForSeconds(0.2f);
        meleeArea.enabled = false;
        
    }

    IEnumerator Swing2()
    {
        meleeArea.enabled = true;
        foreach (GameObject slash in particle)
        {
            slash.SetActive(true);
        }
        yield return new WaitForSeconds(0.4f);
        meleeArea.enabled = false;
        foreach (GameObject slash in particle)
        {
            slash.SetActive(false);
        }
    }
}
