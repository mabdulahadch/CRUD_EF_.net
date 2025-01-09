using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentCRUD.Data;
using StudentCRUD.Models;
using Microsoft.ML;

namespace StudentCRUD.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentCRUDContext _context;
        //private readonly MLContext mLContext;

        public StudentsController(StudentCRUDContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var students = await _context.Student.ToListAsync();

                var vmStudentsList = new List<VMStudent>();
                foreach (var index in students)
                {
                    var vmStudent = new VMStudent
                    {
                        Id = index.Id,
                        Name = index.Name,
                        age = index.age, 
                        address = index.address
                    };
                    vmStudentsList.Add(vmStudent);
                }
                return View(vmStudentsList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        //[Route("Add/{id}/{vStudent}")]
        public async Task<IActionResult> Add(VMStudent vStudent)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (vStudent.Name.Any(char.IsDigit))
                    {
                        ViewData["ErrorMessage"] = "Name should not contain any numbers.";
                        return View(vStudent); 
                    }

                    if (vStudent.age <= 0)
                    {
                        ViewData["ErrorMessage"] = "Age should not be less than 10.";
                        return View(vStudent);
                    }

                    var student = new Student
                    {
                        Id = vStudent.Id,
                        Name = vStudent.Name,
                        age = vStudent.age,
                        address = vStudent.address,
                    };

                    _context.Student.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                    return View(vStudent);
                }
            }

            return View(vStudent);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            try
            {
        
                var student = await _context.Student.FirstOrDefaultAsync(m => m.Id == id);
                var vmStudent = new VMStudent
                {
                    Id = student.Id,
                    Name = student.Name,
                    age = student.age,
                    address = student.address
                };
                if (vmStudent == null)
                {
                    return NotFound();
                }
                return View(vmStudent);
            }
            catch
            {
                throw;
            }
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            try
            {

                var student = await _context.Student.FindAsync(id);
                var vmStudent = new VMStudent
                {
                    Id = student.Id,
                    Name = student.Name,
                    age = student.age,
                    address = student.address
                };

                if (student == null)
                {
                    return NotFound();
                }

                return View(vmStudent);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VMStudent virtualModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var checkRecord = await _context.Student.FirstOrDefaultAsync(x => x.Id == virtualModel.Id);

                    if (checkRecord != null)
                    {
                        checkRecord.Name = virtualModel.Name;
                        checkRecord.age = virtualModel.age;
                        checkRecord.address = virtualModel.address;

                        _context.Entry(checkRecord).State = EntityState.Modified;

                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return NotFound($"Student with ID {virtualModel.Id} not found.");
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return View(virtualModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FirstOrDefaultAsync(m => m.Id == id);


            var vmStudent = new VMStudent
            {
                Id = student.Id,
                Name = student.Name,
                age = student.age,
                address = student.address
            };
            if (student == null)
            {
                return NotFound();
            }

            return View(vmStudent);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Student == null)
            {
                return Problem("Entity set 'StudentCRUDContext.Student'  is null.");
            }

            try
            { 
                var student = await _context.Student.FindAsync(id);

                //Console.WriteLine($"Student Name: {student.Name}");
                //Console.WriteLine($"Student Age: {student.age}");

                if (student == null)
                {
                    return Problem("Id does not exists");
                }

                _context.Entry(student).State = EntityState.Deleted;

                //if (student != null)
                //{
                //    _context.Student.Remove(student);
                //}
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            { 
                throw;
            }
        }

        private bool StudentExists(int id)
        {
            //return (_context.Student?.Any(e => e.Id == id)).GetValueOrDefault();

            if (_context.Student == null)
            {
                return false;
            }
            if (_context.Student.Any(e => e.Id == id))
            {
                return true;
            }
            return false;
        }
    }
}
