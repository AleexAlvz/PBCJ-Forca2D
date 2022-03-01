using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private int numberAttempts;
    private int maxAttempts;
    private int score;

    public GameObject center;
    public GameObject letter;

    private string hiddenWord = "";
    private int hiddenWordLength;
    char[] hiddenLetters;
    bool[] foundLetters;

    // Start is called before the first frame update
    void Start()
    {
        center = GameObject.Find("screenCenter");
        InitGame();
        InitLetter();
        score = 0;
        numberAttempts = 0;
        maxAttempts = hiddenWordLength + 3;
        UpdateNumberAttempts();
        UpdateScoreUI();
    }

    // Update is called once per frame
    void Update()
    {
        CheckKeyBoard();
        VerificaJogo();
    }

    void InitLetter()
    {
        int wordLenght = hiddenWordLength;
        for (int i = 0; i < wordLenght; i++)
        {
            Vector3 newPosition;
            newPosition = new Vector3(center.transform.position.x + ((i-wordLenght/2.0f)*80), center.transform.position.y, center.transform.position.z);
            GameObject newLetter = (GameObject)Instantiate(letter, newPosition, Quaternion.identity);
            newLetter.name = "letra" + (i + 1);
            newLetter.transform.SetParent(GameObject.Find("Canvas").transform);
        }
    }

    void InitGame() 
    {
        hiddenWord = RandomWord();
        print(hiddenWord);
        hiddenWord = hiddenWord.ToUpper();
        hiddenWordLength = hiddenWord.Length;
        foundLetters = new bool[hiddenWordLength+1];
        hiddenLetters = hiddenWord.ToCharArray();
    }

    string RandomWord()
    {
        string[] randomWords = PegaListaDePalavrasDoArquivo();
        int randomIndex = Random.Range(0, randomWords.Length);
        return randomWords[randomIndex];
    }

    string [] PegaListaDePalavrasDoArquivo()
    {
        TextAsset palavrasTextAsset = (TextAsset)Resources.Load("palavras", typeof(TextAsset));
        string palavrasJuntas = palavrasTextAsset.text;
        string[] listaDePalavras = palavrasJuntas.Split(' ');
        return listaDePalavras;
    }

    void CheckKeyBoard()
    {

        if(Input.anyKeyDown)
        {
            char keyDown = Input.inputString.ToCharArray()[0];
            int keyInt = System.Convert.ToInt32(keyDown);

            if(keyInt >= 97 && keyInt <= 122)
            {
                numberAttempts++;
                UpdateNumberAttempts();

                bool letraEncontrada = false;
                
                for (int i=0; i<=hiddenWordLength-1; i++)
                {
                    if(!foundLetters[i])
                    {
                        keyDown = System.Char.ToUpper(keyDown);
                        if (keyDown == hiddenLetters[i])
                        {
                            letraEncontrada = true;
                            foundLetters[i] = true;
                            GameObject.Find("letra" + (i + 1)).GetComponent<Text>().text = keyDown.ToString();
                            score = PlayerPrefs.GetInt("score");
                            score++;
                            PlayerPrefs.SetInt("score", score);

                            UpdateScoreUI();
                        }                    
                    }                   
                }              
                EfeitoPosTentativa(letraEncontrada);
            }
        }
    }

    private void EfeitoPosTentativa(bool letraEncontrada)
    {
        if (letraEncontrada)
        {
            GameObject.Find("RightEffect").GetComponent<AudioSource>().Play();
        }
        else
        {
            GameObject.Find("WrongEffect").GetComponent<AudioSource>().Play();
        }
    }

    private void UpdateNumberAttempts()
    {
        GameObject.Find("attempts").GetComponent<Text>().text = "Tentativas: " + numberAttempts + " | " + maxAttempts;
        
    }

    private void VerificaJogo()
    {
        if (score == hiddenWordLength)
        {
            PlayerPrefs.SetString("ultimaPalavraOculta", hiddenWord);
            SceneManager.LoadScene("Lab1_salvo");
        } else if (numberAttempts >= maxAttempts)
        {
            SceneManager.LoadScene("Lab1_forca");
        }
    }

    private void UpdateScoreUI()
    {
        GameObject.Find("scoreUI").GetComponent<Text>().text = "Pontuação: " + score;
    }
}