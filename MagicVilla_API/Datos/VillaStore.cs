using MagicVilla_API.Models.Dto;

namespace MagicVilla_API.Datos
{
    public static class VillaStore
    {
        public static List<VillaDto> villas = new List<VillaDto>
        {
            new VillaDto{Id=1,Nombre = "Vista al Mar",Ocupantes = 2,MetrosCuadrados=50},
            new VillaDto{Id=2,Nombre = "Vista a guibia",Ocupantes = 4,MetrosCuadrados=100}
        };
    }
}
