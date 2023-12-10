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
    public class examsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public examsController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/exam
        public IQueryable<exam> GetExam()
        {
            return db.Exam;
        }

        // GET: api/exam/5
        [ResponseType(typeof(exam))]




        [Route ("titles")]
        [HttpGet]
        public IHttpActionResult GetExamTitles()
        {
            var examTitles = db.Exam.Select(e => e.Title).ToList();
            return Ok(examTitles);
        }




        // PUT: api/exam/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putexam(int id, exam exam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != exam.ID)
            {
                return BadRequest();
            }

            db.Entry(exam).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!examExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/exam
        [ResponseType(typeof(exam))]
        public async Task<IHttpActionResult> Postexam(exam exam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Exam.Add(exam);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = exam.ID }, exam);
        }

        // DELETE: api/exam/5
        [ResponseType(typeof(exam))]
        public async Task<IHttpActionResult> Deleteexam(int id)
        {
            exam exam = await db.Exam.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            db.Exam.Remove(exam);
            await db.SaveChangesAsync();

            return Ok(exam);
        }


        



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool examExists(int id)
        {
            return db.Exam.Count(e => e.ID == id) > 0;
        }
    }
}