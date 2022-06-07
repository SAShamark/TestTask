using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GoldPenguinController : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private Button _clickUpdateButton;
    [SerializeField] private Button _perSecondUpdateButton;

    [SerializeField] private GameObject _goldPenguin;
    [SerializeField] private TMP_Text _workTimeText;
    private readonly float[] _xPosition = {-3.5f, 3.5f};
    private bool _canDumpling = true;
    private int _doublingBananas = 2;
    private float _workTimeDoubling = 10;
    private float _timeToSpawn;
    private float _speed = 0.1f;

    private void Start()
    {
        _goldPenguin = Instantiate(_goldPenguin);
        GoldPenguin.OnClick += ClickOnGoldPenguin;
        _goldPenguin.SetActive(false);
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
        _gameManager.BananasPerSecond *= _doublingBananas;
        _gameManager.BananasPerClick *= _doublingBananas;
        _clickUpdateButton.interactable = false;
        _perSecondUpdateButton.interactable = false;
        var startWorkTime = _workTimeDoubling;

        while (_workTimeDoubling > -0.1f)
        {
            _workTimeDoubling -= Time.deltaTime;
            _workTimeText.text = Mathf.Round(_workTimeDoubling).ToString();
            if (_workTimeDoubling <= 0)
            {
                _gameManager.BananasPerSecond /= _doublingBananas;
                _gameManager.BananasPerClick /= _doublingBananas;
                _clickUpdateButton.interactable = true;
                _perSecondUpdateButton.interactable = true;
                _workTimeDoubling = startWorkTime;
                _workTimeText.text = "";
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
            _timeToSpawn = Random.Range(2, 5);
            yield return new WaitForSeconds(_timeToSpawn);
            _goldPenguin.transform.position = new Vector3(_xPosition[Random.Range(0, 2)], Random.Range(3.6f, -2.1f), 0);
            _goldPenguin.SetActive(true);
            if (_goldPenguin.transform.position.x < -2.8f)
            {
                _goldPenguin.transform.rotation = new Quaternion(0, 180, 0, 0);
                StartCoroutine(Open(true));
            }
            else if (_goldPenguin.transform.position.x > 2.8f)
            {
                _goldPenguin.transform.rotation = new Quaternion(0, 0, 0, 0);
                StartCoroutine(Open(false));
            }

            yield return new WaitForSeconds(1);

            if (_goldPenguin.transform.position.x > -3.5f && _goldPenguin.transform.position.x < -2.5f)
            {
                StartCoroutine(Close(true));
            }
            else if (_goldPenguin.transform.position.x < 3.5f)
            {
                StartCoroutine(Close(false));
            }
        }
    }

    private IEnumerator Open(bool left)
    {
        if (left)
        {
            var endPosition = new Vector3(-2.75f, _goldPenguin.transform.position.y,
                _goldPenguin.transform.position.z);

            while (_goldPenguin.transform.position.x < -2.8f)
            {
                _goldPenguin.transform.position =
                    Vector3.Lerp(_goldPenguin.transform.position, endPosition, _speed);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            var endPosition = new Vector3(2.75f, _goldPenguin.transform.position.y,
                _goldPenguin.transform.position.z);

            while (_goldPenguin.transform.position.x > 2.8f)
            {
                _goldPenguin.transform.position =
                    Vector3.Lerp(_goldPenguin.transform.position, endPosition, _speed);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private IEnumerator Close(bool left)
    {
        if (left)
        {
            var endPosition = new Vector3(-3.5f, _goldPenguin.transform.position.y,
                _goldPenguin.transform.position.z);

            while (_goldPenguin.transform.position.x > -3.45f)
            {
                _goldPenguin.transform.position =
                    Vector3.Lerp(_goldPenguin.transform.position, endPosition, _speed);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            var endPosition = new Vector3(3.5f, _goldPenguin.transform.position.y,
                _goldPenguin.transform.position.z);

            while (_goldPenguin.transform.position.x < 3.45f)
            {
                _goldPenguin.transform.position =
                    Vector3.Lerp(_goldPenguin.transform.position, endPosition, _speed);
                yield return new WaitForEndOfFrame();
            }
        }

        _goldPenguin.SetActive(false);
    }

    private void OnDestroy()
    {
        GoldPenguin.OnClick -= ClickOnGoldPenguin;
    }
}