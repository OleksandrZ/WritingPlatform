using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WritingPlatformAPI.Authentication;

namespace WritingPlatformAPI.Models
{
    public class Work
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Rating { get; set; }
        public List<Comment> Comments { get; set; }
        public ApplicationUser Author { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MinLength(20)]
        public string Body { get; set; }
        [Required]
        public virtual ICollection<WorkGenre> WorkGenres { get; set; }
    }

    public class WorkDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [MinLength(20)]
        public string Body { get; set; }
        [Required]
        public List<string> Genres { get; set; }
        public string AuthorName { get; set; }
        public string Slug { get; set; }
        public List<CommentDTO> Comments { get; set; }
    }
}
