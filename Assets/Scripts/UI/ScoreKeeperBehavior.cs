using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreKeeperBehavior : MonoBehaviour
{
    public BuffManager buffManager;
    int obtainTrees=0;
    public Text obtainText;
    int remainTrees;
    public Text remainText;
    public Text buffText;
    public Text buffPrefab;
    public GameObject buff;
    float letterPause = 0.03f;
    public AudioClip[] sound;

    public Text currentLevel;
    LevelGenerator generator;
    Vector3 currentPos;
    Vector3 localScale;
    // Start is called before the first frame update
    void Start()
    {
        buffManager = FindObjectOfType<BuffManager>();
        generator = FindObjectOfType<LevelGenerator>();
        currentLevel.text = "Level: " + MainMenu.level;

        DisplayObtainText(obtainTrees.ToString());

        remainTrees = generator.numOfTrees;

        // buff text
        localScale = buffText.transform.localScale;
        //buffText.transform.localScale = Vector3.zero;
    
        //StartCoroutine(BuffMessage("hello Iam so happy!"));
        currentPos = buff.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void plantHealed()
    {
        buffManager.activateRandomBuff();
        obtainTrees++;
        DisplayObtainText(obtainTrees.ToString());
        if (obtainTrees >= 10)
        {
            LoadScene("WinScene");
        }
    }

    public void plantDead()
    {
        remainTrees--;
        remainText.text = "Remain: " + remainTrees;
        // Game over
        if (remainTrees < (20 - obtainTrees))
        {
            remainText.color = Color.red;
            if (remainTrees < (10 - obtainTrees))
                LoadScene("DefeatScene");
        }
    }
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    void DisplayObtainText(string numString)
    {
        obtainText.text = "Obtain: " + numString + "/10";
    }

    IEnumerator TypeText(Text buffText, string message)
    {
        
        int random = Random.Range(0, sound.Length);
        //yield return new WaitForSeconds(0.5f);
        GetComponent<AudioSource>().PlayOneShot(sound[random]);

        foreach (char letter in message.ToCharArray())
        {          
            buffText.text += letter;              
            yield return new WaitForSeconds(letterPause);
        }
    }

    Text[] text = new Text[3];
    public IEnumerator BuffMessage(string message)
    {
        int i = 0;
        
        if(text[0] == null)
        {
            i = 0;
            Destroy(text[1]);
            StartCoroutine(MoveUp(text[2]));
        }
        else if (text[1] == null)
        {
            i = 1;
            StartCoroutine(MoveUp(text[0]));
            Destroy(text[2]);
        }
        else if (text[2] == null)
        {
            i = 2;
            Destroy(text[0]);
            StartCoroutine(MoveUp(text[1]));
            
        }

        text[i] = Instantiate(buffPrefab, buffPrefab.transform.position, Quaternion.identity) as Text;
        text[i].transform.SetParent(GameObject.Find("Canvas").transform, false);
        text[i].transform.position = currentPos;
      
        StartCoroutine(TypeText(text[i], message));
        StartCoroutine(FadeTextToFullAlpha(1.5f, text[i]));
        
        yield return new WaitForSeconds(2);
        StartCoroutine(FadeTextToZeroAlpha(1.5f, text[i]));
    }

    IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        if (i)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
            while (i.color.a < 1.0f)
            {
                if (i)
                    i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
                yield return null;
            }
        }     
    }

    IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        if (i)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
            while (i.color.a > 0.0f)
            {

                if (i)
                {
                    i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));

                }

                yield return null;
            }
            Destroy(i.gameObject);
        }

    }

    
    IEnumerator MoveUp(Text buffText)
    {
        //buffText.transform.localScale = Vector3.zero;
        // moving up
        if (buffText)
        {
            Vector3 up = buffText.transform.position + new Vector3(0, 30f, 0);
            float t = 0;
            while (t < 1f)
            {
                t += 0.005f;
                if (buffText)
                {
                    buffText.transform.position = Vector3.Lerp(buffText.transform.position, up, t);
                    //buffText.transform.localScale = Vector3.Lerp(buffText.transform.localScale, localScale, t / 2);
                }

                // from zero to 1 scale

                yield return null;
            }
        }

        //yield return new WaitForSeconds(1f);
        //up += new Vector3(0, 100f, 0);
        //t = 0;
        //while (t < 1f)
        //{
        //    t += 0.001f;
        //    if(buffText)
        //        buffText.transform.position = Vector3.Lerp(buffText.transform.position, up, t);         
        //    yield return null;
        //}
    }
}
