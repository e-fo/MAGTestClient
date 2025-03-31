using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlertUI : MonoBehaviour
{
    [SerializeField] Button _okButton;               
    [SerializeField] Button _rejectButton;              

    [SerializeField] TextMeshProUGUI _okButtonTxt;   
    [SerializeField] TextMeshProUGUI _rejectButtonTxt;

    [SerializeField] TextMeshProUGUI _messageTxt;

    public async UniTask<bool> Show(string message, string okButtonText = "Yes", string rejectButtonText = "No")
    {
        gameObject.SetActive(true);
        _messageTxt.text = message;
        _okButtonTxt.text = okButtonText;
        _rejectButtonTxt.text = rejectButtonText;

        int index = await UniTask.WhenAny(_okButton.OnClickAsync(), _rejectButton.OnClickAsync());

        return index == 0;
    }
}