using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall_Trap : MonoBehaviour {

    public bool Is_Steped = false;
    private float alpha = 1.0f;
    private Color TrapColor;
    private Vector3 originPos;

    // Use this for initialization
    void Start () {
        TrapColor = this.GetComponent<MeshRenderer>().material.color;
        originPos = this.GetComponent<Transform>().position;
    }
	
	// Update is called once per frame
	void Update () {
        if (Is_Steped)
        {
            if (alpha > 0.8f)
            {
                FadeOut();
            }
            else
            {
                Shake();

                alpha -= Time.deltaTime / 2.0f;
                if (alpha <= 0.0f)
                {
                    this.gameObject.SetActive(false);
                    Is_Steped = false;
                }
            }
        }
	}

    void OnCollisionEnter(Collision col)
    {
        Is_Steped = true;
    }

    void FadeOut()
    {
        alpha -= Time.deltaTime / 2.0f;
        this.GetComponent<MeshRenderer>().material.color =
                    new Color(TrapColor.r, TrapColor.g, TrapColor.b, alpha);
    }

    void Shake()
    {
        this.transform.position = originPos + Random.insideUnitSphere * 0.1f;
    }
}
