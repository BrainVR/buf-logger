using System.Collections.Generic;
using UnityEngine;

namespace BrainVR.Logger.Interfaces
{
    public interface IPlayerController
    {
        string HeaderLine { get; }
        IInput Input { get; }
        GameObject GameObject { get; }
        List<string> PlayerInformation();
    }
}