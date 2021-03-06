﻿using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CartolaDaPelada.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private IUserService _userService;        

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpPost]        
        public JsonResult Post([FromBody]User obj)
        {            
            try
            {                 
                var newUser = _userService.Create(obj);

                return Json(newUser);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public JsonResult Delete([FromRoute]int id)
        {
            try
            {
                _userService.Delete(id);

                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
    }
}
