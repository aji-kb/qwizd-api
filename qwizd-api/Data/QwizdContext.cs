using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using qwizd_api.Data.Contract;
using qwizd_api.Data.Model;


namespace qwizd_api.Data;



public class QwizdContext: DbContext
{

    public QwizdContext(DbContextOptions<QwizdContext> options): base(options)
    {

    }

    public DbSet<User> User {get;set;}
    public DbSet<Quiz> Quizzes {get;set;}
    public DbSet<QuizQuestion> QuizQuestions {get;set;}
    public DbSet<Topic> Topics {get;set;}
    public DbSet<Question> Questions {get;set;}
    public DbSet<Answer> Answers {get;set;}
    public DbSet<QuestionTopicMapping> QuestionTopicMappings {get;set;}
    public DbSet<QuestionAnswerMapping> QuestionAnswerMappings{get;set;}
    public DbSet<IdentityUserLogin<int>> IdentityUserLogin {get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QuestionTopicMapping>().ToTable("QuestionTopicMapping");
        modelBuilder.Entity<Quiz>().ToTable("Quiz");
        modelBuilder.Entity<QuizQuestion>().ToTable("QuizQuestion");
        modelBuilder.Entity<Topic>().ToTable("Topic");
        modelBuilder.Entity<Question>().ToTable("Question");
        modelBuilder.Entity<Answer>().ToTable("Answer");
        modelBuilder.Entity<QuestionAnswerMapping>().ToTable("QuestionAnswerMapping");
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<IdentityUserLogin<int>>().HasNoKey().ToTable("IdentityUserLogin");

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        var trackedEntites = this.ChangeTracker.Entries();

        foreach(var trackedEntity in trackedEntites)
        {
            if(trackedEntity.Entity is ITrackedEntity)
            {
                var baseEntity = (BaseEntity)trackedEntity.Entity;
                if(baseEntity != null)
                {
                    if(trackedEntity.State == EntityState.Added)
                    {
                        baseEntity.CreatedDateUTC = DateTime.UtcNow;
                        baseEntity.CreatedBy = "_akb";
                    }
                    else if(trackedEntity.State == EntityState.Modified)
                    {
                        baseEntity.ModifiedDateUTC = DateTime.UtcNow;
                        baseEntity.ModifiedBy = "_akb";
                    }
                }

            }
        }

        return base.SaveChanges();
    }
}