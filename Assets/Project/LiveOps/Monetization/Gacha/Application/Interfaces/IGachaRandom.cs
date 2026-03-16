using System.Collections.Generic;
using GachaSystem.Domain.Models;

namespace GachaSystem.Application.Interfaces
{
    public interface IGachaRandom
    {
        int PickIndex(List<GachaItem> items);
    }
}