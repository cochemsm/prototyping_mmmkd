using UnityEngine;

namespace Discord_RP {
    public class RichPresenceManager : MonoBehaviour
    {
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

        private void ChangeActivity() {
            var activityManager = _discord.GetActivityManager();
            var activity = new Discord.Activity {
                State = "Playing",
                Details = "DuStinkst"
            };
            activityManager.UpdateActivity(activity, (res) => { });
        }
    }
}