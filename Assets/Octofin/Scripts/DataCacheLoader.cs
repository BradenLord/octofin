using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Octofin.Core.Utility.Cache;
using TMPro;

public class DataCacheLoader : MonoBehaviour {

    public TMP_Text loadingText;
    public RectTransform progressPanel;
    public string mainMenuSceneName;

    private readonly CacheCounter counter = new CacheCounter();
    private bool readyToSwap = false;
    
	// Use this for initialization
	void Start ()
    {
        counter.currentOperation = "Loading...";
        counter.consumedObjects = 0;
        counter.totalObjects = 1;

        ThreadStart loadDelegate = new ThreadStart(loadDataCache);
        Thread loadThread = new Thread(loadDelegate);
        loadThread.Start();
	}
	
	void Update ()
    {
        if (readyToSwap)
        {
            Thread.Sleep(1000);
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            loadingText.text = counter.currentOperation;

            Vector3 currentScale = progressPanel.localScale;
            progressPanel.localScale = new Vector3((float)counter.consumedObjects / (float)counter.totalObjects, currentScale.y, currentScale.z);

            if (counter.operationsFinished)
            {
                readyToSwap = true;
            }
        }
	}

    private void loadDataCache()
    {
        DataCache.importCache(counter);
    }
}
