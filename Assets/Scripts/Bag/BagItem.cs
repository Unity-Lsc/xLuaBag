using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagItem : MonoBehaviour
{

    private Text txtNum;

    private void Awake()
    {
        txtNum = transform.Find("txtNum").GetComponent<Text>();
    }

    public void InitItemInfo(Item item) {
        txtNum.text = item.num + "";
    }
}
