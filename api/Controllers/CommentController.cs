using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        public CommentController(ICommentRepository commentRepo)
        {
            _commentRepo = commentRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var comment = await _commentRepo.GetAllAsync();

            var CommentDto = comment.Select(s => s.toCommentDto());
       
            return Ok(CommentDto);
        }
        
            [HttpGet("{id}")]
           public async Task<IActionResult> GetById([FromRoute] int id){
            var comment = await _commentRepo.GetByIdAsync(id);

            if(comment == null){
                return NotFound();
            }
            else{
                return Ok(comment.toCommentDto());
            }

           }   
    }
}