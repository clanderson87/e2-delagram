using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delagram.DAL;
using Moq;
using System.Collections.Generic; // For List<>
using Delagram.Models;
using System.Linq; // For IQueryable and List.AsQueryable()
using System.Data.Entity; // For DbSet

namespace Delagram.Tests.DAL
{
    [TestClass]
    public class DelagramRepositoryTest
    {
        Mock<DelagramContext> mock_context { get; set; }
        DelagramRepository Repo { get; set; }
        Mock<DbSet<Post>> mock_post_table { get; set; }
        IQueryable<Post> post_data { get; set; }
        List<Post> post_datasource { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            mock_post_table = new Mock<DbSet<Post>>();
            mock_context = new Mock<DelagramContext>();// { CallBase = true};
            Repo = new DelagramRepository(mock_context.Object); // mock_context.Object gives me an instance of what's in angle brakcets
            post_datasource = new List<Post>();
            post_data = post_datasource.AsQueryable();
        }

        void ConnectMocksToDatasource()
        {
            mock_post_table.As<IQueryable<Post>>().Setup(p => p.GetEnumerator()).Returns(post_data.GetEnumerator());
            mock_post_table.As<IQueryable<Post>>().Setup(p => p.ElementType).Returns(post_data.ElementType);
            mock_post_table.As<IQueryable<Post>>().Setup(p => p.Expression).Returns(post_data.Expression);
            mock_post_table.As<IQueryable<Post>>().Setup(p => p.Provider).Returns(post_data.Provider);
            mock_context.Setup(context => context.Posts).Returns(mock_post_table.Object);
            mock_post_table.Setup(post => post.Add(It.IsAny<Post>())).Callback((Post post) => post_datasource.Add(post));
        }

        [TestMethod]
        public void RepoEnsureICanCreateAnInstance()
        {
            DelagramRepository repo = new DelagramRepository();
            Assert.IsNotNull(repo);
        }

        [TestMethod]
        public void RepoEnsureThereAreNoPosts()
        {
            // Arrange
            ConnectMocksToDatasource();

            // Act
            int post_count = Repo.GetPostCount();

            // Assert
            Assert.AreEqual(0, post_count);
        }

        [TestMethod]
        public void RepoEnsureICanCreateAPost()
        {
            // Arrange
            List<Post> post_datasource = new List<Post>();
            IQueryable<Post> post_data = post_datasource.AsQueryable();
            ConnectMocksToDatasource();

            // Act
            DateTime created_at = DateTime.Now;
            ApplicationUser created_by = new ApplicationUser();
            created_by.Id = "fake-user-id";
            string caption = "some caption";
            string image_url = "https://myimages.fb.com/someimage.jpg";

            Repo.AddPost(image_url, created_by, caption: caption, created_at: created_at); //the colon here takes away the positional argument restriction. See below.
            Repo.AddPost(image_url, created_by, created_at: created_at, caption: caption); //even though the order is different, the results are the same thanks to named arguments.

            // Assert
            Assert.AreEqual(1, Repo.GetPostCount());
        }
    }
}
