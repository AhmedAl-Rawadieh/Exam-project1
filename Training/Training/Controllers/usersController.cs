using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
   
    public class usersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public usersController()
        {
            db.Configuration.ProxyCreationEnabled = false;

        }

       

        // Update the API controller method
        [HttpPost]
        [Route("api/users/login")]
        [ResponseType(typeof(users))]
        public IHttpActionResult post(loginDto Obj)
        {
             var user1 = db.users.FirstOrDefault(u => u.UserName.Trim().ToLower() == Obj.username.Trim().ToLower() && u.Password ==   Obj.password);

            if (user1 == null)
            {
                return BadRequest("Invalid credentials");
            }
            else
            {
         
                return Ok(user1);
            }
        }



        //// GET: api/users/5
        //[ResponseType(typeof(users))]
        //public async Task<IHttpActionResult> Getusers(int? ID)
        //{
        //    if (ID <= 0)
        //    {
        //        return BadRequest("Invalid 'ID' parameter.");
        //    }
        //    users users = await db.users.FindAsync(ID);
        //    if (users == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(users);
        //}


        //// PUT: api/users/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> Putusers(int id, users users)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != users.ID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(users).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!usersExists(id))
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



        //// POST: api/users

        //public async Task<IHttpActionResult> Postusers(UserDto user)
        //{
        //    users userObj = new users
        //    {
        //        ID = user.ID,
        //        Name = user.Name,
        //        UserName = user.email, 
        //        Password = user.Password
        //    };



        //    db.users.Add(userObj);
        //    await db.SaveChangesAsync();

        //    return Ok(user);
        //}

        //// POST: api/users


        // [Route("signup")]
        [ResponseType(typeof(users))]
        public async Task<IHttpActionResult> users(UsersDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                if (db.users.Any(u => u.UserName.Trim().ToLower() == user.username.Trim().ToLower()))
                {
                    return BadRequest("Email already exists. Please use a different email address.");
                }
                else
                {

                    users userObj = new users
                    {
                        Name = user.name,
                        UserName = user.username,
                        Password = user.password
                    };



                    db.users.Add(userObj);
                    await db.SaveChangesAsync();

                    return CreatedAtRoute("DefaultApi", new { ID = user.id }, user);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }












        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool usersExists(int ID)
        {
            return db.users.Count(e => e.ID == ID) > 0;
        }


        //[AcceptVerbs("POST")]
        //[ResponseType(typeof(users))]
        //[HttpPost]
        //[Route("signup")]
        //public IHttpActionResult PostSignUp(SignUpRequest signUpRequest)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        var newUser = new users
        //        {
        //            Name=signUpRequest.Name,
        //            UserName = signUpRequest.Username,
        //            Password = signUpRequest.Password,
        //            // Add other properties as needed and map them from SignUpRequest
        //        };

        //        db.users.Add(newUser);
        //        db.SaveChanges();
        //        return CreatedAtRoute("DefaultApi", new { id = newUser.ID }, newUser);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception for debugging
        //        Console.WriteLine(ex.ToString());

        //        // Handle any database update errors here
        //        return InternalServerError(); // You might want to return a more specific error message.
        //    }
        //}




        //public class SignUpRequest
        //{
        //    public string Name { get; set; }
        //    public string Username { get; set; }
        //    public string Password { get; set; }
        //    // Additional properties for registration data if needed
        //}


        //[Route("api/login")]
        //[ResponseType(typeof(users))]
        //public IHttpActionResult GetLogin(LoginRequest loginRequest)
        //{
        //    // Your login logic here

        //    // Replace this with your actual authentication logic
        //    var username = loginRequest.Username;
        //    var password = loginRequest.Password;
        //    // Check username and password against your data source

        //    // If the login is successful, return the user object
        //    users user = new users(); // Replace with your actual user retrieval logic

        //    if (user == null)
        //    {
        //        return NotFound(); // Or return an appropriate response for login failure
        //    }

        //    return Ok(user);
        //}
        //public class LoginRequest
        //{
        //    public string Username { get; set; }
        //    public string Password { get; set; }
        //}
        public class UsersDto
        {
            public int? id { get; set; }
            public string name { get; set; }
            //[Required]
            //[EmailAddress]
            public string username { get; set; }

            //[Required]
            //[MinLength(6)]
            public string password { get; set; }
            public string repeatPassword { get; set; }

        }
        public class loginDto
        {
          
            public string username { get; set; }      
            public string password { get; set; }
        }


    }

}
