using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RichPresenceManager : MonoBehaviour
{
    Discord.Discord discord;
    // Start is called before the first frame update
    void Start()
    {
        //Data With APP ID
        discord = new Discord.Discord(1250746160759439432, (ulong)Discord.CreateFlags.NoRequireDiscord);
        ChangeActivity();
    }

    void OnDisable()
    {
        discord.Dispose();
    }

    //Discords Activity
    public void ChangeActivity()
    {
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            State = "Playing",
            Details = "DuStinkst"

        };
        //activityManager.UpdateActivity(activity, activity(res) => {
            //Debug.log("Updated Activity");
        //});
    }
    // Update is called once per frame
    void Update()
    {
        discord.RunCallbacks();
    }
}