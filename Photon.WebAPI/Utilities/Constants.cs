using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Photon.WebAPI.Utilities
{
    public class Constants
    {
        public const string OccupiedBaths = "OccupiedBaths";
        public const string LastOccupiedTimes = "LastOccupiedTimes";
        public const string LastFreedTimes = "LastFreedTimes";
        public const string LastLineAdvanceTimes = "LastFreedTimes";
        public const string BathLines = "BathLines";
        public const string OccupiedByFirstInLine = "OccupiedByFirstInLine";        


        public const string CacheDuration = "CacheDuration";
        public const string PIRSecondsRequiredToOccupy = "PIRSecondsRequiredToOccupy";
        public const string PIRSecondsRequiredToFree = "PIRSecondsRequiredToFree";
        public const string LightOnThreshold = "LightOnThreshold";
        public const string ProximityThreshold = "ProximityThreshold";

        public const string UsersList = "UsersList";

        public const string KnockSoundFile = "knock.wav";
        public const string ToiletFlushSoundFile = "toilet_flush.wav";
        public const string DoorShutSoundFile = "door_shut.wav";
        
    }
}