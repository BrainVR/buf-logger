using System.Collections.Generic;

namespace BrainVR.Logger.Interfaces
{
    public interface IPlayerController
    {
        string HeaderLine();
        List<string>  PlayerInformation();
    }
}