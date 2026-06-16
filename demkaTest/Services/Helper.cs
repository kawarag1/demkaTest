using demkaTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demkaTest.Services
{
    public class Helper
    {
        private static demkaTestEntities _context;
        public static demkaTestEntities GetContext()
        {
            if (_context == null)
            {
                _context = new demkaTestEntities();
            }
            return _context;
        }
    }
}
