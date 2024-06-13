using UnityEngine;

namespace Discord_RP {
    public class RichPresenceManager : MonoBehaviour {
        private Discord.Discord _discord;
    
        private void Start()           {
            //Data With APP ID
            _discord = new Discord.Discord(1250746160759439432, (ulong)Discord.CreateFlags.NoRequireDiscord);
            ChangeActivity();
        }
        
        private void Update() {
            _discord.RunCallbacks();
        }

        private void OnDisable() {
            _discord.Dispose();
        }

<<<<<<< Updated upstream
        private void ChangeActivity() {
            var activityManager = _discord.GetActivityManager();
            var activity = new Discord.Activity {
                State = "Playing",
                Details = "DuStinkst"
				startTimestamp = 1507665886;
            };
            activityManager.UpdateActivity(activity, (res) => { });
		}
	}
}
=======
    //Discords Activity
    public void ChangeActivity()
    {
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            State = "Playing",
            Details = "DuStinkst",
            Assets =
            {
                LargeImage = "gameico-1024"
            },
            Timestamps =
            {
                Start = 1507665886
            },

    };
        activityManager.UpdateActivity(activity, (res) => { });
    }
    // Update is called once per frame
    void Update()
    {
        discord.RunCallbacks();
    }
}
>>>>>>> Stashed changes
