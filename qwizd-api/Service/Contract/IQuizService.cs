using System;
using qwizd_api.Service.ViewModel;

namespace qwizd_api.Service.Contract;

public interface IQuizService
{
    public QuizViewModel? StartQuiz(int topicId, int userId);
    public QuestionViewModel GetNextQuestion(int quizId);
    public int SaveResponse(int quizId, int questionId, int answerId);
    public QuizResultViewModel? SubmitResponse(int quizId, int questionId, int answerId);
}