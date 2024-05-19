using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBar : MonoBehaviour
{
    [SerializeField] private GameObject lifePrefab;
    [SerializeField] private int prefabWidth = 50;
    
    int currentLifeCount = 5;
    private GameObject[] lifeBarArray;
    
    // Start is called before the first frame update
    void Start()
    {

        DataManager.me.lifeBar = gameObject;
        lifeBarArray = new GameObject[DataManager.me.maxLives];
        
        //float objectWidth = lifePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        for (int i = 0; i < lifeBarArray.Length; i++)
        {
            GameObject tmpObject = Instantiate(lifePrefab, new Vector3(i* prefabWidth, 0, 0), Quaternion.identity);
            Debug.Log("Instantiating the Prefab");

            //this.gameObject.transform.SetParent(lifePrefab.transform, false);
            tmpObject.transform.SetParent(this.gameObject.transform, false);
            lifeBarArray[i] = tmpObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i < lifeBarArray.Length;i++)
        {
            if(i<DataManager.me.lifeCount)
            {
                lifeBarArray[i].SetActive(true);
            }
            else
            {
                lifeBarArray[i].SetActive(false);
            }
        }
    }
}
