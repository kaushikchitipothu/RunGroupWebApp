using RunGroupWebApp.Models;

namespace RunGroupWebApp.ViewModels
{
    public class HomeViewModel
    {
        public string? city  { get; set; }
        public string? state { get; set; }
        public string? country { get; set; }
        public IEnumerable<Club> clubs { get; set; }
    }
}
