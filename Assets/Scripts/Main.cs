using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    private int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        // �I�u�W�F�N�g����Text�R���|�[�l���g���擾
        GameObject score_object = GameObject.Find("Score");
        Text score_text = score_object.GetComponent<Text>();
        // �e�L�X�g�̕\��������s����
        score_text.text = "Score : 0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeScore(int val)
    {
        score += val;
        // �I�u�W�F�N�g����Text�R���|�[�l���g���擾
        GameObject score_object = GameObject.Find("Score");
        Text score_text = score_object.GetComponent<Text>();
        // �e�L�X�g�̕\��������s����
        score_text.text = "Score : " + score;
    }
}
