﻿using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private static ShopManager instance;
    public static ShopManager Instance => instance;
    
    private int coins;

    public int Coins
    {
        get
        {
            return coins;
        }
        set
        {
            coins = value;
        }
    }

    private Coroutine cor;
    private Coroutine placingCor;

    [SerializeField] private GameObject uiShopSequence;
    [SerializeField] private float baseShopTimer;
    private float shopTimer;


    [SerializeField] private Text shopText;
    [SerializeField] private int numberOfWaves;

    [SerializeField] private GameObject turretPrefab;
    private GameObject equipedPrefab;

    public GameObject EquipedPrefab
    {
        get => equipedPrefab;
        set => equipedPrefab = value;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void StartShopSequence()
    {
        if (cor == null)
        {
            cor = StartCoroutine(ShopSequence());
        }
    }

    private IEnumerator ShopSequence()
    {
        // Afficher l'UI du shop
        // Permettre d'acheter une tourelle
        // Permettre de cancel un achat de tourelle
        // Permettre de poser une tourelle
        // TOUT CA SOUS UN TIMER DE 30s
        
        shopTimer = baseShopTimer;
        shopText.text = shopTimer.ToString();
        uiShopSequence.SetActive(true);
        while (shopTimer > 0f)
        {
            shopTimer -= Time.deltaTime;
            shopText.text = shopTimer.ToString();
            if (Input.GetMouseButtonDown(1))
            {
                //txt.text = "Placing turret";
                StartPlacingTurret();
            }
            yield return null;
        }
        
        uiShopSequence.SetActive(false);
        cor = null;
        GameManager.Instance.ChangePhase(GameStateEnum.Wave);
    }

    public void StartPlacingTurret()
    {
        if (placingCor == null)
        {
            placingCor = StartCoroutine(PlaceTurret());
        }
    }
    
    
    // Start is called before the first frame update
    private IEnumerator PlaceTurret()
    {
        bool isTurretPlaced = false;
        MeshRenderer selected = null;
        Material oldMat = null;
        Color oldColor = new Color();
        
        GameObject newTurret = Instantiate(turretPrefab);
        Turret newTurretScript = newTurret.GetComponent<Turret>();
        newTurretScript.enabled = false;

        while (!isTurretPlaced)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1f);
            Ray castPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
            {
                Node n = GameManager.Instance.ActualGrid.GetNodeWithPosition(hit.point);
                    newTurret.transform.position = n.position;
                
                
                
               /* if (selected != hit.collider.gameObject.GetComponent<MeshRenderer>())
                {

                    if (selected != null)
                    {
                        /*
                            selected.material = oldMat;
                            selected.material.color = oldColor;
                    }

                    /*selected = hit.collider.gameObject.GetComponent<MeshRenderer>();
                    oldMat = selected.material;
                    oldColor = selected.material.color;
                }*/



                
                if (CheckIfTurretable(n))
                {
                  //  selected.material.color = Color.green;
                    if (Input.GetAxisRaw("Fire1") == 1f)
                    {
                        newTurretScript.enabled = true;
                        //new Vector3(n.position.x, n.position.y, n.position.z), Quaternion.identity);
                        // Déployer une tourelle
                        equipedPrefab = null;
                        
                        //selected.material = oldMat;
                        //selected.material.color = oldColor;
                        //selected = null;
                        coins--;
                        n.isWalkable = false;
                        n.isTurretable = false;
                        
                        isTurretPlaced = true;
                        //GameManager.Instance.txt.text = "TurretPlaced";
                        break;
                    }
                }
                
                else
                {
                    //selected.material.color = Color.red;
                }
            }
            else
            {
   //             Debug.Log("Color should change back to normal");
                if (selected != null)
                {
//                    Debug.Log("kekw");
                    selected.material.color = oldColor;
                    selected = null;
                }

            /*if (selected != null)
            {
                selected.material = oldMat;
                selected = null;
            }*/
            }

            yield return null;
        }

        placingCor = null;
    }

    bool CheckIfTurretable(Node n)
    {
        return n.isTurretable;
    }
    
}
