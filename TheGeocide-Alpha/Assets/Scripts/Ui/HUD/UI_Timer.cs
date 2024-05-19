using System.Collections;
using TMPro;
using UnityEngine;

public class UI_Timer : MonoBehaviour
{  
    [SerializeField]
    private TMP_Text _timeDecaMinText;
    [SerializeField]
    private TMP_Text _timeMinText;
    [SerializeField]
    private TMP_Text _timeDecaSecText;
    [SerializeField]
    private TMP_Text _timeSecText;
    [SerializeField]
    private GameObject _timeOutPanel;

    private float _maxMinutes = 30;
    private float _maxDuration;
    private float _timer;

    private void Start()
    {
        _timeOutPanel.SetActive(false);
        _maxDuration = _maxMinutes * 60f;
        _timer = 0;
    }
   
    void FixedUpdate()
    {
        if(_timer > _maxDuration)
        {
            return;
        }

        _timer += Time.deltaTime;
        UpdateTimerDiplsay(_timer);
    }

    private void UpdateTimerDiplsay(float timer)
    {
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);

        if(minutes >= _maxMinutes)
        {
            _timeOutPanel.SetActive(true);
            return;
        }

        string currentTime = string.Format("{00:00}{1:00}", minutes, seconds);

         _timeDecaMinText.text = currentTime[0].ToString(); 
         _timeMinText.text = currentTime[1].ToString();
        _timeDecaSecText.text = currentTime[2].ToString();
        _timeSecText.text = currentTime[3].ToString();
    }
}
