using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace WebApplication6.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Person> Persons { get; set; }
        public virtual ICollection<PeoplesUsers> PersonUser { get; set; }
        public virtual ICollection<PeopleComment> Comment { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual string MainImage { get; set; }
        public virtual string Name { get; set; }
        public virtual List<string> BanMassages { get; set; }
        public virtual bool IsBaned { get; set; }
        public virtual string IpAdress { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostDescription> postDescriptions { get; set; }
        public DbSet<PeopleComment> PeopleComments { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<PeoplesUsers> PeopleUsers { get; set; }
        public DbSet<Position> Positions { get; set; }
        public ApplicationDbContext()
        : base("Polit", throwIfV1Schema: false)
        {

        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<WebApplication6.Models.Comment> Comments { get; set; }
    }

}