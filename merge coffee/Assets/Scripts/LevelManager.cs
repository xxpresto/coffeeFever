using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public bool isGameActive = true;

    public GameObject cupPrefab, cupSpawnPos, coffeeEffect;
    float  tapTimer;
    public bool tapped; // true while players tapping
    public bool canSpawn;
    public GameObject[] cupWaitingPositions, allFacucets; // 0 4 first 5 9 second 
    public GameObject cupLastPos;
    public GameObject[] tepsiler, tepsiPoslar, tepsidekiKahvelerinPos;
    public int tepsiSayý, aktifTepsi;
    public GameObject[] spaces, bardakGidisPos;
    public int tepsi0Order = 0, tepsi1Order = 1, tepsi2Order = 2;
    float spawnDelayTimer;
    public GameObject levelUpPanel;
    public GameObject[] levelUps, tikler;
    public GameObject[] butonYesiller;

    public bool[] musluklarAktif;
    public int upgrade1Money, upgrade2Money, upgrade3Money, mergeMoney, nextMapMoney;
    public TextMeshProUGUI moneyText, up1Text, up2Text, up3Text, mergeText;
    void Start()
    {
        if (instance == null) instance = this;

        if (PlayerPrefs.GetInt("faucetCount") >= 1) canSpawn = true;

        openFaucets();

        if (!PlayerPrefs.HasKey("upgrade1Money")) PlayerPrefs.SetInt("upgrade1Money", upgrade1Money);
        if (!PlayerPrefs.HasKey("upgrade2Money")) PlayerPrefs.SetInt("upgrade2Money", upgrade2Money);
        if (!PlayerPrefs.HasKey("upgrade3Money")) PlayerPrefs.SetInt("upgrade3Money", upgrade3Money);
        if (!PlayerPrefs.HasKey("mergeMoney")) PlayerPrefs.SetInt("mergeMoney", mergeMoney);
        if (!PlayerPrefs.HasKey("nextMapMoney")) PlayerPrefs.SetInt("nextMapMoney", nextMapMoney);

        if (!PlayerPrefs.HasKey("money")) PlayerPrefs.SetInt("money", 115);


        if (PlayerPrefs.GetInt("rehber") == 0) //3finish
        {
            rehber.SetActive(true);
            rehber1.SetActive(true);
        }

        else if (PlayerPrefs.GetInt("rehber") == 1)
        {
            rehber.SetActive(true);
            rehber2.SetActive(true);
        }

        else if (PlayerPrefs.GetInt("rehber") == 3)
        {
            rehber.SetActive(false);
           
        }
    }

    float tapYapýlmýyor = 3;
    public GameObject tapUyari;
    public GameObject rehber, rehber1,rehber2, rehber3;

    void tapKapa()
    {
        tapUyari.SetActive(false);
    }
    void Update()
    {
        if (isGameActive)
        {
            moneyText.text = PlayerPrefs.GetInt("money").ToString();
            up1Text.text = "$"+ PlayerPrefs.GetInt("upgrade1Money");
            up2Text.text = "$" + PlayerPrefs.GetInt("upgrade2Money");
            up3Text.text = "$" + PlayerPrefs.GetInt("upgrade3Money");
            mergeText.text = "$" + PlayerPrefs.GetInt("mergeMoney");

            tapTimer += Time.deltaTime;
            if (tapTimer > 1f)
            {
                tapTimer = 0;
                tapped = false;
            }

            tapYapýlmýyor += Time.deltaTime;

            if (tapYapýlmýyor > 5 && !rehber.activeSelf)
            {
                tapYapýlmýyor = 0;
                tapUyari.SetActive(true);
                Invoke("tapKapa", 1.5f);
            }

           
            if (canSpawn && !muslukAktiflikCheck())
            {
                canSpawn = false;
                // spawnCup();
               
                StartCoroutine("spawncups");
            }

            spawnTimer += Time.deltaTime;

            if (spawnTimer > 5)
            {
                canSpawn = false;
                // spawnCup();
                spawnTimer = 0;
                StartCoroutine("spawncups");
            } 

            if (PlayerPrefs.GetInt("reward1") == 1) tikler[0].SetActive(true);
            if (PlayerPrefs.GetInt("reward2") == 1) tikler[1].SetActive(true);
            if (PlayerPrefs.GetInt("reward3") == 1) tikler[2].SetActive(true);
            if (PlayerPrefs.GetInt("reward4") == 1) tikler[3].SetActive(true);
            if (PlayerPrefs.GetInt("reward5") == 1) tikler[4].SetActive(true);

            if (PlayerPrefs.GetInt("money") >= PlayerPrefs.GetInt("upgrade1Money") && PlayerPrefs.GetInt("spaceCount") > PlayerPrefs.GetInt("faucetCount")) butonYesiller[0].SetActive(true);
            else butonYesiller[0].SetActive(false);

            if (PlayerPrefs.GetInt("money") >= PlayerPrefs.GetInt("upgrade2Money")) butonYesiller[1].SetActive(true);
            else butonYesiller[1].SetActive(false);

            if (PlayerPrefs.GetInt("money") >= PlayerPrefs.GetInt("upgrade3Money")) butonYesiller[2].SetActive(true);
            else butonYesiller[2].SetActive(false);

            if (PlayerPrefs.GetInt("money") >= PlayerPrefs.GetInt("mergeMoney")) butonYesiller[3].SetActive(true);
            else butonYesiller[3].SetActive(false);

            if (PlayerPrefs.GetInt("money") >= 25 && PlayerPrefs.GetInt("rehber") == 2 && PlayerPrefs.GetInt("faucetCount") == 2)
            {
                rehber.SetActive(true);
                rehber3.SetActive(true);
                
            }

            //    fazlalýkAt();
        }
    }

    public int order, lastSpaceCount;

    bool spawnWait;

    public void rehberSet(int x)
    {
        PlayerPrefs.SetInt("rehber", x);
    }

    public void fazlalýkAt()
    {
        GameObject[] cups = GameObject.FindGameObjectsWithTag("cup");
        for (int i = 0; i < cups.Length; i++)
        {
         if(cups[i]!=null)
                
                if (!cups[i].GetComponent<CupScript>().coffeeFilled)
            {
                for (int c = 0; c < i; c++)
                {
                        if (cups[c] != null && c != i)
                            if (!cups[c].GetComponent<CupScript>().coffeeFilled && cups[c].GetComponent<CupScript>().myOrder == cups[i].GetComponent<CupScript>().myOrder)
                                cups[c].SetActive(false);

                }

                }
        }
    }

   
    IEnumerator spawncups()
    {
        lastSpaceCount = PlayerPrefs.GetInt("faucetCount");
        order = PlayerPrefs.GetInt("faucetCount")-1;
      //  Debug.Log(lastSpaceCount);
        for (int i = 0; i < lastSpaceCount; i++)
        {
            var cupClone = Instantiate(cupPrefab, cupSpawnPos.transform.position, Quaternion.identity);
            cupClone.transform.DOLocalRotate(new Vector3(-90, 180, 0), 0); 
            cupClone.GetComponent<CupScript>().myOrder = order; // 0 1 2
            order -= 1;
            if (order < 0) order = 0;
            spawnTimer = 0;
            yield return new WaitForSeconds(1.75f);
        }
        order = PlayerPrefs.GetInt("faucetCount")-1;

       



    }

    void orderAyarla()
    {
        for (int i = 0; i < 5; i++)
        {
            string isim = "faucet" + (i + 1);

            for (int c = 0; c < i; c++)
            {
                string isim2 = "faucet" + (c + 1);

                if(PlayerPrefs.GetInt(isim2)==0 && PlayerPrefs.GetInt(isim)!=0)
                {
                    PlayerPrefs.SetInt(isim2, PlayerPrefs.GetInt(isim));
                    PlayerPrefs.SetInt(isim, 0);
                }


            }



            }
        }

    void spawnCup()
    {
        for (int i = 0; i < PlayerPrefs.GetInt("faucetCount"); i++)
        {
           var cupClone = Instantiate(cupPrefab, cupSpawnPos.transform.position, Quaternion.identity);
            cupClone.transform.DOLocalRotate(new Vector3(-90, 180, 0), 0);
            cupClone.GetComponent<CupScript>().myOrder = order;
            order += 1;
        }
        order = 0;
        lastSpaceCount = PlayerPrefs.GetInt("faucetCount");
    }

    public void tap()
    {
        tapTimer = 0;
        tapped = true;
        tapYapýlmýyor = 0;
    }


    public GameObject mergeAlRehber;


    float spawnTimer;
    public void addFaucet()
    {
        if(PlayerPrefs.GetInt("money")>= PlayerPrefs.GetInt("upgrade1Money") && PlayerPrefs.GetInt("spaceCount") > PlayerPrefs.GetInt("faucetCount"))
        {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - PlayerPrefs.GetInt("upgrade1Money"));
            PlayerPrefs.SetInt("faucetCount", PlayerPrefs.GetInt("faucetCount") + 1);
           
            PlayerPrefs.SetInt("faucetFillOrder", PlayerPrefs.GetInt("faucetFillOrder") + 1);

            if (PlayerPrefs.GetInt("ilkGeliþ") != 1)
            {
                canSpawn = true;
                PlayerPrefs.SetInt("ilkGeliþ", 1);
            }

            string isim = "faucet" + PlayerPrefs.GetInt("faucetCount");
            PlayerPrefs.SetInt(isim, 1); // merge edince bunu deðiþtir
            openFaucets();

            PlayerPrefs.SetInt("reward1", 1);


            if (PlayerPrefs.GetInt("upgrade1Money") == 20) PlayerPrefs.SetInt("upgrade1Money", 25);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 25) PlayerPrefs.SetInt("upgrade1Money", 38);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 38) PlayerPrefs.SetInt("upgrade1Money", 59);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 59) PlayerPrefs.SetInt("upgrade1Money", 100);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 100) PlayerPrefs.SetInt("upgrade1Money", 150);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 150) PlayerPrefs.SetInt("upgrade1Money", 201);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 201) PlayerPrefs.SetInt("upgrade1Money", 255);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 255) PlayerPrefs.SetInt("upgrade1Money", 301);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 301) PlayerPrefs.SetInt("upgrade1Money", 364);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 364) PlayerPrefs.SetInt("upgrade1Money", 405);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 405) PlayerPrefs.SetInt("upgrade1Money", 471);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 471) PlayerPrefs.SetInt("upgrade1Money", 525);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 525) PlayerPrefs.SetInt("upgrade1Money", 579);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 579) PlayerPrefs.SetInt("upgrade1Money", 650);
            else PlayerPrefs.SetInt("upgrade1Money", 750);

            PlayerPrefs.SetInt("rehber", 2);

        }
    }

    public void income()
    {
        if (PlayerPrefs.GetInt("money") >= PlayerPrefs.GetInt("upgrade3Money") )
        {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - PlayerPrefs.GetInt("upgrade3Money"));
            

            if (PlayerPrefs.GetInt("income") ==0) PlayerPrefs.SetInt("income", 1);
            else if (PlayerPrefs.GetInt("income") == 1) PlayerPrefs.SetInt("income", 3);
            else if (PlayerPrefs.GetInt("income") == 3) PlayerPrefs.SetInt("income", 5);
            else if (PlayerPrefs.GetInt("income") == 5) PlayerPrefs.SetInt("income", 7);
            else if (PlayerPrefs.GetInt("income") == 7) PlayerPrefs.SetInt("income", 10);


            if (PlayerPrefs.GetInt("upgrade3Money") == 50) PlayerPrefs.SetInt("upgrade3Money", 150);
            else if (PlayerPrefs.GetInt("upgrade3Money") == 150) PlayerPrefs.SetInt("upgrade3Money", 250);
            else if (PlayerPrefs.GetInt("upgrade3Money") == 250) PlayerPrefs.SetInt("upgrade3Money", 370);
            else if (PlayerPrefs.GetInt("upgrade3Money") == 370) PlayerPrefs.SetInt("upgrade3Money", 500);
            else if (PlayerPrefs.GetInt("upgrade3Money") == 500) PlayerPrefs.SetInt("upgrade3Money", 800);
            else PlayerPrefs.SetInt("upgrade3Money", 1000);

        }




        }


    void openFaucets()
    {
        orderAyarla();

        for (int i = 0; i < PlayerPrefs.GetInt("spaceCount"); i++)
        {
            spaces[i].SetActive(true);
        }

        for (int i = 0; i < 25; i++)
        {
            allFacucets[i].SetActive(false);
        }

        if (PlayerPrefs.GetInt("faucet1") != 0) { allFacucets[PlayerPrefs.GetInt("faucet1") - 1].SetActive(true); spaces[0].SetActive(false); }// 0 1 2 3 4
        if (PlayerPrefs.GetInt("faucet2") != 0) { allFacucets[PlayerPrefs.GetInt("faucet2") + 4].SetActive(true); spaces[1].SetActive(false); } // 5 6 7 8 9
        if (PlayerPrefs.GetInt("faucet3") != 0) { allFacucets[PlayerPrefs.GetInt("faucet3") + 9].SetActive(true); spaces[2].SetActive(false); } // 10 11 12 13 14
        if (PlayerPrefs.GetInt("faucet4") != 0) { allFacucets[PlayerPrefs.GetInt("faucet4") + 14].SetActive(true); spaces[3].SetActive(false); }// 15 16 17 18 19
        if (PlayerPrefs.GetInt("faucet5") != 0) { allFacucets[PlayerPrefs.GetInt("faucet5") + 19].SetActive(true); spaces[4].SetActive(false); }// 20 21 22 23 24

        tryMerge();
        if (canMerge) mergeButton.SetActive(true);
        else mergeButton.SetActive(false);


    }
    public GameObject mergeButton;

    public void addSpace()
    {
        if (PlayerPrefs.GetInt("money") >= PlayerPrefs.GetInt("upgrade2Money") && PlayerPrefs.GetInt("spaceCount") != 5)
        {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - PlayerPrefs.GetInt("upgrade2Money"));
          PlayerPrefs.SetInt("spaceCount", PlayerPrefs.GetInt("spaceCount") + 1);

            spaces[PlayerPrefs.GetInt("spaceCount") - 1].SetActive(true);

            if (PlayerPrefs.GetInt("upgrade2Money") == 50) PlayerPrefs.SetInt("upgrade2Money", 55);
            else if (PlayerPrefs.GetInt("upgrade2Money") == 55) PlayerPrefs.SetInt("upgrade2Money", 125);
            else if (PlayerPrefs.GetInt("upgrade2Money") == 125) PlayerPrefs.SetInt("upgrade2Money", 250);
            else if (PlayerPrefs.GetInt("upgrade2Money") == 250) PlayerPrefs.SetInt("upgrade2Money", 350);
           
            else PlayerPrefs.SetInt("upgrade2Money", 500);

            var confetti = Instantiate(konfetiNormal, konfetiPoslar[PlayerPrefs.GetInt("spaceCount")-1].transform.position, Quaternion.identity);
            confetti.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0);
            Destroy(confetti, 1f);

            PlayerPrefs.SetInt("rehber", 1);

        }
    }

    bool muslukAktiflikCheck()
    {
        for(int i = 0; i < 5; i++)
        {
            if (musluklarAktif[i]) return true;

        }
        return false;
    }

    bool canMerge;
    public GameObject konfetiNormal, konfetiRainbow;
    int y;
    void bosmuCheck()
    {
        GameObject[] cups = GameObject.FindGameObjectsWithTag("cup");
        
        for(int i=0; i<cups.Length; i++)
        {
            if (cups[i].GetComponent<CupScript>().canDestroy) y += 1;
        }
        if (y == cups.Length) canSpawn = true;
        y = 0;
    }


    public void merge()
    {
        tryMerge();
        if (PlayerPrefs.GetInt("money") >= PlayerPrefs.GetInt("mergeMoney") && canMerge)
        {
            string isim11 = "faucet" + (mergeInt1 + 1);
            string isim22 = "faucet" + (mergeInt2 + 1);

            PlayerPrefs.SetInt(isim11, PlayerPrefs.GetInt(isim11) + 1);
            PlayerPrefs.SetInt(isim22, 0);

            string odul = "reward" + PlayerPrefs.GetInt(isim11);
            if (PlayerPrefs.GetInt(odul) != 1)
            {
                PlayerPrefs.SetInt(odul, 1);
                levelUpPanel.SetActive(true);
                levelUps[PlayerPrefs.GetInt(isim11) - 2].SetActive(true);
            }

            var confetti = Instantiate(konfetiRainbow, konfetiPoslar[mergeInt1].transform.position, Quaternion.identity);
            confetti.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0);
            Destroy(confetti, 1f);

            mergeItem1.SetActive(false);
            mergeItem2.SetActive(false);

            PlayerPrefs.SetInt("faucetCount", PlayerPrefs.GetInt("faucetCount") - 1);
           
       //     lastSpaceCount -= 1;
            canMerge = false;
            openFaucets();
            //     Invoke("bosmuCheck", 2f);


            Debug.Log("mergeint2:" + mergeInt2);
                GameObject[] cups = GameObject.FindGameObjectsWithTag("cup");
                for (int i = 0; i < cups.Length; i++)
                {
                   // if (!cups[i].GetComponent<CupScript>().coffeeFilled && cups[i].GetComponent<CupScript>().myOrder == mergeInt2) Destroy(cups[i]);
                }
            if (mergeInt2 == PlayerPrefs.GetInt("faucetCount")+1)
            {
                GameObject[] cups1 = GameObject.FindGameObjectsWithTag("cup");
                for (int i = 0; i < cups1.Length; i++)
                {
                    if (!cups1[i].GetComponent<CupScript>().coffeeFilled)
                    {
                        if (cups1[i].GetComponent<CupScript>().myOrder >= enYüksekSayý) enYüksekSayý = cups1[i].GetComponent<CupScript>().myOrder;
                    }
                }

                GameObject[] cups2 = GameObject.FindGameObjectsWithTag("cup");
                for (int i = 0; i < cups2.Length; i++)
                {

                    if (cups2[i].GetComponent<CupScript>().myOrder == enYüksekSayý) cups2[i].GetComponent<CupScript>().üretebilir = true;

                }

            }


            if (PlayerPrefs.GetInt("mergeMoney") == 25) PlayerPrefs.SetInt("upgrade1Money", 75);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 75) PlayerPrefs.SetInt("upgrade1Money", 150);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 150) PlayerPrefs.SetInt("upgrade1Money", 300);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 300) PlayerPrefs.SetInt("upgrade1Money", 425);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 425) PlayerPrefs.SetInt("upgrade1Money", 550);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 550) PlayerPrefs.SetInt("upgrade1Money", 675);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 675) PlayerPrefs.SetInt("upgrade1Money", 800);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 800) PlayerPrefs.SetInt("upgrade1Money", 950);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 950) PlayerPrefs.SetInt("upgrade1Money", 1100);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 1100) PlayerPrefs.SetInt("upgrade1Money", 1250);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 1250) PlayerPrefs.SetInt("upgrade1Money", 1380);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 1380) PlayerPrefs.SetInt("upgrade1Money", 1500);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 1500) PlayerPrefs.SetInt("upgrade1Money", 1750);
            else if (PlayerPrefs.GetInt("upgrade1Money") == 1750) PlayerPrefs.SetInt("upgrade1Money", 2000);
            else PlayerPrefs.SetInt("upgrade1Money", 2000);


            PlayerPrefs.SetInt("rehber", 3);

        }
    }
    int enYüksekSayý;
    public GameObject mergeItem1, mergeItem2;
    int mergeInt1, mergeInt2;
    public GameObject[] konfetiPoslar;
    void tryMerge()
    {
        for(int i=0; i< PlayerPrefs.GetInt("faucetCount"); i++)
        {
            for (int k = 0; k < PlayerPrefs.GetInt("faucetCount"); k++)
            {
                if (i != k)
                {
                    string isim1 = "faucet" + (i+1);
                    string isim2 = "faucet" + (k+1);

                    if(PlayerPrefs.GetInt(isim1) == PlayerPrefs.GetInt(isim2) && PlayerPrefs.GetInt(isim2) !=5)
                    {
                        mergeItem1 = allFacucets[PlayerPrefs.GetInt(isim1) - 1 + (i * 4)];
                        mergeItem2 = allFacucets[PlayerPrefs.GetInt(isim2) - 1 + (k * 4)];
                        canMerge = true;
                        mergeInt1 = i;
                        mergeInt2 = k;
                        Debug.Log("i" + i);
                        Debug.Log("k" + k);

                      
                       



                        break;
                    }

                }
            
            }


            if (canMerge) break;


            }
    }

    public GameObject duvar, zemin;
    public Material mavi, turuncu, beyaz;
    public void changeMaterial(string renk)
    {
        if(renk == "turuncu")
        {
            duvar.GetComponent<MeshRenderer>().material = turuncu;
            zemin.GetComponent<MeshRenderer>().material = turuncu;
        }

        if (renk == "mavi")
        {
            duvar.GetComponent<MeshRenderer>().material = mavi;
            zemin.GetComponent<MeshRenderer>().material = mavi;
        }

        if (renk == "beyaz")
        {
            duvar.GetComponent<MeshRenderer>().material = beyaz;
            zemin.GetComponent<MeshRenderer>().material = beyaz;
        }
    }
    public TextMeshProUGUI rate;
    public int incomeRateDefault = 1;
    public void incomeRate(int x)
    {
        incomeRateDefault = x;
        rate.text = "Income multiple: " + x;
    }

    public AudioSource bgMusic;

    public void soundSet()
    {
        if (bgMusic.mute) bgMusic.mute = false;
        else bgMusic.mute = true;
    }

}
