using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Training.Data;
using Training.Models;

namespace Training.Controllers
{
    public class questionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //
        public questionsController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }






        // GET: api/questions/questions-and-answers


        [Route("api/questions/questionswithanswers")]
        [HttpPost]
        public IHttpActionResult getQuestionAndAnswerByExamIdt(ExamIdDto ex)
        {


            List<qustionDto> qustionDtos = new List<qustionDto>();
            List<question> questionsWithAnswers = db.Question
                .Include(q => q.Answer).Where(x => x.ExamID == ex.examId)
                .ToList();

            if (questionsWithAnswers == null)
            {
                return NotFound();
            }

            questionsWithAnswers.ForEach(question =>
            {
                qustionDto qustionDto = new qustionDto();
                qustionDto.id = question.ID;
                qustionDto.text = question.Text;

                if (question.Answer.Any(answer => answer.IsCorrect))
                {
                    answerDto answerDto = new answerDto();
                    answerDto.id = question.Answer.FirstOrDefault(a => a.IsCorrect)?.id ?? 0;
                    answerDto.text = question.Answer.FirstOrDefault(a => a.IsCorrect)?.Text;

                    qustionDto.answer = answerDto;
                    qustionDtos.Add(qustionDto);
                }
            });

            return Ok(qustionDtos);


        }
        [Route("api/questions/Postexam")]
        [ResponseType(typeof(List<qustionDto2>))]
        public async Task<IHttpActionResult> Postexam(examDto examDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            exam examObj = new exam();
            List<question> questions = new List<question>();

            examObj.Title = examDto.title;

            examDto.question.ForEach(objQus =>
            {
                question qus = new question();
                qus.Text = objQus.text;
                qus.Mark = objQus.mark;
                qus.ID = objQus.id;


                questions.Add(qus);
            });

            examObj.questions = questions;

            db.Exam.Add(examObj);
            await db.SaveChangesAsync();

            return Ok(examObj);


        }
        public class examDto
        {
            public string title { get; set; }
            public List<qustionDto2> question { get; set; }
        }

        public class qustionDto2
        {
            public string text { get; set; }
            public int mark { get; set; }
            public int id { get; set; }

        }

       


        public class answerDto
                {
                    public string text { get; set; }
                    public int id { get; set; }
                }
        public class qustionDto
        {
            public string text { get; set; }
            public int id { get; set; }
            public answerDto answer { get; set; }

        }

       


        public class ExamIdDto
        {
            public int examId { get; set; }
        }

        [Route("api/questions/questionsbyexamid")]
        [HttpPost]
        public IHttpActionResult GetQuestionsByExamId(ExamIdDto sup)
        {
            var questionsAndAnswers = db.Question
                .Where(q => q.ExamID == sup.examId)
                .Include(q => q.Answer)

                .ToList();

            return Ok(questionsAndAnswers);
        }
      

        [Route("api/questions/questionsbyexamidWhithoutAnswers")]
        [HttpPost]
        public IHttpActionResult GetQuestionsByExamIdWhithoutAnswers(ExamIdDto sup)
        {
            var questionsAndAnswers = db.Question
                .Where(q => q.ExamID == sup.examId)
                .ToList();

            return Ok(questionsAndAnswers);
        }

        [Route("api/questions/getQuestionId")]
        [HttpGet]
        public IQueryable<question> GetExamTitles()
        {
            //var theId = db.Question.Select(e => e.ID).ToList();
            return db.Question;
        }



        [HttpPost]
        [Route("api/questions/total-mark")]

        public IHttpActionResult GetFinalMark(totalMark theMark)
        {
            try
            {

                var userResults = db.Question
                    .Where(r => r.ExamID == theMark.examId)
                    .ToList();


                if (userResults.Count == 0)
                {
                    return NotFound();
                }


                int TotalMark = userResults.Sum(r => r.Mark);

                return Ok(TotalMark);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public class totalMark
        {
          //  public int userId { get; set; }
            public int examId { get; set; }

        }


        // GET: api/questions/5
        //[ResponseType(typeof(question))]
        //public async Task<IHttpActionResult> Getquestion(int id)
        //{
        //    question question = await db.Question.FindAsync(id);
        //    if (question == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(question);
        //}

        // PUT: api/questions/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> Putquestion(int id, question question)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != question.ID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(question).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!questionExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/questions
        //[ResponseType(typeof(question))]
        //public async Task<IHttpActionResult> Postquestion(question question)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Question.Add(question);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = question.ID }, question);
        //}

        //// DELETE: api/questions/5
        //[ResponseType(typeof(question))]
        //public async Task<IHttpActionResult> Deletequestion(int id)
        //{
        //    question question = await db.Question.FindAsync(id);
        //    if (question == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Question.Remove(question);
        //    await db.SaveChangesAsync();

        //    return Ok(question);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool questionExists(int id)
        {
            return db.Question.Count(e => e.ID == id) > 0;
        }
    }
}