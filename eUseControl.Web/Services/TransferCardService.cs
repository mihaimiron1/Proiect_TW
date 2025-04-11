using System.Collections.Generic;
using System.Linq;
using eUseControl.Web.Data;
using eUseControl.Web.Models;

namespace eUseControl.Web.Services
{
    public class TransferCardService
    {
        private readonly ApplicationDbContext _context;

        public TransferCardService()
        {
            _context = new ApplicationDbContext();
        }

        public List<TransferCard> GetAllTransfers()
        {
            return _context.TransferCards.ToList();
        }

        public void CreateTransfer(TransferCard transfer)
        {
            _context.TransferCards.Add(transfer);
            _context.SaveChanges();
        }
    }
} 