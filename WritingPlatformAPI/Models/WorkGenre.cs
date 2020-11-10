using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WritingPlatformAPI.Models
{
    public class WorkGenre
    {
        [Key]
        public int WorkId { get; set; }
        public Work Work { get; set; }
        [Key]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        public static List<string> GenresToList(ICollection<WorkGenre> genres)
        {
            List<string> Genres = new List<string>();
            foreach (var item in genres)
            {
                Genres.Add(item.Genre.Name);
            }
            return Genres;
        }
    }
}
