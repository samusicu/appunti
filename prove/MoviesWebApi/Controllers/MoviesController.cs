using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesWebApi.DTOs;
using MoviesWebApi.Models;

namespace MoviesWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MyMoviesContext _context;

        public MoviesController(MyMoviesContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var movies = await _context.Movies
                .Include(m => m.Genres)
                .Select(m => new MovieDto
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Year = m.Year,
                    Genres = m.Genres.Select(g => new GenreDto
                    {
                        GenreId = g.GenreId,
                        Name = g.GenreName
                    }).ToList()
                })
                .ToListAsync();

            return movies;
        }

        // Post: api/Movies
        [HttpPost("filteredMovies")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetFilteredMovies(MovieFilter filter)
        {
            var query = _context.Movies
                .Include(m => m.Genres).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Title))
                query = query.Where(m => m.Title == filter.Title);
            if (filter.Year != null)
                query = query.Where(m => m.Year == filter.Year);
            //if (!string.IsNullOrWhiteSpace(filter.Genre))
            //    movies = movies.Where(m => m.Genres.Contains(filter.Genre));

            var movies = await query.Select(m => new MovieDto
             {
                 MovieId = m.MovieId,
                 Title = m.Title,
                 Year = m.Year,
                 Genres = m.Genres.Select(g => new GenreDto
                 {
                     GenreId = g.GenreId,
                     Name = g.GenreName
                 }).ToList()
             })
            .ToListAsync();

            return movies;
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.MovieId == id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.MovieId)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MovieExists(movie.MovieId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMovie", new { id = movie.MovieId }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }
    }
}
