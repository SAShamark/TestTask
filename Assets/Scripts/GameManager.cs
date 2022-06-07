using System.Collections.Generic;
using Ads;
using Clicks;
using TMPro;
using Units;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> PenguinPerSecondObjects => _penguinPerSecondObjects;
    public float BananasPerClick { get; set; } = 10;
    public float BananasPerSecond { get; set; }
    private int PriceForClickUpdate { get; set; } = 100;
    private int ClickUpdateLevel { get; set; } = 1;
    private int PriceForPerSecond { get; set; } = 100;
    private int PerSecondLevel { get; set; } = 1;


    [SerializeField] private ClickOnPlayer _clickOnPlayer;
    [SerializeField] private ButtonClickUpdate _buttonUpdateClick;
    [SerializeField] private ButtonClickUpdate _buttonUpdatePerSecond;
    [SerializeField] private HorseController _horseController;
    [SerializeField] private TMP_Text _bananasNumberText;
    [SerializeField] private TMP_Text _bananasPerSecondText;
    [SerializeField] private GameObject _penguinClickObject;
    [SerializeField] private GameObject _penguinPerSecondObject;
    [SerializeField] private Transform _transformForSpawn;
    private List<GameObject> _penguinPerSecondObjects;
    private List<GameObject> _penguinPerClickObjects;

    public InterstitialAds _interstitialAds;
    private Vector3 _penguinSpawnPosition;

    private float _allBananas;
    private int _numberClickPerSecond;
    private float _timeForBananasPerSecond;
    private const int ValueForChangeClickPrice = 2;
    private const float ValueForChangeClickFarm = 1.1f;
    private const int ValueForChangePerSecondPrice = 2;

    private void Start()
    {
        if (PlayerPrefs.HasKey("FirstStart"))
        {
            _allBananas = PlayerPrefs.GetFloat("AllBananas");
            ClickUpdateLevel = PlayerPrefs.GetInt("ClickLevel");
            PerSecondLevel = PlayerPrefs.GetInt("PerSecondLevel");
            _penguinPerClickObjects = new List<GameObject>();

            for (int i = 1; i < ClickUpdateLevel; i++)
            {
                _penguinPerClickObjects.Add(SpawnPenguin(_penguinClickObject));
                BananasPerClick *= ValueForChangeClickFarm;
                PriceForClickUpdate *= ValueForChangeClickPrice;
            }

            _penguinPerSecondObjects = new List<GameObject>();

            for (int i = 1; i < PerSecondLevel; i++)
            {
                _penguinPerSecondObjects.Add(SpawnPenguin(_penguinPerSecondObject));
                BananasPerSecond += _penguinPerSecondObjects[i - 1].GetComponent<PenguinPerSecond>()
                    .NumberBananasPerSecond;
                PriceForPerSecond *= ValueForChangePerSecondPrice;
            }
        }

        _bananasNumberText.text = _allBananas.ToString();
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
        _buttonUpdateClick.Level.text = ClickUpdateLevel.ToString();
        _buttonUpdateClick.Price.text = PriceForClickUpdate.ToString();
        _buttonUpdateClick.Number.text = BananasPerClick + " Per click";
    }

    private void SetPerSecondUpdateInfo()
    {
        _buttonUpdatePerSecond.Level.text = PerSecondLevel.ToString();
        _buttonUpdatePerSecond.Price.text = PriceForPerSecond.ToString();
        _buttonUpdatePerSecond.Number.text = BananasPerSecond + " Per second";
    }

    public void ChangeNumberBananas(float valueToAddBananas)
    {
        _allBananas += valueToAddBananas;
        _bananasNumberText.text = Mathf.Round(_allBananas).ToString();
        for (int i = 1, countBananas = 10; i < 10; i++, countBananas *= 10)
        {
            if (_allBananas >= countBananas && i > PlayerPrefs.GetInt("InterstitialAds"))
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
        if (_allBananas >= PriceForClickUpdate)
        {
            _allBananas -= PriceForClickUpdate;
            BananasPerClick *= ValueForChangeClickFarm;
            PriceForClickUpdate *= ValueForChangeClickPrice;
            ClickUpdateLevel++;
            SpawnPenguin(_penguinClickObject);
        }
    }


    private void ChangeNumberBananasPerSecond()
    {
        if (_allBananas >= PriceForPerSecond)
        {
            _allBananas -= PriceForPerSecond;
            PriceForPerSecond *= ValueForChangePerSecondPrice;
            PerSecondLevel++;
            _penguinPerSecondObjects.Add(SpawnPenguin(_penguinPerSecondObject));
            BananasPerSecond += _penguinPerSecondObjects[_penguinPerSecondObjects.Count - 1]
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
            _bananasPerSecondText.text = bananasPerSecond + "/sec";
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
        PlayerPrefs.SetFloat("AllBananas", _allBananas);
        PlayerPrefs.SetInt("ClickLevel", ClickUpdateLevel);
        PlayerPrefs.SetInt("PerSecondLevel", PerSecondLevel);
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