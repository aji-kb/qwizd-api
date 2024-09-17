using System;
using qwizd_api.Data;
using qwizd_api.Data.Model;
using qwizd_api.Service.Contract;
using qwizd_api.Service.ViewModel;

namespace qwizd_api.Service;

public class QuizService : IQuizService
{
    private readonly QwizdContext _qwizdContext;

    public QuizService(QwizdContext qwizdContext)
    {
        _qwizdContext = qwizdContext;
    }

    public QuestionViewModel GetNextQuestion(int quizId)
    {

        var quizQuestions = from qq in _qwizdContext.QuizQuestions
                                    join q in _qwizdContext.Questions on qq.QuestionId equals q.Id
                                    where qq.QuizId == quizId && qq.UserAnswerId == null
                                    orderby qq.Id
                                    select new QuestionViewModel
                                    {
                                        Id = q.Id,
                                        Text = q.Text 
                                    };

        var nextQuestion = quizQuestions.First();

        if(quizQuestions.Count() <=1)
            nextQuestion.IsLastQuestion = true;


        nextQuestion.Answers = (from qam in _qwizdContext.QuestionAnswerMappings
                        join a in _qwizdContext.Answers on qam.AnswerId equals a.Id
                        where qam.QuestionId == nextQuestion.Id
                        select new AnswerViewModel
                        {
                            Id = a.Id,
                            Text = a.Text
                        }).ToList();

        //Check the session for the current question. 
        return nextQuestion;
    }

    public int SaveResponse(int quizId, int questionId, int answerId)
    {
        var quizQuestion = _qwizdContext.QuizQuestions.Where(x=>x.QuizId == quizId && x.QuestionId == questionId).FirstOrDefault();

        if(quizQuestion != null)
        {
            var answer = _qwizdContext.QuestionAnswerMappings.Where(x=>x.QuestionId == questionId && x.AnswerId == answerId).FirstOrDefault();
            quizQuestion.UserAnswerId = answerId;
            if(answer != null && answer.IsCorrectAnswer)
                quizQuestion.IsCorrectAnswer = true;
            else
                quizQuestion.IsCorrectAnswer = false;

            return _qwizdContext.SaveChanges();
        }
        else
            return 0;
    }

    public QuizResultViewModel? SubmitResponse(int quizId, int questionId, int answerId)
    {
        var saveResponseResult = SaveResponse(quizId, questionId, answerId);
        if(saveResponseResult > 0)
        {
            //Now fetch the final results - questions list with correctanswer flag and total question count and correct answer count
            var questions = from qq in _qwizdContext.QuizQuestions
                            join q in _qwizdContext.Questions on qq.QuestionId equals q.Id
                            where qq.QuizId == quizId
                            select new QuestionResult
                            {
                                Text = q.Text,
                                IsCorrectAnswer = qq.IsCorrectAnswer
                            };

            var quizResult = new QuizResultViewModel
            {
                QuestionResults = questions.ToList(),
                CorrectAnswers = questions.ToList().Where(x=>x.IsCorrectAnswer == true).Count(),
                TotalQuestions = questions.Count()
            };

            return quizResult;

        }
        else
            return null;
    }

    public QuizViewModel? StartQuiz(int topicId, int userId)
    {
        var result = 0;
        //Check if questions are available for the given topicId and get the questions (need to limit the number of questions fetched)
        var questionsOnTopic = _qwizdContext.QuestionTopicMappings.Where(x=>x.TopicId == topicId);

        if(questionsOnTopic.Count() > 0)
        {
            //create a quiz
            Quiz quiz = new Quiz
            {
                StartTimestamp = DateTime.Now,
                UserId = userId
            };
            _qwizdContext.Quizzes.Add(quiz);
            result = _qwizdContext.SaveChanges();

            if(result > 0)
            {

                var quizId = quiz.Id;
                var topQuestionsOnTopic = questionsOnTopic.Take(10);
                if(topQuestionsOnTopic.Count() > 0)
                {
                    //Attach top 10 questions to the quiz
                    foreach(var q in topQuestionsOnTopic)
                    {
                        var qq = new QuizQuestion{
                            QuizId = quizId, 
                            QuestionId = q.QuestionId
                        } ;
                        _qwizdContext.QuizQuestions.Add(qq);
                    }

                    _qwizdContext.SaveChanges();

                }

                QuizViewModel quizViewModel = new QuizViewModel
                {
                    Id = quizId,
                    TotalQuestions = topQuestionsOnTopic.Count()
                };

                //return the quizId
                return quizViewModel;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }

    }
}
