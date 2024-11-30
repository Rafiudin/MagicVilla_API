using MagicVilla_VillaAPI.Models.DTO;

namespace MagicVilla_VillaAPI.Models.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
            {
                new VillaDto{Id=1, Name="Pool Villa", Sqft = 100, Occupancy = 4},
                new VillaDto{Id=2, Name="Beach Villa", Sqft = 200, Occupancy = 3}
            };
    }
}
