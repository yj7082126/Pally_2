using LightBuzz.Azure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LightBuzzManager : MonoBehaviour
{
    public string mobileAppUri = "https://pallytestapp.azurewebsites.net";
    public bool supportLocalDatabase = true;
    private LightBuzzMobileServiceClient azureClient;
    private AppServiceTableDAO<TodoItem> todoTableDAO;
    public List<TodoItem> todoItems;
    public Text controlText2;

    // Start is called before the first frame update
    private async void Start()
    {
        await Init();

        todoItems = await todoTableDAO.FindAll();

        //TodoItem video = todoItems.Where(x => x.Text == "watch the video").SingleOrDefault();
        //video.Complete = false;
        //UpdateElement(video);

        for (int i = 0; i < todoItems.Count; i++)
        {
            TodoItem item = todoItems[i];
            controlText2.text += (item.Text + ", " + item.Complete + "\n");
        }

    }

    private async Task Init()
    {
        try
        {
            controlText2 = gameObject.GetComponent<Text>();
            // Initialize Azure
            azureClient = new SampleMobileClient(mobileAppUri, supportLocalDatabase);
            await azureClient.InitializeLocalStore();

            // Retrieve the items from the server
            todoTableDAO = new AppServiceTableDAO<TodoItem>(azureClient);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public async void UpdateElement(TodoItem e) {
        Debug.Log("Updating element: " + e.Text + " as " + e.Complete);
        await todoTableDAO.Update(e);
    }
}
