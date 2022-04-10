using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rpcmanager : MonoBehaviour
{
    public Material screenMat;
    public Texture rock;
    public Texture scissors;
    public Texture paper;
    public Texture vs;
    public Texture win;
    public Texture loose;
    public Texture tie;
    public Texture startup;
    string active = "";
    int index;
    bool counterStart = false;
    float counter = 0;
    string bossSel = "";
    public bool lost = false;
    // Start is called before the first frame update
    void Start()
    {
        index = screenMat.shader.GetPropertyNameId(0);
        screenMat.SetTexture(index, startup);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(counter);
        if (counterStart)
        {
            counter += Time.deltaTime;
            if (counter > 1 && counter < 3)
            {
                screenMat.SetTexture(index, vs);
            }
            else if (counter > 3 && counter < 5)
            {
                if (bossSel == "rock")
                {
                    screenMat.SetTexture(index, rock);
                }
                if (bossSel == "paper")
                {
                    screenMat.SetTexture(index, paper);
                }
                if (bossSel == "scissors")
                {
                    screenMat.SetTexture(index, scissors);
                }

            }
            else if (counter > 5 && counter<8)
            {
                if(active== "rock")
                {
                    if (bossSel == "rock")
                    {
                        screenMat.SetTexture(index, tie);
                    }
                    if (bossSel == "paper")
                    {
                        screenMat.SetTexture(index, loose);
                        lost = true;
                    }
                    if (bossSel == "scissors")
                    {
                        screenMat.SetTexture(index, win);
                    }
                }
                if (active == "paper")
                {
                    if (bossSel == "paper")
                    {
                        screenMat.SetTexture(index, tie);
                    }
                    if (bossSel == "scissors")
                    {
                        screenMat.SetTexture(index, loose);
                        lost = true;
                    }
                    if (bossSel == "rock")
                    {
                        screenMat.SetTexture(index, win);
                    }
                }
                if (active == "scissors")
                {
                    if (bossSel == "scissors")
                    {
                        screenMat.SetTexture(index, tie);
                    }
                    if (bossSel == "rock")
                    {
                        screenMat.SetTexture(index, loose);
                        lost = true;
                    }
                    if (bossSel == "paper")
                    {
                        screenMat.SetTexture(index, win);
                    }
                }

            }
            else if (counter > 8)
            {
                //Debug.Log("reset");
                counterStart = false;
                counter = 0;
                screenMat.SetTexture(index, startup);
                active = "";
                bossSel = "";
            }
        }
    }

    public void b1Press()
    {
        if (!counterStart)
        {
            Debug.Log("b1press");
            screenMat.SetTexture(index, rock);
            active = "rock";
        }

    }
    public void b2Press()
    {
        if (!counterStart)
        {
            Debug.Log("b2press");
            screenMat.SetTexture(index, scissors);
            active = "scissors";
        }
    }
    public void b3Press()
    {
        if (!counterStart)
        {
            Debug.Log("b3press");
            screenMat.SetTexture(index, paper);
            active = "paper";
        }
    }
    public void b4Press()
    {
        Debug.Log("b4press");
        if (active != "")
        {
            //screenMat.SetTexture(index, vs);
            counterStart = true;
            float rand = Random.Range(0, 3);
            if (rand < 1)
            {
                bossSel = "rock";
            }
            else if (rand > 1 && rand < 2)
            {
                bossSel = "scissors";
            }
            else
            {
                bossSel = "paper";
            }
        }
    }
}
