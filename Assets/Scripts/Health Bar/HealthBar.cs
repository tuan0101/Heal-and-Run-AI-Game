using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Transform bar;
    Image colorBar;
    Color currentColor;

    // Start is called before the first frame update
    void Start()
    {
        bar = GetComponent<Transform>();
        colorBar = GetComponent<Image>();
        currentColor = colorBar.color;
    }
    private void Update()
    {
        StartCoroutine(SetSize(0.2f));
        StartCoroutine(Flasher());
    }

    // Update is called once per frame
    public IEnumerator SetSize(float sizeNormalized)
    {
        //bar.localScale = new Vector3(sizeNormalized, 1f);
        Vector3 targetPoint = new Vector3(sizeNormalized, 1f);
        bar.localScale = Vector3.Lerp(bar.localScale, targetPoint, 0.03f);
        yield return null;
    }

    IEnumerator Flasher()
    {
        while (bar.localScale.x < 0.3)
        {
            colorBar.color = Color.white;
            yield return new WaitForSeconds(.15f);

            colorBar.color = currentColor;
            yield return new WaitForSeconds(.15f);

            colorBar.color = Color.red;
            yield return new WaitForSeconds(.15f);

        }
    }
}
