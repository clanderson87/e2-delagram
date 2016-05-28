using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Delagram.Models;

namespace Delagram.DAL
{
    public class DelagramRepository
    {
        public DelagramContext context { get; set; }
        
        public DelagramRepository()
        {
            context = new DelagramContext();
        }

        // Allows us to isolate Repo from context during testing
        public DelagramRepository(DelagramContext _context)
        {
            context = _context;
        }

        public int GetPostCount()
        {
            return context.Posts.Count();
        }

        public void AddPost(string image_url, ApplicationUser created_by, DateTime created_at, string caption = null)//if a caption isn't provided, assume it to be null
        {
            //this is advanced thing...
            Post new_post = new Post { ImageUrl = image_url, CreatedBy = created_by, Caption = caption, Time = created_at };
            context.Posts.Add(new_post);
            context.SaveChanges();
        }

        public void AddPost(string image_url, ApplicationUser created_by)
        {
            //this is method overloading
            Post new_post = new Post { ImageUrl = image_url, CreatedBy = created_by, Time = DateTime.Now };
            context.Posts.Add(new_post);
            context.SaveChanges();
        }
    }
}