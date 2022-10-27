using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageShops : MonoBehaviour
{
    // Prefab Tienda
    [SerializeField]
    private GameObject tienda;

    // Posiciones de prueba
    [SerializeField]
    private Transform[] posicionesIniciales;

    // Imagenes de los objetos
    [SerializeField]
    private Sprite[] spritesAmmo;
    [SerializeField]
    private Sprite[] spritesPerks;
    [SerializeField]
    private Sprite[] spritesGuns;

    // Display de t�tulo
    [SerializeField]
    private Text[] titleText;
    // Display de Sprites
    [SerializeField]
    private Image[] displaysAmmoAndGuns;
    [SerializeField]
    private Image[] displaysPerks;
    // Display de precio
    [SerializeField]
    private Text[] coinsText;


    // Precios
    [SerializeField]
    private int[] ammoPrices;    // En orden: Subfusil, Rifle, Francotirador, Bazooka
    [SerializeField]
    private int[] gunPrices;     // En orden: Subfusil, Rifle, Francotirador, Bazooka
    [SerializeField]
    private int[] ammoQuantity;  // En orden: Subfusil, Rifle, Francotirador, Bazooka

    // Textos
    [SerializeField]
    private string[] gunTexts;
    [SerializeField]
    private string[] ammoTexts;

    void Start()
    {
        for (int i = 0; i < posicionesIniciales.Length; i++)
        {
            GameObject t = Instantiate(tienda, posicionesIniciales[i].position, Quaternion.identity);
        }

        // Prueba
        displaysAmmoAndGuns[0].sprite = spritesAmmo[0];
        titleText[0].text = "PISTOLAA";
        coinsText[0].text = ammoPrices[0].ToString();
    }

    public void RefreshShop()
    {
        Debug.Log("NUEVOS OBJETOS!");

        int numGunsHasPlayer = 0;

        // MUNICION Y ARMAS
        bool[] playerGuns = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ShootSystem>().availableGuns;
        for (int i = 0; i < playerGuns.Length; i++) {
            if (playerGuns[i]) {
                numGunsHasPlayer++;
            }
        }

        // En caso de que el jugador no tenga compradas todas las armas, se ofrece alguna opci�n
        if (numGunsHasPlayer == 1) // Solo tiene la pistola, por lo que la tienda ofrece 3 armas ya que no podr� comprar munici�n (la de la pistola es infinita)
        {
            int numGeneratedGuns = 0;
            int[] generatedGunsIndexes = new int[3];
            bool[] generatedInShopGuns = (bool[])playerGuns.Clone();
            while (numGeneratedGuns != 3) {
                int idx = Random.Range(1, playerGuns.Length); // Se genera a partir del 1 ya que el indice 0 es la propia pistola
                if (!generatedInShopGuns[idx]) { // Si el arma no ha sido ya generada en la tienda se genera dicha arma
                    generatedGunsIndexes[numGeneratedGuns] = idx;
                    numGeneratedGuns++;
                    generatedInShopGuns[idx] = true;
                }
            }

            // En este punto tenemos los indices de las armas a mostrar en tienda por lo que actualizamos la UI
            for (int i = 0; i < generatedGunsIndexes.Length; i++) {
                titleText[i].text = gunTexts[generatedGunsIndexes[i]];
                displaysAmmoAndGuns[i].sprite = spritesGuns[generatedGunsIndexes[i]];
                coinsText[i].text = gunPrices[generatedGunsIndexes[i]].ToString();
            }
        } 
        else if (numGunsHasPlayer == 2) // Solo tiene la pistola y un arma m�s, por lo que se ofrecen 2 armas y munici�n de la que ya se tiene
        {
            // Generacion de armas
            int numGeneratedGuns = 0;
            int[] generatedGunsIndexes = new int[2];
            bool[] generatedInShopGuns = (bool[])playerGuns.Clone();
            while (numGeneratedGuns != 2)
            {
                int idx = Random.Range(1, playerGuns.Length);
                if (!generatedInShopGuns[idx])
                {
                    generatedGunsIndexes[numGeneratedGuns] = idx;
                    numGeneratedGuns++;
                    generatedInShopGuns[idx] = true;
                }
            }

            // Generaci�n de munici�n
            int ammoIdx = 0;
            for (int i = 1; i < playerGuns.Length; i++) {
                if (playerGuns[i]) {
                    ammoIdx = i;
                    break;
                }
            }

            // Actualizacion de UI de armas
            for (int i = 0; i < generatedGunsIndexes.Length; i++)
            {
                titleText[i].text = gunTexts[generatedGunsIndexes[i]];
                displaysAmmoAndGuns[i].sprite = spritesGuns[generatedGunsIndexes[i]];
                coinsText[i].text = gunPrices[generatedGunsIndexes[i]].ToString();
            }

            // Actualizacion de UI de municion
            titleText[2].text = ammoTexts[ammoIdx];
            displaysAmmoAndGuns[2].sprite = spritesAmmo[ammoIdx];
            coinsText[2].text = ammoPrices[ammoIdx].ToString();
        } 
        else if (numGunsHasPlayer == 3 && playerGuns[0]) // Se tienen todas las armas y una de ellas es la pistola, por lo que se ofrece un arma para cambiar por la pistola
        {
            // Generacion de armas
            int numGeneratedGuns = 0;
            int generatedGunIndex = 0;
            bool[] generatedInShopGuns = (bool[])playerGuns.Clone();
            while (numGeneratedGuns != 1)
            {
                int idx = Random.Range(1, playerGuns.Length);
                if (!generatedInShopGuns[idx])
                {
                    generatedGunIndex = idx;
                    numGeneratedGuns++;
                }
            }

            // Generacion de municion
            int numGeneratedAmmo = 0;
            int[] generatedAmmoIndexes = new int[2];
            bool[] generatedInShopAmmo = new bool[2];
            while (numGeneratedAmmo != 2)
            {
                int idx = Random.Range(1, playerGuns.Length);
                if (!generatedInShopAmmo[idx] && playerGuns[idx]) // Si la municion no ha sido generada y el arma la posee el jugador
                {
                    generatedAmmoIndexes[numGeneratedAmmo] = idx;
                    numGeneratedAmmo++;
                    generatedInShopAmmo[idx] = true;
                }
            }


            // Actualizacion de UI de armas
            titleText[0].text = gunTexts[generatedGunIndex];
            displaysAmmoAndGuns[0].sprite = spritesGuns[generatedGunIndex];
            coinsText[0].text = gunPrices[generatedGunIndex].ToString();

            // Actualizacion de UI de municion
            for (int i = 1; i < 3; i++) { 
                titleText[i].text = ammoTexts[generatedAmmoIndexes[i - 1]];
                displaysAmmoAndGuns[i].sprite = spritesAmmo[generatedAmmoIndexes[i - 1]];
                coinsText[i].text = ammoPrices[generatedAmmoIndexes[i - 1]].ToString();
            }
        } 
        else if (numGunsHasPlayer == 3 && !playerGuns[0]) // El jugador tiene ya 3 armas, sienda una de ellas distinta a la pistola por lo que se ofrecen los 3 tipos de munici�n
        {
            // Se muestran las municiones de las armas que tiene el jugador
            for (int i = 0; i < playerGuns.Length; i++)
            {
                if (playerGuns[i]) { 
                    titleText[i].text = ammoTexts[i];
                    displaysAmmoAndGuns[i].sprite = spritesAmmo[i];
                    coinsText[i].text = ammoPrices[i].ToString();
                }
            }
        }


        // PERKS/VENTAJAS
        int numPerksHasPlayer = 0;
        bool[] playerPerks = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PerksManager>().availablePerks;
        for (int i = 0; i < playerPerks.Length; i++)
        {
            if (playerPerks[i])
            {
                numPerksHasPlayer++;
            }
        }

        // Se mostrar�n �nicamente ventajas nuevas en caso de que el jugador no tenga 3 por el momento
        if (numPerksHasPlayer < 3)
        {
            int numNewGeneratedPerks = 0;
            int[] generatedPerksIndexes = new int[3];
            bool[] generatedInShopPerks = (bool[])playerPerks.Clone();

            while (numNewGeneratedPerks != 3) { 
                int idx = Random.Range(0, playerPerks.Length);
                if (!generatedInShopPerks[idx]) {
                    generatedPerksIndexes[numNewGeneratedPerks] = idx;
                    numNewGeneratedPerks++;
                    generatedInShopPerks[idx] = true;
                }
            }
        }
        else if (numPerksHasPlayer == 3) // Caso de que al jugador le quedan 2 ventajas por conseguir. Muestra 1 para mejorar y 2 para comprar nuevas.
        {

        }
        else if (numPerksHasPlayer == 4) // Caso de que al jugador le quedan 1 ventaja por conseguir. Muestra 2 para mejorar y 1 para comprar nueva.
        {

        }
        else // Caso de que el jugador tiene todas las ventajas. Muestra 3 para mejorar.
        { 
        
        }

    }

    public void BuyAmmo1()
    {
        Debug.Log("Has comprado Ammo1");

        // Prueba
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ShootSystem>().guns.AddAmmo(0, 300);
    }
    public void BuyAmmo2()
    {
        Debug.Log("Has comprado Ammo2");
    }
    public void BuyAmmo3()
    {
        Debug.Log("Has comprado Ammo3");
    }
    public void BuyPerk1()
    {
        Debug.Log("Has comprado Perk1");
    }
    public void BuyPerk2()
    {
        Debug.Log("Has comprado Perk2");
    }
    public void BuyPerk3()
    {
        Debug.Log("Has comprado Perk3");
    }
}
