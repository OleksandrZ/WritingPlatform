using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WritingPlatformAPI.Authentication;
using WritingPlatformAPI.Models;
using WritingPlatformAPI.Utils;

namespace WritingPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICurrentUserAccessor currentUserAccessor;

        public CommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ICurrentUserAccessor currentUserAccessor)
        {
            this.context = context;
            this.userManager = userManager;
            this.currentUserAccessor = currentUserAccessor;
        }
        //[HttpGet]
        //[Route("getComments")]
        //public async Task<IActionResult> GetComments(string slug)
        //{
        //    var work = await context.Works.Include(x => x.Comments).Include(x=> x.Author).Where(x => x.Slug == slug).FirstOrDefaultAsync();

        //    if(work == null)
        //    {
        //        return StatusCode(StatusCodes.Status404NotFound, new { Status = "Error", Message = "Can't find comments, this work doesnt't exist" });
        //    }


        //}

        [Authorize]
        [HttpPost]
        [Route("addComment")]
        public async Task<IActionResult> AddComment([FromBody]CommentDTO commentDTO)
        {
            var work = await context.Works.Include(x => x.Comments).Where(x => x.Slug == commentDTO.WorkSlug).FirstOrDefaultAsync();
            if (work == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { Status = "Error", Message = "Cant add comment, work wasnt found" });
            }
            var user = await userManager.FindByNameAsync(currentUserAccessor.GetCurrentUsername());
            Comment comment = new Comment()
            {
                Author = user,
                CreationDate = DateTime.Now,
                Value = commentDTO.Value,
                Work = work
            };
            work.Comments.Add(comment);
            context.Comments.Add(comment);

            await context.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("deleteComment")]
        public async Task<IActionResult> deleteComment(int id, string slug)
        {
            var work = await context.Works.Include(x => x.Comments).Where(x => x.Slug == slug).FirstOrDefaultAsync();

            if (work == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { Status = "Error", Message = "Can`t delete comment, work wasn`t found" });
            }

            var comment = await context.Comments.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(comment == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { Status = "Error", Message = "This comment doesn`t exist" });
            }

            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}