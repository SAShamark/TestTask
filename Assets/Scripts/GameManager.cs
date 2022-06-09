using System.Collections.Generic;
using Ads;
using Clicks;
using Data;
using TMPro;
using Units;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public InterstitialAds _interstitialAds;
    private MainData _mainData;
    public List<GameObject> PenguinPerSecondObjects { get; private set; }

    public float BananasPerClick { get; set; } = 10;
    public float BananasPerSecond { get; set; }
    private int PriceForClickUpdate { get; set; } = 100;
    private int PriceForPerSecond { get; set; } = 100;

    [SerializeField] private ClickOnPlayer _clickOnPlayer;
    [SerializeField] private ButtonClickUpdate _buttonUpdateClick;
    [SerializeField] private ButtonClickUpdate _buttonUpdatePerSecond;
    [SerializeField] private HorseController _horseController;
    [SerializeField] private TMP_Text _bananasNumberText;
    [SerializeField] private TMP_Text _bananasPerSecondText;
    [SerializeField] private GameObject _penguinClickObject;
    [SerializeField] private GameObject _penguinPerSecondObject;
    [SerializeField] private Transform _transformForSpawn;
    private List<GameObject> _penguinPerClickObjects;
    private Vector3 _penguinSpawnPosition;
    private int _numberClickPerSecond;
    private float _timeForBananasPerSecond;
    private const int ValueForChangeClickPrice = 2;
    private const float ValueForChangeClickFarm = 1.1f;
    private const int ValueForChangePerSecondPrice = 2;

    private void Start()
    {
        _mainData = new MainData();
        if (PlayerPrefs.HasKey("FirstStart"))
        {
            _mainData.AllBananas = PlayerPrefs.GetFloat("AllBananas");
            _mainData.ClickUpdateLevel = PlayerPrefs.GetInt("ClickLevel");
            _mainData.PerSecondLevel = PlayerPrefs.GetInt("PerSecondLevel");

            _penguinPerClickObjects = new List<GameObject>();
            for (int i = 1; i < _mainData.ClickUpdateLevel; i++)
            {
                _penguinPerClickObjects.Add(SpawnPenguin(_penguinClickObject));
                BananasPerClick *= ValueForChangeClickFarm;
                PriceForClickUpdate *= ValueForChangeClickPrice;
            }

            PenguinPerSecondObjects = new List<GameObject>();
            for (int i = 1; i < _mainData.PerSecondLevel; i++)
            {
                PenguinPerSecondObjects.Add(SpawnPenguin(_penguinPerSecondObject));
                BananasPerSecond += PenguinPerSecondObjects[i - 1].GetComponent<PenguinPerSecond>()
                    .NumberBananasPerSecond;
                PriceForPerSecond *= ValueForChangePerSecondPrice;
            }
        }

        _bananasNumberText.text = _mainData.AllBananas.ToString();
        _clickOnPlayer.Initialize();
        _clickOnPlayer.OnClick += ClickOnPlayZone;
        _buttonUpdateClick.Initialize();
        _buttonUpdateClick.OnClickUpdate += ChangeNumberBananasPerClick;
        _buttonUpdateClick.OnClickUpdate += SetClickUpdateInfo;
        _buttonUpdatePerSecond.Initialize();
        _buttonUpdatePerSecond.OnClickUpdate += ChangeNumberBananasPerSecond;
        _buttonUpdatePerSecond.OnClickUpdate += SetPerSecondUpdateInfo;
        SetClickUpdateInfo();
        SetPerSecondUpdateInfo();
    }

    private void Update()
    {
        CalculateBananasPerSecond();
    }

    private void SetClickUpdateInfo()
    {
        _buttonUpdateClick.Level.text = _mainData.ClickUpdateLevel.ToString();
        _buttonUpdateClick.Price.text = PriceForClickUpdate.ToString();
        _buttonUpdateClick.Number.text = BananasPerClick + " Per click";
    }

    private void SetPerSecondUpdateInfo()
    {
        _buttonUpdatePerSecond.Level.text = _mainData.PerSecondLevel.ToString();
        _buttonUpdatePerSecond.Price.text = PriceForPerSecond.ToString();
        _buttonUpdatePerSecond.Number.text = BananasPerSecond + " Per second";
    }

    public void ChangeNumberBananas(float valueToAddBananas)
    {
        _mainData.AllBananas += valueToAddBananas;
        _bananasNumberText.text = Mathf.Round(_mainData.AllBananas).ToString();
        for (int i = 1, countBananas = 10; i < 10; i++, countBananas *= 10)
        {
            if (_mainData.AllBananas >= countBananas && i > PlayerPrefs.GetInt("InterstitialAds"))
            {
                _interstitialAds.ShowAd();
                PlayerPrefs.SetInt("InterstitialAds", i);
            }
        }
    }

    private void ClickOnPlayZone()
    {
        _numberClickPerSecond++;
        ChangeNumberBananas(BananasPerClick);
        StartCoroutine(_horseController.PrintText(BananasPerClick));
    }

    private void ChangeNumberBananasPerClick()
    {
        if (_mainData.AllBananas >= PriceForClickUpdate)
        {
            _mainData.AllBananas -= PriceForClickUpdate;
            BananasPerClick *= ValueForChangeClickFarm;
            PriceForClickUpdate *= ValueForChangeClickPrice;
            _mainData.ClickUpdateLevel++;
            SpawnPenguin(_penguinClickObject);
        }
    }


    private void ChangeNumberBananasPerSecond()
    {
        if (_mainData.AllBananas >= PriceForPerSecond)
        {
            _mainData.AllBananas -= PriceForPerSecond;
            PriceForPerSecond *= ValueForChangePerSecondPrice;
            _mainData.PerSecondLevel++;
            PenguinPerSecondObjects.Add(SpawnPenguin(_penguinPerSecondObject));
            BananasPerSecond += PenguinPerSecondObjects[PenguinPerSecondObjects.Count - 1]
                .GetComponent<PenguinPerSecond>().NumberBananasPerSecond;
        }
    }

    private GameObject SpawnPenguin(GameObject penguinGameObject)
    {
        var penguin = Instantiate(penguinGameObject, _transformForSpawn);
        penguin.transform.position += _penguinSpawnPosition;
        _penguinSpawnPosition += new Vector3(1, 0, 0);
        if (_penguinSpawnPosition.x >= 5)
        {
            _penguinSpawnPosition += new Vector3(-5, -2f, 0);
        }

        return penguin;
    }

    private void CalculateBananasPerSecond()
    {
        _timeForBananasPerSecond += Time.deltaTime;
        if (_timeForBananasPerSecond <= 1)
        {
            var bananasPerSecond = BananasPerSecond + (BananasPerClick * _numberClickPerSecond);
            _bananasPerSecondText.text = Mathf.Round(bananasPerSecond) + "/sec";
        }
        else
        {
            _timeForBananasPerSecond = 0;
            _numberClickPerSecond = 0;
        }
    }


    public void OnPauseAndSave()
    {
        Time.timeScale = 0;
        PlayerPrefs.SetFloat("AllBananas", _mainData.AllBananas);
        PlayerPrefs.SetInt("ClickLevel", _mainData.ClickUpdateLevel);
        PlayerPrefs.SetInt("PerSecondLevel", _mainData.PerSecondLevel);
    }

    public void OffPause()
    {
        Time.timeScale = 1;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void DeleteAllKey()
    {
        PlayerPrefs.DeleteAll();
    }

    private void OnDestroy()
    {
        _clickOnPlayer.OnClick -= ClickOnPlayZone;
        _buttonUpdateClick.OnClickUpdate -= ChangeNumberBananasPerClick;
        _buttonUpdateClick.OnClickUpdate -= SetClickUpdateInfo;
        _buttonUpdatePerSecond.OnClickUpdate -= ChangeNumberBananasPerSecond;
        _buttonUpdatePerSecond.OnClickUpdate -= SetPerSecondUpdateInfo;
    }
}