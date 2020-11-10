using System;
using System.ComponentModel.DataAnnotations;
using WritingPlatformAPI.Authentication;

namespace WritingPlatformAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public virtual Work Work { get; set; }
        public DateTime CreationDate{ get; set; }
        public string Value { get; set; }
    }
    public class CommentDTO
    {
        public string AuthorName { get; set; }
        [Required]
        public string Value { get; set; }
        public string WorkSlug { get; set; }
        public DateTime CreationDate { get; set; }
    }
}