namespace MASAR.Model
{
    public class FavoriteRoute
    {
        public int FavoriteRouteId { get; set; }
        public string StudentId { get; set; }
        public ApplicationUser User { get; set; }
        public string RoutingId { get; set; }
        public Routing Routing { get; set; }
    }
}