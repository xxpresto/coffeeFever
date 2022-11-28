using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class CupScript : MonoBehaviour
{

    float moveTimer;
    bool canMove = true;

    public GameObject coffee, cover;
   public bool coffeeFilled, cupFinish, canDestroy;

   public int myOrder;
    public Material[] sývýRenkler;
    
    void Start()
    {
      
    }



    int mytepsiPos;
    void Update()
    {
        if (canMove)
        {
            if (transform.position.x > LevelManager.instance.cupWaitingPositions[myOrder].transform.position.x && !coffeeFilled)  // kahve dolum
            {
                canMove = false;
                coffeeFilled = true;
                coffee.SetActive(true);
                LevelManager.instance.musluklarAktif[myOrder] = true;
                //  LevelManager.instance.coffeeEffect.GetComponent<ParticleSystem>().Play();
                coffeeEffectOpen();

                string renkadý = "faucet" + (myOrder + 1);
                Debug.Log(renkadý+ "="+ PlayerPrefs.GetInt(renkadý).ToString());
                
                if (PlayerPrefs.GetInt(renkadý) == 0) Destroy(this.gameObject);
                coffee.GetComponent<MeshRenderer>().material = sývýRenkler[PlayerPrefs.GetInt(renkadý)-1];

                Invoke("bardakDoldur", 0.25f);
               Invoke("coffeeEffectClose", 2f);
                Invoke("canMoveSetTrue", 2.25f);
              

            }

            if (transform.position.x > LevelManager.instance.cupLastPos.transform.position.x && !cupFinish)  // kahve dolum
            {
                cupFinish = true;

                canMove = false;
                transform.DOLocalMove(LevelManager.instance.tepsidekiKahvelerinPos[LevelManager.instance.tepsiSayý].transform.position, 0.5f);
                transform.DOScale(LevelManager.instance.tepsidekiKahvelerinPos[LevelManager.instance.tepsiSayý].transform.localScale, 0.5f);
                mytepsiPos = LevelManager.instance.tepsiSayý;
                Invoke("tepsiyeÝndi", 0.5f);

                LevelManager.instance.tepsiSayý += 1;
                if (LevelManager.instance.tepsiSayý == 4)
                {
                    Invoke("tepsiKaydýr", 1f); 
                    Invoke("moveAll", 1f); 
                    Invoke("destroyAll", 1.8f);
                    LevelManager.instance.tepsiSayý = 0;
                }

            }

            string yer = "faucet" + (myOrder + 1);
           if (PlayerPrefs.GetInt(yer) == 0)
            {
             
                Destroy(this.gameObject);
            } 
            

                moveTimer += Time.deltaTime;
            if (moveTimer > 0.01f)
            {
                moveTimer = 0;
            if(!LevelManager.instance.tapped)    transform.position += new Vector3(0.0015f, 0, 0);
                else transform.position += new Vector3(0.003f, 0, 0);
            }

        }
    }



    void bardakDoldur()
    {
        coffee.transform.DOScaleY(0.06f, 2f);
    }

    void destroyAll()
    {
        GameObject[] cups = GameObject.FindGameObjectsWithTag("cup");
        for (int i = 0; i< cups.Length;i++)
        {
         if(cups[i].GetComponent<CupScript>().canDestroy)  Destroy(cups[i]);
        }
    }


    void moveAll()
    {
        GameObject[] cups = GameObject.FindGameObjectsWithTag("cup");
        for (int i = 0; i < cups.Length; i++)
        {
            if (cups[i].GetComponent<CupScript>().canDestroy) cups[i].transform.DOLocalMove(LevelManager.instance.bardakGidisPos[mytepsiPos].transform.position, 0.75f);
        }
    }
    public GameObject para3DText;
    int fiyat;
    void tepsiyeÝndi()
    {
        cover.SetActive(true);  
        canDestroy = true;

        // lw1 1 - lw2 - 2 - lw3 5 - lw4 10 - lw5 20
        string ad = "faucet" + (myOrder + 1);
        if (PlayerPrefs.GetInt(ad) == 1) fiyat = 1+ PlayerPrefs.GetInt("income"); 
     else   if (PlayerPrefs.GetInt(ad) == 2) fiyat = 2 + PlayerPrefs.GetInt("income");
        else if (PlayerPrefs.GetInt(ad) == 3) fiyat = 5 + PlayerPrefs.GetInt("income");
        else if (PlayerPrefs.GetInt(ad) == 4) fiyat = 10 + PlayerPrefs.GetInt("income");
        else if (PlayerPrefs.GetInt(ad) == 5) fiyat =20 + PlayerPrefs.GetInt("income");
        else fiyat = 1;
        int posOrder;
        if (LevelManager.instance.tepsiSayý - 1 != -1) posOrder = LevelManager.instance.tepsiSayý - 1;
        else posOrder = LevelManager.instance.tepsiSayý + 3;
        var paraPrefab = Instantiate(para3DText, LevelManager.instance.tepsidekiKahvelerinPos[posOrder].transform.position, Quaternion.identity);
        paraPrefab.GetComponent<TextMeshPro>().text = "$" + (fiyat * LevelManager.instance.incomeRateDefault);

        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + (fiyat*LevelManager.instance.incomeRateDefault));

    }

    // 0 ilk tepsi, en baþta ortada olan // 1. tepsi soldaki, 2.tepsi saðdaki.
    
    void tepsiKaydýr()
    {
        LevelManager.instance.tepsi0Order += 1;
        LevelManager.instance.tepsi1Order += 1;
        LevelManager.instance.tepsi2Order += 1;

        if (LevelManager.instance.tepsi0Order == 3) LevelManager.instance.tepsi0Order = 0;
        if (LevelManager.instance.tepsi1Order == 3) LevelManager.instance.tepsi1Order = 0;
        if (LevelManager.instance.tepsi2Order == 3) LevelManager.instance.tepsi2Order = 0;

        if (LevelManager.instance.tepsi0Order == 2) LevelManager.instance.tepsiler[0].transform.DOLocalMove(LevelManager.instance.tepsiPoslar[LevelManager.instance.tepsi0Order].transform.position,0);
        else LevelManager.instance.tepsiler[0].transform.DOLocalMove(LevelManager.instance.tepsiPoslar[LevelManager.instance.tepsi0Order].transform.position, 0.75f);

        if (LevelManager.instance.tepsi1Order == 2) LevelManager.instance.tepsiler[1].transform.DOLocalMove(LevelManager.instance.tepsiPoslar[LevelManager.instance.tepsi1Order].transform.position, 0);
        else  LevelManager.instance.tepsiler[1].transform.DOLocalMove(LevelManager.instance.tepsiPoslar[LevelManager.instance.tepsi1Order].transform.position, 0.75f);

        if (LevelManager.instance.tepsi2Order == 2) LevelManager.instance.tepsiler[2].transform.DOLocalMove(LevelManager.instance.tepsiPoslar[LevelManager.instance.tepsi2Order].transform.position, 0);
        else LevelManager.instance.tepsiler[2].transform.DOLocalMove(LevelManager.instance.tepsiPoslar[LevelManager.instance.tepsi2Order].transform.position, 0.75f);
    }

    bool objeVar;
    void sonObjeBul()
    {
        GameObject[] cups = GameObject.FindGameObjectsWithTag("cup");
        for (int i = 0; i < cups.Length; i++)
        {
            if (!cups[i].GetComponent<CupScript>().canDestroy && cups[i].GetComponent<CupScript>().myOrder == LevelManager.instance.lastSpaceCount)
            {
                objeVar = true;
                break;
            }
        }
    }

    public bool üretebilir;

    public   void canMoveSetTrue()
    {
        string yer = "faucet" + (myOrder + 1);
           
        canMove = true;
        if ((myOrder + 1 == LevelManager.instance.lastSpaceCount && PlayerPrefs.GetInt(yer) != 0) || üretebilir)
        {
            LevelManager.instance.canSpawn = true;
            Debug.Log(name + " order:"+ myOrder + " lastcount:" + LevelManager.instance.lastSpaceCount);
            üretebilir = false;
        }
       
    }

    void coffeeEffectClose()
    {
        LevelManager.instance.musluklarAktif[myOrder] = false;
        string ad = "faucet" + (myOrder + 1);
        if (PlayerPrefs.GetInt(ad) != 0)
        {
            if (myOrder == 0 && LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet1") - 1].activeSelf)
            {
                if (PlayerPrefs.GetInt("faucet1") == 1) LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet1") - 1].transform.Find("akma efekt").GetComponent<ParticleSystem>().Stop();
                else
                {
                    string tür = "akma efekt (" + (PlayerPrefs.GetInt("faucet1") + -1) + ")";
                    LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet1") - 1].transform.Find(tür).GetComponent<ParticleSystem>().Stop();
                }
            }


            if (myOrder == 1 && LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet2") + 4].activeSelf)
            {
                if (PlayerPrefs.GetInt("faucet2") == 1) LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet2") + 4].transform.Find("akma efekt").GetComponent<ParticleSystem>().Stop();
                else
                {
                    string tür = "akma efekt (" + (PlayerPrefs.GetInt("faucet2") + -1) + ")";
                    Debug.Log(tür);
                    LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet2") + 4].transform.Find(tür).GetComponent<ParticleSystem>().Stop();
                }
            }


            if (myOrder == 2 && LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet3") + 9].activeSelf)
            {
                if (PlayerPrefs.GetInt("faucet3") == 1) LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet3") + 9].transform.Find("akma efekt").GetComponent<ParticleSystem>().Stop();
                else
                {
                    string tür = "akma efekt (" + (PlayerPrefs.GetInt("faucet3") + -1) + ")";
                    if (LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet3") + 9].activeSelf) LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet3") + 9].transform.Find(tür).GetComponent<ParticleSystem>().Stop();
                }
            }


            if (myOrder == 3 && LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet4") + 14].activeSelf)
            {
                if (PlayerPrefs.GetInt("faucet4") == 1) LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet4") + 14].transform.Find("akma efekt").GetComponent<ParticleSystem>().Stop();
                else
                {
                    string tür = "akma efekt (" + (PlayerPrefs.GetInt("faucet4") + -1) + ")";
                    LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet4") + 14].transform.Find(tür).GetComponent<ParticleSystem>().Stop();
                }
            }



            if (myOrder == 4 && LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet5") + 19].activeSelf)
            {
                if (PlayerPrefs.GetInt("faucet5") == 1) LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet5") + 19].transform.Find("akma efekt").GetComponent<ParticleSystem>().Stop();
                else
                {
                    string tür = "akma efekt (" + (PlayerPrefs.GetInt("faucet5") + -1) + ")";
                    if (LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet5") + 19].activeSelf) LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet5") + 19].transform.Find(tür).GetComponent<ParticleSystem>().Stop();
                }
            }
        }
    }

    void coffeeEffectOpen()
    {
       
        if (myOrder == 0 && LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet1") - 1].activeSelf)
        {
       if(PlayerPrefs.GetInt("faucet1")==1)     LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet1") - 1].transform.Find("akma efekt").GetComponent<ParticleSystem>().Play();
       else
            {
                string tür = "akma efekt (" + (PlayerPrefs.GetInt("faucet1") + -1) + ")";
               LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet1") - 1].transform.Find(tür).GetComponent<ParticleSystem>().Play();
            }
        }


        if (myOrder == 1 && LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet2") +4].activeSelf)
        {
            if (PlayerPrefs.GetInt("faucet2") == 1) LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet2")+4].transform.Find("akma efekt").GetComponent<ParticleSystem>().Play();
            else
            {
                string tür = "akma efekt (" + (PlayerPrefs.GetInt("faucet2") + -1) + ")";
                LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet2") +4].transform.Find(tür).GetComponent<ParticleSystem>().Play();
            }
        }


        if (myOrder == 2 && LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet3") + 9].activeSelf)
        {
            if (PlayerPrefs.GetInt("faucet3") == 1) LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet3") +9].transform.Find("akma efekt").GetComponent<ParticleSystem>().Play();
            else
            {
                string tür = "akma efekt (" + (PlayerPrefs.GetInt("faucet3") + -1) + ")";
              LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet3")+9].transform.Find(tür).GetComponent<ParticleSystem>().Play();
            }
        }


        if (myOrder == 3 && LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet4") + 14].activeSelf)
        {
            if (PlayerPrefs.GetInt("faucet4") == 1) LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet4") +14].transform.Find("akma efekt").GetComponent<ParticleSystem>().Play();
            else
            {
                string tür = "akma efekt (" + (PlayerPrefs.GetInt("faucet4") + -1) + ")";
              LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet4") +14].transform.Find(tür).GetComponent<ParticleSystem>().Play();
            }
        }



        if (myOrder == 4 && LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet5") + 19].activeSelf)
        {
            if (PlayerPrefs.GetInt("faucet5") == 1) LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet5") + 19].transform.Find("akma efekt").GetComponent<ParticleSystem>().Play();
            else
            {
                string tür = "akma efekt (" + (PlayerPrefs.GetInt("faucet5") + -1) + ")";
                LevelManager.instance.allFacucets[PlayerPrefs.GetInt("faucet5") +19].transform.Find(tür).GetComponent<ParticleSystem>().Play();
            }
        }
    }

}
