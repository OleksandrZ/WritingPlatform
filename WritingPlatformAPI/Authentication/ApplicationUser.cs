using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WritingPlatformAPI.Models;

namespace WritingPlatformAPI.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Work> Works { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
