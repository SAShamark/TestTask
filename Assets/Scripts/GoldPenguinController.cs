using System.Collections;
using DG.Tweening;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GoldPenguinController : MonoBehaviour
{
    private GameMechanicsManager _gameMechanicsManager;

    [SerializeField] private Button _clickUpdateButton;
    [SerializeField] private Button _perSecondUpdateButton;
    [SerializeField] private GameObject _goldPenguin;
    [SerializeField] private GameObject _doubling;
    [SerializeField] private TMP_Text _workTimeText;
    private readonly float[] _xPosition = {-3.5f, 3.5f};
    private bool _canDumpling = true;
    private const int DoublingBananas = 2;
    private float _workTimeDoubling = 10;
    private float _timeToSpawn;
    private const float Speed = 0.1f;

    private void Start()
    {
        _gameMechanicsManager = GameMechanicsManager.SingletonGameMechanicsManager;
        _goldPenguin = Instantiate(_goldPenguin);
        GoldPenguin.OnClick += ClickOnGoldPenguin;
        _goldPenguin.SetActive(false);
        _doubling.SetActive(false);
        StartCoroutine(Spawner());
    }

    private void ClickOnGoldPenguin()
    {
        if (_canDumpling)
        {
            _canDumpling = false;
            StartCoroutine(Dumpling());
        }
    }

    private IEnumerator Dumpling()
    {
        _doubling.SetActive(true);

        _gameMechanicsManager.BananasPerSecond *= DoublingBananas;
        _gameMechanicsManager.BananasPerClick *= DoublingBananas;
        foreach (var penguinPerSecondObject in _gameMechanicsManager.PenguinPerSecondObjects)
        {
            penguinPerSecondObject.GetComponent<PenguinPerSecond>().NumberBananasPerSecond *= 2;
        }

        _clickUpdateButton.interactable = false;
        _perSecondUpdateButton.interactable = false;
        var startWorkTime = _workTimeDoubling;

        while (_workTimeDoubling > -0.1f)
        {
            _workTimeDoubling -= Time.deltaTime;
            _workTimeText.text = Mathf.Round(_workTimeDoubling).ToString();
            if (_workTimeDoubling <= 0)
            {
                _gameMechanicsManager.BananasPerSecond /= DoublingBananas;
                _gameMechanicsManager.BananasPerClick /= DoublingBananas;
                foreach (var penguinPerSecondObject in _gameMechanicsManager.PenguinPerSecondObjects)
                {
                    penguinPerSecondObject.GetComponent<PenguinPerSecond>().NumberBananasPerSecond /= 2;
                }

                _clickUpdateButton.interactable = true;
                _perSecondUpdateButton.interactable = true;
                _workTimeDoubling = startWorkTime;
                _doubling.SetActive(false);
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        _canDumpling = true;
    }

    private IEnumerator Spawner()
    {
        while (true)
        {
            _timeToSpawn = Random.Range(30, 120);
            yield return new WaitForSeconds(_timeToSpawn);
            _goldPenguin.transform.position = new Vector3(_xPosition[Random.Range(0, 2)], Random.Range(3.6f, -2.1f), 0);
            _goldPenguin.SetActive(true);
            if (_goldPenguin.transform.position.x < -2.5f)
            {
                _goldPenguin.transform.rotation = new Quaternion(0, 180, 0, 0);
                OpenAndClose(-2.4f);
            }
            else if (_goldPenguin.transform.position.x > 2.5f)
            {
                _goldPenguin.transform.rotation = new Quaternion(0, 0, 0, 0);
                OpenAndClose(2.4f);
            }
        }
    }

    private void OpenAndClose(float endPositionX)
    {
        var sequence = DOTween.Sequence();
        var startPosition = _goldPenguin.transform.position;
        var endPosition = new Vector3(endPositionX, startPosition.y, startPosition.z);

        sequence.Append(_goldPenguin.transform.DOMove(endPosition, Speed));
        sequence.AppendInterval(2f);

        sequence.Append(_goldPenguin.transform.DOMove(startPosition, Speed));
        sequence.OnComplete(Closed);
    }

    private void Closed()
    {
        _goldPenguin.SetActive(false);
    }

    private void OnDestroy()
    {
        GoldPenguin.OnClick -= ClickOnGoldPenguin;
    }
}