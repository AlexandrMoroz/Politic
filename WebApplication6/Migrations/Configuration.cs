namespace WebApplication6.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebApplication6.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;

        }

        protected override void Seed(ApplicationDbContext context)
        {
            ApplicationUser user = new ApplicationUser();
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };
                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "alexmoroz@gmail.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                user = new ApplicationUser { UserName = "alexmoroz@gmail.com", Email = "alexmoroz@gmail.com" };

                manager.Create(user, "Alex-123");
                manager.AddToRole(user.Id, "Admin");
            }
            Party party = new Party()
            {
                Name = "политическая партия Всеукраинское объединение Батьківщина",
                PolitDerect = "Left"
            };
            City city = new City()
            {
                Name = "Киев"
            };
            Position posit = new Position()
            {
                Name = "Народный депутат"
            };
            List<Person> pers = new List<Person>()
            {
               new Person(){
                Name = "Иван",
                Family = "Иванович",
                Surname = "Иванов",
                Foto = "nf80.jpg",
                Party = party,
                City = city,
                Date =  DateTime.Now,
                Position=posit,
                Submitted=user,
                Rate=0

               },new Person(){
                Name = "Иван",
                Family = "Иванович",
                Surname = "Иванов",
                Foto = "nf80.jpg",
                Party = party,
                City = city,
                Position=posit,
                Date =  DateTime.Now,
                Submitted=user,
                Rate=0
               },new Person(){
                Name = "Иван",
                Family = "Иванович",
                Surname = "Иванов",
                Foto = "nf80.jpg",
                Party = party,
                City = city,
                Date =  DateTime.Now,
                Position=posit,
                Submitted=user,
                Rate=0
               },new Person(){
                Name = "Иван",
                Family = "Иванович",
                Surname = "Иванов",
                Foto = "nf80.jpg",
                Party = party,
                City = city,
                Position=posit,
                Date =  DateTime.Now,
                Submitted=user,
                Rate=0
               },new Person(){
                Name = "Иван",
                Family = "Иванович",
                Surname = "Иванов",
                Foto = "nf80.jpg",
                Party = party,
                City = city,
                Position=posit,
                Date =  DateTime.Now,
                Submitted=user,
                Rate=0
               },new Person(){
                Name = "Иван",
                Family = "Иванович",
                Surname = "Иванов",
                Foto = "nf80.jpg",
                Party = party,
                City = city,
                Position=posit,
                Date =  DateTime.Now,
                Submitted=user,
                Rate=0
               },new Person(){
                Name = "Иван",
                Family = "Иванович",
                Surname = "Иванов",
                Foto = "nf80.jpg",
                Party = party,
                City = city,
                Position=posit,
                Date =  DateTime.Now,
                Submitted=user,
                Rate=0
               },new Person(){
                Name = "Иван",
                Family = "Иванович",
                Surname = "Иванов",
                Foto = "nf80.jpg",
                Party = party,
                City = city,
                Position=posit,
                Date =  DateTime.Now,
                Submitted=user,
                Rate=0
               }
           };
            List<PeopleComment> com1 = new List<PeopleComment>()
            {
                new PeopleComment(){
                Person = pers[1],
                Text = "LJMHasdasd1231asdasdasd13123123",
                User = user,
                Datetime = DateTime.Now,
                },new PeopleComment(){
                Person = pers[1],
                Text = "LJMHasdasd1231asdasdasd13123123",
                User = user,
                Datetime = DateTime.Now,
                },new PeopleComment(){
                Person = pers[1],
                Text = "LJMHasdasd1231asdasdasd13123123",
                User = user,
                Datetime = DateTime.Now,
                },new PeopleComment(){
                Person = pers[1],
                Text = "LJMHasdasd1231asdasdasd13123123",
                User = user,
                Datetime = DateTime.Now,
                },new PeopleComment(){
                Person = pers[1],
                Text = "LJMHasdasd1231asdasdasd13123123",
                User = user,
                Datetime = DateTime.Now,
                }

            };
            PeopleComment com2 = new PeopleComment()
            {
                Person = pers[1],
                Text = "LJMHasdasd1231asdasdasd13123123",
                User = user,
                Datetime = DateTime.Now,
                Childrens = com1.Cast<Comment>().ToList()
            };
            List<Tag> tag = new List<Tag>()
            {
               new Tag(){ Name = "криминал" },
               new Tag(){ Name="хорошие дела"}
            };
            List<PostComment> postComments = new List<PostComment>()
            {
                new PostComment(){

                Text = "LJMHasdasd1231asdasdasd13123123",
                User = user,
                Datetime = DateTime.Now,
                },new PostComment(){

                Text = "LJMHasdasd1231asdasdasd13123123",
                User = user,
                Datetime = DateTime.Now,
                },new PostComment(){

                Text = "LJMHasdasd1231asdasdasd13123123",
                User = user,
                Datetime = DateTime.Now,
                },new PostComment(){

                Text = "LJMHasdasd1231asdasdasd13123123",
                User = user,
                Datetime = DateTime.Now,
                },new PostComment(){

                Text = "LJMHasdasd1231asdasdasd13123123",
                User = user,
                Datetime = DateTime.Now,
                }
            };
            PostDescription PostDescription = new PostDescription()
            {
                Content = "Picture removal detract earnest is by. Esteems met joy attempt way clothes yet demesne tedious. Replying an marianne do it an entrance advanced. Two dare say play when hold. Required bringing me material stanhill jointure is as he. Mutual indeed yet her living result matter him bed whence." +
                "Ask especially collecting terminated may son expression.Extremely eagerness principle estimable own was man.Men received far his dashwood subjects new.My sufficient surrounded an companions dispatched in on.Connection too unaffected expression led son possession.New smiling friends and her another.Leaf she does none love high yet.Snug love will up bore as be.Pursuit man son musical general pointed.It surprise informed mr advanced do outweigh.",
                Type = "text",

            };
            Post post = new Post()
            {
                Person = pers[1],
                User = user,
                Title = "Effect if in up no depend seemed. Ecstatic elegance gay but disposed",
                PostedOn = DateTime.Now,
                Tags = new List<Tag>() { tag[0], new Tag() { Name = "депутат" } },
                PostFile = "Effect_if_in_up_no_depend_seemed.",
                PostComment = postComments,
                Description= new List<PostDescription>() { PostDescription } 
            };
            context.Posts.Add(post);
            context.PostComments.AddRange(postComments);
            context.Tags.AddRange(tag);
            context.postDescriptions.Add(PostDescription);
            context.PeopleComments.Add(com2);
            context.Positions.Add(posit);
            context.Parties.Add(party);
            context.Cities.Add(city);
            context.Persons.AddRange(pers);
            base.Seed(context);
        }


    }
}
