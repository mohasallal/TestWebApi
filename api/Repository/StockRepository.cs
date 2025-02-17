using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {

        private readonly ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext context){
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockmodel)
        {
            await _context.Stocks.AddAsync(stockmodel);
            await _context.SaveChangesAsync();
            return stockmodel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if(stock == null) return null;
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
           return await _context.Stocks.Include(c => c.Comments).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.FindAsync(id);
        }

        public Task<bool> StockExists(int id)
        {
            return _context.Stocks.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existingStock = await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
            if(existingStock == null) return null;
            else{
                existingStock.Symbol = stockDto.Symbol;
                existingStock.CompanyName = stockDto.CompanyName;
                existingStock.Purchase = stockDto.Purchase;
                existingStock.LastDiv = stockDto.LastDiv;
                existingStock.Industry = stockDto.Industry;
                existingStock.MarketCap = stockDto.MarketCap;
                await _context.SaveChangesAsync();
                return existingStock;
            }
        }
    }
}