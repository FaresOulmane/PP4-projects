using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManagerFares : MonoBehaviour
{
  public bool preparePhase;
  public GameObject uiPrepare;
  public float timeToPrepare;
  public float actualTime;
  public SpawnerEnnemies se;
  public Text txtvague;
  public float nombreDeVague;

  void Update()
  {
   
    if (se.aliveEnemies <= 0)
    {
      preparePhase = true;
    }
    ShopPhase();
  }
 
  private void ShopPhase()
  {
    if (preparePhase)
    {
      uiPrepare.SetActive(true);
      actualTime -= 1 * Time.deltaTime;
      
    }
    if (actualTime<= 0)
    {
      StopCoroutine(nameof(ShowVague));
      StartCoroutine(nameof(ShowVague));
      preparePhase = false;
      uiPrepare.SetActive(false);
      actualTime = timeToPrepare;
      se.StartCoroutine(nameof(se.Wave));

    }
  

  }

  IEnumerator ShowVague()
  {
    nombreDeVague++;
    se.totalEnemies += 1;
    txtvague.text = "start vague " + nombreDeVague;
    yield return new WaitForSeconds(2f);
    txtvague.text = "";
  }
}
