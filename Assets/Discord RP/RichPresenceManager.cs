using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Discord_RP
{
    public class RichPresenceManager : MonoBehaviour
    {
        private Discord.Discord _discord;
        private long unixTimestamp;

        private void Start()
        {
            // Call the method to get the Unix timestamp
            unixTimestamp = GetUnixTimestamp();

            //Data With APP ID
            _discord = new Discord.Discord(1250746160759439432, (ulong)Discord.CreateFlags.NoRequireDiscord);
            ChangeActivity();
        }

        long GetUnixTimestamp()
        {
            // Get the current time in UTC
            DateTime currentTime = DateTime.UtcNow;

            // Define the Unix epoch (January 1, 1970, at midnight)
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Calculate the Unix timestamp
            long unixTimestamp = (long)(currentTime - unixEpoch).TotalSeconds;

            return unixTimestamp;
        }

        private void Update()
        {
            _discord.RunCallbacks();
        }

        private void OnDisable()
        {
            _discord.Dispose();
        }

        private void ChangeActivity()
        {
            var activityManager = _discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                State = "Playing",
                // Details = "DuStinkst",
                Assets = {
                    LargeImage = "gameico-1024"
                },
                Timestamps = {
                    Start = 0 + unixTimestamp
                },
            };
            activityManager.UpdateActivity(activity, (res) => { });
        }
    }
}
