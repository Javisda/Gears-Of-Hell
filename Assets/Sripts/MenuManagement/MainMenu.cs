using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Transform cameraDefaultPos;
    [SerializeField]
    private Transform cameraSelectorPos;

    public GameObject[] characters;
    private int selectedCharacter = 0;
    private int previousSelected = 0;

    // Cursor
    [SerializeField] private Texture2D cursorSprite;
    private Vector2 cursorHotSpot;

    // Platform control 
    private bool desktop;

    // Progress ui
    [SerializeField] private GameObject loaderUI;
    [SerializeField] private Slider progressSlider;

    private void Awake()
    {
        if (Application.isMobilePlatform)
        {
            desktop = false;
        }
        else if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            desktop = true;
        }
    }

    private void Start()
    {
        cursorHotSpot = new Vector2(0, 0);
        //if (desktop)
        //    Cursor.SetCursor(cursorSprite, cursorHotSpot, CursorMode.ForceSoftware);
    }

    public void PlayGame() {
        StartCoroutine(LoadScene_Coroutine());
    }

    

    public void MenuToSelector() {
        //characters[selectedCharacter].SetActive(true);
        Camera.main.transform.position = cameraSelectorPos.transform.position;
    }

    public void SelectorToMenu()
    {
        //characters[selectedCharacter].SetActive(false);
        Camera.main.transform.position = cameraDefaultPos.transform.position;
    }

    public void MenuToOptions()
    {
        characters[selectedCharacter].SetActive(false);
    }

    public void OptionsToMenu()
    {
        characters[selectedCharacter].SetActive(true);
    }

    private IEnumerator LoadScene_Coroutine() {
        progressSlider.value = 0;
        loaderUI.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("MainScene");
        asyncOperation.allowSceneActivation = false;
        float progress = 0;
        while (!asyncOperation.isDone) {
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
            progressSlider.value = progress;
            if (progress >= 0.9f) {
                progressSlider.value = 1;
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }



    // ---------- CHARACTERS SELECTOR IN BESTIARY --------------
    //public void SelectJacob()
    //{
    //    characters[1].SetActive(false);
    //    selectedCharacter = 0;
    //    PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
    //    characters[0].SetActive(true);
    //    characters[0].transform.rotation = characters[previousSelected].transform.rotation;
    //    previousSelected = 0;
    //}

    //public void SelectBrenda()
    //{
    //    characters[1].SetActive(true);
    //    selectedCharacter = 1;
    //    PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
    //    characters[0].SetActive(false);
    //    characters[1].transform.rotation = characters[previousSelected].transform.rotation;
    //    previousSelected = 1;
    //}

    public void SelectJacob() {
        characters[previousSelected].SetActive(false);
        characters[0].SetActive(true);
        characters[0].transform.rotation = characters[previousSelected].transform.rotation;
        previousSelected = 0;
    }

    public void SelectGunnerTier2() {
        characters[previousSelected].SetActive(false);
        characters[2].SetActive(true);
        characters[2].transform.rotation = characters[previousSelected].transform.rotation;
        previousSelected = 2;
    }

    public void SelectGunslingerTier2()
    {
        characters[previousSelected].SetActive(false);
        characters[3].SetActive(true);
        characters[3].transform.rotation = characters[previousSelected].transform.rotation;
        previousSelected = 3;
    }

    public void SelectWorkerTier2()
    {
        characters[previousSelected].SetActive(false);
        characters[4].SetActive(true);
        characters[4].transform.rotation = characters[previousSelected].transform.rotation;
        previousSelected = 4;
    }
}
