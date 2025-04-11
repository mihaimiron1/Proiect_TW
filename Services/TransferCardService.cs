using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Proiect_TW.Data;
using Proiect_TW.Models;

namespace Proiect_TW.Services
{
    public class TransferCardService
    {
        private readonly ApplicationDbContext _context;

        public TransferCardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TransferCard>> GetAllTransfersAsync()
        {
            return await _context.TransferCards.ToListAsync();
        }

        public async Task<TransferCard> CreateTransferAsync(TransferCard transfer)
        {
            _context.TransferCards.Add(transfer);
            await _context.SaveChangesAsync();
            return transfer;
        }
    }
} 