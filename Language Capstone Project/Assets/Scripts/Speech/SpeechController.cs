using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;



public struct Sentence
{
    private string[] _sentence;

    public int Length { get { return _sentence.Length; } }

    public Sentence(int i)
    {
        _sentence = new string[i];
    }

    void ClearSentence()
    {
        for (int i = 0; i < _sentence.Length - 1; i++)
        {
            _sentence[i] = "";
        }
    }

    bool isEmpty()
    {
        return _sentence[0] == "";
    }
    bool isEmpty(int i)
    {
        return _sentence[i] == "";
    }

    public void SetWord(int i, string s)
    {
        _sentence[i] = s;
    }

    public string Print()
    {
        var temp = "";

        for (int i = 0; i < _sentence.Length; i++)
        {
            if (isEmpty(i)) break;
            var space = " ";
            if (i == 0 || i == _sentence.Length) space = "";
            temp = string.Concat(temp, space, _sentence[i]);
        }
        return temp;
    }
}

public class SpeechController : MonoBehaviour {

    public string[] keywords = new string[] { "he", "brushes", "his", "hair", "stop"};
    public ConfidenceLevel confidence = ConfidenceLevel.Low;

    public float speed = 1;

    public GameObject target;
    public Text results;

    KeywordRecognizer recognizer;

    public int amountOfWords = 4;
    public int wordPosition = 0;

    public Sentence formingSentence;

    

    void Start ()
    {
        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
        }


        formingSentence = new Sentence(amountOfWords);
    }


    private void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        // translates the result into text
        var word = args.text;

        // set the resulat in the correct position in the sentence
        formingSentence.SetWord(wordPosition, word);

        // print the sentence thus far
        results.text = formingSentence.Print();

        // set position back to 0
        if(wordPosition++ > formingSentence.Length - 2)
        {
            wordPosition = 0;
        }
    }
}
