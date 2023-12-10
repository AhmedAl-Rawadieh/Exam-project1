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
    public class answersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //
        public answersController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }



   

        // GET: api/answers
        public IQueryable<answer> GetAnswer()
        {
            return db.Answer;
        }

        // GET: api/answers/5
        //[ResponseType(typeof(answer))]
        //public async Task<IHttpActionResult> Getanswer(int id)
        //{
        //    answer answer = await db.Answer.FindAsync(id);
        //    if (answer == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(answer);
        //}

        // PUT: api/answers/5
        //[ResponseType(typeof(void))]
        //[HttpPut]
        //public async Task<IHttpActionResult> Putanswer(int id, answer answer)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != answer.id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(answer).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!answerExists(id))
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



        // GET: api/answers/user-answers
        [Route("api/answers/user-answers")]
        [ResponseType(typeof(List<answer>))]
        public IHttpActionResult GetUserAnswers()
        {


            List<answer> userAnswers = new List<answer>
            {
                // Retrieve user answers and add them to the list
            };

            if (userAnswers == null)
            {
                return NotFound();
            }

            return Ok(userAnswers);
        }
        



        [Route("api/answers/CorrectAnswers")]
        [HttpGet]
        [ResponseType(typeof(List<answer>))]
        public IHttpActionResult getCorrectAnswers()
        {
            var correctAnswers = db.Answer.Where(a => a.IsCorrect).ToList();

            if (correctAnswers == null)
            {
                return NotFound();
            }

            return Ok(correctAnswers);
        }


        [Route("api/answers/postNewAnswer")]
        [ResponseType(typeof(answer))]
        public async Task<IHttpActionResult> Post(List<answerDto> answers)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                foreach (var answerDto in answers)
                {
                    answer newAnswer = new answer
                    {
                        Text = answerDto.text,
                        IsCorrect = answerDto.isCorrect,
                        QuestionID = answerDto.questionID
                    };

                    db.Answer.Add(newAnswer);

                    await db.SaveChangesAsync();
                }

                return Ok("Answers saved successfully");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }



        public class answerDto
        {
            public string text { get; set; }
            public bool isCorrect { get; set; }
            public int questionID { get; set; }
        }



        // POST: api/answers
        [ResponseType(typeof(answer))]
        public async Task<IHttpActionResult> Post(answer answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Answer.Add(answer);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = answer.id }, answer);
        }



        // DELETE: api/answers/5
        [ResponseType(typeof(answer))]
        public async Task<IHttpActionResult> Deleteanswer(int id)
        {
            answer answer = await db.Answer.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }

            db.Answer.Remove(answer);
            await db.SaveChangesAsync();

            return Ok(answer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool answerExists(int id)
        {
            return db.Answer.Count(e => e.id == id) > 0;
        }
    }
}