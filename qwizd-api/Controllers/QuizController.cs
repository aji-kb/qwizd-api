using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using qwizd_api.Data;
using qwizd_api.Data.Model;
using qwizd_api.Service.Contract;
using qwizd_api.Service.ViewModel;

namespace qwizd
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly QwizdContext _qwizdContext;
        private readonly IQuizService _quizService;

        private readonly ILogger<QuizController> _logger;

        public QuizController(QwizdContext qwizdContext, ILogger<QuizController> logger, IQuizService quizService)
        {
            _qwizdContext = qwizdContext;
            _logger = logger;
            _quizService = quizService;
        }

        [HttpGet]
        public IActionResult NextQuestion(int id)
        {
            try
            {
                var question = _quizService.GetNextQuestion(id);
                return Ok(question);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in QuizController.NextQuestion");
                return StatusCode(500, "Unknown Error Occurred");
            }
            
        }

        [HttpPost]
        [Route("Save")]

        public IActionResult SaveResponse([FromBody]QuizResponseViewModel quizReponse)
        {
            try
            {
                var response = _quizService.SaveResponse(quizReponse.QuizId, quizReponse.QuestionId, quizReponse.AnswerId);

                if(response> 0)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(500, "Error in saving Quiz Response");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in QuizController.SaveResponse");
                return StatusCode(500, "Unknown Error Occurred");
            }
        }

        [HttpPost]
        [Route("submit")]
        public IActionResult SubmitResponse([FromBody]QuizResponseViewModel quizReponse)
        {
            try
            {
                var response = _quizService.SubmitResponse(quizReponse.QuizId, quizReponse.QuestionId, quizReponse.AnswerId);
                if(response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(500, "Error in showing results");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in QuizController.SubmitResponse");
                return StatusCode(500, "Unknown Error Occurred");
            }
        }

        [HttpGet]
        [Route("categories")]
        public IActionResult GetCategories(int? parentTopicId)
        {

            var categories = _qwizdContext.Topics.ToList();

            if(parentTopicId.HasValue)
            {
                categories = categories.Where(x=>x.ParentTopicId == parentTopicId).ToList();
            }
            else
            {
                categories = categories.Where(x=>x.ParentTopicId is null).ToList();
            }

            return Ok(categories);
        }  

        [HttpPost]
        [Route("start")]
        public IActionResult StartQuiz([FromBody]StartQuizRequestViewModel startQuizRequestViewModel)
        {
            try
            {
                var loggedInUserString = HttpContext.Session.GetString("logged_in_user");
                var userId = 0;
                if(!string.IsNullOrEmpty(loggedInUserString))
                {
                    var loggedInUser = JsonConvert.DeserializeObject<User>(loggedInUserString);
                    if(loggedInUser != null)
                    {
                        userId = loggedInUser.Id;
                    }
                }
                var quizViewModel = _quizService.StartQuiz(startQuizRequestViewModel.TopicId, userId);
                if(quizViewModel != null)
                    return Ok(quizViewModel);
                else
                    return StatusCode(500, "Cannot Start the quiz");
                //create a quiz (randomly identify 10 (configurable) questions from the topic. questions should give high priority to questions not asked previously )
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in QuizController.StartQuiz");
                return StatusCode(500, "Unknown error occurred. Please contact administrator");
            }
        }


    }
}
