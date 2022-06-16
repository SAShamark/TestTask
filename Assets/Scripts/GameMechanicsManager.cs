using System.Collections.Generic;
using Ads;
using Data;
using TMPro;
using UI.Clicks;
using Units;
using UnityEngine;

public class GameMechanicsManager : MonoBehaviour
{
    public static GameMechanicsManager SingletonGameMechanicsManager;
    public MainData _mainData;
    public List<GameObject> PenguinPerSecondObjects { get; private set; }

    public float BananasPerClick { get; set; } = 10;
    public float BananasPerSecond { get; set; }
    private int PriceForClickUpdate { get; set; } = 100;
    private int PriceForPerSecond { get; set; } = 100;
    [SerializeField] private InterstitialAds _interstitialAds;
    [SerializeField] private SaveData _saveData;
    [SerializeField] private ClickOnPlayer _clickOnPlayer;
    [SerializeField] private ButtonClickUpdate _buttonUpdateClick;
    [SerializeField] private ButtonClickUpdate _buttonUpdatePerSecond;
    [SerializeField] private Horse _horse;
    [SerializeField] private TMP_Text _bananasNumberText;
    [SerializeField] private TMP_Text _bananasPerSecondText;
    [SerializeField] private GameObject _penguinClickObject;
    [SerializeField] private GameObject _penguinPerSecondObject;
    [SerializeField] private Transform _transformForSpawn;
    private Vector3 _penguinSpawnPosition;
    private int _numberClickPerSecond;
    private float _timeForBananasPerSecond;
    private const int ValueForChangeClickPrice = 2;
    private const float ValueForChangeClickFarm = 1.1f;
    private const int ValueForChangePerSecondPrice = 2;

    private void Awake()
    {
        if (SingletonGameMechanicsManager==null)
        {
            SingletonGameMechanicsManager = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    private void Start()
    {
        LoadInfo();
        _bananasNumberText.text = _mainData.AllBananas.ToString();
        _clickOnPlayer.Initialize();
        _clickOnPlayer.OnClick += ClickOnPlayZone;
        _buttonUpdateClick.Initialize();
        _buttonUpdateClick.OnClickUpdate += UpdateNumberBananasPerClick;
        _buttonUpdateClick.OnClickUpdate += SetClickUpdateInfo;
        _buttonUpdatePerSecond.Initialize();
        _buttonUpdatePerSecond.OnClickUpdate += UpdateNumberBananasPerSecond;
        _buttonUpdatePerSecond.OnClickUpdate += SetPerSecondUpdateInfo;
        SetClickUpdateInfo();
        SetPerSecondUpdateInfo();
    }

    private void Update()
    {
        CalculateBananasPerSecond();
    }

    private void LoadInfo()
    {
        _mainData = new MainData();
        _saveData.Load();
        for (int i = 1; i < _mainData.ClickUpdateLevel; i++)
        {
            SpawnPenguin(_penguinClickObject);
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

    private void SetClickUpdateInfo()
    {
        _buttonUpdateClick.Level.text = _mainData.ClickUpdateLevel.ToString();
        _buttonUpdateClick.Price.text = PriceForClickUpdate.ToString();
        _buttonUpdateClick.Number.text = Mathf.Round(BananasPerClick) + " Per click";
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
        OrShowAds();
    }

    private void OrShowAds()
    {
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
        _horse.PrintText(BananasPerClick);
    }

    private void UpdateNumberBananasPerClick()
    {
        if (Mathf.Round(_mainData.AllBananas) >= PriceForClickUpdate)
        {
            _mainData.AllBananas -= PriceForClickUpdate;
            _bananasNumberText.text = Mathf.Round(_mainData.AllBananas).ToString();
            BananasPerClick *= ValueForChangeClickFarm;
            PriceForClickUpdate *= ValueForChangeClickPrice;
            _mainData.ClickUpdateLevel++;
            SpawnPenguin(_penguinClickObject);
        }
    }


    private void UpdateNumberBananasPerSecond()
    {
        if (Mathf.Round(_mainData.AllBananas) >= PriceForPerSecond)
        {
            _mainData.AllBananas -= PriceForPerSecond;
            _bananasNumberText.text = Mathf.Round(_mainData.AllBananas).ToString();
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
        _penguinSpawnPosition += new Vector3(0.8f, 0, 0);
        if (_penguinSpawnPosition.x >= 4f)
        {
            _penguinSpawnPosition += new Vector3(-4, -1.2f, 0);
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

    private void OnDestroy()
    {
        _clickOnPlayer.OnClick -= ClickOnPlayZone;
        _buttonUpdateClick.OnClickUpdate -= UpdateNumberBananasPerClick;
        _buttonUpdateClick.OnClickUpdate -= SetClickUpdateInfo;
        _buttonUpdatePerSecond.OnClickUpdate -= UpdateNumberBananasPerSecond;
        _buttonUpdatePerSecond.OnClickUpdate -= SetPerSecondUpdateInfo;
    }
}