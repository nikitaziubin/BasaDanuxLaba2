namespace Laba2.ViewModels
{
    public class DivisioStadiumViwModel
    {
        public Stadium Stadium { get; set; }
        public Manager Manager { get; set; }
        public Team Team{ get; set; }
        public List<Stadium> ?stadiums { get; set; } = new List<Stadium>();
        public List<Division>? divisions { get; set; } = new List<Division>();
        public List<Manager>? managers { get; set; } = new List<Manager>();
        public List<Match>? matches { get; set; } = new List<Match>();
        public List<Team>? teams { get; set; } = new List<Team>();
        public List<Participate>? participates { get; set; } = new List<Participate>();

    }
}
