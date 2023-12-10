using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    public class resultsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public resultsController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }



        [HttpPost]
        [Route("api/results/userAnswer")]

        public IHttpActionResult getUserAnswers(theUserAnswer theAnswer)
        {
            try
            {

                List<qustionDto> qustionDtos = new List<qustionDto>();

                List<result> userResults = db.Result
               .Where(r => r.UserID == theAnswer.userId && r.ExamID == theAnswer.examId).Include(q => q.Question).Include(q => q.Answer)
               .ToList();

                if (userResults.Count == 0)
                {
                    return NotFound();
                }

                userResults.ForEach(rsult =>
                {
                    qustionDto qustionDto = new qustionDto();
                    qustionDto.id = rsult.Question.ID;
                    qustionDto.text = rsult.Question.Text;
                    
                    answerDto answerDto = new answerDto();
                    answerDto.id = rsult.Answer.id;
                    answerDto.text = rsult.Answer.Text;

                    qustionDto.answer = answerDto;

                    qustionDtos.Add(qustionDto);
                });   

                return Ok(qustionDtos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public class theUserAnswer
        {
            public int userId { get; set; }
            public int examId { get; set; }

        }

        public class qustionDto
        {
            public string text { get; set; }
            public int id { get; set; }
            public answerDto answer { get; set; }

        }

        public class answerDto
        {
            public string text { get; set; }
            public int id { get; set; }
        }



        [HttpPost]
        [Route("api/results/final-mark")]

        public IHttpActionResult GetFinalMark(theFinalMark theMark)
        {
            try
            {

                var userResults = db.Result
                    .Where(r => r.UserID == theMark.userId && r.ExamID == theMark.examId)
                    .ToList();


                if (userResults.Count == 0)
                {
                    return NotFound();
                }


                int finalMark = userResults.Sum(r => r.FinalMark);

                return Ok(finalMark);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public class theFinalMark
        {
            public int userId { get; set; }
            public int examId { get; set; }

        }

        [HttpPost]
        [Route("api/results/PostResult")]

        public async Task<IHttpActionResult> Post(List<resultDto> results)
        {



            try
            {
                if (results == null || results.Count < 1)
                {
                    return Json(results);
                }

                List<result> listResult = new List<result>();

                results.ForEach(res =>
                {
                    result resultObj = new result
                    {

                        ExamID = res.examID,
                        UserID = res.userID,
                        FinalMark = res.finalMark,
                        QuestionID = res.questionId,
                        AnswerID = res.answerId
                    };

                    listResult.Add(resultObj);
                });

                db.Result.AddRange(listResult);
                await db.SaveChangesAsync();
                return Ok("saved Successfully");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


        }





        public class resultDto
        {

            public int examID { get; set; }
            public int userID { get; set; }
            public int finalMark { get; set; }
            public int questionId { get; set; }
            public int answerId { get; set; }



        }



        //// POST: api/results
        //[ResponseType(typeof(result))]
        //public IHttpActionResult PostResult(int finalMark)
        //{
        //    using (var db = new ApplicationDbContext())
        //    {

        //        var result = new result
        //        {

        //            FinalMark = finalMark,
        //        };

        //        db.Result.Add(result);
        //        db.SaveChanges();
        //        return CreatedAtRoute("DefaultApi", new { id = result.ID }, result);

        //        return Ok("Final mark submitted successfully");
        //    }
        //}



        //// DELETE: api/results/5
        //[ResponseType(typeof(result))]
        //public async Task<IHttpActionResult> Deleteresult(int id)
        //{
        //    result result = await db.Result.FindAsync(id);
        //    if (result == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Result.Remove(result);
        //    await db.SaveChangesAsync();

        //    return Ok(result);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool resultExists(int id)
        {
            return db.Result.Count(e => e.ID == id) > 0;
        }
    }
}