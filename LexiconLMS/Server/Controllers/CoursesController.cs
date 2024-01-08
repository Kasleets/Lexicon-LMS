﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LexiconLMS.Domain.Entities;
using LexiconLMS.Server.Data;
using LexiconLMS.Server.Services;
using LexiconLMS.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace LexiconLMS.Server.Controllers
{
    [Route("api/courses")]
    [ApiController]

    public class CoursesController : ControllerBase
    {
        private readonly IServiceManager serviceManager;

        public CoursesController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }
        [Authorize(Roles="Admin")]
        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourse(bool includeAll = false)
        {
            return Ok(await serviceManager.CourseService.GetCoursesAsync(includeAll));

        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(Guid id)
        {
            return Ok((CourseDto?)await serviceManager.CourseService.GetCourseAsync(id));

            //var courseDto = await serviceManager.CourseService.GetCourseAsync(id);
            //if (courseDto == null) return NotFound();

            //return Ok(courseDto);

            //if (_context.Course == null)
            //{
            //    return NotFound();
            //}
            //var course = await _context.Course.FindAsync(id);

            //if (course == null)
            //{
            //    return NotFound();
            //}

            //return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCourse(Guid id, CourseDto course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }
            await serviceManager.CourseService.UpdateCourseAsync(id, course);

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CourseAddDto>> PostCourse(CourseAddDto course)
        {
            if (course != null)
            {
                return Ok(await serviceManager.CourseService.CreateCourseAsync(course));
            }
            else
            {
                return BadRequest("Data missing.");
            }
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            //if (_context.Course == null)
            //{
            //    return NotFound();
            //}
            //var course = await _context.Course.FindAsync(id);
            //if (course == null)
            //{
            //    return NotFound();
            //}

            //_context.Course.Remove(course);
            //await _context.SaveChangesAsync();

            //return NoContent();

            return null;
        }

        private bool CourseExists(Guid id)
        {
            return false;
            //return (_context.Course?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet("activity/{id}")]
        public async Task<ActionResult<ActivityDto>> GetActivity(Guid id)
        {
            var activityDto = await serviceManager.ActivityService.GetActivityAsync(id);
            return activityDto;
        }
    }
}
