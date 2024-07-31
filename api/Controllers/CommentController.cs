using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
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

           [HttpPost("{StockId}")]
              public async Task<IActionResult> Create([FromRoute] int StockId , CreateCommentDto commentDto){
                
                if(!await _stockRepo.StockExists(StockId)){
                    return BadRequest("Stock doesnt Exist");
                }

                var commentModel = commentDto.toCommentFromCreate(StockId);
                await _commentRepo.CreateAsync(commentModel);
                return CreatedAtAction(nameof(GetById), new {id = commentModel}, commentModel.toCommentDto());

              }
    }
}