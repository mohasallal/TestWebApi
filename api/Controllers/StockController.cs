using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stocks")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll(){
            var stocks = _context.Stocks.ToList()
            .Select(s => s.ToStockDto());
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id){
            var stock = _context.Stocks.Find(id);
            if(stock == null){
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            _context.Stocks.Add(stockModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());

        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockFromDb = _context.Stocks.Find(id);
            if(stockFromDb == null){
                return NotFound();
            }
           else{
            stockFromDb.Symbol = updateDto.Symbol;
            stockFromDb.CompanyName = updateDto.CompanyName;
            stockFromDb.Purchase = updateDto.Purchase;
            stockFromDb.LastDiv = updateDto.LastDiv;
            stockFromDb.Industry = updateDto.Industry;
            stockFromDb.MarketCap = updateDto.MarketCap;
            _context.SaveChanges();
            return Ok(stockFromDb.ToStockDto());
           }

        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var stockFromDb = _context.Stocks.Find(id);
            if(stockFromDb == null){
                return NotFound();
            }
            _context.Stocks.Remove(stockFromDb);
            _context.SaveChanges();
            return NoContent();
        }
    }
}