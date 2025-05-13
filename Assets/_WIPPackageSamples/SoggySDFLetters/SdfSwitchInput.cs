using TMPro;
using UnityEngine;

public class SdfSwitchInput : MonoBehaviour
{
    public TMP_Text keyboardOutput;
    public SdfSwitch vfxController;

    private string lastText = "";

    void Update()
    {
        if (keyboardOutput == null || vfxController == null) return;

        string currentText = keyboardOutput.text;

        if (currentText != lastText && currentText.Length > 0)
        {
            char lastChar = currentText[currentText.Length - 1];
            vfxController.SetSDFByLetter(lastChar);
            lastText = currentText;
        }
    }
}
