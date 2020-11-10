using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WritingPlatformAPI.Authentication;
using WritingPlatformAPI.Models;
using WritingPlatformAPI.Utils;

namespace WritingPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorksController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private ICurrentUserAccessor currentUserAccessor;

        public WorksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ICurrentUserAccessor currentUserAccessor)
        {
            this.context = context;
            this.userManager = userManager;
            this.currentUserAccessor = currentUserAccessor;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("allworks/{searchString?}")]
        public async Task<IActionResult> GetAll(string searchString)
        {
            context.Comments.Include(x => x.Author);
            var works = await context.Works.Include(x => x.WorkGenres).Include(x => x.Comments).ThenInclude(x => x.Author).ToListAsync();
            List<WorkDTO> worksDto = new List<WorkDTO>();
            List<CommentDTO> commentDTOs = new List<CommentDTO>();

            if (!string.IsNullOrEmpty(searchString))
            {
                works = works.Where(x => x.Name.Contains(searchString) || x.Author.UserName.Contains(searchString)).ToList();
            }
            foreach (var item in works)
            {
                    commentDTOs = new List<CommentDTO>();
                foreach (var comment in item.Comments)
                {
                    commentDTOs.Add(new CommentDTO()
                    {
                        AuthorName = comment.Author.UserName,
                        CreationDate = comment.CreationDate,
                        Value = comment.Value,
                        WorkSlug = item.Slug
                    });
                }
                worksDto.Add(new WorkDTO()
                {
                    AuthorName = item.Author.UserName,
                    Body = item.Body,
                    Genres = WorkGenre.GenresToList(item.WorkGenres),
                    Name = item.Name,
                    Slug = item.Slug,
                    Comments = commentDTOs
                });
            }
            return Ok(worksDto);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getWork")]
        public async Task<IActionResult> GetWork(string slug)
        {
            var works = await context.Works.Include(x => x.Author).Include(x => x.WorkGenres).Include(x => x.Comments).ToListAsync();

            var work = works.Find(work => work.Slug == slug);

            List<CommentDTO> commentDTOs = new List<CommentDTO>();

            foreach (var comment in work.Comments)
            {
                commentDTOs.Add(new CommentDTO()
                {
                    AuthorName = comment.Author.UserName,
                    CreationDate = comment.CreationDate,
                    Value = comment.Value,
                    WorkSlug = work.Slug
                });
            }
            WorkDTO workDTO = new WorkDTO()
            {
                AuthorName = work.Author.UserName,
                Genres = WorkGenre.GenresToList(work.WorkGenres),
                Name = work.Name,
                Body = work.Body,
                Comments = commentDTOs,
                Slug = work.Slug
            };
            return Ok(workDTO);
        }

        [Authorize]
        [HttpPost]
        [Route("addWork")]
        public async Task<IActionResult> AddWork([FromBody]WorkDTO workDTO)
        {
            var user = await userManager.FindByNameAsync(currentUserAccessor.GetCurrentUsername());
            if (user != null)
            {
                var work = new Work()
                {
                    Name = workDTO.Name,
                    Author = user,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Body = workDTO.Body,
                    Slug = workDTO.Name.GenerateSlug()
                };
                List<WorkGenre> workGenres = new List<WorkGenre>();
                foreach (var genre in workDTO.Genres)
                {
                    if (context.Genres.Where(g => g.Name == genre).Any())
                    {
                        workGenres.Add(new WorkGenre()
                        {
                            Genre = context.Genres.Where(g => g.Name == genre).FirstOrDefault(),
                            Work = work
                        });
                    }
                }
                work.WorkGenres = workGenres;
                context.Works.Add(work);
                user.Works.Add(work);
                await context.SaveChangesAsync();
            }
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        [Route("deleteWork")]
        public async Task<IActionResult> DeleteWork(string slug)
        {
            var work = await context.Works.Include(a => a.Author).Where(w => w.Slug == slug).FirstOrDefaultAsync();
            if (work != null)
            {
                var user = await userManager.FindByNameAsync(currentUserAccessor.GetCurrentUsername());
                if (user != null && user == work.Author)
                {
                    context.Works.Remove(work);
                    await context.SaveChangesAsync();
                    return Ok();
                }
                return StatusCode(StatusCodes.Status403Forbidden, new { Status = "Error", Message = "You dont have rights to delete this work" });
            }
            return StatusCode(StatusCodes.Status404NotFound, new { Status = "Error", Message = "Work not found" });
        }

        [Authorize]
        [HttpPost]
        [Route("editwork")]
        public async Task<IActionResult> EditWork([FromBody]WorkDTO edit)
        {
            var work = await context.Works.Include(x => x.WorkGenres).Include(x => x.Author).Where(x => x.Slug == edit.Slug).FirstOrDefaultAsync();

            if (work == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { Status = "Error", Message = "Work not found" });
            }

            work.Body = edit.Body;
            work.Name = edit.Name;
            work.Slug = edit.Name.GenerateSlug();
            List<WorkGenre> workGenres = new List<WorkGenre>();
            foreach (var genre in edit.Genres)
            {
                if (context.Genres.Where(g => g.Name == genre).Any())
                {
                    workGenres.Add(new WorkGenre()
                    {
                        Genre = context.Genres.Where(g => g.Name == genre).FirstOrDefault(),
                        Work = work
                    });
                }
            }
            work.WorkGenres = workGenres;

            if (context.ChangeTracker.Entries().First(x => x.Entity == work).State == EntityState.Modified)
            {
                work.UpdatedAt = DateTime.Now;
            }

            await context.SaveChangesAsync();
            return Ok();
        }
    }
}