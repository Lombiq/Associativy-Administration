
namespace Associativy.Administration.Models
{
    public class GraphSettings
    {
        public int NeighboursDisplayedMaxCount { get; set; }
        public string ImplicitlyCreatableContentType { get; set; }


        public GraphSettings()
        {
            NeighboursDisplayedMaxCount = 50;
            ImplicitlyCreatableContentType = null;
        }
    }
}