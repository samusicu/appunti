namespace MoviesWebApi.DTOs
{
    public class MovieDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public List<GenreDto> Genres { get; set; }
    }

    public class GenreDto
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
    }
}
