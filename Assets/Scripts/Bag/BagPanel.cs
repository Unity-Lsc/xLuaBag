using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : MonoBehaviour
{
    private RectTransform scrollView;
    private RectTransform content;

    private int gapX = 15;//横向间隙
    private int gapY = 15;//纵向间隙
    private int columnNum = 3;//最大列数
    private float cellWidth = 180;//格子宽
    private float cellHeight = 180;//格子高
    private float offsetX, offsetY;
    private Dictionary<int,GameObject> curShowItemsDict = new Dictionary<int, GameObject>();

    private void Awake()
    {
        scrollView = transform.Find("Scroll View").GetComponent<RectTransform>();
        content = scrollView.Find("Viewport/Content").GetComponent<RectTransform>();
    }

    private void Start()
    {
        offsetX = cellWidth + gapX;
        offsetY = cellHeight + gapY;
        BagMgr.GetInstance().InitItemsInfo();
        content.sizeDelta = new Vector2(0,BagMgr.GetInstance().itemList.Count / 3 * offsetY);
        scrollView.transform.GetComponent<ScrollRect>().onValueChanged.AddListener(OnScrollChanged);
        //CheckShowOrHide();
    }

    private void OnScrollChanged(Vector2 vec) {
        CheckShowOrHide();
    }

    void CheckShowOrHide() {
        int minIndex = (int)(content.anchoredPosition.y / offsetY) * columnNum;
        int maxIndex = (int)((content.anchoredPosition.y + scrollView.sizeDelta.y) / offsetY) * columnNum + (columnNum - 1);
        Debug.Log(minIndex + "  " + maxIndex);
        for (int i = minIndex; i < maxIndex; i++) {
            if(curShowItemsDict.ContainsKey(i))
                continue;
            int index = i;
            curShowItemsDict.Add(index,null);
            PoolMgr.GetInstance().Get("Prefabs/BagItem", (obj) =>{
                //设置父物体
                obj.transform.SetParent(content);
                //设置缩放大小
                obj.transform.localScale = Vector3.one;
                //设置位置
                obj.transform.localPosition = new Vector3((index % columnNum) * offsetX,-index / columnNum * offsetY,0);
                obj.GetComponent<BagItem>().InitItemInfo(BagMgr.GetInstance().itemList[index]);
                if(curShowItemsDict.ContainsKey(index)) {
                    curShowItemsDict[index] = obj;
                }
            });
        }

    }

}
